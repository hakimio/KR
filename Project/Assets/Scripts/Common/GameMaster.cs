using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GameMaster : MonoBehaviour
{
    public bool inCombat = false;
    public static GameMaster instance = null;
    public List<BaseChar> characters;
    public BaseChar selectedChar;
	
	void Awake()
	{
        if (instance != null)
        {
            characters = instance.characters;
            selectedChar = instance.selectedChar;
            foreach (BaseChar character in characters)
                character.LostHP = 0;
            instantiateCharacters();
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        
        characters = new List<BaseChar>();
        characters.Add(Characters.TheRobot);
        characters.Add(Characters.EricFrost);
        characters.Add(Characters.Lerpz);
        selectedChar = characters[0];

        instantiateCharacters();
	}

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            InGameMenu.instance.enabled = !InGameMenu.instance.enabled;
    }

    protected abstract void instantiateCharacters();
}