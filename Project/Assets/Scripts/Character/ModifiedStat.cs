using System.Collections.Generic;
using System;
using UnityEngine;

public class ModifiedStat: BaseStat
{
	private List<ModAttribute> modifyingAttributes;
    private BaseChar myChar;
    private int value = -1;
	
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
            return value;
		}
	}

    public void calcValue()
    {
        float statValue = baseValue + gainedValue;

        foreach (ModAttribute modAttr in modifyingAttributes)
            statValue += myChar.getAttr((int)modAttr.attrName).Value *
                modAttr.modifier;
        value = (int)statValue;
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
