using UnityEngine;
using System.Collections;

public class TradeScreen: MonoBehaviour
{
    public GUISkin skin;
    public static TradeScreen instance = null;

    private Texture2D bottomPanel;
    private Texture2D background;
    private Item floatingItem = null;
    private Vector2 mousePos = new Vector2();
    private Vector2[] scrollPos;
    private int itemTakenFromChar;

    private enum Toggle
    {
        Weapons,
        MedicineAndArmor,
        Keys
    }
    private Toggle[] selectedToggles;

    void Awake()
    {
        instance = this;
        bottomPanel = Helper.getImage("Trade Screen/bottom line");
        background = Helper.getImage("Trade Screen/char items");
    }

    void OnEnable()
    {
        selectedToggles = new Toggle[GameMaster.instance.characters.Count];
        scrollPos = new Vector2[selectedToggles.Length];
        for (int i = 0; i < selectedToggles.Length; i++)
        {
            selectedToggles[i] = Toggle.Weapons;
            scrollPos[i] = new Vector2();
        }

        if (CharacterScreen.instance.enabled)
            CharacterScreen.instance.enabled = false;
        if (InventoryGUI.instance.enabled)
            InventoryGUI.instance.enabled = false;
        if (SkillTreeGUI.instance.enabled)
            SkillTreeGUI.instance.enabled = false;
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
    }

    BaseChar character;
    float offset;

    void OnGUI()
    {
        GUI.depth = 0;
        GUI.skin = skin;

        float width = 221 * GameMaster.instance.characters.Count;
        GUI.BeginGroup(new Rect(Screen.width/2 - width / 2, 
            Screen.height / 2 - 300, width, 600));
        
        for(int i = 0; i < GameMaster.instance.characters.Count; i++)
        {
            character = GameMaster.instance.characters[i];
            offset = i * 221;
            GUI.DrawTexture(new Rect(i * 221, 0, 221, 562), background);
            showCharInfo();
            showCharItems();
            showItemToggles();
        }
        if (Event.current.type.Equals(EventType.Repaint))
            Graphics.DrawTexture(new Rect(0, 562, width, 38), bottomPanel,
                new Rect((1024 - width)/1024, 0, width / 1024, 1), 0, 0, 0, 0);
        showCloseBtn();
        GUI.EndGroup();
        showDraggedItem();
        showTooltip();
    }

