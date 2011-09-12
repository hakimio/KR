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

        PlayerPrefs.SetString("Archetype", bcClass.Archetype.getName());

        for (int i = 0; i < Enum.GetValues(typeof(AttrNames)).Length; i++)
            PlayerPrefs.SetInt((AttrNames)i + " Base Value",
                               bcClass.getAttr(i).baseValue);
        string disciplineNames = "";

        Archetype archetype = bcClass.Archetype;
        for (int i = 0; i < archetype.getDisciplines().Length; i++)
            if (archetype.getDiscipline(i).known)
                disciplineNames += archetype.getDiscipline(i).getName() + "|";

        PlayerPrefs.SetString("Disciplines", disciplineNames);

        Discipline[] disciplines = archetype.getDisciplines();
        string featNames = "";

        for (int i = 0; i < disciplines.Length; i++)
            for (int j = 0; j < disciplines[i].getFeats().Length; j++)
                if (disciplines[i].getFeat(j).known)
                    featNames += disciplines[i].getFeat(j).getName() + "|";

        PlayerPrefs.SetString("Feats", featNames);
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

        String[] disciplineNames = PlayerPrefs.GetString("Disciplines").Split('|');
        String[] featNames = PlayerPrefs.GetString("Feats").Split('|');
        Discipline[] disciplines = bcClass.Archetype.getDisciplines();

        int j = 0;
        int k = 0;
        for (int i = 0; i < disciplines.Length; i++)
            if (disciplines[i].getName().Equals(disciplineNames[j]))
            {
                disciplines[i].known = true;
                for (int l = 0; l < disciplines[i].getFeats().Length; l++)
                {
                    Feat feat = disciplines[i].getFeat(l);
                    if (feat.getName().Equals(featNames[k]))
                    {
                        feat.known = true;
                        k++;
                    }
                }
                j++;
            }

        return bcClass;
    }
}
