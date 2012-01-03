using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree
{
    private Texture2D background;
    private Dictionary<string, Skill> skills;
    private Dictionary<Skill, Vector2> skillPos;
    private int skillPoints;

    public SkillTree(Texture2D background, Dictionary<string, Skill> skills, 
        Dictionary<Skill, Vector2> skillPos)
    {
        skillPoints = 10;
        this.background = background;
        this.skills = skills;
        this.skillPos = skillPos;
    }

    public Texture2D Background
    {
        get
        {
            return background;
        }
    }

    public int SkillPoints
    {
        get { return skillPoints; }
        set { skillPoints = value; }
    }

    public Dictionary<string, Skill> Skills
    {
        get
        {
            return skills;
        }
    }

    public Dictionary<Skill, Vector2> SkillPos
    {
        get
        {
            return skillPos;
        }
    }
}
