using UnityEngine;
using System.Collections;

public class Monster: MonoBehaviour
{
    void OnMouseUp()
    {
        bool success;
        int luck = Random.Range(0, 10);
        Debug.Log("Luck: " + luck);
        if (luck > 4)
            success = Shooter.instance.shootAt(gameObject, false);
        else
            success = Shooter.instance.shootAt(gameObject, true);
        if (!success)
            Debug.Log("Target blocked.");
    }
}
