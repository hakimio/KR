using System;
using System.Collections.Generic;
using UnityEngine;

public static class SkillTrees
{
    const int yOffset = 120;
    const int xOffset = 100;
    const int btnXDist = 10; //30
    const int btnYDist = 60;
    public const int btnWidth = 80; //120
    public const int btnHeight = 50; //60

    private static SkillTree stormerSkills = null;

    public static SkillTree StormerSkills
    {
        get
        {
            if (stormerSkills != null)
                return stormerSkills;

            Dictionary<string, Skill> skills;
            Dictionary<Skill, Vector2> skillPos;
            skills = new Dictionary<string,Skill>();
            skillPos = new Dictionary<Skill,Vector2>();

            skillPos.Add(Skills.ViperStyle, new Vector2(xOffset, yOffset 
                + 2 * btnHeight + 2 * btnYDist));
            skillPos.Add(Skills.KnifeMaster, new Vector2(xOffset + btnWidth
                + btnXDist, skillPos[Skills.ViperStyle].y));
            skillPos.Add(Skills.HermesStyle, new Vector2(xOffset + 2 * btnWidth
                + 2 * btnXDist, skillPos[Skills.ViperStyle].y));

            skillPos.Add(Skills.KnifeExpert, 
                new Vector2(skillPos[Skills.KnifeMaster].x, yOffset + btnHeight 
                    + btnYDist));
            skillPos.Add(Skills.DeftHands, 
                new Vector2(skillPos[Skills.KnifeExpert].x + btnXDist + 
                    btnWidth, skillPos[Skills.KnifeExpert].y));
            skillPos.Add(Skills.KissTwice,
                new Vector2(skillPos[Skills.DeftHands].x + 2 * btnWidth,
                    skillPos[Skills.DeftHands].y));
            skillPos.Add(Skills.Aim, new Vector2(skillPos[Skills.KissTwice].x +
                btnXDist + btnWidth, skillPos[Skills.KissTwice].y));

            skillPos.Add(Skills.KnifeTraining,
                new Vector2(skillPos[Skills.KnifeExpert].x + btnWidth/2 + 
                    btnXDist/2, yOffset));
            skillPos.Add(Skills.Stormer,
                new Vector2(skillPos[Skills.KissTwice].x + btnWidth / 2 +
                    btnXDist / 2, yOffset));

            foreach(Skill skill in skillPos.Keys)
                skills.Add(skill.Name, skill);
                

            stormerSkills = new SkillTree(Helper.getImage("StormerSkills"), 
                skills, skillPos);

            return stormerSkills;
        }
    }
}
