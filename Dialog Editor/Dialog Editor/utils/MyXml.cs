using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Dialog_Editor.dialog_classes;
using Dialog_Editor.utils;
using System.Drawing;

namespace Dialog_Editor
{
    class MyXml
    {
        public static void save(Conversation conversation, String fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement conversationXE = xmlDoc.CreateElement("conversation");
            XmlElement dialogXE = xmlDoc.CreateElement("dialog");
            XmlElement npcNameXE = xmlDoc.CreateElement("npcName");
            XmlElement resetConvXE = xmlDoc.CreateElement("resetConversation");
            xmlDoc.AppendChild(conversationXE);
            conversationXE.AppendChild(npcNameXE);
            npcNameXE.SetAttribute("npcName", conversation.npcName);
            conversationXE.AppendChild(resetConvXE);
            resetConvXE.SetAttribute("resetConversation",
                conversation.resetConversationOnEnd.ToString());
            conversationXE.AppendChild(dialogXE);

            DialogNode[] dialog = conversation.getRootNodes();
            for (int i = 0; i < dialog.Length; i++)
            {
                saveNode(dialog[i], ref dialogXE, ref xmlDoc);
            }

            xmlDoc.Save(fileName);
        }

        private static void saveNode(DialogNode node, ref XmlElement xmlEl,
            ref XmlDocument xmlDoc)
        {
            XmlElement xmlChildEl = xmlDoc.CreateElement("dialogNode");
            xmlChildEl.SetAttribute("id", node.id);
            xmlChildEl.SetAttribute("npcPhrase", node.npcDialog);
            xmlChildEl.SetAttribute("voiceFile", node.npcVoiceFile);

            DialogResponse[] responses = node.getResponses();
            for (int j = 0; j < responses.Length; j++)
            {
                DialogResponse response = responses[j];
                XmlElement responseXE = xmlDoc.CreateElement("response");
                responseXE.SetAttribute("pcPhrase", response.response);
                responseXE.SetAttribute("link", response.link);
                responseXE.SetAttribute("linkType", 
                    response.linkType.ToString());
                responseXE.SetAttribute("switchConversation",
                    response.switchConversation);
                responseXE.SetAttribute("onlyAllowOnce", 
                    response.onlyAllowOnce.ToString());
                xmlChildEl.AppendChild(responseXE);
                if (response.childNode != null)
                    saveNode(response.childNode, ref responseXE, ref xmlDoc);
            }
            xmlEl.AppendChild(xmlChildEl);
        }

        public static Conversation load(String fileName)
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
            bool resetConv=bool.Parse(xmlEl.GetAttribute("resetConversation"));
            conv.resetConversationOnEnd = resetConv;

            xmlNL = xmlDoc.SelectNodes("/conversation/dialog/dialogNode");
            for (int i = 0; i < xmlNL.Count; i++)
            {
                xmlEl = (XmlElement)xmlNL[i];
                DialogNode node = loadNode(xmlEl, ref conv);
                conv.addRootNode(node);
            }

            return conv;
        }

        private static DialogNode loadNode(XmlElement xmlEl, 
            ref Conversation conversation)
        {
            string id = xmlEl.GetAttribute("id");
            string npcPhrase = xmlEl.GetAttribute("npcPhrase");
            string voiceFile = xmlEl.GetAttribute("voiceFile");
            DialogNode node = new DialogNode(id, npcPhrase, voiceFile);

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
                    DialogNode dn  = loadNode(childNode, ref conversation);
                    response.childNode = dn;
                    conversation.addDialogNode(dn);
                }
            }
            return node;
        }

        public static void saveSettings(Settings settings, String fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement optionsXmlEl = xmlDoc.CreateElement("settings");
            
            XmlElement childXmlEl = xmlDoc.CreateElement("windowSize");

            xmlDoc.AppendChild(optionsXmlEl);
            optionsXmlEl.AppendChild(childXmlEl);
            childXmlEl.SetAttribute("width", settings.windowSize.Width.
                ToString());
            childXmlEl.SetAttribute("height", settings.windowSize.Height.
                ToString());

            childXmlEl = xmlDoc.CreateElement("windowLocation");
            optionsXmlEl.AppendChild(childXmlEl);
            childXmlEl.SetAttribute("x", settings.windowLocation.X.
                ToString());
            childXmlEl.SetAttribute("y", settings.windowLocation.Y.
                ToString());

            childXmlEl = xmlDoc.CreateElement("treeViewSettings");
            optionsXmlEl.AppendChild(childXmlEl);
            childXmlEl.SetAttribute("idColumnWidth", 
                settings.idColumnWidth.ToString());
            childXmlEl.SetAttribute("linkColumnWidth", 
                settings.linkColumnWidth.ToString());

            xmlDoc.Save(fileName);
        }

        public static Settings loadSettings(String fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            
            XmlNodeList xmlNL = xmlDoc.GetElementsByTagName("windowSize");
            XmlElement xmlEl = (XmlElement)xmlNL[0];
            int width = Int32.Parse(xmlEl.GetAttribute("width"));
            int height = Int32.Parse(xmlEl.GetAttribute("height"));
            Size windowSize = new Size(width, height);
            
            xmlNL = xmlDoc.GetElementsByTagName("windowLocation");
            xmlEl = (XmlElement)xmlNL[0];
            int x = Int32.Parse(xmlEl.GetAttribute("x"));
            int y = Int32.Parse(xmlEl.GetAttribute("y"));
            Point windowLocation = new Point(x, y);
            
            xmlNL = xmlDoc.GetElementsByTagName("treeViewSettings");
            xmlEl = (XmlElement)xmlNL[0];
            int idColumnWidth = Int32.Parse(xmlEl.
                GetAttribute("idColumnWidth"));
            int linkColumnWidth = Int32.Parse(xmlEl.
                GetAttribute("linkColumnWidth"));

            return new Settings(idColumnWidth, linkColumnWidth, windowSize, 
                windowLocation);
        }
    }
}
