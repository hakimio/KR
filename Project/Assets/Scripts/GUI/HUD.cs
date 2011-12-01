using UnityEngine;
using System.Collections;

public class HUD: MonoBehaviour
{
    private const int BUTTON_WIDTH = 124;
    private const int BUTTON_HEIGHT = 24;
    private const int OFFSET = 5;
    public static HUD instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void OnGUI()
    {
        showButtons();
    }

    void showButtons()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - BUTTON_WIDTH / 2,
            Screen.height - BUTTON_HEIGHT - OFFSET, BUTTON_WIDTH,
            BUTTON_HEIGHT), "Inventory"))
            Messenger.Broadcast("toggleInventoryVisibility");
        if (GUI.Button(new Rect(Screen.width / 2 + BUTTON_WIDTH / 2 + OFFSET,
            Screen.height - BUTTON_HEIGHT - OFFSET, BUTTON_WIDTH,
            BUTTON_HEIGHT), "Character"))
            Messenger.Broadcast("toggleCharScreenVisibility");
    }
}
