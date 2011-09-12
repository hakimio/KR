using System;

//Base Character. Iš šios klasės pasidarysim savo standartinius veikėjus.
public class BaseChar
{
    public BaseChar()
    {
        charName = "Joe Doe";
        archetype = Archetypes.Stormer;
        level = 1;
        exp = 0;
        weight = 70;
        height = 180;

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

    public BaseChar(string charName, int level, int exp, Archetype archetype): 
        this()
    {
        this.charName = charName;
        this.level = level;
        this.exp = exp;
        this.archetype = archetype;

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
        Archetype archetype) : this(charName, level, exp, archetype)
    {
        this.attributes = attributes;
        setAttributeDescriptions();
        setSecondaryAttributeDescriptions();
    }

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

    private void setAttributeDescriptions()
    {
        attributes[(int)AttrNames.Strength].description = "Raw physical strength. A high" +
            " strength is important for physical characters, because it helps" +
            " them prevail in combat.";
        attributes[(int)AttrNames.Dexterity].description = "Dexterity measures hand-eye" +
            " coordination, agility, reflexes, and balance. This ability is " +
            "important for characters who typically wear light or medium armor.";
        attributes[(int)AttrNames.Vitality].description = "Vitality represents " +
            "your character's health and stamina.";
        attributes[(int)AttrNames.Technique].description = "Technique determines resistance," +
            " mana and the amount of damage done by magical attacks.";
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
        modStat = getSecondaryAttr((int)SecondaryAttrNames.Movement_speed);
        modStat.baseValue = 4;
        mod = new ModAttribute(AttrNames.Dexterity, 0.04f);
        modStat.addModifyingAttribute(mod);
	}
}
