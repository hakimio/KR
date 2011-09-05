using UnityEngine;
using System.Collections.Generic;
using System;

public class DialogTemplate : MonoBehaviour 
{
	private bool dialogEnabled = false;
	public Conversation conversation = null;
	private float dialogHeight = 120f;
	private float dialogSpacer = 5f;
    //private Vector2 scrollPosition = new Vector2();
    private Transform PC;
    private string npcName;
    private bool justStarted;
    private int curPiece;
    private string[] npcPhrasePieces = new string[1];
    private const float maxNpcPhraseHeight = 55f;
    //private const float maxNPCPhraseWidth = 790f;
    private Vector3 cameraPosition;
    private Quaternion cameraRotation;

    private Rect boxDimensions, contentDimensions;
    private DialogNode curNode;
    private GUIStyle npcPhraseStyle, pcPhraseStyle;
    private DialogResponse[] responses;
    private float responseHeight = 0;
    private float ratio, actualNPCHeight, availableSpace;

	void onEndDialog () 
	{
	
	}

    void Start()
    {
        boxDimensions = new Rect(Screen.width / 2 - (800 / 2 - dialogSpacer),
                              Screen.height - dialogHeight - dialogSpacer,
                              800 - dialogSpacer * 2, dialogHeight);
        contentDimensions = new Rect(boxDimensions.xMin + 5,
            boxDimensions.yMin + 5, boxDimensions.width - 10, 
            boxDimensions.height - 10);
        Messenger<string>.AddListener("dialog starting", startDialog);
        Messenger<string>.AddListener("show conversation", showConversation);

        PC = GameObject.Find("Player Character").transform;

        GameObject go = GameObject.Find("  Game Master");
        GameMaster gm = go.GetComponent<GameMaster>();
        npcName = transform.gameObject.GetComponent<NPC>().npcName;

        foreach(Conversation conv in gm.dialogs)
            if (npcName.Equals(conv.npcName))
            {
                conversation = conv;
                break;
            }
    }

    void showConversation(string npcName)
    {
        if (this.npcName.Equals(npcName))
            dialogEnabled = true;
    }

    void startDialog(string name)
    {
        if (!npcName.Equals(name))
            return;
        if (conversation == null)
        {
            Debug.LogError("No conversation found!");
            return;
        }

        Messenger<bool>.Broadcast("enable phrases", false);
        cameraPosition = MyCamera.instance.transform.position;
        cameraRotation = MyCamera.instance.transform.rotation;
        MyCamera.instance.controllingEnabled = false;

        GameObject go = GameObject.Find(npcName + "'s Camera");
        StartCoroutine(Helper.transitionCamera(go.transform, false, npcName));

        justStarted = true;
        curPiece = 0;
        Vector3 pcTarget = transform.position;
        pcTarget.y = PC.position.y;
        PC.LookAt(pcTarget);
        Vector3 npcTarget = PC.position;
        npcTarget.y = transform.position.y;
        transform.LookAt(npcTarget);
        //dialogEnabled = true;
    }
	
	void OnGUI()
	{
		if (dialogEnabled)
			drawDialog();
	}

	void drawDialog()
	{
		GUI.Box(boxDimensions, "");
        GUILayout.BeginArea(contentDimensions);

        if (justStarted)
        {
            curNode = conversation.curNode;
            npcPhraseStyle = new GUIStyle("label");
            npcPhraseStyle.wordWrap = true;

            pcPhraseStyle = new GUIStyle("button");
            pcPhraseStyle.wordWrap = true;

            responses = curNode.getResponses();
            responseHeight = 0;

            foreach (DialogResponse response in responses)
            {
                string msg = response.response;
                responseHeight += pcPhraseStyle.CalcHeight(new GUIContent(msg),
                    boxDimensions.width);
                responseHeight += pcPhraseStyle.padding.top * 2;
            }

            availableSpace = boxDimensions.height - responseHeight;
            actualNPCHeight = npcPhraseStyle.CalcHeight(new GUIContent(
                curNode.npcDialog), contentDimensions.width);
            actualNPCHeight += npcPhraseStyle.padding.top +
                npcPhraseStyle.padding.bottom;

            if (availableSpace > maxNpcPhraseHeight)
                availableSpace = maxNpcPhraseHeight;
            ratio = availableSpace / actualNPCHeight;

            if (ratio < 1f)
                npcPhrasePieces = Helper.cutPhrase(availableSpace,
                    contentDimensions.width, curNode.npcDialog, 
                    npcPhraseStyle);
            else
                npcPhrasePieces[0] = curNode.npcDialog;

            justStarted = false;
        }

        if (GUILayout.Button(npcPhrasePieces[curPiece], npcPhraseStyle,
            GUILayout.MaxWidth(contentDimensions.width)))
        {
            curPiece++;
            curPiece = curPiece % npcPhrasePieces.Length;
        }
		
        for (int i = 0; i < responses.Length; i++)
		{
            DialogResponse response = responses[i];
			if (response.enabled)
			{
                if (GUILayout.Button(response.response, pcPhraseStyle, 
                    GUILayout.ExpandWidth(false)))
				{
					if (response.onlyAllowOnce)
						response.enabled = false;
					linkToNode(response);
                    curNode = conversation.curNode;
                    responses = curNode.getResponses();

                    responseHeight = 0;

                    foreach (DialogResponse r in responses)
                    {
                        string msg = r.response;
                        responseHeight += pcPhraseStyle.
                            CalcHeight(new GUIContent(msg), boxDimensions.width);
                        responseHeight += pcPhraseStyle.padding.top * 2;
                    }

                    availableSpace = boxDimensions.height - responseHeight;
                    actualNPCHeight = npcPhraseStyle.
                        CalcHeight(new GUIContent(curNode.npcDialog), 
                        contentDimensions.width);
                    actualNPCHeight += npcPhraseStyle.padding.top +
                        npcPhraseStyle.padding.bottom;

                    if (availableSpace > maxNpcPhraseHeight)
                        availableSpace = maxNpcPhraseHeight;
                    ratio = availableSpace / actualNPCHeight;

                    if (ratio < 1f)
                        npcPhrasePieces = Helper.cutPhrase(availableSpace,
                        contentDimensions.width, curNode.npcDialog, 
                        npcPhraseStyle);
                    else
                        npcPhrasePieces = new string[] { curNode.npcDialog };
                    curPiece = 0;
				}
			}
		}
        GUILayout.EndArea();
	}

	void linkToNode(DialogResponse response)
	{
		if (response.linkType == ResponseLinkType.endConversation)
			endDialog(false);
		else if (response.linkType == ResponseLinkType.dialogNode)
			conversation.curNode = response.childNode;
		else if (response.linkType == ResponseLinkType.
		         endAndChangeConversation)
		{
			conversation.curNode = response.switchNode;
			endDialog(true);
		}
	}
	
	void endDialog(bool switched)
	{
		dialogEnabled = false;
		if (conversation.resetConversationOnEnd && !switched)
			conversation.curNode = conversation.startNode;
        Messenger<bool>.Broadcast("enable phrases", true);
        transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
        GameObject target = new GameObject("cameraPosition");
        target.transform.position = cameraPosition;
        target.transform.rotation = cameraRotation;
        StartCoroutine(Helper.transitionCamera(target.transform, true, 
            npcName));
        //MyCamera.instance.enabled = true;
		onEndDialog();
	}
	
	public void beginDialog()
	{
		dialogEnabled = true;
	}
	
	public void terminateDialog()
	{
		endDialog(false);
	}
}
