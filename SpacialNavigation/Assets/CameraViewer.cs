//Author: Dustin Grady
//Purpose: Attempt to identify objects (using a camera) and replace them with virtual objects within a virtual space
//Status: Beginning to be able to sense objects in high contrast (background white, objects black, etc)

// Starts the default camera and assigns the texture to the current renderer
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraViewer : MonoBehaviour{
	WebCamTexture webcamTexture;
	public Texture aTexture;
	private int cellWidth = 64;
	private int cellHeight = 48;
	private int gridSize = 10;
	private int generationCount = 0;
	private int generationDelay = 500;
	private int totalCells;
	private bool drawBox = false; //Used to draw box around detected objects
	private bool testFlag = false; //Enable for test output
	private bool cycleComplete = true; //Acts as a 'lock' when processing frames
	private bool finishedGenerating = false; //Used as a 'lock' to allow us to fully analyze a frame before beginning the next
	private double[,] resultValues; //2D array to store our results (on a per-frame basis)
	private float boxPosX; //Position of box used as visual aid when identifying objects
	private float boxPosY;
	private List<Rect> rectBoxes = new List<Rect>();
	private Color[] pixelChunk; //Flattened array of (r,g,b) pixel values
	private Color colors;
	Vector2Int[] validMoves = new Vector2Int[] {
		new Vector2Int (-1, 0),
		new Vector2Int (1, 0),
		new Vector2Int (0, -1),
		new Vector2Int (0, 1)
	} ; //List of valid moves we can perform on our grid

	/*Initialize values*/
	void Start(){
		webcamTexture = new WebCamTexture();
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = webcamTexture;
		webcamTexture.Play ();
	}

	void Update(){
		if(testFlag){
			TestFunction (); //For debugging/ testing
		}
		if(cycleComplete){
			rectBoxes.Clear ();
			CellGenerator();
		}
	}

	//Struct to allow us to return values from AnalyzeImage function (because apparently Unity doesn't like tuples!)
	struct RGBValues{
		public double avgRedVal;
		public double avgGreenVal;
		public double avgBlueVal;
	}

	/*Allows us to verify that the corner pixels are updating when new colors are presented*/
	void TestFunction(){
		Debug.Log("Bottom left: " + webcamTexture.GetPixel (0, 0)); //Testing
		Debug.Log("Top left: " + webcamTexture.GetPixel (0, webcamTexture.height -1)); //Testing
		Debug.Log("Bottom right: " + webcamTexture.GetPixel (webcamTexture.width -1, 0)); //Testing
		Debug.Log("Top right: " + webcamTexture.GetPixel (webcamTexture.width -1, webcamTexture.height -1)); //Testing
	}

	/*Break current frame into a series of 'pixel-sized blocks'*/
	void CellGenerator(){
		cycleComplete = false; //We are now in a cycle, do not allow update to call us until finished
		Color[,] resultValues = new Color[gridSize,gridSize];
		int xCoord = -1;
		int yCoord = -1;
		for (int x = 0; x < webcamTexture.width; x += cellWidth) {
			xCoord++;
			for (int y = 0; y < webcamTexture.height; y += cellHeight) {
				yCoord++;
				if (testFlag) {
					//Debug.Log ("xCoord: " + xCoord);
					//Debug.Log ("yCoord: " + yCoord);
				}
				pixelChunk = webcamTexture.GetPixels (x, y, cellWidth, cellHeight); //x and y are starting coords of where the pixelChunk will be referenced (left->right / top->bottom)
				Color colors = new Color((float)AveragePixelChunk(pixelChunk).avgRedVal, (float)AveragePixelChunk(pixelChunk).avgGreenVal, (float)AveragePixelChunk(pixelChunk).avgBlueVal);
				resultValues[xCoord,yCoord] = colors;
				//Debug.Log (resultValues[9,9]); //TESTING ERROR HERE
			}
			yCoord = -1; //Reset
		}
		xCoord = -1; //Reset

		DetectObjects (resultValues); //Pass our 2D array to DetectObjects to attempt to identify objects in frame
	}

	/*Find average RGB values for a given 'pixel-sized chunk'*/
	RGBValues AveragePixelChunk(Color[] passedVal){
		double avgRed = 0.0;
		double avgGreen = 0.0;
		double avgBlue = 0.0;
		for(int i=0; i<cellWidth*cellHeight; i++){ //Itterate over every pixel in this chunk of pixels
			avgRed += passedVal [i].r;
			avgGreen += passedVal [i].g;
			avgBlue += passedVal [i].b;
		}
		//Calculate average r,g,b values for this chunk
		avgRed = avgRed / (cellWidth * cellHeight); 
		avgGreen = avgGreen / (cellWidth * cellHeight); 
		avgBlue = avgBlue / (cellWidth * cellHeight); 

		RGBValues rgb = new RGBValues(); //Create reference to our struct
		rgb.avgRedVal = avgRed; //Add average values to struct
		rgb.avgGreenVal = avgGreen;
		rgb.avgBlueVal = avgBlue;
		return rgb; //Return our struct with average values in it
	}

	/*Compare neighboring cells for similar rgb values to detect an 'object'*/
	void DetectObjects(Color[,] inputValues){
		double toleranceThreshold = 0.50; //Tolerance we will allow when comparing neighbors
		int requiredNeighbors = 5;

		//Compare rgb values between all cells and their neighbors
		for(int y = 0; y < gridSize; y++){
			for(int x = 0; x < gridSize; x++){
				List<Vector2Int> neighbors = getNeighbors (new Vector2Int (x, y)); //Get neighbors of current node
				foreach (var neighbor in neighbors) { //Compare neighbors average rgb to that of current node
					if (!checkTolerance (inputValues [x, y], inputValues [neighbor.x, neighbor.y], toleranceThreshold)) { //If it doesn't share common values with it's neighbors
						boxPosX = (x * ((Screen.width/4)/gridSize));
						boxPosY = (y * ((Screen.height/4)/gridSize));
						rectBoxes.Add(new Rect(boxPosX, boxPosY, (Screen.width/4)/gridSize, (Screen.height/4)/gridSize));
						generationCount++;
						if(generationCount >= generationDelay){ //Slow down object generation
							GenerateObject((GameObject.FindGameObjectWithTag("MainCamera").transform.position.x)-(boxPosX/5), GameObject.FindGameObjectWithTag("MainCamera").transform.position.z); //Instantiate object
							//Debug.Log("xPos: " + ((GameObject.FindGameObjectWithTag("MainCamera").transform.position.x) - boxPosX));
							//Debug.Log("boxPosX: " + boxPosX);
							//Debug.Log ("CamX: " + GameObject.FindGameObjectWithTag("MainCamera").transform.position.x);
						}
						drawBox = true;
					}
				}
			}
		}
		cycleComplete = true;
	}

	/*Used as a visual aid to draw rectangle around detected objects*/
	void OnGUI(){
		if (drawBox) {
			foreach (var rect in rectBoxes) {
				//rectBoxes.Add (new Rect(0,0, Screen.width/gridSize, Screen.height/gridSize)); //TESTING
				GUI.DrawTexture (rect, aTexture, ScaleMode.ScaleAndCrop, true, 1.0f, Color.red, 1.0f, 0);
			}
		} 
	}

	/*Gets neighbors of passed in node*/
	List <Vector2Int> getNeighbors(Vector2Int position){
		List<Vector2Int> neighbors = new List<Vector2Int> ();
		foreach (var move in validMoves) {
			Vector2Int neighbor = position + move;
			if (neighbor.x < 0 || neighbor.y < 0 || neighbor.x >= gridSize || neighbor.y >= gridSize) { //If we are out of bounds
				//Do nothing
			}  else {
				neighbors.Add (neighbor);
			}
		}
		return neighbors;
	}

	/*Checks values to determine if their rgb values are within allowable range*/
	bool checkTolerance(Color firstValue, Color secondValue, double tolerance){
		if (((firstValue.r + tolerance) >= secondValue.r || (firstValue.r - tolerance) >= secondValue.r) &&
			((firstValue.g + tolerance) >= secondValue.g || (firstValue.g - tolerance) >= secondValue.g) && 
			((firstValue.b + tolerance) >= secondValue.b || (firstValue.b - tolerance) >= secondValue.b)) {
			return true;
		}
		return false;
	}

	/* Instantiate a 3D object to place where a webcam object has been detected */
	void GenerateObject(float xPos, float zPos){
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.AddComponent<Rigidbody>();
		cube.transform.localScale = new Vector3 (3.0f, 3.0f, 3.0f);
		cube.transform.position = new Vector3(xPos, 0.2f, zPos + 50.0f); //Add offset to Z so that object appears infront of us and not at our Z pos
		generationCount = 0;
	}
}
