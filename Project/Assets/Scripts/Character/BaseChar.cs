using System;
using UnityEngine;
using System.Collections.Generic;

//Base Character. Iš šios klasės pasidarysim savo standartinius veikėjus.
public class BaseChar
{
    public BaseChar()
    {
        charName = "Joe Doe";
        charClass = Classes.Stormer;
        level = 1;
        exp = 0;
        weight = 70;
        height = 180;
        image = new Texture2D(0, 0);
        Items = new Inventory(new Item[0]);

        attributes = new BaseStat[Enum.GetValues(typeof(AttrNames)).Length];
        secondaryAttr = new ModifiedStat[Enum.
            GetValues(typeof(SecondaryAttrNames)).Length];

        for (int i = 0; i < attributes.Length; i++)
            attributes[i] = new BaseStat(((AttrNames)i).ToString());
        for (int i = 0; i < secondaryAttr.Length; i++)
            secondaryAttr[i] = new ModifiedStat(((SecondaryAttrNames)i).ToString(), 
                this);
        setupModifiers();
    }

    public BaseChar(string charName, int level, int exp, Class charClass, 
        Texture2D image): this()
    {
        this.charName = charName;
        this.level = level;
        this.exp = exp;
        this.charClass = charClass;
        this.image = image;

        attributes = new BaseStat[Enum.GetValues(typeof(AttrNames)).Length];
        secondaryAttr = new ModifiedStat[Enum.
            GetValues(typeof(SecondaryAttrNames)).Length];

        for (int i = 0; i < attributes.Length; i++)
            attributes[i] = new BaseStat(((AttrNames)i).ToString());
        setAttributeDescriptions();

        for (int i = 0; i < secondaryAttr.Length; i++)
            secondaryAttr[i] = new ModifiedStat(((SecondaryAttrNames)i).ToString(),
                this);
        setSecondaryAttributeDescriptions();

        setupModifiers();
    }

    public BaseChar(string charName, int level, int exp, BaseStat[] attributes,
        Class charClass, Texture2D image): this(charName, level, exp, 
        charClass, image)
    {
        this.attributes = attributes;
        setAttributeDescriptions();
        setSecondaryAttributeDescriptions();
    }

    public BaseChar(string charName, int level, int exp, BaseStat[] attributes,
        Class charClass, Texture2D image, Item[] items): this(charName, level, 
        exp, attributes, charClass, image)
    {
        this.Items = new Inventory(items);
    }

    public GameObject gameObject;
	public string charName;
	public int level;
	private int exp;
	public int weight;
	public int height;
    private Texture2D image;
	private BaseStat[] attributes;
	private ModifiedStat[] secondaryAttr;
    private Class charClass;
    public Inventory Items;
    private int lostHitPoints = 0;

    public Texture2D Image
    {
        get { return image; }
    }

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
	
    public int CurrentHP
    {
        get
        {
            return secondaryAttr[(int)SecondaryAttrNames.Hit_Points].Value 
                - lostHitPoints;
        }
    }

    public int LostHP
    {
        get
        {
            return lostHitPoints;
        }
        set
        {
            lostHitPoints = value;
            if (lostHitPoints < 0)
            {
                lostHitPoints = 0;
            }
        }
    }

	public int nextLevelExp
    {
        get
        {
            //TODO: change to proper formula
            return 220;
        }
    }

	public Class CharClass
    {
        get { return charClass; }
    }

    private void setAttributeDescriptions()
    {
        attributes[(int)AttrNames.Strength].description = "Strength is " + 
            "ability to strike hard and fast in melee. The higher Strength " +
            "the easier is to hit the enemy and greater damage will be " + 
            "inflicted.";
        attributes[(int)AttrNames.Dexterity].description = "Dexterity " + 
            "measures speed and reaction as well as ranged precision. " + 
            "Improve this score for greater defense and marksmanship.";
        attributes[(int)AttrNames.Vitality].description = "High Vitality " + 
            "results in number of Hit Points and biological resistance to " +
            "hostile enviroments and magic effects.";
        attributes[(int)AttrNames.Soulpower].description = "Soulpower is " + 
            "power to resist negative and amplify positive magic influences." +
            " Crucial to any character who depends on spellcasting.";
        attributes[(int)AttrNames.Technique].description = "Technique is " + 
            "ability to efficiently use skills and spells. Determines how " +
            "well and how often they can be used.";
    }

