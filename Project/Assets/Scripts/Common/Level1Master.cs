using UnityEngine;
using System.Collections;

public class Level1Master: GameMaster
{
    public GameObject characterGO;

    protected override void instantiateCharacters()
    {
        GameObject spawnPoint = GameObject.Find("Spawn Point");
        GameObject pc = (GameObject)Instantiate(characterGO,
                spawnPoint.transform.position, Quaternion.identity);
        pc.name = "Player Character";
        pc.transform.localScale = new Vector3(3f, 3f, 3f);
        MyCamera.instance.TargetLookAt = pc.transform;
        characters[1].gameObject = pc;
        GameObject.Destroy(spawnPoint);
    }
}
