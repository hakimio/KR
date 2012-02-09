using System;
using System.Collections.Generic;

public class Skill
{//TODO: overrides/replaces, mode, -standalone
    private string name, description;
    private string[] gunNames, dependencies, replaces;
    private Mode mode;
    public bool Known;
    private int rank, maxRank;
    private Dictionary<BonusType, float> bonuses;

    public Skill(string name, string description, string[] gunNames,
        string[] dependencies, string[] replaces, Mode mode, int maxRank, 
        Dictionary<BonusType, float> bonuses)
    {
        this.name = name;
        this.description = description;
        this.gunNames = gunNames;
        this.dependencies = dependencies;
        this.replaces = replaces;
        this.mode = mode;
        Known = false;
        this.bonuses = bonuses;
        this.rank = 0;
        this.maxRank = maxRank;
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
    }

    public string[] GunNames
    {
        get
        {
            return gunNames;
        }
    }

    public string[] Dependencies
    {
        get
        {
            return dependencies;
        }
    }

    public Mode Mode
    {
        get
        {
            return mode;
        }
    }

    public string[] Replaces
    {
        get
        {
            return replaces;
        }
    }

    public Dictionary<BonusType, float> Bonuses
    {
        get
        {
            return bonuses;
        }
        set
        {
            bonuses = value;
        }
    }

    public int MaxRank
    {
        get
        {
            return maxRank;
        }
    }

    public int Rank
    {
        get
        {
            return rank;
        }
        set
        {
            rank = value;
        }
    }
}

public enum BonusType
{
    Damage,
    Attack,
    Distance,
    nrOfHits,
    DamageMultiplier
}

public enum Mode
{
    All,
    MultiHit,
    MultiGun,
    MultiGunAndHit
}