

public class Archetype
{
	private string name;
	private string description;
	private Discipline[] disciplines;
	
	public Archetype(string name, string description, Discipline[] disciplines)
	{
		this.name = name;
		this.description = description;
		this.disciplines = disciplines;
	}
	
	public Discipline[] getDisciplines()
	{
		return disciplines;
	}
	
	public Discipline getDiscipline(int index)
	{
		return disciplines[index];
	}
	
	public string getName()
	{
		return name;
	}
	
	public string getDescription()
	{
		return description;
	}
}
