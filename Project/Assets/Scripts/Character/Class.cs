using System;

public class Class
{
    private string name, description;
    private SkillTree skillTree;

    public Class(string name, string description, SkillTree skillTree)
    {
        this.name = name;
        this.description = description;
        this.skillTree = skillTree;
    }

    public string Name
    {
        get { return name; }
    }
    public string Description
    {
        get { return description; }
    }
    public SkillTree SkillTree
    {
        get { return skillTree; }
    }
}
