/* Simulate a collection of independent objects by telling each to process one frame of behavior at a time. */

public class UpdateMethod 
{
	private String enemy1;
	private String enemy2;
	private String enemy3;
	private int skeletonSprinterFrames = 0;
	private int skeletonArcherFrames = 0;
	private int wizardFrames = 0;
	private double alteredTime = 1000; //Only needed to slow things down for displaying when printing
	
	//-----------------Constructors-----------------
	public UpdateMethod(String enemy1, String enemy2, String enemy3){
		initializeEnemies(enemy1, enemy2, enemy3);
	}

	public UpdateMethod() {
		
	}

	//Our update loop. Events will update at independent rates.
	public void update(){
		while(true){
			try {
				if(++skeletonSprinterFrames >= 100 * alteredTime){
					skeletonSprinterFrames = 0;
					//Perform actions here
					performAction(enemy1);
					Thread.sleep(1000);//Sleep for 1 second (otherwise you will not be able to read what is happening)
				}
				if(++skeletonArcherFrames >= 300 * alteredTime){
					skeletonArcherFrames = 0;
					//Perform actions here
					performAction(enemy2);
					Thread.sleep(1000); //Sleep for 1 second
				}
				if(++wizardFrames >= 400 * alteredTime){
					wizardFrames = 0;
					//Perform actions here
					performAction(enemy3);
					Thread.sleep(1000); //Sleep for 1 second
				}
			} 
			catch (InterruptedException e) {
				e.printStackTrace();
			}
		}
		
	}
	
	private void initializeEnemies(String enemy1, String enemy2, String enemy3){
		this.enemy1 = enemy1; 
		this.enemy2 = enemy2;
		this.enemy3 = enemy3;
		update();
	}
	
	private void performAction(String enemyCharacter){
		switch(enemyCharacter){
		case "enemyWizard":
			System.out.println("An enemy wizard casts lightning bolt");
			break;
		case "skeletonSprinter":
			System.out.println("Skeleton sprinter runs towards you");
			break;
		case "skeletonArcher":
			System.out.println("Skeleton archer fires a flaming arrow");
			break;
		}
	}
	//Handle user input and render game here
}
