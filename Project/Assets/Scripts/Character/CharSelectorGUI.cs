using UnityEngine;
using System.Collections;

public class CharSelectorGUI: MonoBehaviour
{
    private const int OFFSET = 5;
    private const int LINE_HEIGHT = 25;

    private const int LABEL_WIDTH = 150;
    private const int VALUE_WIDTH = 30;

    private const int ATTR_OFFSET = 40;

    private BaseChar[] characters;
    private int selectedChar = 0;

    public GUISkin mySkin;
    public GUIStyle descriptionstyle;
    public GUIStyle titleStyle;

    void Start()
    {
        characters = new BaseChar[1];
        characters[0] = Characters.EricFrost;
    }

    void OnGUI()
    {
        GUI.skin = mySkin;
        showTitle();
        showPics();
        showBasics();
        showBaseAttr();
        showSecondaryAttr();
        showDescription();
        showButtons();
    }

    void showTitle()
    {
        GUI.Label(new Rect(Screen.width / 2 - 125,
            Screen.height / 2 - 300 + OFFSET, 250, LINE_HEIGHT), 
            "Character Selection", titleStyle);
    }

    void showPics()
    {
        GUI.DrawTexture(new Rect(Screen.width / 2 - 320 + OFFSET, Screen.height 
            / 2 - 260 + 2 * OFFSET, 230, 250), characters[selectedChar].Image,
            ScaleMode.ScaleToFit, true);
    }

    void showBasics()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 + 50 + OFFSET, 
            Screen.height / 2 - 260 + OFFSET, 150, 2 * (LINE_HEIGHT + OFFSET)));
        GUI.Label(new Rect(OFFSET, 0, 150, LINE_HEIGHT),
            characters[selectedChar].charName);
        GUIContent content = new GUIContent("Class: " +
            characters[selectedChar].Archetype.getName(), 
            characters[selectedChar].Archetype.getDescription());
        GUI.Label(new Rect(OFFSET, LINE_HEIGHT, 150, LINE_HEIGHT), content);
        GUI.EndGroup();
    }

    void showBaseAttr()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 + 50 + OFFSET, 
            Screen.height / 2 - 170 + OFFSET, 300, 150));
        for (int i = 0; i < System.Enum.GetValues(typeof(AttrNames)).Length; 
            i++)
        {
            GUIContent content = new GUIContent(((AttrNames)i).ToString(),
                characters[selectedChar].getAttr(i).description);
            GUI.Label(new Rect(OFFSET, i * LINE_HEIGHT,
                LABEL_WIDTH, LINE_HEIGHT), content);
            GUI.Label(new Rect(LABEL_WIDTH + 4 * OFFSET, 
                i * LINE_HEIGHT, VALUE_WIDTH, LINE_HEIGHT),
                characters[selectedChar].getAttr(i).Value.ToString());
        }
        GUI.EndGroup();
    }

    private void showSecondaryAttr()
    {
        string attrName;
        GUI.BeginGroup(new Rect(Screen.width / 2 + 50 + OFFSET, 
            Screen.height / 2 + 10 + OFFSET, 300, 250));
        for (int i = 0; i < System.Enum.
            GetValues(typeof(SecondaryAttrNames)).Length; i++)
        {
            attrName = ((SecondaryAttrNames)i).ToString().Replace('_', ' ');
            GUIContent content = new GUIContent(attrName,
                characters[selectedChar].getSecondaryAttr(i).description);
            GUI.Label(new Rect(OFFSET, i * LINE_HEIGHT,
                LABEL_WIDTH, LINE_HEIGHT), content);
            GUI.Label(new Rect(LABEL_WIDTH + 4 * OFFSET,
                i * LINE_HEIGHT, VALUE_WIDTH, LINE_HEIGHT),
                characters[selectedChar].getSecondaryAttr(i).Value.ToString());
        }
        GUI.EndGroup();
    }

    private void showDescription()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 - 400 + 4 * OFFSET, 
            Screen.height / 2 + 10 + OFFSET, 400 - 2 * OFFSET, 250));

        if (GUI.tooltip.Equals(""))
            GUI.Label(new Rect(OFFSET, 0, 400 - 2 * OFFSET, 250), 
                "Character description goes here...", descriptionstyle);
        else
            GUI.Label(new Rect(OFFSET, 0, 400 - 2 * OFFSET, 250), GUI.tooltip, 
                descriptionstyle);
        GUI.EndGroup();
    }

    private void showButtons()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 400 + 2 * OFFSET,
            Screen.height / 2 + 300 - 12 * OFFSET, 200,
            LINE_HEIGHT), "Previous Character"))
        {
            if (selectedChar == 0)
                selectedChar = characters.Length - 1;
            else
                selectedChar--;
        }

        if (GUI.Button(new Rect(Screen.width / 2 + 350 - OFFSET - LABEL_WIDTH,
            Screen.height / 2 + 300 - 12 * OFFSET, 200,
            LINE_HEIGHT), "Next Character"))
        {
            selectedChar = (selectedChar + 1) % characters.Length;
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 400 + 2 * OFFSET,
            Screen.height / 2 + 300 - OFFSET - LINE_HEIGHT, LABEL_WIDTH,
            LINE_HEIGHT), "Back"))
            Application.LoadLevel("MainMenu");

        if (GUI.Button(new Rect(Screen.width / 2 + 400 - OFFSET - LABEL_WIDTH,
            Screen.height / 2 + 300 - OFFSET - LINE_HEIGHT, LABEL_WIDTH,
            LINE_HEIGHT), "Take"))
        {
            GameSettings.instance.saveChar(characters[selectedChar]);
            Application.LoadLevel("Level1");
        }
    }
}
