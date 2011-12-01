using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GameMaster : MonoBehaviour
{
	public GameObject playerChar;
	public GameObject gameSettings;
    public List<Conversation> dialogs;
    public List<Message> messages;
    public static GameMaster instance;
    public List<BaseChar> characters;
    public int selectedChar = 0;

	private GameObject pc;
	//private BaseChar charClass;
	
	void Awake()
	{
        instance = this;
        loadDialogs();
        loadMessages();
		MyCamera.acquireCamera();
        instantiatePC();
        characters = new List<BaseChar>();
        characters.Add(Characters.EricFrost);
        characters.Add(Characters.FrostEric);
        Messenger.AddListener("MessageBox Ready", showIntroMessage);
	}

    void showIntroMessage()
    {
        foreach (Message message in messages)
            if (message.Name.Equals("intro"))
            {
                MessageBox.instance.showMessage(message.Content);
                break;
            }
    }

    private void loadDialogs()
    {
        dialogs = new List<Conversation>();
        Uri uri = new Uri(Directory.GetCurrentDirectory() +
            "/Assets/Dialogs");
        foreach (String file in Directory.GetFiles(uri.LocalPath))
        {
            if (Path.GetExtension(file).Equals(".meta"))
                continue;
            Conversation conv = XmlReader.read(file);
            dialogs.Add(conv);
        }
    }

    private void loadMessages()
    {
        messages = new List<Message>();
        Uri uri = new Uri(Directory.GetCurrentDirectory() +
            "/Assets/Messages");
        foreach (String file in Directory.GetFiles(uri.LocalPath))
        {
            if (Path.GetExtension(file).Equals(".meta"))
                continue;
            string name = Path.GetFileNameWithoutExtension(file);
            string content = TxtReader.read(file);
            messages.Add(new Message(name, content));
        }
    }

    private void instantiatePC()
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
            pc = (GameObject)Instantiate(playerChar,
                spawnPoint.transform.position, Quaternion.identity);
            pc.name = "Player Character";
            pc.transform.localScale = new Vector3(3f, 3f, 3f);
        }
        MyCamera.instance.TargetLookAt = pc.transform;
        GameObject.Destroy(spawnPoint);
        //charClass = pc.GetComponent<BaseChar>();
    }

	void Start ()
	{
		//loadChar();
	}
	
	// Update is called once per frame
	private void loadChar ()
	{
		GameObject gs = GameObject.Find("Game Settings");
		if (gs == null)
		{
			gs = (GameObject)Instantiate(gameSettings, Vector3.zero, 
                Quaternion.identity);
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
