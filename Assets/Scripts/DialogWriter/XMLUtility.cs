using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public static class XMLUtility
{
	public static string path = Application.dataPath + "/OutputXml/dialogWriter.xml";
	public static DialogWriter LoadXMLEvent ()
	{
		XmlReader reader = new XmlTextReader (path);

		DialogWriter newDialogWriter = null;
		DialogEvent newDialogEvent = null;
		TalkNode newTalkNode = null;
		TalkContent newTalkContent = null;

		newDialogWriter = new DialogWriter ();
		while (reader.Read ()) {
			if (reader.NodeType == XmlNodeType.Element) {

				if (reader.LocalName == "DialogWriter") {
					if (newDialogWriter != null) {

					}
					//不会执行


				} else if (reader.LocalName == "Event") {
					if (newDialogEvent != null) {
						newDialogWriter.m_dialogEventList.Add (newDialogEvent);
					}
					newDialogEvent = new DialogEvent ("");
					for (int i = 0; i < reader.AttributeCount; i++) {
						reader.MoveToAttribute (i);
						if (reader.Name == "Name") {
							newDialogEvent.m_name = reader.Value;
						} else if (reader.Name == "EventOrder") {
							newDialogEvent.m_eventOrder = int.Parse (reader.Value);
						}
					}
				} else if (reader.LocalName == "TalkNode") {
					if (newTalkNode != null) {
						newDialogEvent.m_nodeList.Add (newTalkNode);

					}
					newTalkNode = new TalkNode ("");
					for (int i = 0; i < reader.AttributeCount; i++) {
						reader.MoveToAttribute (i);
						if (reader.Name == "Name") {
							newTalkNode.m_name = reader.Value;

						} else if (reader.Name == "NodeType") {
							newTalkNode.m_dialogType = (DialogNode.NodeType)System.Enum.Parse (typeof (DialogNode.NodeType), reader.Value);
						}
					}
				} else if (reader.LocalName == "Background") {
					for (int i = 0; i < reader.AttributeCount; i++) {
						reader.MoveToAttribute (i);

						if (reader.Name == "Name") {
							newTalkNode.m_background.Add (reader.Value);
						}

					}
				} else if (reader.LocalName == "Tachie") {
					for (int i = 0; i < reader.AttributeCount; i++) {
						reader.MoveToAttribute (i);
						if (reader.Name == "Name") {
							newTalkNode.m_tachie.Add (reader.Value);
						}
					}
				} else if (reader.LocalName == "TalkContent") {
					if (newTalkContent != null) {
						newTalkNode.m_talkContents.Add (newTalkContent);
					}
					newTalkContent = new TalkContent ();
					for (int i = 0; i < reader.AttributeCount; i++) {
						reader.MoveToAttribute (i);
						if (reader.Name == "Background") {
							newTalkContent.m_backGround = int.Parse (reader.Value);
						} else if (reader.Name == "Tachie") {
							newTalkContent.m_tachie = int.Parse (reader.Value);
						} else if (reader.Name == "Name") {
							newTalkContent.m_name = reader.Value;
						} else if (reader.Name == "Content") {
							newTalkContent.m_content = reader.Value;
						}
					}
				}
			}


		}
		if (newTalkContent != null && newTalkNode != null) {
			newTalkNode.m_talkContents.Add (newTalkContent);

		}

		if (newDialogEvent != null && newTalkNode != null) {
			newDialogEvent.m_nodeList.Add (newTalkNode);
		}

		if (newDialogWriter != null && newDialogEvent != null) {
			newDialogWriter.m_dialogEventList.Add (newDialogEvent);
		}

		return newDialogWriter;
	}

	public static DialogWriter ProduceWriter ()
	{
		GameObject writerGo = GameObject.Find ("Dialog Writer");
		DialogWriter writer = new DialogWriter ();
		for (int i = 0; i < writerGo.transform.childCount; i++) {
			Transform t1 = writerGo.transform.GetChild (i);
			DialogEvent dialogevent = t1.gameObject.GetComponent<MonoDialogEvent> ().m_event;
			DialogEvent newDialogEvent = new DialogEvent (dialogevent.m_name);
			dialogevent.m_eventOrder = i;
			writer.m_dialogEventList.Add (newDialogEvent);
			for (int j = 0; j < t1.childCount; j++) {
				Transform t2 = t1.GetChild (j);
				TalkNode talkNode = t2.gameObject.GetComponent<MonoTalkNode> ().m_node;
				TalkNode newTalkNode = new TalkNode (talkNode.m_name);
				newTalkNode.m_background = talkNode.m_background;
				newTalkNode.m_tachie = talkNode.m_tachie;
				newTalkNode.m_name = talkNode.m_name;
				newTalkNode.m_dialogType = DialogNode.NodeType.Talk;
				newDialogEvent.m_nodeList.Add (newTalkNode);
				for (int k = 0; k < t2.childCount; k++) {
					Transform t3 = t2.GetChild (k);
					TalkContent talkContent = t3.gameObject.GetComponent<MonoTalkContent> ().m_talkContent;
					newTalkNode.m_talkContents.Add (talkContent);
				}
			}
		}
		return writer;
	}

	public static void SaveXMLEvent ()
	{
		DialogWriter dialogWriter = ProduceWriter ();
		path = Application.dataPath + "/OutputXml/" + dialogWriter.GetType () + ".xml";
		FileInfo fi = new FileInfo (path);

		if (fi.Exists) {
			Debug.Log ("dsadasdsdasdadadasdasdasdwdw");
			//fi.MoveTo ("./backup");
			fi.Delete ();
		}

		List<DialogEvent> dialogEventLsit = dialogWriter.m_dialogEventList;

		XmlDocument doc = new XmlDocument ();
		XmlElement dialogWriterElem = doc.CreateElement ("DialogWriter");
		for (int i = 0; i < dialogEventLsit.Count; i++) {
			XmlElement eve = doc.CreateElement ("Event");
			DialogEvent dialogEvent = dialogEventLsit [i];
			eve.SetAttribute ("Name", dialogEvent.m_name);
			eve.SetAttribute ("EvenOrder", dialogEvent.m_eventOrder.ToString ());
			for (int j = 0; j < dialogEvent.m_nodeList.Count; j++) {
				XmlElement node = doc.CreateElement ("TalkNode");
				TalkNode talkNode = (TalkNode)dialogEvent.m_nodeList [j];
				node.SetAttribute ("Name", talkNode.m_name);
				node.SetAttribute ("NodeType", talkNode.m_dialogType.ToString ());
				for (int k = 0; k < talkNode.m_background.Count; k++) {
					//node.SetAttribute ("Background"+k, );
					XmlElement background = doc.CreateElement ("Background");
					background.SetAttribute ("Name", talkNode.m_background [k]);
					node.AppendChild (background);
				}
				for (int k = 0; k < talkNode.m_tachie.Count; k++) {
					XmlElement tachie = doc.CreateElement ("Tachie");
					tachie.SetAttribute ("Name", talkNode.m_tachie [k]);
					node.AppendChild (tachie);
					//node.SetAttribute ("Tachie"+k, talkNode.m_tachie [k]);
				}
				for (int k = 0; k < talkNode.m_talkContents.Count; k++) {
					XmlElement content = doc.CreateElement ("TalkContent");
					TalkContent talkContent = talkNode.m_talkContents [k];
					content.SetAttribute ("Background", talkContent.m_backGround.ToString ());
					content.SetAttribute ("Tachie", talkContent.m_tachie.ToString ());
					content.SetAttribute ("Name", talkContent.m_name);
					content.SetAttribute ("Content", talkContent.m_content);
					node.AppendChild (content);
				}
				eve.AppendChild (node);
			}
			dialogWriterElem.AppendChild (eve);
		}
		doc.AppendChild (dialogWriterElem);

		doc.Save (path);
	}
}