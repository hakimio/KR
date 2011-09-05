using System;
using System.Collections.Generic;


public class Conversation
{
    public String npcName;
    private List<DialogNode> dialog;
    private List<DialogNode> rootNodes;
    public bool resetConversationOnEnd = false;
	public DialogNode curNode = null;
	public DialogNode startNode = null;

    public Conversation(String npcName)
    {
        this.npcName = npcName;
        dialog = new List<DialogNode>();
        rootNodes = new List<DialogNode>();
    }

    public Conversation(String npcName, DialogNode[] dialogNodes): 
        this(npcName)
    {
        rootNodes.AddRange(dialogNodes);
    }

    public void addDialogNode(DialogNode dialogNode)
    {
        dialog.Add(dialogNode);
    }

    public void addRootNode(DialogNode dialogNode)
    {
        rootNodes.Add(dialogNode);
        dialog.Add(dialogNode);
    }

    public void clearDialog()
    {
        dialog.Clear();
        rootNodes.Clear();
    }

    public void removeRootNode(DialogNode dialogNode)
    {
        rootNodes.Remove(dialogNode);
        dialog.Remove(dialogNode);
    }

    public void removeNode(DialogNode dialogNode)
    {
        dialog.Remove(dialogNode);
    }

    public DialogNode getNodeById(String id)
    {
        foreach (DialogNode node in dialog)
            if (node.id.Equals(id))
                return node;
        
        return null;
    }

    public DialogNode[] getRootNodes()
    {
        return rootNodes.ToArray();
    }

    public DialogNode getRootNode(int id)
    {
        try
        {
            return rootNodes[id];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    public DialogNode[] getDialog()
    {
        return dialog.ToArray();
    }

    public DialogNode getDialogNode(int id)
    {
        try
        {
            return dialog[id];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }
}
