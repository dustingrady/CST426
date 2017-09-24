/* Decouple when a message or event is sent from when it is processed. */

import java.util.LinkedList;
import java.util.Queue;

public class EventQueue {
	private Queue<String> eventsQueue = new LinkedList<>();
	
	//Constructor
	public EventQueue(){

	}
	
	public void displayInfo() throws InterruptedException{
		for(String s : eventsQueue){
			Thread.sleep(1000);
			System.out.println(s.toString());
		}
	}
	
	public void attack(){
		//Other actions could be added here
		eventsQueue.add("Enemy attacks you.");
	}
	public void run(){
		//Other actions could be added here
		eventsQueue.add("Enemy runs away from you.");
	}
	public void taunt(){
		//Other actions could be added here
		eventsQueue.add("Enemy taunts you.");
	}
	public void heal(){
		//Other actions could be added here
		eventsQueue.add("Enemy heals themself.");
	}
}
