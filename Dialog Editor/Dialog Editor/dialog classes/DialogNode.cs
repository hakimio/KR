using System;
using System.Collections.Generic;
using System.Text;

namespace Dialog_Editor.dialog_classes
{
    class DialogNode
    {
        public String id;
        public String npcDialog;
        public String npcVoiceFile;
        private List<DialogResponse> responses;

        public DialogNode(String id, String npcDialog)
        {
            this.id = id;
            this.npcDialog = npcDialog;
            npcVoiceFile = "";
            responses = new List<DialogResponse>();
        }

        public DialogNode(String id, String npcDialog, String npcVoiceFile): 
            this(id, npcDialog)
        {
            this.npcVoiceFile = npcVoiceFile;
        }

        public void addResponse(DialogResponse dialogResponse)
        {
            responses.Add(dialogResponse);
        }

        public void removeResponse(DialogResponse dialogResponse)
        {
            responses.Remove(dialogResponse);
        }

        public DialogResponse[] getResponses()
        {
            return responses.ToArray();
        }

        public DialogResponse getResponse(int id)
        {
            try
            {
                return responses[id];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }
    }
}
