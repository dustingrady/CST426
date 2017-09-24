/*Allow the flexible creation of new “classes” by creating a single class, each instance of which represents a different type of object.*/
public class TypeObject {

	//This is our base class
	public abstract class Superpower
	{        
	    //This is our main "blue print" method that a subclass has to have its own version of
	    public abstract void Activate();

	    //All of the operations a derived class needs to perform - called from Activate()
	    protected void Move(float speed)
	    {
	        System.out.println("Moving with speed " + speed);
	    }

	    protected void PlaySound(String coolSound)
	    {
	    	System.out.println("Playing sound " + coolSound);
	    }

	    protected void SpawnParticles()
	    {
	    	System.out.println("You spawned some particles, cool");
	    }
	}

	//Subclasses
	public class SkyLaunch extends Superpower //Inherit from Superpower
	{
	    //Has its own version of Activate()
	    public void Activate()
	    {
	        //Add operations this class has to perform
	    	Move(10f);
	        PlaySound("SkyLaunch");
	        SpawnParticles();
	    }
	}

	public class GroundDive extends Superpower //Inherit from Superpower
	{
	    //Has its own version of Activate()
	    public void Activate()
	    {
	        //Add operations this class has to perform
	        Move(15f);
	        PlaySound("GroundDive");
	        SpawnParticles();
	    }
	}
}	
