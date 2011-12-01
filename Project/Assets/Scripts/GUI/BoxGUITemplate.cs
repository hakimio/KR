using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BoxGUITemplate: MonoBehaviour
{
    public GUISkin skin;
    public static BoxGUITemplate instance = null;
    private bool show = false;
    public string boxName = "box1";
    private BaseChar selectedChar;
    protected List<Item> storedItems;
    private bool[] toggles;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Messenger<string>.AddListener("toggleBoxVisibility", toggleVisibility);
        storedItems = new List<Item>();
        addItems();
    }

    void toggleVisibility(string name)
    {
        if (!this.boxName.Equals(name))
            return;

        Clicker.instance.hitBox = false;

        show = !show;

        if (!show)
        {
            Messenger<bool>.Broadcast("enable movement", true);
            MyCamera.instance.controllingEnabled = true;
            Messenger<bool>.Broadcast("enable phrases", true);
            HUD.instance.enabled = true;
            return;
        }

        if (storedItems.Count == 0)
        {
            MessageBox.instance.showMessage("The box is empty.");
            show = false;
            return;
        }

        toggles = new bool[storedItems.Count];
        int charIndex = GameMaster.instance.selectedChar;
        selectedChar = GameMaster.instance.characters[charIndex];

        Messenger<bool>.Broadcast("enable movement", false);
        MyCamera.instance.controllingEnabled = false;
        Messenger<bool>.Broadcast("enable phrases", false);
        HUD.instance.enabled = false;
    }

    protected abstract void addItems();

    void OnGUI()
    {
        if (!show)
            return;
        GUI.skin = skin;

        float itemsHeight = storedItems.Count * 120 + 37 * 3;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 105,
            Screen.height / 2 - itemsHeight / 2, 210, itemsHeight));
        showItems();
        showButtons();
        GUI.EndGroup();
        showTooltip();
    }

    void showItems()
    {
        for (int i = 0; i < storedItems.Count; i++)
        {
            Item item = storedItems[i];
            Rect rect = new Rect(0, i * 120, 210, 120);
            
            toggles[i] = GUI.Toggle(rect, toggles[i], 
                new GUIContent(item.Image, GUIHelper.getInfo(item)));
            if (item.Quantity > 1)
                GUI.Label(rect, item.Quantity.ToString());
        }
    }

    void showButtons()
    {
        float framesHeight = storedItems.Count * 120;
        if  (GUI.Button(new Rect(0, framesHeight, 210, 37), "Take All"))
        {
            foreach (Item item in storedItems)
                addItemToBag(item);
            storedItems.Clear();
            toggleVisibility(this.boxName);
        }

        if (GUI.Button(new Rect(0, framesHeight + 37, 210, 37), 
            "Take Selected"))
        {
            List<Item> itemsToRemove = new List<Item>();
            foreach (Item item in storedItems)
                if (toggles[storedItems.IndexOf(item)])
                {
                    itemsToRemove.Add(item);
                    addItemToBag(item);
                }
            foreach (Item item in itemsToRemove)
                storedItems.Remove(item);

            toggleVisibility(this.boxName);
        }

        if  (GUI.Button(new Rect(0, framesHeight + 74, 210, 37), "Cancel"))
        {
            toggleVisibility(this.boxName);
        }
    }

    void addItemToBag(Item item)
    {
        bool found = false;
        foreach (Item storedItem in selectedChar.Items.Bag)
            if (item.Name.Equals(storedItem.Name))
            {
                found = true;
                storedItem.Quantity += item.Quantity;
            }
        if (!found)
            selectedChar.Items.Bag.Add(item);
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
}
