
//Pasinaudodami šia klase apsirašysim mūsų disciplinas
public class Discipline
{
	private string name;
	private string description;
	private Feat[] feats;
	public bool known;
	
	public Discipline (string name, string description, Feat[] feats)
	{
		this.name = name;
		this.description = description;
		this.feats = feats;
	}
	
	public string getName()
	{
		return name;
	}

	public string getDescription()
	{
		return description;
	}
	public Feat[] getFeats()
	{
		return feats;
	}
	
	public Feat getFeat(int index)
	{
		return feats[index];
	}
}

