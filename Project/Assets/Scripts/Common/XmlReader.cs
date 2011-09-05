using System.Collections.Generic;
using System.Xml;
using System;

public class XmlReader {

    public static Conversation read(String fileName)
    {
        Conversation conv = new Conversation("");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(fileName);
        XmlNodeList xmlNL = xmlDoc.GetElementsByTagName("npcName");
        XmlElement xmlEl;
        xmlEl = (XmlElement)xmlNL[0];
        conv.npcName = xmlEl.GetAttribute("npcName");
        xmlNL = xmlDoc.GetElementsByTagName("resetConversation");
        xmlEl = (XmlElement)xmlNL[0];
        bool resetConv = bool.Parse(xmlEl.GetAttribute("resetConversation"));
        conv.resetConversationOnEnd = resetConv;

        xmlNL = xmlDoc.SelectNodes("/conversation/dialog/dialogNode");
        List<DialogResponse> respWithouChildren = new List<DialogResponse>();
        List<DialogResponse> respThatSwitchConv = new List<DialogResponse>();
        for (int i = 0; i < xmlNL.Count; i++)
        {
            xmlEl = (XmlElement)xmlNL[i];
            DialogNode node = loadNode(xmlEl, ref conv, 
                ref respWithouChildren, ref respThatSwitchConv);
            conv.addRootNode(node);
        }
        for (int i = 0; i < respWithouChildren.Count; i++)
        {
            DialogResponse response = respWithouChildren[i];
            DialogNode node = conv.getNodeById(response.link);
            respWithouChildren[i].childNode = node;
        }

        for (int i = 0; i < respThatSwitchConv.Count; i++)
        {
            DialogResponse response = respThatSwitchConv[i];
            DialogNode node = conv.getNodeById(response.switchConversation);
            respThatSwitchConv[i].switchNode = node;
        }

        conv.curNode = conv.getRootNode(0);
        conv.startNode = conv.getRootNode(0);

        return conv;
    }

    private static DialogNode loadNode(XmlElement xmlEl,
        ref Conversation conversation, 
        ref List<DialogResponse> respWithoutChildren,
        ref List<DialogResponse> respThatSwitchConv)
    {
        string id = xmlEl.GetAttribute("id");
        string npcPhrase = xmlEl.GetAttribute("npcPhrase");
        //string voiceFile = xmlEl.GetAttribute("voiceFile");
        DialogNode node = new DialogNode(id, npcPhrase);

        XmlNodeList responsesXNL = xmlEl.ChildNodes;
        for (int j = 0; j < responsesXNL.Count; j++)
        {
            XmlElement responseXE = (XmlElement)responsesXNL[j];
            string pcPhrase = responseXE.GetAttribute("pcPhrase");
            string link = responseXE.GetAttribute("link");
            ResponseLinkType linkType = ResponseLinkType.dialogNode;
            if (responseXE.GetAttribute("linkType").
                Equals("dialogNode"))
                linkType = ResponseLinkType.dialogNode;
            else if (responseXE.GetAttribute("linkType").
                Equals("endConversation"))
                linkType = ResponseLinkType.endConversation;
            else
                linkType = ResponseLinkType.endAndChangeConversation;

            string switchConv = responseXE.
                GetAttribute("switchConversation");
            bool onlyAllowOnce = bool.Parse(responseXE.
                GetAttribute("onlyAllowOnce"));

            DialogResponse response = new DialogResponse(pcPhrase, link,
                onlyAllowOnce, linkType, switchConv);
            node.addResponse(response);
            if (responseXE.HasChildNodes)
            {
                XmlElement childNode = (XmlElement)responseXE.FirstChild;
                DialogNode dn = loadNode(childNode, ref conversation,
                    ref respWithoutChildren, ref respThatSwitchConv);
                response.childNode = dn;
                conversation.addDialogNode(dn);
            }
            else if (linkType == ResponseLinkType.dialogNode)
                respWithoutChildren.Add(response);
            if (linkType == ResponseLinkType.endAndChangeConversation)
                respThatSwitchConv.Add(response);
        }
        return node;
    }
}
