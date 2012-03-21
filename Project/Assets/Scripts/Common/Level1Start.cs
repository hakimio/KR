using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class Level1Start: MonoBehaviour
{
    public List<Conversation> dialogs;
    public List<Message> messages;
    
    void Awake()
    {
        loadDialogs();
        loadMessages();
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
            if (System.IO.Path.GetExtension(file).Equals(".meta"))
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
            if (System.IO.Path.GetExtension(file).Equals(".meta"))
                continue;
            string name = System.IO.Path.GetFileNameWithoutExtension(file);
            string content = TxtReader.read(file);
            messages.Add(new Message(name, content));
        }
    }
}
