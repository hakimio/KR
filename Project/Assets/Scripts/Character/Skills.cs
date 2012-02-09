using System;
using System.Collections.Generic;

public static class Skills
{
    #region Stormer skills
    private static Skill knifeTraining = null;
    public static Skill KnifeTraining
    {
        get
        {
            if (knifeTraining != null)
                return knifeTraining;

            string[] gunNames = new string[] { "Combat Knife" };
            Dictionary<BonusType, float> bonuses;
            bonuses = new Dictionary<BonusType, float>();
            bonuses.Add(BonusType.Attack, 5);
            bonuses.Add(BonusType.DamageMultiplier, 0.1f);

            knifeTraining = new Skill("Knife Training", "Attack +5, Damage" +
                " +10%", gunNames, new string[0], new string[0], Mode.All, 3, 
                bonuses);
            return knifeTraining;
        }
    }

    private static Skill knifeExpert = null;
    public static Skill KnifeExpert
    {
        get
        {
            if (knifeExpert != null)
                return knifeExpert;

            string[] gunNames = new string[] { "Combat Knife" };
            string[] dependencies = new string[] { "Knife Training" };
            Dictionary<BonusType, float> bonuses;
            bonuses = new Dictionary<BonusType, float>();
            bonuses.Add(BonusType.Attack, -20);
            bonuses.Add(BonusType.nrOfHits, 2);

            knifeExpert = new Skill("Knife Expert", "Attack with knife twice" +
                " with -20 attack penalty.", gunNames, dependencies, 
                new string[0], Mode.MultiHit, 1, bonuses);
            return knifeExpert;
        }
    }

    private static Skill knifeMaster = null;
    public static Skill KnifeMaster
    {
        get
        {
            if (knifeMaster != null)
                return knifeMaster;

            string[] gunNames = new string[] { "Combat Knife" };
            string[] dependencies = new string[] { "Knife Expert" };
            string[] replaces = { "Knife Expert" };
            Dictionary<BonusType, float> bonuses;
            bonuses = new Dictionary<BonusType, float>();
            bonuses.Add(BonusType.Attack, -30);
            bonuses.Add(BonusType.nrOfHits, 3);

            knifeMaster = new Skill("Knife Master", "Attack with knife three" +
                " times with -30 attack penalty.", gunNames, dependencies, 
                replaces, Mode.MultiHit, 1, bonuses);
            return knifeMaster;
        }
    }

    private static Skill deftHands = null;
    public static Skill DeftHands
    {
        get
        {
            if (deftHands != null)
                return deftHands;

            string[] gunNames = new string[] { "Combat Knife" };
            string[] dependencies = new string[] { "Knife Training" };
            Dictionary<BonusType, float> bonuses;
            bonuses = new Dictionary<BonusType, float>();
            bonuses.Add(BonusType.Attack, -10);
            bonuses.Add(BonusType.DamageMultiplier, 1);

            deftHands = new Skill("Deft Hands", "Attack with two knives at " + 
                "once. Attack -10, double the damage.", gunNames, dependencies,
                new string[0], Mode.MultiGun, 1, bonuses);
            return deftHands;
        }
    }

    private static Skill hermesStyle = null;
    public static Skill HermesStyle
    {
        get
        {
            if (hermesStyle != null)
            {
                /*if (knifeMaster != null && knifeMaster.Known)
                {
                    hermesStyle.Bonuses[BonusType.Attack] = -30;
                    hermesStyle.Bonuses[BonusType.nrOfHits] = 3;
                }*/
                    
                return hermesStyle;
            }

            string[] gunNames = new string[] { "Combat Knife" };
            string[] dependencies = { "Knife Expert", "Deft Hands" };
            string[] replaces = { "Knife Expert", "Knife Master", 
                                    "Deft Hands" };
            Dictionary<BonusType, float> bonuses;
            bonuses = new Dictionary<BonusType, float>();
            bonuses.Add(BonusType.DamageMultiplier, 1);
            bonuses.Add(BonusType.Attack, -20);
            bonuses.Add(BonusType.nrOfHits, 2);

            hermesStyle = new Skill("Hermes Style", "Combine Knive Expert or" +
                " Master skills with two knife fighting.", gunNames, 
                dependencies, replaces, Mode.MultiGunAndHit, 1, bonuses);
            return hermesStyle;
        }
    }

    private static Skill viperStyle = null;
    public static Skill ViperStyle
    {
        get
        {
            if (viperStyle != null)
                return viperStyle;
            string[] gunNames = new string[] { "Combat Knife" };
            string[] dependencies = new string[] { "Knife Expert" };
            Dictionary<BonusType, float> bonuses;
            bonuses = new Dictionary<BonusType, float>();
            bonuses.Add(BonusType.Attack, 10);

            viperStyle = new Skill("Viper Style", "Reduce Knife Expert and " +
                "Knife Master penalties by 10 when fighting with one knife.",
                gunNames, dependencies, new string[0], Mode.MultiHit, 1, 
                bonuses);
            return viperStyle;
        }
    }

    private static Skill stormer = null;
    public static Skill Stormer
    {
        get
        {
            if (stormer != null)
                return stormer;
            string[] gunNames = new string[] { "Shotgun" };
            Dictionary<BonusType, float> bonuses;
            bonuses = new Dictionary<BonusType, float>();
            bonuses.Add(BonusType.Attack, 5);
            bonuses.Add(BonusType.Damage, 5);
            stormer = new Skill("Stormer", "Shotgun Attack and Damage +5.",
                gunNames, new string[0], new string[0], Mode.All, 3, bonuses);
            return stormer;
        }
    }

    private static Skill kissTwice = null;
    public static Skill KissTwice
    {
        get
        {
            if (kissTwice != null)
                return kissTwice;
            string[] gunNames = new string[] { "Shotgun" };
            string[] dependencies = new string[] { "Stormer" };
            Dictionary<BonusType, float> bonuses;
            bonuses = new Dictionary<BonusType, float>();
            bonuses.Add(BonusType.DamageMultiplier, 0.5f);

            kissTwice = new Skill("Kiss Twice", "Shoot 2 bullets at once " +
                "increasing weapon damage by 50%", gunNames, dependencies, 
                new string[0], Mode.All, 1, bonuses);
            return kissTwice;
        }
    }

    private static Skill aim = null;
    public static Skill Aim
    {
        get
        {
            if (aim != null)
                return aim;
            string[] gunNames = new string[] { "Shotgun" };
            string[] dependencies = new string[] { "Stormer" };
            Dictionary<BonusType, float> bonuses;
            bonuses = new Dictionary<BonusType, float>();
            bonuses.Add(BonusType.Distance, 2);

            aim = new Skill("Aim", "Increase shotgun's distance by 2 hexes.",
                gunNames, dependencies, new string[0], Mode.All, 2, bonuses);
            return aim;
        }
    }
    #endregion
}
