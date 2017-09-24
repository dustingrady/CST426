import java.util.ArrayList;

public class TypeObjectTest{
	public static void main (String args[]){
		GameController gc = new GameController();
		gc.Start();
		gc.Update();
	}
	 public static class GameController extends TypeObject
	 {
		 ArrayList <Superpower> superPowers = new ArrayList<Superpower>();
	     
		 //Add subclasses to our array list for later processing
		 void Start()
	     {
	         superPowers.add(new SkyLaunch());
	         superPowers.add(new GroundDive());
	     }

	     void Update()
	     {
	    	 //Activate each superpower each update
	         for (int i = 0; i < superPowers.size(); i++)
	         {
	        	 System.out.println("---Calling Activate() method on element: " + i + " of our list of subclasses---");
	             superPowers.get(i).Activate();
	             System.out.println();
	         }
	     }
	 }
}
