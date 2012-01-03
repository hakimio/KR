using UnityEngine;
using System.Collections;
using System;

public class GameSettings: MonoBehaviour
{
    public static GameSettings instance;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void saveChar(BaseChar character)
    {
        //GameObject pc = GameObject.Find("Player Character");
        BaseChar bcClass = character;//pc.GetComponent<BaseChar>();
        PlayerPrefs.SetString("Char name", bcClass.charName);
        PlayerPrefs.SetInt("Weight", bcClass.weight);
        PlayerPrefs.SetInt("Height", bcClass.height);

        PlayerPrefs.SetString("Class", bcClass.CharClass.Name);

        for (int i = 0; i < Enum.GetValues(typeof(AttrNames)).Length; i++)
            PlayerPrefs.SetInt((AttrNames)i + " Base Value",
                               bcClass.getAttr(i).baseValue);
        string skills = "";

        Class charClass = bcClass.CharClass;
        foreach (Skill skill in charClass.SkillTree.Skills.Values)
            if (skill.Known)
                skills += skill.Name + "|";

        PlayerPrefs.SetString("Skills", skills);
    }
    public BaseChar loadChar()
    {
        //GameObject pc = GameObject.Find("Player Character");
        BaseChar bcClass = new BaseChar();//pc.GetComponent<BaseChar>();

        bcClass.charName = PlayerPrefs.GetString("Char name");
        bcClass.weight = PlayerPrefs.GetInt("Weight");
        bcClass.height = PlayerPrefs.GetInt("Height");

        //Archetype loading is missing

        for (int i = 0; i < Enum.GetValues(typeof(AttrNames)).Length; i++)
            bcClass.getAttr(i).baseValue = PlayerPrefs.
                GetInt((AttrNames)i + " Base Value");

        string[] skills = PlayerPrefs.GetString("Skills").Split('|');
        SkillTree skillTree = bcClass.CharClass.SkillTree;

        foreach (string skill in skills)
            skillTree.Skills[skill].Known = true;

        return bcClass;
    }
}
