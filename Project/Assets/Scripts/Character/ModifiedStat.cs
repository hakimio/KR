using System.Collections.Generic;
using System;

//Antrinių atributų klasė
public class ModifiedStat: BaseStat
{
	//Pirminių atributų sąrašėlis, nuo kurių priklauso šio atributo vertė
	private BaseStat modifyingAttr;
	//ar vertė priklauso nuo lygio
	public bool changesAfterLevelUp;
	private BaseChar myChar;
	//added after leveling up
	public int levelBonus;
	
	public ModifiedStat(string name, BaseChar myChar): base(name)
	{
		changesAfterLevelUp = true;
		this.myChar = myChar;
	}

	public static int calcModValue(int attrValue)
	{
		/*double mod;
		
		if (attrValue%2 == 0)
			attrValue++;
		mod = ((attrValue - 13) / 2) + 1;*/
				
		return (int)Math.Floor((double)((attrValue - 10) / 2));
	}
	
	public new int Value
	{
		get
		{
			int statValue = baseValue + gainedValue;
			int mod = calcModValue(modifyingAttr.Value);
			statValue += mod;
			
			if (changesAfterLevelUp)
				statValue += (myChar.level - 1) * (levelBonus + mod);
			return statValue;
		}
	}
	
	//Metodas, kurio pagalba nurodom modifikuojantį atributą
	public void setModifyingAttr(BaseStat attr)
	{
		modifyingAttr = attr;
	}
}
