using System;

//Šioje klasėje susidėsim mūsų archetipus
public static class Archetypes
{
	public static Archetype Stormer = new Archetype("Stormer",		 "Relentless "+
	"frontline breaker, who's shotgun blasts through clusters of enemy cannon"+
	" fooder, and heavy hammer crushes head of any would-be hero, who dares to"+
	" stand in his way.\nStormer is tough combatant, who's slow movement is "+
	"outweighted(quite literally) by his massive armor and high endurance. "+
	"A rookie Stormer can withstand lots of physical damage, a veteran is not"+
	" afraid of magic.", stormerDisciplines());
	
	private static Discipline[] stormerDisciplines()
	{
		//Stormer disciplines - Dreadnought, Shock and Awe, Shotgun
		Discipline[] disciplines = new Discipline[]{Disciplines.Dreadnought,
		Disciplines.ShockAndAwe, Disciplines.Shotgun};
		
		return disciplines;
	}
}

