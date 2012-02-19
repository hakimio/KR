using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class DebugInfoDisplay: MonoBehaviour
{
    string widthStr, heightStr;

    void Start()
    {
        GameObject groundGO = GameObject.Find("Ground");
        widthStr = groundGO.transform.localScale.x.ToString();
        heightStr = groundGO.transform.localScale.z.ToString();
    }

    void OnGUI()
    {
        string widthStrToTest, heightStrToTest;
        GUI.BeginGroup(new Rect(5, 5, 100, 94));
        GUI.Box(new Rect(0, 0, 100, 94), "");
        GUI.Label(new Rect(8, 8, 60, 23), "Width: ");
        widthStrToTest = GUI.TextField(new Rect(53, 8, 42, 20), widthStr);
        GUI.Label(new Rect(8, 33, 60, 23), "Height: ");
        heightStrToTest = GUI.TextField(new Rect(53, 33, 42, 20), heightStr);

        if (Regex.IsMatch(widthStrToTest, @"^[0-9]*\.?[0-9]*$"))
            widthStr = widthStrToTest;
        if (Regex.IsMatch(heightStrToTest, @"^[0-9]*\.?[0-9]*$"))
            heightStr = heightStrToTest;

        if (GUI.Button(new Rect(20, 61, 60, 25), "Update") &&
            !CharacterMovement.instance.IsMoving)
        {
            try
            {
                float width = float.Parse(widthStr);
                float height = float.Parse(heightStr);
                GridManager.instance.setGroundSize(width, height);
            }
            catch (FormatException) {}
        }
        GUI.EndGroup();

        Tile selTile = GridManager.instance.selectedTile;
        if (selTile == null)
            return;
        GUI.Box(new Rect(Screen.width - 70, 5, 65, 25), selTile.ToString());
    }
}