    private void setSecondaryAttributeDescriptions()
    {
        secondaryAttr[(int)SecondaryAttrNames.Defense].description = "Defense modifies the"
            + " chance to hit this character.";
        secondaryAttr[(int)SecondaryAttrNames.Hit_Points].description = "Hit points " + 
            "determine how much damage your character can take before dying." +
            " If you reach 0 HP or less, you are dead.";
        secondaryAttr[(int)SecondaryAttrNames.Technique_Points].description = "Technique" +
            " points determine how often character will be able to use his"+
            " special techniques.";
    }

	public BaseStat getAttr(int index)
	{
		return attributes[index];
	}
	
    public BaseStat[] getAttributes()
    {
        return attributes;
    }

    public ModifiedStat[] getSecondaryAttributes()
    {
        return secondaryAttr;
    }

	public ModifiedStat getSecondaryAttr(int index)
	{
		return secondaryAttr[index];
	}
	
	//Čia nurodysim, kaip bus skaičiuojami mūsų Antriniai atributai
	private void setupModifiers()
	{
        ModAttribute mod;
        ModifiedStat modStat;
        //HP = 60 + 3 * vitality
        modStat = getSecondaryAttr((int)SecondaryAttrNames.Hit_Points);
        modStat.baseValue = 60;
        mod = new ModAttribute(AttrNames.Vitality, 3);
        modStat.addModifyingAttribute(mod);
        //TP = 5 + 1 * Technique
        modStat = getSecondaryAttr((int)SecondaryAttrNames.Technique_Points);
        modStat.baseValue = 5;
        mod = new ModAttribute(AttrNames.Technique, 1);
        modStat.addModifyingAttribute(mod);
        //Defense = 10 + dexterity * 1
        modStat = getSecondaryAttr((int)SecondaryAttrNames.Defense);
        modStat.baseValue = 10;
        mod = new ModAttribute(AttrNames.Dexterity, 1);
        modStat.addModifyingAttribute(mod);
        //Melee attack = 30 + Strength * 0.5 + Dexterity * 0.5;
        modStat = getSecondaryAttr((int)SecondaryAttrNames.Melee_Attack);
        modStat.baseValue = 30;
        mod = new ModAttribute(AttrNames.Strength, 0.5f);
        modStat.addModifyingAttribute(mod);
        mod = new ModAttribute(AttrNames.Dexterity, 0.5f);
        modStat.addModifyingAttribute(mod);
        //Ranged attack = 10 + Dexterity * 2;
        modStat = getSecondaryAttr((int)SecondaryAttrNames.Ranged_Attack);
        modStat.baseValue = 10;
        mod = new ModAttribute(AttrNames.Dexterity, 2);
        modStat.addModifyingAttribute(mod);
        //Damage resistance = 20 + Vitality * 0.5;
        modStat = getSecondaryAttr((int)SecondaryAttrNames.Damage_Resistance);
        modStat.baseValue = 20;
        mod = new ModAttribute(AttrNames.Vitality, 0.5f);
        modStat.addModifyingAttribute(mod);
        //Magic resistance = 10 + soulpower * 1 + vitality * 0.5;
        modStat = getSecondaryAttr((int)SecondaryAttrNames.Magic_Resistance);
        modStat.baseValue = 10;
        mod = new ModAttribute(AttrNames.Soulpower, 1);
        modStat.addModifyingAttribute(mod);
        mod = new ModAttribute(AttrNames.Vitality, 0.5f);
        modStat.addModifyingAttribute(mod);
        //Movement speed = 4 + dexterity * 0.04;
        modStat = getSecondaryAttr((int)SecondaryAttrNames.Movement_Speed);
        modStat.baseValue = 4;
        mod = new ModAttribute(AttrNames.Dexterity, 0.04f);
        modStat.addModifyingAttribute(mod);
	}
}

public struct Inventory
{
    public List<Item> Bag;
    public Item Slot1, Slot2, Armor;
    public ItemSlots ActiveSlot;

    public Inventory(Item[] itemsInBag): this()
    {
        Bag = new List<Item>();
        Slot1 = null;
        Slot2 = null;
        Armor = null;
        ActiveSlot = ItemSlots.Slot1;
        Bag.AddRange(itemsInBag);
    }

    public Inventory(Item[] itemsInBag, Item slot1, Item slot2, Item armor)
        : this(itemsInBag)
    {
        this.Slot1 = slot1;
        this.Slot2 = slot2;
        this.Armor = armor;
    }
}

public enum ItemSlots
{
    Slot1,
    Slot2
}