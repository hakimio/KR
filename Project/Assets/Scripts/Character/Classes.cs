using System;

public static class Classes
{
    public static Class Stormer
    {
        get
        {
            return new Class("Stormer", "Relentless frontline breaker, who's "
            + "shotgun blasts through clusters of enemy cannon fooder, and "
            + "heavy hammer crushes head of any would-be hero, who dares to "
            + "stand in his way.\nStormer is tough combatant, who's slow "
            + "movement is outweighted(quite literally) by his massive armor "
            + "and high endurance. A rookie Stormer can withstand lots of "
            + "physical damage, a veteran is not afraid of magic.", 
            SkillTrees.StormerSkills);
        }
    }
}