    void showCharInfo()
    {
        GUI.DrawTexture(new Rect(24 + offset, 12, 45, 50), character.Image);
        GUI.Label(new Rect(offset + 83, 9, 150, 20), character.charName);
        GUI.Label(new Rect(offset + 83, 29, 150, 20), 
            "Class: " + character.CharClass.Name);
        GUI.Label(new Rect(offset + 83, 49, 150, 20), 
            "Level: " + character.level);
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

    void showCharItems()
    {
        GUIContent content;
        int charIndex = GameMaster.instance.characters.IndexOf(character);

        RectOffset btnOffset = skin.FindStyle("Button").padding;
        int padding = btnOffset.top + btnOffset.bottom;
        int totalHeight = 0;
        foreach (Item item in character.Items.Bag)
        {
            if (selectedToggles[charIndex] == Toggle.Weapons 
                && !(item is Weapon))
                continue;
            if (selectedToggles[charIndex] == Toggle.MedicineAndArmor &&
                !(item is Medication || item is Armor))
                continue;
            if (selectedToggles[charIndex] == Toggle.Keys && !(item is Key))
                continue;
            if (item.State != ItemState.Positioned)
                continue;
            totalHeight += item.Image.height + padding;
        }

        scrollPos[charIndex] = GUI.BeginScrollView(new Rect(offset + 3, 140, 
            212, 413), scrollPos[charIndex], new Rect(0, 0, 192, totalHeight));
        int itemOffset = 0;
        foreach (Item item in character.Items.Bag)
        {
            if (selectedToggles[charIndex] == Toggle.Weapons 
                && !(item is Weapon))
                continue;
            if (selectedToggles[charIndex] == Toggle.MedicineAndArmor &&
                !(item is Medication || item is Armor))
                continue;
            if (selectedToggles[charIndex] == Toggle.Keys && !(item is Key))
                continue;

            //Workaround for RepeatButton bug...
            if (item.State != ItemState.Positioned)
                content = new GUIContent(new Texture2D(0, 0));
            else
                content = new GUIContent(item.Image, GUIHelper.getInfo(item));

            if (GUI.RepeatButton(new Rect(0, itemOffset, 192,
                item.Image.height + padding), content))
            {
                if (floatingItem == null)
                {
                    itemTakenFromChar = charIndex;
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
                    GUI.Label(new Rect(0, itemOffset, 192, item.Image.height +
                        padding), item.Quantity.ToString(), "quantity");
                itemOffset += item.Image.height + padding;
            }
        }
        GUI.EndScrollView();
    }

    void showItemToggles()
    {
        int charIndex = GameMaster.instance.characters.IndexOf(character);
        GUIContent content = new GUIContent("", "Weapons");
        if (GUI.Toggle(new Rect(offset + 9, 82, 61, 38),
           selectedToggles[charIndex] == Toggle.Weapons, content, 
           "gun toggle"))
            selectedToggles[charIndex] = Toggle.Weapons;
        content = new GUIContent("", "Healing & Armor");
        if (GUI.Toggle(new Rect(offset + 80, 82, 61, 38),
           selectedToggles[charIndex] == Toggle.MedicineAndArmor, content,
           "medicine toggle"))
            selectedToggles[charIndex] = Toggle.MedicineAndArmor;
        content = new GUIContent("", "Key Items");
        if (GUI.Toggle(new Rect(offset + 151, 82, 61, 38),
           selectedToggles[charIndex] == Toggle.Keys, content, "key toggle"))
            selectedToggles[charIndex] = Toggle.Keys;
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

        if (mouseX + maxWidth + 18 > Screen.width)
            xOffset = maxWidth + 31;
        else
            xOffset = 0;
        int nameLength = GUI.tooltip.IndexOf('\n');
        if (nameLength > -1)
        {
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
            GUI.Box(new Rect(mouseX + 11 - xOffset, mouseY - yOffset - 7,
                maxWidth + 18, height + 14), "");
            GUI.Label(new Rect(mouseX + 20 - xOffset, mouseY - yOffset,
                190, height), GUI.tooltip, "tooltip");
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

    void showCloseBtn()
    {
        float left = GameMaster.instance.characters.Count * 221 - 135;
        if (GUI.Button(new Rect(left, 569, 125, 27), "Close", "close button"))
        {
            enabled = false;
        }
    }

    void Update()
    {
        int chars = GameMaster.instance.characters.Count;
        Rect dropPos = new Rect((Screen.width - chars * 221) / 2 + 6,
            Screen.height / 2 - 160, chars * 189 + 32 * (chars - 1), 403);
        int charIndex = (int)(mousePos.x - dropPos.x - 32 * (chars - 1)) / 189;
        BaseChar takenFromChar = GameMaster.instance.
            characters[itemTakenFromChar];

        if (Input.GetKeyUp(KeyCode.Mouse0) && floatingItem != null)
        {
            floatingItem.State = ItemState.Positioned;
            
            if (dropPos.Contains(mousePos) && charIndex != itemTakenFromChar)
            {
                takenFromChar.Items.Bag.Remove(floatingItem);
                BaseChar givenToChar = GameMaster.instance.
                    characters[charIndex];
                putInBag(floatingItem, givenToChar);
            }
            else if (!takenFromChar.Items.Bag.Contains(floatingItem))
                putInBag(floatingItem, takenFromChar);

            floatingItem = null;
            Screen.showCursor = true;
        }
    }
}
