using UnityEngine;
using System.Collections;

public class Obstacle: MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Messenger.Broadcast("obstaclePositioned");
    }
}
