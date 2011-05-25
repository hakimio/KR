using System;

//Šioje klasėje saugosim mūsų jau apsirašytas disciplinas
public static class Disciplines
{
	public static Discipline Dreadnought = new Discipline("Dreadnought", 
	"A stormer, disciplined in this tactic becomes an unstopable battlefield"+
	" horror, who's inhuman stamina strikes fear into any foe, stupid enough"+
	" not to cease firing and run.", setupDreadnoughtFeats());
	
	public static Discipline ShockAndAwe = new Discipline("Shock and Awe", 
	"High endurance means nothing when you are not able to hurt your enemy..."+
	" badly. This discipline focuses on use of warhammer vs. the toughest of "+
	"enemies.", setupShockAndAweFeats());
	
	public static Discipline Shotgun = new Discipline("Shotgun", "Shotgun is "+
	"excellent solution for dispatching swarms of weaker enemies and slowly "+ 
	"advancing towards the goal.\nAmmo specialization allows to enhance "+
	"weapon properties, but it cannot be used with shooting techniques and "+
	"ammo effects do not stack with each other.", setupShotgunFeats());
	
	enum dreadnoughtFeatNames
	{
		Die_Hard,
		Fireproof,
		Grounding,
		Thick_Skin,
		Antibodies,
		
		Iron_Skin,
		Magic_Resistance,
		
		Armor_Mastery,
		Capace
	}
	
	enum shockAndAweFeatNames
	{
		Bone_Breaker,
		Heavy_Hand,
		Shell_Crusher,
		Rule_of_Iron,
		Little_Thor,
		
		Rams_Kiss,
		Wheel_of_Fortune,
		Rise_and_Fall,
		
		Twin_Cannon,
		Scatter_shot,
		Escalation,
		Show_Me_The_Bodies
	}
	
	enum shotgunFeatNames
	{
		AP_shells,
		Incinerator_Shells,
		Nitrogen_Shells
	}
	
	private static Feat[] setupDreadnoughtFeats()
	{
		int nrOfFeats = Enum.GetValues(typeof(dreadnoughtFeatNames)).Length;
		string name;
		Feat[] feats = new Feat[nrOfFeats];
		for (int i = 0; i < nrOfFeats; i++)
		{
			name = Enum.GetNames(typeof(dreadnoughtFeatNames))[i].ToString().Replace('_',' ');
			feats[i] = new Feat(name,"");
		}
		//Nuo 2-o iki 4-o (imtinai) feat'ų (žr. į Enumą) dependency bus DieHard
		for (int i = 1; i < 5; i++)
			feats[i].addDependency(feats[(int)dreadnoughtFeatNames.Die_Hard]);
		
		//DieHard description
		feats[(int)dreadnoughtFeatNames.Die_Hard].setDescription("This tactic "+
		"focuses on surviving long enough to reach the enemy and start the "+
		"butcher's work. 2 Hp/Lv.");
		//IronSkin description
		feats[(int)dreadnoughtFeatNames.Iron_Skin].setDescription("Even thickest"+
		" armor cannot always protect from lucky hit, thus Stormer's body learned"+
		" to withstand some portion of punishment. DR 3/-.");
		//ArmorMastery description
		feats[(int)dreadnoughtFeatNames.Armor_Mastery].setDescription("Armor becomes"+
		" virtually second skin, thus granting permanent +2 AC bonus, when wearing" +
		" Medium or Heavy armor.");
		
		//MagicResistance dependency - IronSkin
		feats[(int)dreadnoughtFeatNames.Magic_Resistance].
			addDependency(feats[(int)dreadnoughtFeatNames.Iron_Skin]);
		//MagicResistance 2-as dependency - DieHard
		feats[(int)dreadnoughtFeatNames.Magic_Resistance].
			addDependency(feats[(int)dreadnoughtFeatNames.Die_Hard]);
		//Capace dependency - ArmorMastery
		feats[(int)dreadnoughtFeatNames.Capace].
			addDependency(feats[(int)dreadnoughtFeatNames.Armor_Mastery]);
		
		return feats;
	}
	
	private static Feat[] setupShockAndAweFeats()
	{
		int nrOfFeats = Enum.GetValues(typeof(shockAndAweFeatNames)).Length;
		string name;
		Feat[] feats = new Feat[nrOfFeats];
		for (int i = 0; i < nrOfFeats; i++)
		{
			name = Enum.GetNames(typeof(shockAndAweFeatNames))[i].ToString().Replace('_',' ');
			feats[i] = new Feat(name,"");
		}
		
		feats[(int)shockAndAweFeatNames.Rams_Kiss].setName("Ram's Kiss");
		//Placeholder iki tol, kol prieis rankos sudėliot normalius dependencies
		//TODO: setup REAL dependencies
		for (int i = 1; i < 5; i++)
			feats[i].addDependency(feats[(int)shockAndAweFeatNames.Bone_Breaker]);
		
		for (int i = 6; i < 8; i++)
			feats[i].addDependency(feats[(int)shockAndAweFeatNames.Rams_Kiss]);

		for (int i = 9; i < 12; i++)
			feats[i].addDependency(feats[(int)shockAndAweFeatNames.Twin_Cannon]);
		
		//BoneBreaker description
		feats[(int)shockAndAweFeatNames.Bone_Breaker].setDescription("Heavy swing"+
		" with nothing held back. Weapon damage x2, Str damage x2. Str damage "+
		"becomes x3, after learning Heavy Hand Talent.");
		//RamsKiss description
		feats[(int)shockAndAweFeatNames.Rams_Kiss].setDescription("Strike with"+
		" haft into the face. Stuns enemy for a short time.");
		//TwinCannon description
		feats[(int)shockAndAweFeatNames.Twin_Cannon].setDescription("Double tap "+
		"makes fiery burst at short range, damaging those few losers caught in the"+
		" blast.\n\nNormal Damage vs 1-3 nearby enemies.");

		return feats;
	}
	
	private static Feat[] setupShotgunFeats()
	{
		int nrOfFeats = Enum.GetValues(typeof(shotgunFeatNames)).Length;
		Feat[] feats = new Feat[nrOfFeats];
		string name;
		for (int i = 0; i < nrOfFeats; i++)
		{
			name = Enum.GetNames(typeof(shotgunFeatNames))[i].ToString().Replace('_', ' ');
			feats[i] = new Feat(name,"");
		}
		//APshells description
		feats[(int)shotgunFeatNames.AP_shells].setDescription("These shells focus "+
		"energy to penetrate any conventional armor.\n\nEffect: Ignore half of AC.");
		//IncineratorShells description
		feats[(int)shotgunFeatNames.Incinerator_Shells].setDescription("Fire element"+
		" burns enemies badly, giving them penalty on attacks for short time.\n\n"+
		"Effect: +1d6 cold damage,  -2 penalty on attack for short time.");
		//NitrogenShells description
		feats[(int)shotgunFeatNames.Nitrogen_Shells].setDescription("Ice element"+
		" damages and freezes enemies.\n\nEffect: +1d6 cold damage and enemy "+
		"AC-2 for short time.");
		
		return feats;
	}
}

