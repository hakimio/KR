using UnityEngine;
using System.Collections;

public class OSD: MonoBehaviour
{
    bool tooltipVisible = false;
    string charName;
    int curHP, maxHP;
    GameObject charGO;
    public static OSD instance = null;
    bool gameLost = false;
    bool gameWon = false;
    bool showRestartScreen = false;
    public GUISkin skin;

    void Awake()
    {
        instance = this;
    }

    void OnGUI()
    {
        GUI.skin = skin;
        if (tooltipVisible)
            drawTooltip();
        if (gameLost && !showRestartScreen)
            showHugeMessage("You Lose");
        if (gameWon && !showRestartScreen)
            showHugeMessage("You Win");
        if (showRestartScreen)
            drawRestartGameScreen();
    }

    void showHugeMessage(string message)
    {
        GUIStyle style = GUI.skin.GetStyle("box");
        int oldSize = style.fontSize;
        style.fontSize = 72;
        if (GUI.Button(new Rect(Screen.width / 2 - 175,
            Screen.height / 2 - 50, 350, 100), message, style))
        {
            showRestartScreen = true;
        }
        style.fontSize = oldSize;
    }

    void drawRestartGameScreen()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 30,
            160, 60));
        GUI.Box(new Rect(0, 0, 160, 60), "RESTART COMBAT?");
        if (GUI.Button(new Rect(7, 25, 70, 30), "YES"))
        {
            Messenger.RemoveAllListeners();
            Application.LoadLevel(Application.loadedLevel);
        }
        if (GUI.Button(new Rect(83, 25, 70, 30), "NO"))
        {
            Messenger.RemoveAllListeners();
            Application.LoadLevel("MainMenu");
        }
        GUI.EndGroup();
    }

    void drawTooltip()
    {
        string tooltipText = charName + "\nHP: " + curHP + " / " + maxHP;
        float mouseX = Input.mousePosition.x;
        float mouseY = Screen.height - Input.mousePosition.y;
        float maxWidth = 0;
        float minWidth = 0;
        GUIStyle style = GUI.skin.GetStyle("Label");
        style.CalcMinMaxWidth(new GUIContent(tooltipText), out minWidth,
            out maxWidth);
        float height = style.CalcHeight(new GUIContent(GUI.tooltip), maxWidth);
        GUI.Box(new Rect(mouseX + 11, mouseY - 7, maxWidth + 18, height + 14), 
            tooltipText);
    }

    public void showTooltip(Monster monster)
    {
        enabled = true;
        tooltipVisible = true;
        charName = monster.monsterName;
        curHP = monster.HP;
        maxHP = monster.totalHP;
        charGO = monster.gameObject;
    }

    public void updateTooltip(Monster monster)
    {
        if (this.charGO != monster.gameObject || !tooltipVisible)
            return;
        curHP = monster.HP;
        maxHP = monster.totalHP;
    }

    public void showTooltip(BaseChar character)
    {
        enabled = true;
        tooltipVisible = true;
        charName = character.charName;
        curHP = character.CurrentHP;
        maxHP = character.CurrentHP + character.LostHP;
    }

    public void hideTooltip(GameObject charGO)
    {
        if (this.charGO == charGO)
        {
            tooltipVisible = false;
            enabled = false;
        }
    }

    public void hideTooltip()
    {
        hideTooltip(charGO);
    }

    public void showGameLostScreen()
    {
        enabled = true;
        gameLost = true;
    }

    public void showGameWonScreen()
    {
        enabled = true;
        gameWon = true;
        HUD.instance.clickable = false;
    }

    public GameObject getTooltipMonster()
    {
        return charGO;
    }
}
