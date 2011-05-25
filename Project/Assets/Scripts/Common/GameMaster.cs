using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour
{
	public GameObject playerChar;
	public GameObject gameSettings;

	private GameObject pc;
	private BaseChar charClass;
	
	void Awake()
	{
		MyCamera.acquireCamera();
	}
	
	void Start ()
	{
		GameObject spawnPoint = GameObject.Find("Player Spawn Point");

		if (spawnPoint == null)
		{
			spawnPoint = new GameObject("Player Spawn Point");
			spawnPoint.transform.position = new Vector3(230, 0, 210);
		}

		pc = GameObject.Find("Player Character");
		
		if (pc == null)
		{
			pc = (GameObject)Instantiate(playerChar, spawnPoint.transform.position,
			                             Quaternion.identity);
			pc.name = "Player Character";
			pc.transform.localScale = new Vector3(3f,3f,3f);
		}
		MyCamera.instance.TargetLookAt = pc.transform;
		
		charClass = pc.GetComponent<BaseChar>();
		loadChar();
	}
	
	// Update is called once per frame
	private void loadChar ()
	{
		GameObject gs = GameObject.Find("Game Settings");
		if (gs == null)
		{
			gs = (GameObject)Instantiate(gameSettings, Vector3.zero, Quaternion.identity);
			gs.name = "Game Settings";
		}

		GameSettings gsClass = gs.GetComponent<GameSettings>();
		gsClass.loadChar();
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.LoadLevel("MainMenu");
	}
}
