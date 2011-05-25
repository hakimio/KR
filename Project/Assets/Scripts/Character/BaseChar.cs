using UnityEngine;
using System.Collections;
using System;

//Base Character. Iš šios klasės pasidarysim savo standartinius veikėjus.
public class BaseChar : MonoBehaviour 
{
	//negalėjau naudot tiesiog name, nes MonoBehaviour klasėje name jau yra
	public string charName;
	public int level;
	private int exp;
	public int weight;
	public int height;
	//pirminiai atributai
	private BaseStat[] attributes;
	//HP, AC, TP
	private ModifiedStat[] secondaryAttr;
	private Archetype archetype;
	
	public int Exp
	{
		get {return exp;}
	}
	
	public void addExp(int exp)
	{
		this.exp += exp;
		
		calcLevel();
	}
	
	private void calcLevel()
	{
		//TODO
	}
		
	public Archetype Archetype
	{
		get {return archetype; }
		set {archetype = value; }
	}
	
	//Klasėse išvestose iš MonoBehaviour, Awake naudojamas kaip konstruktorius
	public void Awake()
	{
		charName = "Joe Doe";
		archetype = Archetypes.Stormer;
		level = 1;
		exp = 0;
		weight = 70;
		height = 180;
		
		attributes = new BaseStat[Enum.GetValues(typeof(AttrNames)).Length];
		secondaryAttr = new ModifiedStat[Enum.GetValues(typeof(SecondaryAttrNames)).Length];
		
		for (int i = 0; i < attributes.Length; i++)
			attributes[i] = new BaseStat(((AttrNames)i).ToString());
		for (int i = 0; i < secondaryAttr.Length; i++)
			secondaryAttr[i] = new ModifiedStat(((SecondaryAttrNames)i).ToString(),this);
		setupModifiers();
	}
	
	public BaseStat getAttr(int index)
	{
		return attributes[index];
	}
	
	public ModifiedStat getSecondaryAttr(int index)
	{
		return secondaryAttr[index];
	}
	
	//Čia nurodysim, kaip bus skaičiuojami mūsų Antriniai atributai
	private void setupModifiers()
	{
		//HP = 12 + constModifier + (level - 1)*(6+constModifier)
		ModifiedStat hp = getSecondaryAttr((int)SecondaryAttrNames.HP);
		hp.baseValue = 12;
		hp.setModifyingAttr(getAttr((int)AttrNames.Constitution));
		hp.levelBonus = 6;
		//AC = 10 + dexModifier + armorMod
		ModifiedStat ac = getSecondaryAttr((int)SecondaryAttrNames.AC);
		ac.baseValue = 10;
		ac.setModifyingAttr(getAttr((int)AttrNames.Dexterity));
		ac.changesAfterLevelUp = false;
		//TP = 3 + techMod + (level - 1) * (1 + techMod) 
		ModifiedStat tp = getSecondaryAttr((int)SecondaryAttrNames.TP);
		tp.baseValue = 3;
		tp.setModifyingAttr(getAttr((int)AttrNames.Technique));
		tp.levelBonus = 1;
	}
	
	//Pasikeitus pirminių atributų vertėm, perskaičiuojam antrinius
	/*public void attrUpdate()
	{
		for (int i = 0; i < secondaryAttr.Length; i++)
			secondaryAttr[i].update();
	}*/
}
