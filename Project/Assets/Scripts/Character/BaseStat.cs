//Bazinė attributų klasė
public class BaseStat
{
	//bazinė vertė atributo vertė
	public int baseValue;
	//įgyjama vertė (feats, Archetype, items, potions ir pan gali pridet kažkiek)
	public int gainedValue;
	private string name;
	
	public BaseStat(string name)
	{
		this.name = name;
		baseValue   = 1;
		gainedValue = 0;
	}
	
	public int Value
	{
		get {return baseValue + gainedValue;}
	}
	
	public string Name
	{
		get {return name;}
	}
}
