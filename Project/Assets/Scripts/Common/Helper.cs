using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public static class Helper
{
    public static string[] cutPhrase(float availableSpace, float width, 
        string wholePhrase, GUIStyle style)
    {
        List<string> pieces = new List<string>();
        wholePhrase = wholePhrase.Replace("\n", "\n ");
        String[] words = wholePhrase.Split(' ');

        float lineHeight = style.CalcSize(new GUIContent(words[0])).y;
        lineHeight -= 2 * style.padding.top;
        int nrOfLines = (int)(availableSpace / lineHeight);
        float curWidth;
        String piece = ""; 
        String line = "";
        int i = 0;

        while (i < words.Length)
        {
            //piece = "";
            for (int j = 0; j < nrOfLines; j++)
            {
                line = "";
                if (i == words.Length)
                    break;
                curWidth = 0;
                while (curWidth < width && i < words.Length)
                {
                    if (!line.Equals("") && line[line.Length-1] != '\n')
                        line += " " + words[i];
                    else
                        line += words[i];

                    if (words[i].IndexOf('\n') > -1 && j + 1 == nrOfLines)
                    {
                        i++;
                        j++;
                        break;
                    }
                    else if (words[i].IndexOf('\n') > -1)
                        j++;
                    if (i + 1 < words.Length)
                        curWidth = style.CalcSize(new GUIContent(line + 
                           " " + words[i+1])).x;
                    
                    if (j + 1 == nrOfLines && i + 1 < words.Length)
                    {
                        float nextWidth = curWidth + style.
                            CalcSize(new GUIContent(" »")).x;

                        if (nextWidth > width)
                        {
                            curWidth = curWidth + style.CalcSize(
                                new GUIContent(" »")).x;
                        }
                    }
                    i++;
                }
                if (!piece.Equals(""))
                    piece += " " + line;
                else
                    piece += line;
            }
            piece = piece.Trim();

            if (i == words.Length && !piece.Equals(""))
            {
                pieces.Add(piece);
                piece = "";
            }
            else if (!piece.Equals(""))
            {
                pieces.Add(piece + " »");
                piece = "";
            }
        }

        return pieces.ToArray();
    }

    public static IEnumerator transitionCamera(Transform target, 
        bool cameraControlEnabled, string npcName)
    {
        float transitionDuration = 2.1f;
        float t = 0.0f;
        Transform transform = MyCamera.instance.transform;
        Vector3 startingPos = transform.position;
        Quaternion rotation = transform.rotation;

        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.position = Vector3.Lerp(startingPos, target.position, t);
            transform.rotation = Quaternion.Slerp(rotation, target.rotation, t);
            if (transform.position == target.position && cameraControlEnabled)
            {
                MyCamera.instance.controllingEnabled = true;
                Messenger<bool>.Broadcast("enable movement", true);
            }
            else if (transform.position == target.position)
                Messenger<string>.Broadcast("show conversation", npcName);
            yield return 0;
        }
    }
}
