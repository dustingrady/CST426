import java.util.ArrayList;
import java.util.List;

public class Prototype implements Cloneable{
	private List<String> characterAttributes; //This list will contain common character attributes that we may want to change slightly for new character types
	
	public Prototype(){
		characterAttributes = new ArrayList<String>();
	}
	
	public Prototype(List<String> list){ //Constructor
		this.characterAttributes=list;
	}
	public void loadCharacterData(){
		//Add character attributes for main char template (stamp)
		characterAttributes.add("Class: Wizard");
		characterAttributes.add("Strength: 20");
		characterAttributes.add("Intellegence: 90");
		characterAttributes.add("HP: 100");
	}
	
	public List<String> getCharacterList() {
		return characterAttributes;
	}

	//Our clone function to "stamp" out copies similar to the original
	public Object clone() throws CloneNotSupportedException{
		List<String> temp = new ArrayList<String>(); //Temporary list which will be passed to constructor
		for(String str : this.getCharacterList()){ //Add all str elements from character list into a temp list to be cloned
			temp.add(str);
		}
		return new Prototype(temp); //Pass to constructor
	}
	
}