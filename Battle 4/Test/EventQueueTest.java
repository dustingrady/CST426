
public class EventQueueTest {
	public static void main (String args[]) throws InterruptedException{
		EventQueue eventQueue = new EventQueue();
		System.out.println("---Populating our queue with actions---");
		Thread.sleep(2000);
		System.out.println("---Calling queue to retreive actions---");
		eventQueue.attack();
		eventQueue.taunt();
		eventQueue.attack();
		eventQueue.attack();
		eventQueue.heal();
		eventQueue.run();
		eventQueue.displayInfo();
	}
}
