public class BaseStat
{
	public int baseValue;
	public int gainedValue;
    public string description;
	private string name;
	
	public BaseStat(string name)
	{
		this.name = name;
        description = "";
		baseValue   = 1;
		gainedValue = 0;
	}

    public BaseStat(string name, int baseValue): this(name)
    {
        this.baseValue = baseValue;
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
