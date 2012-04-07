using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaMaster: GameMaster
{
    public List<GameObject> characterGOs;

    protected override void instantiateCharacters()
    {
        instance.inCombat = true;
        GameObject parent = new GameObject("Characters");
        GameObject spawnPoint;
        for (int i = 1; i <= characters.Count; i++)
        {
            spawnPoint = GameObject.Find("Spawn point " + i);
            Vector3 pos = spawnPoint.transform.position;
            GameObject go = (GameObject)Instantiate(characterGOs[i - 1]);
            pos.y = go.transform.position.y;
            go.transform.position = pos;
            go.transform.parent = parent.transform;
            characters[i - 1].gameObject = go;
            GameObject.Destroy(spawnPoint);
        }
    }
}
