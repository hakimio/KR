using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD: MonoBehaviour
{
    private const int BUTTON_WIDTH = 124;
    private const int BUTTON_HEIGHT = 24;
    private const int OFFSET = 6;
    public static HUD instance = null;
    public bool clickable = true;
    string messages = "";
    Vector2 scrollPos = new Vector2();
    public GUISkin skin;

    bool minimized = false;
    List<string> modes = new List<string>();
    int activeMode = 0;

    public bool Minimized
    {
        get
        {
            return minimized;
        }
    }

    public void addMessage(string msg)
    {
        if (messages.Length > 0)
            messages += "\n·" + msg;
        else
            messages = "·" + msg;
        GUIStyle style = skin.GetStyle("label");
        float height = style.CalcHeight(new GUIContent(messages), 194);
        if (height > 78)
        {
            height = style.CalcHeight(new GUIContent(messages), 177);
            scrollPos.y = height - 78;
        }
    }

    void Awake()
    {
        Messenger<ItemSlots>.AddListener("ItemSlotChanged", activeItemChanged);
        if (instance == null)
            instance = this;
    }
    void OnGUI()
    {
        GUI.enabled = clickable;
        Color color = GUI.color;
        if (!clickable)
            GUI.color = new Color(1, 1, 1, 2);
        GUI.skin = skin;
        if (minimized)
            showMinPanel();
        else
            showMaxPanel();
        showCharSelector();
        showTooltip();
        if (!clickable)
            GUI.color = color;
        GUI.enabled = true;
    }

    void showMinPanel()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 - 208, Screen.height - 34,
            445, 34));
        Texture2D minPanel = Helper.getImage("Panel/Min Panel");
        GUI.DrawTexture(new Rect(0, 0, 445, 34), minPanel);
        showMinPanelBtns();
        GUI.EndGroup();
    }

    void showMaxPanel()
    {
        BaseChar selectedChar = GameMaster.instance.selectedChar;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 400, Screen.height - 90,
            800, 90));
        Texture2D panel = Helper.getImage("Panel/Panel");
        GUI.DrawTexture(new Rect(0, 0, 800, 90), panel);
        showMessages();
        showMaxPanelBtns();
        showItems(selectedChar);
        showBigButtons(selectedChar);
        showModes();
        GUI.EndGroup();
    }

    void showMessages()
    {
        float y = 7;
        GUIStyle style = skin.GetStyle("label");
        float height = style.CalcHeight(new GUIContent(messages), 192);
        if (height > 78)
        {
            Texture2D scrollbar = Helper.getImage("Panel/scrollbar");
            GUI.DrawTexture(new Rect(185, 0, 17, 84), scrollbar);
            height = 78;
        }
        else
        {
            y += 78 - height;
        }
        GUILayout.BeginArea(new Rect(8, y, 194, height));
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.Label(messages);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    void showMaxPanelBtns()
    {
        if (GUI.Button(new Rect(212, 29, 99, 25), "Inventory", "leftButtons"))
            Messenger.Broadcast("toggleInventoryVisibility");
        if (GUI.Button(new Rect(212, 60, 99, 25), "Trade", "leftButtons"))
            Messenger.Broadcast("toggleTradeScreenVisibility");
        if (GUI.Button(new Rect(531, 29, 98, 25), "Skill Tree", "rightButtons"))
            Messenger.Broadcast("toggleSkillTreeVisibility");
        if (GUI.Button(new Rect(531, 60, 98, 25), "Character", "rightButtons"))
            Messenger.Broadcast("toggleCharScreenVisibility");
        if (GUI.Button(new Rect(771, 9, 26, 17), "", "minButton"))
            minimized = true;
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
        if (mouseX + maxWidth + 20 > Screen.width)
            xOffset = maxWidth + 35;
        GUI.Box(new Rect(mouseX + 11 - xOffset, mouseY - yOffset - 7,
                maxWidth + 18, height + 14), "");
        GUI.Label(new Rect(mouseX + 20 - xOffset, mouseY - yOffset,
            190, height), GUI.tooltip, "tooltip");
    }

    void showItems(BaseChar selectedChar)
    {
        Inventory inv = selectedChar.Items;
        GUIContent content;

        if (inv.ActiveSlot == ItemSlots.Slot1)
        {
            if (inv.Slot1 != null)
                content = new GUIContent(inv.Slot1.Image, inv.Slot1.Name);
            else
                content = new GUIContent("Punch");
        }
        else
        {
            if (inv.Slot2 != null)
                content = new GUIContent(inv.Slot2.Image, inv.Slot2.Name);
            else
                content = new GUIContent("Kick");
        }
        if (GUI.Button(new Rect(376, 10, 140, 71), content)) { }
    }

    void showBigButtons(BaseChar selectedChar)
    {
        if (GUI.Button(new Rect(321, 4, 37, 38), "»", "nextItemBtn"))
        {
            Inventory inv = selectedChar.Items;
            int activeSlot = ((int)inv.ActiveSlot + 1) % 2;
            selectedChar.Items.ActiveSlot = (ItemSlots)activeSlot;
            Messenger<ItemSlots>.Broadcast("ItemSlotChanged",
                (ItemSlots)activeSlot);
        }
        if (GUI.Button(new Rect(321, 47, 37, 38), "M", "modeBtn"))
        {
            if (modes.Count != 0)
                activeMode = (activeMode + 1) % modes.Count;
        }
    }

    void showModes()
    {
        if (modes.Count == 0)
            return;
        string path = "Items/Weapons/Modes/";
        string selectedMode = modes[activeMode];
        Texture2D modePic = Helper.getImage(path + selectedMode);
        if (modePic == null)
            modePic = new Texture2D(0, 0);
        GUI.DrawTexture(new Rect(517 - modePic.width, 9, modePic.width,
            modePic.height), modePic);
    }

    void activeItemChanged(ItemSlots slot)
    {
        modes.Clear();
        BaseChar selectedChar = GameMaster.instance.selectedChar;
        if (slot != selectedChar.Items.ActiveSlot)
            return;
        Item activeItem;
        if (selectedChar.Items.ActiveSlot == ItemSlots.Slot1)
            activeItem = selectedChar.Items.Slot1;
        else
	        activeItem = selectedChar.Items.Slot2;
        if (activeItem == null || !(activeItem is Weapon))
            return;

        List<string> knownSkills = new List<string>();
        List<string> replacedSkills = new List<string>();

        foreach (Skill skill in selectedChar.CharClass.SkillTree.Skills.Values)
            if (skill.Known)
                foreach (string gunName in skill.GunNames)
                    if (gunName.Equals(activeItem.Name))
                    {
                        knownSkills.Add(skill.Name);
                        replacedSkills.AddRange(skill.Replaces);
                    }

        foreach (string skillName in replacedSkills)
            if (knownSkills.Contains(skillName))
                knownSkills.Remove(skillName);

        foreach (string skillName in knownSkills)
        {
            Skill skill = selectedChar.CharClass.SkillTree.Skills[skillName];
            string mode = activeItem.Name;

            if ((skill.Mode == Mode.MultiHit 
                || skill.Mode == Mode.MultiGunAndHit) 
                && skill.Bonuses.ContainsKey(BonusType.nrOfHits))
            {
                mode += " " + (int)skill.Bonuses[BonusType.nrOfHits] + "Hits";
            }
            if (skill.Mode == Mode.MultiGun 
                || skill.Mode == Mode.MultiGunAndHit)
            {
                mode += " 2Guns";
            }
            if (mode.Equals(activeItem.Name))
                continue;
            modes.Add(mode);
        }
        if (modes.Count > 0)
            modes.Insert(0, "");

        activeMode = 0;
    }

    void showCharSelector()
    {
        GameMaster gm = GameMaster.instance;
        int charCount = gm.characters.Count;
        int oldSelectedCharIndex = gm.characters.IndexOf(gm.selectedChar);
        float y = (Screen.height - 90 * charCount) / 2;
        List<BaseChar> chars = gm.characters;
        GUIContent content;
        int newSelectedCharIndex = oldSelectedCharIndex;
        Texture2D hpBar = Helper.getImage("Panel/hp bar");
        Texture2D selector = Helper.getImage("Panel/selector");

        for (int i = 0; i < charCount; i++)
        {
            content = new GUIContent(chars[i].Image, chars[i].charName);
            if (GUI.Toggle(new Rect(0, y + 90 * i, 67, 90),
                 i == newSelectedCharIndex, content))
            {
                newSelectedCharIndex = i;
                GUI.DrawTexture(new Rect(8, y + 7 + 90 * i, 52, 63), selector);
            }
            int totalHP = chars[i].LostHP + chars[i].CurrentHP;
            GUI.DrawTexture(new Rect(8, y + 71 + 90 * i, 
                52 * chars[i].CurrentHP / totalHP, 11), hpBar);
            GUI.Label(new Rect(8, y + 71 + 90 * i, 52, 11),
                chars[i].CurrentHP + "/" + totalHP, "charSelector");
        }

        if (oldSelectedCharIndex != newSelectedCharIndex)
        {
            gm.selectedChar = gm.characters[newSelectedCharIndex];
            Messenger.Broadcast("selectedCharChanged");
            Messenger<ItemSlots>.Broadcast("ItemSlotChanged",
                gm.selectedChar.Items.ActiveSlot);
        }
    }

    void showMinPanelBtns()
    {
        if (GUI.Button(new Rect(4, 4, 99, 25), "Trade", "leftButtons"))
            Messenger.Broadcast("toggleTradeScreenVisibility");
        if (GUI.Button(new Rect(107, 4, 99, 25), "Inventory", "leftButtons"))
            Messenger.Broadcast("toggleInventoryVisibility");
        if (GUI.Button(new Rect(210, 4, 99, 25), "Skill Tree", "leftButtons"))
            Messenger.Broadcast("toggleSkillTreeVisibility");
        if (GUI.Button(new Rect(313, 4, 99, 25), "Character", "leftButtons"))
            Messenger.Broadcast("toggleCharScreenVisibility");
        if (GUI.Button(new Rect(415, 4, 25, 25), "", "maxButton"))
            minimized = false;
    }


}
