using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Phrases: MonoBehaviour
{
    private Camera myCamera;
    private string text = "";
    private bool showEmpty = true;
    private int index = 0;
    public List<string> phrases;
    private GUIStyle style;
    private bool styleInitialized = false;
    private Vector2 size;
    private bool phrasesShown = true;

    // Use this for initialization
    void Start()
    {
        myCamera = MyCamera.instance.camera;
        StartCoroutine(changeText());
        Messenger<bool>.AddListener("enable phrases", showPhrases);
    }

    void OnGUI()
    {
        if (!styleInitialized)
        {
            style = new GUIStyle("box");
            style.padding = new RectOffset(3, 3, 0, 0);
            style.normal.textColor = new Color(0.75f, 0.75f, 0.75f);
            size = new Vector2();
            styleInitialized = true;
        }
        
        if (!text.Equals(""))
        {
            size = style.CalcSize(new GUIContent(text));
            Vector3 transformPos = transform.position;
            transformPos.y += 7.9f;
            Vector3 pos = myCamera.WorldToScreenPoint(transformPos);
            Rect labelPos = new Rect(pos.x - 35, Screen.height - pos.y, size.x, 
                size.y);
            GUI.Label(labelPos, text, style);
        }
        
    }

    IEnumerator changeText()
    {
        if (showEmpty)
        {
            text = "";
            showEmpty = false;
            yield return new WaitForSeconds(5);
        }
        else
        {
            text = phrases[index % phrases.Count];
            //style.normal.background = getRectangle(style, text, Color.black);
            //style.normal.textColor = new Color(0.75f, 0.75f, 0.75f);
            index++;
            showEmpty = true;
            yield return new WaitForSeconds(3);
        }
        if (phrasesShown)
            StartCoroutine(changeText());
    }

    public void showPhrases(bool show)
    {
        if (show && !phrasesShown)
        {
            StartCoroutine(changeText());
            phrasesShown = true;
        }
        else if (!show && phrasesShown)
        {
            text = "";
            showEmpty = true;
            phrasesShown = false;
            StopCoroutine("changeText");
        }
    }

    private Texture2D getRectangle(GUIStyle style, string text, Color color)
    {
        size = style.CalcSize(new GUIContent(text));
        Texture2D texture = new Texture2D((int)size.x, (int)size.y);
        texture.SetPixel(0, 0, color);
        for (int i = 0; i < (int)size.x; i++)
        {
            for (int j = 0; j < (int)size.y; j++)
            {
                texture.SetPixel(i, j, color);
            }
        }
        texture.Apply();

        return texture;
    }
}
