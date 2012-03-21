using UnityEngine;
using System.Collections;

public class InGameMenu: MonoBehaviour
{
    public static InGameMenu instance = null;
    public GUISkin skin;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void OnGUI()
    {
        GUI.skin = skin;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 110,
            160, 220));
        GUI.Box(new Rect(0, 0, 160, 220), "MENU");
        if (GUI.Button(new Rect(7, 25, 146, 30), "RESUME"))
            enabled = false;
        GUI.Button(new Rect(7, 65, 146, 30), "SAVE GAME");
        GUI.Button(new Rect(7, 105, 146, 30), "LOAD GAME");
        GUI.Button(new Rect(7, 145, 146, 30), "SETTINGS");
        if (GUI.Button(new Rect(7, 185, 146, 30), "EXIT"))
        {
            //Messenger.RemoveAllListeners();
            Application.LoadLevel("MainMenu");
            Destroy(gameObject);
        }
            
        GUI.EndGroup();
    }

    void OnEnable()
    {
        if (CharacterScreen.instance.enabled)
            CharacterScreen.instance.enabled = false;
        if (InventoryGUI.instance.enabled)
            InventoryGUI.instance.enabled = false;
        if (TradeScreen.instance.enabled)
            TradeScreen.instance.enabled = false;
        if (SkillTreeGUI.instance.enabled)
            SkillTreeGUI.instance.enabled = false;

        Messenger<bool>.Broadcast("enable movement", false);
        MyCamera.instance.controllingEnabled = false;
        if (!GameMaster.instance.inCombat)
            Messenger<bool>.Broadcast("enable phrases", false);
        HUD.instance.clickable = false;
    }

    void OnDisable()
    {
        Messenger<bool>.Broadcast("enable movement", true);
        MyCamera.instance.controllingEnabled = true;
        HUD.instance.clickable = true;
        if (!GameMaster.instance.inCombat)
            Messenger<bool>.Broadcast("enable phrases", true);
    }
}
