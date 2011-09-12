using System.Collections.Generic;
using System;
using UnityEngine;

//Antrinių atributų klasė
public class ModifiedStat: BaseStat
{
	//Pirminių atributų sąrašėlis, nuo kurių priklauso šio atributo vertė
	private List<ModAttribute> modifyingAttributes;
    private BaseChar myChar;
	
	public ModifiedStat(string name, BaseChar character): base(name) 
    {
        myChar = character;
        modifyingAttributes = new List<ModAttribute>();
    }

	public static int calcModValue(int attrValue)
	{
		return (int)Math.Floor((double)((attrValue - 10) / 2));
	}
	
	public new int Value
	{
		get
		{
			float statValue = baseValue + gainedValue;

            foreach (ModAttribute modAttr in modifyingAttributes)
                statValue += myChar.getAttr((int)modAttr.attrName).Value * 
                    modAttr.modifier;
            
            //if ((int)statValue%2 == 0)
            //    statValue += 0.1f;

            //statValue = (float)Math.Round(statValue);
			return (int)statValue;
		}
	}

    public void addModifyingAttributes(ModAttribute[] modAttributes)
    {
        modifyingAttributes.AddRange(modAttributes);
    }
	
	public void addModifyingAttribute(ModAttribute modAttribute)
	{
        modifyingAttributes.Add(modAttribute);
	}
}

public struct ModAttribute
{
    public AttrNames attrName;
    public float modifier;

    public ModAttribute(AttrNames attrName, float modifier)
    {
        this.attrName = attrName;
        this.modifier = modifier;
    }
}
