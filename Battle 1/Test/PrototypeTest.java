import java.util.List;

public class PrototypeTest {

	public static void main(String[] args) throws CloneNotSupportedException {
		Prototype character = new Prototype();
		character.loadCharacterData();
		
		Prototype protoNew1 = (Prototype) character.clone(); //New prototype object1
		Prototype protoNew2 = (Prototype) character.clone(); //New prototype object2
		List<String> list1 = protoNew1.getCharacterList();
		list1.set(0, "Class: Fire Wizard");
		list1.add("Resistance: Fire +20");
		List<String> list2 = protoNew2.getCharacterList();
		list2.remove("Class: Wizard");
		list2.add(0, "Class: Warlock");
		
		
		System.out.println("-----Displaying 'base' character attributes-----");
		System.out.println("Character attributes: " + character.getCharacterList() + "\n");
		System.out.println("-----Displaying attributes of new 'stamped' (clone) character-----");
		System.out.println("New character attributes: " + list1 + "\n");
		System.out.println("-----Displaying attributes of another 'stamped' (cloned) character with different attributes-----");
		System.out.println("Another new characters attributes: " + list2);
	}

}