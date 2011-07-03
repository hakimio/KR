using System;
using System.Collections.Generic;
using System.Text;

namespace Dialog_Editor.dialog_classes
{
    public enum ResponseLinkType
    {
        dialogNode,
        endConversation,
        endAndChangeConversation
    }

    class DialogResponse
    {
        //response message
        public String response;
        //link id
        public String link;
        public ResponseLinkType linkType = ResponseLinkType.dialogNode;
        //conversation to change to
        public String switchConversation = "";
        //option selectable only once
        public bool onlyAllowOnce = false;
        public DialogNode childNode = null;
        //public String id;

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
}
