using UnityEngine;
using System.Collections;

public class CharacterScreen: MonoBehaviour
{
    public GUISkin skin;
    public static CharacterScreen instance = null;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        if (InventoryGUI.instance.enabled)
            InventoryGUI.instance.enabled = false;
        if (SkillTreeGUI.instance.enabled)
            SkillTreeGUI.instance.enabled = false;
        if (TradeScreen.instance.enabled)
            TradeScreen.instance.enabled = false;
        MyCamera.instance.controllingEnabled = false;
        Messenger<bool>.Broadcast("enable movement", false);
        if (GameMaster.instance.inCombat)
            return;
        Messenger<bool>.Broadcast("enable phrases", false);
    }

    void OnDisable()
    {
        MyCamera.instance.controllingEnabled = true;
        Messenger<bool>.Broadcast("enable movement", true);
        if (GameMaster.instance.inCombat)
            return;
        Messenger<bool>.Broadcast("enable phrases", true);
        return;
    }

    void OnGUI()
    {
        GUI.depth = 0;
        GUI.skin = skin;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 122, 
            Screen.height / 2 - 212, 244, 425));
        showBackground();
        showCharacterInfo();
        GUI.EndGroup();
        showTooltip();
    }

    void showTooltip()
    {
        if (GUI.tooltip.Equals(""))
            return;

        float mouseX = Input.mousePosition.x;
        float mouseY = Screen.height - Input.mousePosition.y;
        GUIStyle style = skin.GetStyle("tooltip");
        float height = style.CalcHeight(new GUIContent(GUI.tooltip), 190f);
        float maxWidth = 0;
        float minWidth = 0;
        style.CalcMinMaxWidth(new GUIContent(GUI.tooltip), out minWidth,
            out maxWidth);
        if (maxWidth > 190)
            maxWidth = 190;
        float yOffset = 0;
        float xOffset = 0;
        if (mouseY + height > Screen.height)
            yOffset = mouseY + height - Screen.height;
        if (mouseX + 210 > Screen.width)
            xOffset = 220;
        GUI.Box(new Rect(mouseX + 11 - xOffset, mouseY - yOffset - 7,
                maxWidth + 18, height + 14), "");
        GUI.Label(new Rect(mouseX + 20 - xOffset, mouseY - yOffset,
            190, height), GUI.tooltip, "tooltip");
    }

    void showBackground()
    {
        Texture2D background = Helper.getImage("Character screen/background");
        GUI.DrawTexture(new Rect(0, 0, 244, 425), background);
    }

    void showCharacterInfo()
    {
        GUIContent content;
        BaseChar selectedChar = GameMaster.instance.selectedChar;

        GUI.DrawTexture(new Rect(17, 19, 76, 84), selectedChar.Image,
            ScaleMode.ScaleToFit, true);
        GUI.Label(new Rect(98, 19, 150, 20), selectedChar.charName, "title");
        GUI.Label(new Rect(98, 39, 150, 20),
            "Class: " + selectedChar.CharClass.Name, "title");
        GUI.Label(new Rect(98, 59, 150, 20), "Level: " + selectedChar.level, 
            "title");

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
            GUI.Label(new Rect(21, offset + i * 18, 150, 23), content);
            if (i == (int)SecondaryAttrNames.Hit_Points)
            {
                GUI.Label(new Rect(171, offset + i * 18, 53, 23),
                    selectedChar.CurrentHP.ToString() + "/" +
                    secAttr.Value.ToString(), "AttrValues");
                offset += 23;
                float totalHP = selectedChar.LostHP + selectedChar.CurrentHP;
                Texture2D PBEmpty, PBFull;
                PBEmpty = Helper.getImage("Inventory/ProgressBarEmpty");
                PBFull = Helper.getImage("Inventory/ProgressBarFull");
                GUI.DrawTexture(new Rect(21, offset + i * 18, 204, 10),
                    PBEmpty, ScaleMode.ScaleAndCrop);
                GUI.DrawTextureWithTexCoords(new Rect(21, offset + i * 18,
                    204 * selectedChar.CurrentHP / totalHP, 10), PBFull, 
                    new Rect(0, 0, selectedChar.CurrentHP / totalHP, 1));
                offset += 10;
                GUI.Label(new Rect(21, offset + i * 18, 150, 23),
                    "Experience");
                GUI.Label(new Rect(171, offset + i * 18, 53, 23),
                    selectedChar.Exp.ToString() + "/" +
                    selectedChar.nextLevelExp.ToString(), "AttrValues");
                offset += 23;
                GUI.DrawTexture(new Rect(21, offset + i * 18, 204, 10),
                    PBEmpty, ScaleMode.ScaleAndCrop);
                offset -= 3;
            }
            else
                GUI.Label(new Rect(171, offset + i * 18, 53, 23),
                    secAttr.Value.ToString(), "AttrValues");
        }
    }
}
