using UnityEngine;
using System.Collections;

public class DescriptionOnClick: MonoBehaviour
{
    public string message;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1))
            HUD.instance.addMessage(message);
    }
}
