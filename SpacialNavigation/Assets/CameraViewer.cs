//Author: Dustin Grady
//Purpose: Attempt to identify objects (using a camera) and replace them with virtual objects within a virtual space
//Status: Unfinished/ in development

// Starts the default camera and assigns the texture to the current renderer
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraViewer : MonoBehaviour
{
	private int cellWidth = 64;
	private int cellHeight = 48;
	private int totalCells;
	private bool testFlag = false; //Enable for test output
	private bool finishedGenerating = false; //Used as a 'lock' to allow us to fully analyze a frame before beginning the next
	private double[,] resultValues; //2D array to store our results (on a per-frame basis)
	private Color[] pixelChunk; //Flattened array of (r,g,b) pixel values
	private Color colors;
	WebCamTexture webcamTexture;

	/*Initialize values*/
	void Start()
	{
		webcamTexture = new WebCamTexture();
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = webcamTexture;
		webcamTexture.Play ();			
	}

	void Update(){
		if(testFlag){
			TestFunction (); //For debugging/ testing
		}
		CellGenerator();
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
		Color[,] resultValues = new Color[10,10];
		int xCoord = -1;
		int yCoord = -1;
		//if (finishedGenerating == false) {
			for (int x = 0; x < webcamTexture.width; x += cellWidth) {
				xCoord++;
				for (int y = 0; y < webcamTexture.height; y += cellHeight) {
					yCoord++;
					if (testFlag) {
						Debug.Log ("xCoord: " + xCoord);
						Debug.Log ("yCoord: " + yCoord);
					}
					pixelChunk = webcamTexture.GetPixels (x, y, cellWidth, cellHeight); //x and y are starting coords of where the pixelChunk will be referenced (left->right / top->bottom)
					Color colors = new Color((float)AveragePixelChunk(pixelChunk).avgRedVal, (float)AveragePixelChunk(pixelChunk).avgGreenVal, (float)AveragePixelChunk(pixelChunk).avgBlueVal);
					resultValues[xCoord,yCoord] = colors;
				}
				yCoord = -1; //Reset
			}
			xCoord = -1; //Reset
		//}
		DetectObjects (resultValues); //Pass our 2D array to DetectObjects to attempt to identify objects in frame
		finishedGenerating = true; //We have finished feeding and analyzing this frame, we can start processing another
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
		if(testFlag){
			Debug.Log ("Average Red: " + avgRed); 
			Debug.Log ("Average Green: " + avgGreen); 
			Debug.Log ("Average Blue: " + avgBlue); 
		}
		RGBValues rgb = new RGBValues(); //Create reference to our struct
		rgb.avgRedVal = avgRed; //Add average values to struct
		rgb.avgGreenVal = avgGreen;
		rgb.avgBlueVal = avgBlue;
		return rgb; //Return our struct with average values in it
	}

	/*Compare neighboring cells for similar rgb values to detect an 'object'*/
	void DetectObjects(Color[,] inputValues){
		double toleranceThreshold = 0.0015; //Tolerance we will allow when comparing neighbors
		Debug.Log ("Bottom left: " + inputValues[0,0]);//Testing bottom left cell
		Debug.Log ("Top left: " + inputValues[0,9]);//Testing top left cell
		Debug.Log ("Bottom right: " + inputValues[9,0]);//Testing bottom right cell
		Debug.Log ("Top right: " + inputValues[9,9]); //Testing top right cell
	}

	/*Function to take a static picture and save to HDD before processing (too slow)*/
	/*
	void TakePicture(){
		string savePath = "C:/Temp/UnityTest";
		int captureCounter = 0;
		Texture2D snap = new Texture2D (webcamTexture.width, webcamTexture.height);
		snap.SetPixels (webcamTexture.GetPixels());
		snap.Apply ();

		System.IO.File.WriteAllBytes (savePath + captureCounter.ToString () + ".png", snap.EncodeToPNG ());
		++captureCounter;
		//Debug.Log(webcamTexture.GetPixels());
		//Debug.Log(webcamTexture.GetPixel(0,0));
	}
	*/
}