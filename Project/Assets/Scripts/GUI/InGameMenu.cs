using UnityEngine;
using System.Collections;

public class InGameMenu: MonoBehaviour
{
    private bool show = false;
    public static InGameMenu instance = null;
    public GUISkin skin;

    void Awake()
    {
        instance = this;
    }

    void OnGUI()
    {
        if (!show)
            return;
        GUI.skin = skin;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 110,
            160, 220));
        GUI.Box(new Rect(0, 0, 160, 220), "MENU");
        if (GUI.Button(new Rect(7, 25, 146, 30), "RESUME"))
            toggleVisibility();
        GUI.Button(new Rect(7, 65, 146, 30), "SAVE GAME");
        GUI.Button(new Rect(7, 105, 146, 30), "LOAD GAME");
        GUI.Button(new Rect(7, 145, 146, 30), "SETTINGS");
        if (GUI.Button(new Rect(7, 185, 146, 30), "EXIT"))
        {
            Messenger.RemoveAllListeners();
            Application.LoadLevel("MainMenu");
        }
            
        GUI.EndGroup();
    }

    public void toggleVisibility()
    {
        show = !show;

        if (!show)
        {
            Messenger<bool>.Broadcast("enable movement", true);
            MyCamera.instance.controllingEnabled = true;
            Messenger<bool>.Broadcast("enable phrases", true);
            HUD.instance.clickable = true;
            return;
        }

        Messenger<bool>.Broadcast("enable movement", false);
        MyCamera.instance.controllingEnabled = false;
        Messenger<bool>.Broadcast("enable phrases", false);
        HUD.instance.clickable = false;
    }
}
