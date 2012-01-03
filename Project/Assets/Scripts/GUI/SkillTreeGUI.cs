using UnityEngine;
using System.Collections;

public class SkillTreeGUI: MonoBehaviour
{
    public GUISkin skin;
    public static SkillTreeGUI instance = null;
    public Texture2D tick;

    private bool show = false;
    private bool tooltipEnabled = false;
    private BaseChar selectedChar;

    public bool Visible
    {
        get { return show; }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Messenger.AddListener("toggleSkillTreeVisibility", toggleVisibility);
    }

    void toggleVisibility()
    {
        show = !show;
        if (!show)
        {
            Messenger<bool>.Broadcast("enable movement", true);
            MyCamera.instance.controllingEnabled = true;
            Messenger<bool>.Broadcast("enable phrases", true);
            return;
        }
        
        if (CharacterScreen.instance.Visible)
            Messenger.Broadcast("toggleCharScreenVisibility");
        if (InventoryGUI.instance.Visible)
            Messenger.Broadcast("toggleInventoryVisibility");

        Messenger<bool>.Broadcast("enable movement", false);
        MyCamera.instance.controllingEnabled = false;
        Messenger<bool>.Broadcast("enable phrases", false);
        
        int charIndex = GameMaster.instance.selectedChar;
        selectedChar = GameMaster.instance.characters[charIndex];
    }

    void OnGUI()
    {
        if (!show)
            return;
        GUI.skin = skin;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 400,
            Screen.height / 2 - 300, 800, 600));
        GUI.DrawTexture(new Rect(0, 0, 800, 600),
            selectedChar.CharClass.SkillTree.Background);
        showHeader();
        showSkills();
        showCloseButton();
        GUI.EndGroup();
        showTooltip();
    }

    void showHeader()
    {
        GUI.Label(new Rect(360, 5, 100, 20), "Skill Tree", "Title");
        int skillPoints = selectedChar.CharClass.SkillTree.SkillPoints;
        GUI.Label(new Rect(650, 10, 150, 20), "Skill Points: " + skillPoints, 
            "Skill Points");
        GUI.DrawTexture(new Rect(310, 38, 45, 50), selectedChar.Image, 
            ScaleMode.ScaleToFit, true);
        GUI.Label(new Rect(369, 35, 150, 20), selectedChar.charName);
        GUI.Label(new Rect(369, 55, 150, 20),
            "Class: " + selectedChar.CharClass.Name);
        GUI.Label(new Rect(369, 75, 150, 20), "Level: " + selectedChar.level);
    }

    void showSkills()
    {
        SkillTree skillTree = selectedChar.CharClass.SkillTree;
        int width = SkillTrees.btnWidth;
        int height = SkillTrees.btnHeight;
        float x, y;
        foreach (var skillPos in skillTree.SkillPos)
        {
            x = skillPos.Value.x;
            y = skillPos.Value.y;
            Skill skill = skillPos.Key;
            GUIContent content = new GUIContent(skill.Name, getInfo(skill));
            bool dependenciesMet = true;
            foreach(string skillName in skill.Dependencies)
                if (!skillTree.Skills[skillName].Known)
                {
                    dependenciesMet = false;
                    break;
                }
            Color color = new Color();
            if (skill.Rank == skill.MaxRank || skillTree.SkillPoints == 0 || 
                !dependenciesMet)
                GUI.enabled = false;
            if (!GUI.enabled && skill.Known)
            {
                color = GUI.color;
                GUI.color = new Color(1, 1, 1, 2);
            }
            //Ignore right clicks and middle clicks
            if (Event.current.button > 0 
                && Event.current.type != EventType.Repaint 
                && Event.current.type != EventType.Layout)
                Event.current.Use();

            if (GUI.Button(new Rect(x, y, width, height), content))
            {
                skill.Known = true;
                skill.Rank++;
                skillTree.SkillPoints--;
            }
            x = x + width - tick.width - 2;
            y = y + height - tick.height - 2;
            for (int i = 0; i < skill.Rank; i++)
                GUI.DrawTexture(new Rect(x - i * (tick.width - 2), y,
                    tick.width, tick.height), tick);
            if (!GUI.enabled && skill.Known)
                GUI.color = color;
            GUI.enabled = true;
        }
    }

    string getInfo(Skill skill)
    {
        string info = skill.Name + '\n';
        info += "Rank: " + skill.Rank + "/" + skill.MaxRank + '\n';
        if (skill.Dependencies.Length > 0)
        {
            info += "Requires: ";
            for (int i = 0; i < skill.Dependencies.Length; i++)
            {
                info += skill.Dependencies[i];
                if (i + 1 != skill.Dependencies.Length)
                    info += ", ";
            }
            info += "\n";
        }
        info += skill.Description;

        return info;
    }

    void showTooltip()
    {
        if (!tooltipEnabled || GUI.tooltip.Equals(""))
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
        int nameLength = GUI.tooltip.IndexOf('\n');
        string skillName = GUI.tooltip.Substring(0, nameLength);
        string description = GUI.tooltip.Replace(skillName + '\n', "");
        GUI.Box(new Rect(mouseX + 11 - xOffset, mouseY - yOffset - 7,
            maxWidth + 18, height + 14), "");
        skin.FindStyle("tooltip").normal.textColor = Color.yellow;
        GUI.Label(new Rect(mouseX + 20 - xOffset, mouseY - yOffset,
            190, 23), skillName, "tooltip");
        skin.FindStyle("tooltip").normal.textColor = new Color(203f / 255f,
            220f / 255f, 220f / 255f);
        GUI.Label(new Rect(mouseX + 20 - xOffset, mouseY + 17 - yOffset,
            190, height - 23), description, "tooltip");
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
            tooltipEnabled = true;
        else
            tooltipEnabled = false;
    }

    void showCloseButton()
    {
        if (GUI.Button(new Rect(675, 569, 105, 23), "Close"))
            toggleVisibility();
    }
}
