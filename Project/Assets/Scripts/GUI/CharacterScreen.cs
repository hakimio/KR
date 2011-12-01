using UnityEngine;
using System.Collections;

public class CharacterScreen: MonoBehaviour
{
    private bool show = false;
    public GUISkin skin;
    private BaseChar selectedChar;
    static public CharacterScreen instance = null;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Messenger.AddListener("toggleCharScreenVisibility", toggleVisibility);
    }

    private void toggleVisibility()
    {
        show = !show;

        if (!show)
        {
            Messenger<bool>.Broadcast("enable movement", true);
            MyCamera.instance.controllingEnabled = true;
            Messenger<bool>.Broadcast("enable phrases", true);
            return;
        }

        int charIndex = GameMaster.instance.selectedChar;
        selectedChar = GameMaster.instance.characters[charIndex];

        if (InventoryGUI.instance.Visible)
            Messenger.Broadcast("toggleInventoryVisibility");

        Messenger<bool>.Broadcast("enable movement", false);
        MyCamera.instance.controllingEnabled = false;
        Messenger<bool>.Broadcast("enable phrases", false);
    }

    void OnGUI()
    {
        if (!show)
            return;
        GUI.skin = skin;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 122, 
            Screen.height / 2 - 212, 244, 425));
        showBackground();
        showCharacterInfo();
        //showButton();
        GUI.EndGroup();
    }

    void showButton()
    {
        if (GUI.Button(new Rect(6, 424, 233, 45), "Close"))
            toggleVisibility();
    }

    public bool Visible
    {
        get
        {
            return show;
        }
    }

    void showBackground()
    {
        Texture2D background = Helper.getImage("Character screen/background");
        GUI.DrawTexture(new Rect(0, 0, 244, 425), background);
    }

    void showCharacterInfo()
    {
        GUIContent content;

        GUI.DrawTexture(new Rect(17, 19, 76, 84), selectedChar.Image,
            ScaleMode.ScaleToFit, true);
        skin.label.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(98, 19, 150, 20), selectedChar.charName);
        GUI.Label(new Rect(98, 39, 150, 20),
            "Class: " + selectedChar.Archetype.getName());
        GUI.Label(new Rect(98, 59, 150, 20), "Level: " + selectedChar.level);
        skin.label.fontStyle = FontStyle.Normal;

        for (int i = 0; i < selectedChar.getAttributes().Length; i++)
        {
            BaseStat attribute = selectedChar.getAttr(i);
            content = new GUIContent(attribute.Name, attribute.description);
            GUI.Label(new Rect(21, 108 + i * 17, 100, 23), content);
            GUI.Label(new Rect(126, 108 + i * 17, 30, 23),
                attribute.Value.ToString());
        }

        int nrOfPrimaryAttr = System.Enum.GetValues(typeof(AttrNames)).Length;
        int offset = 118 + nrOfPrimaryAttr * 17;
        for (int i = 0; i < selectedChar.getSecondaryAttributes().Length; i++)
        {
            ModifiedStat secAttr = selectedChar.getSecondaryAttr(i);
            string attrName = secAttr.Name.Replace('_', ' ');
            content = new GUIContent(attrName, secAttr.description);
            GUI.Label(new Rect(21, offset + i * 17, 150, 23), content);
            if (i == (int)SecondaryAttrNames.Hit_Points)
            {
                GUI.Label(new Rect(171, offset + i * 17, 60, 23),
                    selectedChar.CurrentHP.ToString() + "/" +
                    secAttr.Value.ToString());
                offset += 21;
                GUI.DrawTexture(new Rect(21, offset + i * 17, 204, 10),
                    Helper.getImage("Inventory/ProgressBarFull"));
                offset += 12;
                GUI.Label(new Rect(21, offset + i * 17, 150, 23),
                    "Experience");
                GUI.Label(new Rect(171, offset + i * 17, 60, 23),
                    selectedChar.Exp.ToString() + "/" +
                    selectedChar.nextLevelExp.ToString());
                offset += 21;
                GUI.DrawTexture(new Rect(21, offset + i * 17, 204, 10),
                    Helper.getImage("Inventory/ProgressBarEmpty"));
                offset += 2;
            }
            else
                GUI.Label(new Rect(171, offset + i * 17, 60, 23),
                    secAttr.Value.ToString());
        }
    }
}
