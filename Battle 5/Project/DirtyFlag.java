/* Avoid unnecessary work by deferring it until the result is needed. */

public class DirtyFlag {
	//private ArrayList<String> inputList = new ArrayList<>();
	private String[] inputList = new String[5];
	private int[] dirtyFlag = new int[5];
	private String input;
	private int index;
	
	//Constructor
	public DirtyFlag(String input){
		this.input = input;
		populateArray(input, index++);
	}
	
	private void populateArray(String input, int index){
		inputList[index] = this.input;
	}
	
	//Check for changes/ mark accordingly
	public boolean deltaDetected(String input, int index){
		if(!input.equals(inputList[index])){ //Change detected, mark dirty
			dirtyFlag[index] = 1; //Mark as dirty
			inputList[index] = input; //Add updated value to replace old value
			render(this.input, index);
			return true;
		}
		else{
			dirtyFlag[index] = 0;
			System.out.println("Clean value\nValue: " + inputList[index] + "\n" + "Flag status: " + dirtyFlag[index] + "\nNo need to call render\n");//Testing
			return false;
		}
	}
	
	//Render scene here (only if dirty flag detected)
	private void render(String input, int index){
		if(deltaDetected(this.input, this.index)){
			System.out.println("Dirty flag detected\nNew value: " + inputList[index] + "\n" + "Flag status: " + dirtyFlag[index]);//Testing
			System.out.println("Rendering changes...\n");
		}
	}
}
