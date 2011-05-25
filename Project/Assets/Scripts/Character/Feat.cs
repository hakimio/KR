using System;
using System.Collections.Generic;

//Šios klasės dėka apsirašysim savo feat'us
public class Feat
{
	private string name;
	private string description;
	public bool known;
	//Feat'ai, kuriuos iš pradžių reiks gauti, norint gauti šį feat'ą
	private List<Feat> dependencies;
	//Feat'ai gali keisti pirminius arba antrinius atributus.
	//Šiame sąraše saugosim tuos pakeitimus.
	private List<BaseStat> mods;
	
	public Feat (string name, string description)
	{
		this.name = name;
		this.description = description;
		dependencies = new List<Feat>();
		mods = new List<BaseStat>();
	}
	
	public Feat (string name, string description, Feat[] dependencies):
		this(name,description)
	{
		this.dependencies.AddRange(dependencies);
	}
	
	public Feat (string name, string description, BaseStat[] mods):
		this(name,description)
	{
		this.mods.AddRange(mods);
	}
	
	public Feat(string name, string description, BaseStat[] mods, 
	            Feat[] dependencies): this(name, description, mods)
	{
		this.dependencies.AddRange(dependencies);
	}
	
	public void addDependency(Feat dependency)
	{
		dependencies.Add(dependency);
	}
	
	public void addMod(BaseStat mod)
	{
		mods.Add(mod);
	}
	
	public BaseStat[] getModifyingAttr()
	{
		return mods.ToArray();
	}
	
	public Feat[] getDependencies()
	{
		return dependencies.ToArray();
	}
	
	public string getName ()
	{
		return name;
	}
	
	public string getDescription()
	{
		return description;
	}
	
	public void setName(string name)
	{
		this.name = name;
	}
	
	public void setDescription(string description)
	{
		this.description = description;
	}
}

