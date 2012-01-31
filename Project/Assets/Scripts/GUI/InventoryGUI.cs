using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryGUI: MonoBehaviour
{
    public GUISkin skin;
    private Vector2 scrollPos = new Vector2();

    private Item floatingItem = null;

    private GUIContent armorSlotContent;
    private GUIContent item1SlotContent;
    private GUIContent item2SlotContent;
    private Vector2 mousePos = new Vector2();
    public static InventoryGUI instance = null;

    private enum Toggle
    {
        Weapons,
        MedicineAndArmor,
        Keys
    }
    private Toggle selectedToggle = Toggle.Weapons;
    private BaseChar selectedChar;

    void Awake()
    {
        instance = this;
    }

    public bool Visible
    {
        get
        {
            return show;
        }
    }

    void Start()
    {
        Messenger.AddListener("toggleInventoryVisibility", toggleVisibility);
    }

    private bool show = false;

    void toggleVisibility()
    {
        show = !show;

        GameObject IC = GameObject.Find("InventoryCamera");
        Camera cam = IC.GetComponent<Camera>();

        if (!show)
        {
            Messenger<bool>.Broadcast("enable movement", true);
            MyCamera.instance.controllingEnabled = true;
            cam.enabled = false;
            Messenger<bool>.Broadcast("enable phrases", true);
            return;
        }

        int charIndex = GameMaster.instance.selectedChar;
        selectedChar = GameMaster.instance.characters[charIndex];

        setItemSlotContent();

        if (CharacterScreen.instance.Visible)
            Messenger.Broadcast("toggleCharScreenVisibility");
        if (SkillTreeGUI.instance.Visible)
            Messenger.Broadcast("toggleSkillTreeVisibility");
        if (TradeScreen.instance.Visible)
            Messenger.Broadcast("toggleTradeScreenVisibility");

        Messenger<bool>.Broadcast("enable movement", false);
        MyCamera.instance.controllingEnabled = false;
        float x = (Screen.width / 2 - 151f) / Screen.width;
        float y = (Screen.height / 2 - 114f) / Screen.height;
        float width = 278f / Screen.width;
        float height = 308f / Screen.height;
        Rect rect = new Rect(x, y, width, height);
        cam.rect = rect;
        cam.enabled = true;
        Messenger<bool>.Broadcast("enable phrases", false);
    }

    void setItemSlotContent()
    {
        if (selectedChar.Items.Armor == null)
            armorSlotContent = new GUIContent("Armor");
        else
            armorSlotContent = new GUIContent(selectedChar.Items.Armor.Image,
                GUIHelper.getInfo(selectedChar.Items.Armor));
        if (selectedChar.Items.Slot1 == null)
            item1SlotContent = new GUIContent("Primary Item");
        else
            item1SlotContent = new GUIContent(selectedChar.Items.Slot1.Image,
                GUIHelper.getInfo(selectedChar.Items.Slot1));
        if (selectedChar.Items.Slot2 == null)
            item2SlotContent = new GUIContent("Secondary Item");
        else
            item2SlotContent = new GUIContent(selectedChar.Items.Slot2.Image,
                GUIHelper.getInfo(selectedChar.Items.Slot2));
    }

    void OnGUI()
    {
        if (!show)
            return;
        GUI.skin = skin;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 400, 
            Screen.height / 2 - 300, 800, 600));
        showBackground();
        showItemToggles();
        showBottomButtons();
        showItemSlots();
        showCharacterInfo();
        showCharSelector();
        showItems();
        GUI.EndGroup();
        showTooltip();
        showDraggedItem();
    }

    void showCharSelector()
    {
        int selectedCharIndex = GameMaster.instance.selectedChar;
        BaseChar[] chars = GameMaster.instance.characters.ToArray();
        GUIContent content;
        int selectedIndex = selectedCharIndex;
        for (int i = 0; i < chars.Length; i++)
        {
            content = new GUIContent(chars[i].Image, chars[i].charName);
            if (GUI.Toggle(new Rect(244 + i * 61, 18, 51, 61),
                i == selectedIndex, content))
                selectedIndex = i;
        }

        if (selectedCharIndex != selectedIndex)
        {
            GameMaster.instance.selectedChar = selectedIndex;
            selectedChar = GameMaster.instance.characters[selectedIndex];
            selectedCharIndex = selectedIndex;
            setItemSlotContent();
        }
    }

    void showTooltip()
    {
        if (GUI.tooltip.Equals("") || floatingItem != null)
            return;

        float mouseX = Input.mousePosition.x;
        float mouseY = Screen.height - Input.mousePosition.y;
        GUIStyle style = skin.GetStyle("tooltip");
        float height = style.CalcHeight(new GUIContent(GUI.tooltip), 190f);
        float maxWidth = 0;
        float minWidth = 0;
        style.CalcMinMaxWidth(new GUIContent(GUI.tooltip), out minWidth,
            out maxWidth);
        float yOffset = 0;
        float xOffset = 0;
        if (mouseY + height > Screen.height)
            yOffset = mouseY + height - Screen.height;
        if (mouseX + 210 > Screen.width)
            xOffset = 220;

        if ((mouseX < Screen.width / 2 - 170 
            && mouseY > Screen.height / 2 - 220)
            || mouseY > Screen.height / 2 + 135)
        {
            if (mouseX + maxWidth + 18 > Screen.width)
                xOffset = maxWidth + 31;
            else
                xOffset = 0;
            int nameLength = GUI.tooltip.IndexOf('\n');
            string itemName = GUI.tooltip.Substring(0, nameLength);
            string description = GUI.tooltip.Replace(itemName + '\n', "");
            GUI.Box(new Rect(mouseX + 11 - xOffset, mouseY - yOffset - 7,
                maxWidth + 18, height + 14), "");
            GUI.Label(new Rect(mouseX + 20 - xOffset, mouseY - yOffset, 
                160, 23), itemName, "tooltip");
            skin.FindStyle("tooltip").normal.textColor = Color.yellow;
            GUI.Label(new Rect(mouseX + 20 - xOffset, mouseY + 17 - yOffset, 
                160, height - 23), description, "tooltip");
            skin.FindStyle("tooltip").normal.textColor = new Color(203f / 255f,
                220f / 255f, 220f / 255f);
        }
        else
        {
            if (maxWidth > 190)
                maxWidth = 190;
            GUI.Box(new Rect(mouseX + 11 - xOffset, mouseY - yOffset - 7,
                maxWidth + 18, height + 14), "");
            GUI.Label(new Rect(mouseX + 20 - xOffset, mouseY - yOffset,
                190, height), GUI.tooltip, "tooltip");
        }
            
    }

    void showCharacterInfo()
    {
        GUIContent content;

        GUI.DrawTexture(new Rect(567, 29, 76, 84), selectedChar.Image, 
            ScaleMode.ScaleToFit, true);
        skin.label.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(648, 29, 150, 20), selectedChar.charName);
        GUI.Label(new Rect(648, 49, 150, 20), 
            "Class: " + selectedChar.CharClass.Name);
        GUI.Label(new Rect(648, 69, 150, 20), "Level: " + selectedChar.level);
        skin.label.fontStyle = FontStyle.Normal;

        for (int i = 0; i < selectedChar.getAttributes().Length; i++)
        {
            BaseStat attribute = selectedChar.getAttr(i);
            content = new GUIContent(attribute.Name, attribute.description);
            GUI.Label(new Rect(571, 118 + i * 17, 100, 23), content);
            GUI.Label(new Rect(676, 118 + i * 17, 30, 23), 
                attribute.Value.ToString());
        }

        int nrOfPrimaryAttr = System.Enum.GetValues(typeof(AttrNames)).Length;
        int offset = 128 + nrOfPrimaryAttr * 17;
        for (int i = 0; i < selectedChar.getSecondaryAttributes().Length; i++)
        {
            ModifiedStat secAttr = selectedChar.getSecondaryAttr(i);
            string attrName = secAttr.Name.Replace('_', ' ');
            content = new GUIContent(attrName, secAttr.description);
            GUI.Label(new Rect(571, offset + i * 17, 150, 23), content);
            if (i == (int)SecondaryAttrNames.Hit_Points)
            {
                GUI.Label(new Rect(721, offset + i * 17, 60, 23),
                    selectedChar.CurrentHP.ToString() + "/" + 
                    secAttr.Value.ToString());
                offset += 21;
                GUI.DrawTexture(new Rect(571, offset + i * 17, 204, 10),
                    Helper.getImage("Inventory/ProgressBarFull"));
                offset += 12;
                GUI.Label(new Rect(571, offset + i * 17, 150, 23),
                    "Experience");
                GUI.Label(new Rect(721, offset + i * 17, 60, 23),
                    selectedChar.Exp.ToString() + "/" +
                    selectedChar.nextLevelExp.ToString());
                offset += 21;
                GUI.DrawTexture(new Rect(571, offset + i * 17, 204, 10),
                    Helper.getImage("Inventory/ProgressBarEmpty"));
                offset += 2;
            }
            else
                GUI.Label(new Rect(721, offset + i * 17, 60, 23),
                    secAttr.Value.ToString());
        }
    }

    void showDraggedItem()
    {
        if (floatingItem != null)
        {
            Screen.showCursor = false;
            float mouseX = Input.mousePosition.x;
            float invMouseY = Screen.height - Input.mousePosition.y;
            float textX = mouseX - floatingItem.Image.width / 2;
            float textY = invMouseY - floatingItem.Image.height / 2;
            GUI.DrawTexture(new Rect(textX, textY, floatingItem.Image.width,
                floatingItem.Image.height), floatingItem.Image);
            mousePos = new Vector2(mouseX, invMouseY);
        }
    }

    void showItemSlots()
    {
        if (GUI.RepeatButton(new Rect(240, 447, 140, 100), armorSlotContent))
        {
            if (selectedChar.Items.Armor != null)
            {
                selectedChar.Items.Armor.State = ItemState.Floating;
                floatingItem = selectedChar.Items.Armor;
                selectedChar.Items.Armor = null;
                armorSlotContent = new GUIContent("Armor");
            }
        }
        if (GUI.RepeatButton(new Rect(399, 447, 190, 100), item1SlotContent))
        {
            if (selectedChar.Items.Slot1 != null)
            {
                selectedChar.Items.Slot1.State = ItemState.Floating;
                floatingItem = selectedChar.Items.Slot1;
                selectedChar.Items.Slot1 = null;
                item1SlotContent = new GUIContent("Primary Item");
            }
        }
        if (GUI.RepeatButton(new Rect(593, 447, 190, 100), item2SlotContent))
        {
            if (selectedChar.Items.Slot2 != null)
            {
                selectedChar.Items.Slot2.State = ItemState.Floating;
                floatingItem = selectedChar.Items.Slot2;
                selectedChar.Items.Slot2 = null;
                item2SlotContent = new GUIContent("Secondary Item");
            }
        }
    }

    void Update()
    {
        Rect armorPos = new Rect(Screen.width / 2 - 160, 
            Screen.height / 2 + 147, 140, 100);
        Rect slot1Pos = new Rect(Screen.width / 2 - 1, 
            Screen.height / 2 + 147, 190, 100);
        Rect slot2Pos = new Rect(Screen.width / 2 + 193, 
            Screen.height / 2 + 147, 190, 100);
        int chars = GameMaster.instance.characters.Count;
        Rect charsPos = new Rect(Screen.width / 2 - 156,
            Screen.height / 2 - 282, chars * 51 + 10 * (chars - 1), 61);

        if (Input.GetKeyUp(KeyCode.Mouse0) && floatingItem != null)
        {
            if (charsPos.Contains(mousePos))
            {
                int charIndex = (int)(mousePos.x - charsPos.x - 10 * 
                    (chars - 1)) / 51;
                selectedChar.Items.Bag.Remove(floatingItem);
                floatingItem.State = ItemState.Positioned;
                BaseChar character = GameMaster.instance.characters[charIndex];
                putInBag(floatingItem, character);
            }
            else if (armorPos.Contains(mousePos) &&
                floatingItem is Armor)
            {
                if (selectedChar.Items.Armor != null)
                {
                    Armor armor = (Armor)selectedChar.Items.Armor;
                    putInBag(armor);
                    selectedChar.getSecondaryAttr((int)SecondaryAttrNames.
                            Defense).gainedValue -= armor.Defense;
                }

                armorSlotContent = new GUIContent(floatingItem.Image, 
                    GUIHelper.getInfo(floatingItem));

                selectedChar.Items.Bag.Remove(floatingItem);
                floatingItem.State = ItemState.Positioned;
                selectedChar.Items.Armor = floatingItem;
                selectedChar.getSecondaryAttr((int)SecondaryAttrNames.Defense).
                    gainedValue += ((Armor)floatingItem).Defense;
            }
            else if (slot1Pos.Contains(mousePos) &&
                !(floatingItem is Armor))
            {
                if (selectedChar.Items.Slot1 != null)
                    putInBag(selectedChar.Items.Slot1);

                item1SlotContent = new GUIContent(floatingItem.Image,
                    GUIHelper.getInfo(floatingItem));
                selectedChar.Items.Bag.Remove(floatingItem);
                floatingItem.State = ItemState.Positioned;
                selectedChar.Items.Slot1 = floatingItem;
            }
            else if (slot2Pos.Contains(mousePos) &&
                !(floatingItem is Armor))
            {
                if (selectedChar.Items.Slot2 != null)
                    putInBag(selectedChar.Items.Slot2);

                item2SlotContent = new GUIContent(floatingItem.Image,
                    GUIHelper.getInfo(floatingItem));
                selectedChar.Items.Bag.Remove(floatingItem);
                floatingItem.State = ItemState.Positioned;
                selectedChar.Items.Slot2 = floatingItem;
            }

            int index = selectedChar.Items.Bag.IndexOf(floatingItem);
            if (index > -1)
                selectedChar.Items.Bag[index].State = ItemState.Positioned;
            else if (floatingItem.State == ItemState.Floating)
            {
                floatingItem.State = ItemState.Positioned;
                putInBag(floatingItem);
                if (floatingItem is Armor)
                    selectedChar.getSecondaryAttr((int)SecondaryAttrNames.
                        Defense).gainedValue -= ((Armor)floatingItem).Defense;
            }
            floatingItem = null;
            Screen.showCursor = true;
        }
    }

    void putInBag(Item item, BaseChar character)
    {
        foreach (Item i in character.Items.Bag)
            if (i.Name.Equals(item.Name) && i != floatingItem)
            {
                i.Quantity++;
                return;
            }

        character.Items.Bag.Add(item);
    }

    void putInBag(Item item)
    {
        putInBag(item, selectedChar);
    }

    void showItems()
    {
        GUIContent content;

        RectOffset btnOffset = skin.FindStyle("Button").padding;
        int padding = btnOffset.top + btnOffset.bottom;
        int totalHeight = 0;
        foreach (Item item in selectedChar.Items.Bag)
        {
            if (selectedToggle == Toggle.Weapons && !(item is Weapon))
                continue;
            if (selectedToggle == Toggle.MedicineAndArmor &&
                !(item is Medication || item is Armor))
                continue;
            if (selectedToggle == Toggle.Keys && !(item is Key))
                continue;
            if (item.State != ItemState.Positioned)
                continue;
            totalHeight += item.Image.height + padding;
        }
        
        scrollPos = GUI.BeginScrollView(new Rect(11, 78, 212, 475), scrollPos,
            new Rect(0, 0, 192, totalHeight));
        int offset = 0;
        foreach(Item item in selectedChar.Items.Bag)
        {
            if (selectedToggle == Toggle.Weapons && !(item is Weapon))
                continue;
            if (selectedToggle == Toggle.MedicineAndArmor &&
                !(item is Medication || item is Armor))
                continue;
            if (selectedToggle == Toggle.Keys && !(item is Key))
                continue;
            
            //Workaround for RepeatButton bug...
            if (item.State != ItemState.Positioned)
                content = new GUIContent(new Texture2D(0, 0));
            else
                content = new GUIContent(item.Image, GUIHelper.getInfo(item));

            if (GUI.RepeatButton(new Rect(0, offset, 192, 
                item.Image.height + padding), content))
            {
                if (floatingItem == null)
                {
                    if (item.Quantity > 1)
                    {
                        Item clone = item.Clone();
                        clone.Quantity = 1;
                        clone.State = ItemState.Floating;
                        item.Quantity--;
                        floatingItem = clone;
                    }
                    else
                    {
                        item.State = ItemState.Floating;
                        floatingItem = item;
                    }
                }
            }

            if (item.State == ItemState.Positioned)
            {
                if (item.Quantity > 1)
                    GUI.Label(new Rect(0, offset, 192, item.Image.height + 
                        padding), item.Quantity.ToString(), "quantity");
                offset += item.Image.height + padding;
            }
        }
        GUI.EndScrollView();
    }

    void showItemToggles()
    {
        GUIContent content = new GUIContent("", "Weapons");
        if (GUI.Toggle(new Rect(17, 20, 61, 38),
           selectedToggle == Toggle.Weapons, content, "gun toggle"))
            selectedToggle = Toggle.Weapons;
        content = new GUIContent("", "Healing & Armor");
        if (GUI.Toggle(new Rect(88, 20, 61, 38),
           selectedToggle == Toggle.MedicineAndArmor, content, 
           "medicine toggle"))
            selectedToggle = Toggle.MedicineAndArmor;
        content = new GUIContent("", "Key Items");
        if (GUI.Toggle(new Rect(158, 20, 61, 38),
           selectedToggle == Toggle.Keys, content, "key toggle"))
            selectedToggle = Toggle.Keys;
    }

    void showBottomButtons()
    {
        if (GUI.Button(new Rect(9, 569, 482, 27), "Skills", "skills button"))
        {
            Messenger.Broadcast("toggleSkillTreeVisibility");
        }
        if (GUI.Button(new Rect(665, 569, 125, 27), "Close", "close button"))
        {
            toggleVisibility();
        }
    }

    void showBackground()
    {
        Texture2D background = Helper.getImage("Inventory/inventory");
        GUI.DrawTexture(new Rect(0, 0, 800, 600), background);
    }
}
