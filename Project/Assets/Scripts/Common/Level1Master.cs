using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level1Master: GameMaster
{
    public List<GameObject> characterGOs;

    protected override void instantiateCharacters()
    {
        GameObject parent = new GameObject("Characters");
        GameObject spawnPoint;
        for (int i = 1; i <= characters.Count; i++)
        {
            spawnPoint = GameObject.Find("Spawn point " + i);
            Vector3 pos = spawnPoint.transform.position;
            GameObject go = (GameObject)Instantiate(characterGOs[i - 1]);
            go.name = characters[i - 1].charName;
            go.transform.position = pos;
            characters[i - 1].gameObject = go;
            go.transform.parent = parent.transform;
            if (i == 1)
                MyCamera.instance.setTarget(go.transform.Find("CameraTarget"));
            GameObject.Destroy(spawnPoint);
        }
    }
}
