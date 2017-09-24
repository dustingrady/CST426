
public class DirtyFlagTest {
	
	public static void main(String args[]){
		DirtyFlag dirtyFlagObj1 = new DirtyFlag("xPos: 10.1, yPos: 7.8, zPos: 13.9"); //Initialize value via constructor
		dirtyFlagObj1.deltaDetected("xPos: 10.1, yPos: 7.8, zPos: 13.9", 0);
		dirtyFlagObj1.deltaDetected("xPos: 10.7, yPos: 8.8, zPos: 18.9", 0); 
		dirtyFlagObj1.deltaDetected("xPos: 10.7, yPos: 8.8, zPos: 18.9", 0);
		 
		//--------Order of events---------
		//Original value (render)
		//Original value (no need to render)
		//New value (render)
		//This value again ^ (no need to render)
	}
}
