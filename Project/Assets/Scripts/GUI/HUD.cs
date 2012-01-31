using UnityEngine;
using System.Collections;

public class HUD: MonoBehaviour
{
    private const int BUTTON_WIDTH = 124;
    private const int BUTTON_HEIGHT = 24;
    private const int OFFSET = 6;
    public static HUD instance = null;
    public bool clickable = true;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void OnGUI()
    {
        GUI.enabled = clickable;
        Color color = GUI.color;
        if (!clickable)
            GUI.color = new Color(1, 1, 1, 2);
        showButtons();
        if (!clickable)
            GUI.color = color;
        GUI.enabled = true;
    }

    void showButtons()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - BUTTON_WIDTH * 2f 
            - 1.5f * OFFSET, Screen.height - BUTTON_HEIGHT - OFFSET, 
            BUTTON_WIDTH, BUTTON_HEIGHT), "Trade Items"))
            Messenger.Broadcast("toggleTradeScreenVisibility");
        if (GUI.Button(new Rect(Screen.width / 2 - BUTTON_WIDTH 
            - 0.5f * OFFSET, Screen.height - BUTTON_HEIGHT - OFFSET, 
            BUTTON_WIDTH, BUTTON_HEIGHT), "Skills"))
            Messenger.Broadcast("toggleSkillTreeVisibility");
        if (GUI.Button(new Rect(Screen.width / 2 + 0.5f * OFFSET,
            Screen.height - BUTTON_HEIGHT - OFFSET, BUTTON_WIDTH,
            BUTTON_HEIGHT), "Inventory"))
            Messenger.Broadcast("toggleInventoryVisibility");
        if (GUI.Button(new Rect(Screen.width / 2 + BUTTON_WIDTH 
            + 1.5f * OFFSET, Screen.height - BUTTON_HEIGHT - OFFSET, 
            BUTTON_WIDTH, BUTTON_HEIGHT), "Character"))
            Messenger.Broadcast("toggleCharScreenVisibility");
    }
}
