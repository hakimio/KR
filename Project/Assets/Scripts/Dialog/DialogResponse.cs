using System;
using System.Collections.Generic;
using System.Text;

public enum ResponseLinkType
{
    dialogNode,
    endConversation,
    endAndChangeConversation
}

public class DialogResponse
{
    //response message
    public String response;
    //link id
    public String link;
    public ResponseLinkType linkType = ResponseLinkType.dialogNode;
    //conversation to change to
    public DialogNode switchNode = null;
	public String switchConversation = "";
    //option selectable only once
    public bool onlyAllowOnce = false;
    public DialogNode childNode = null;
	public bool enabled = true;

    public DialogResponse(String response, String link, 
        bool onlyAllowOnce)
    {
        this.response = response;
        this.link = link;
        this.onlyAllowOnce = onlyAllowOnce;
    }

    public DialogResponse(String response, String link, bool onlyAllowOnce,
        ResponseLinkType linkType, String switchConversation):
        this(response, link, onlyAllowOnce)
    {
        this.linkType = linkType;
        this.switchConversation = switchConversation;
    }
}
