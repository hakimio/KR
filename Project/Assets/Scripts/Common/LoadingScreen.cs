using UnityEngine;
using System.Collections;

public class LoadingScreen: MonoBehaviour
{
    public GUISkin skin;
    AsyncOperation async;

    void Start()
    {
        async = Application.LoadLevelAsync("Arena");
    }

    void OnGUI()
    {
        GUI.skin = skin;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 375,
            Screen.height / 2 - 310, 750, 620));
        int percentLoaded = (int)(100 * async.progress);
        GUI.Box(new Rect(15, 560, 720, 35), percentLoaded + "%");
        GUI.Box(new Rect(15, 560, percentLoaded / 100f * 720, 35), "");
        GUI.EndGroup();
    }
}
