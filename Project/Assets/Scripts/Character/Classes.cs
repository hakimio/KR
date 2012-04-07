using System;

public static class Classes
{
    public static Class Stormer
    {
        get
        {
            return new Class("Stormer", "Stormers are elite vanguard of  " + 
            "attacking army. They are tough, strong and well armored, " + 
            "trained to break any defensive line at any cost. Since on the " +
            "battlefield they dance toe to toe with enemy, they are experts " +
            "with all close range weapons.", SkillTrees.StormerSkills);
        }
    }

    public static Class Sniper
    {
        get
        {
            return new Class("Sniper", "Deadly at far range, elusive when " +
            "cornered, snipers are trained to efficiently thin enemy ranks " +
            "and break morale. Although not so efficent at close quaters " + 
            "combat, they should not be underestimated - sniper's knife " +
            "killed many foes.", SkillTrees.StormerSkills);
        }
    }

    public static Class GunSlinger
    {
        get
        {
            return new Class("Gunslinger", "As saying goes: \"One thing " +
            "worse than a charging stormer is a gunslinger at your flank\". " + 
            "Popular among allies and fiercely hated among enemies, " +
            "gunslingers are the most tenacious adversaries, who employ " + 
            "high mobility and and lots of bullets at any range.", 
            SkillTrees.StormerSkills);
        }
    }
}
