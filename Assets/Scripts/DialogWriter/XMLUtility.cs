using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public static class XMLUtility
{
<<<<<<< HEAD
	public static string path = Application.dataPath + "/OutputXml/dialogWriter.xml";
	public static DialogWriter LoadXMLEvent ()
	{
		XmlReader reader = new XmlTextReader (path);

		DialogWriter newDialogWriter = null;
		DialogEvent newDialogEvent = null;
		TalkNode newTalkNode = null;
		TalkContent newTalkContent = null;
=======
	//保存路径

	public static string path = Application.dataPath + "/OutputXml/dialogWriter.xml";

	//加载xm文件
	public static DialogWriter LoadXMLEvent ()
	{

		bool m_isTalkNode = true;


		if (Application.platform == RuntimePlatform.OSXPlayer) {
			Console.Log ("sfdbdfsdgsfdgsfdds");
			path = Application.dataPath + "/Resources/Data/StreamingAssets/dialogWriter.xml";
			Console.Log (path);
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			path = Application.dataPath + "/Raw/";
		} else if (Application.platform == RuntimePlatform.Android) {
			path = "jar:file//" + Application.dataPath + "!/assets/";
		}
		//} else if(Application.platform == RuntimePlatform.){
		//	path = Application.dataPath + "/StreamingAssets/";
		//}


		XmlReader reader = new XmlTextReader (path);

		//提前设置的变量，用于保存读取文件时的各个子节点
		DialogWriter newDialogWriter = null;
		DialogEvent newDialogEvent = null;
		TalkNode newTalkNode = null;
		SelectionNode newSelectionNode = null;
		TalkContent newTalkContent = null;
		string newSelect = "";
>>>>>>> temp

		newDialogWriter = new DialogWriter ();
		while (reader.Read ()) {
			if (reader.NodeType == XmlNodeType.Element) {

				if (reader.LocalName == "DialogWriter") {
					if (newDialogWriter != null) {

					}
					//不会执行


				} else if (reader.LocalName == "Event") {
<<<<<<< HEAD
					if (newDialogEvent != null) {
						newDialogWriter.m_dialogEventList.Add (newDialogEvent);
					}
=======

>>>>>>> temp
					newDialogEvent = new DialogEvent ("");
					for (int i = 0; i < reader.AttributeCount; i++) {
						reader.MoveToAttribute (i);
						if (reader.Name == "Name") {
							newDialogEvent.m_name = reader.Value;
						} else if (reader.Name == "EventOrder") {
							newDialogEvent.m_eventOrder = int.Parse (reader.Value);
						}
					}
<<<<<<< HEAD
				} else if (reader.LocalName == "TalkNode") {
					if (newTalkNode != null) {
						newDialogEvent.m_nodeList.Add (newTalkNode);

					}
					newTalkNode = new TalkNode ("");
=======
					if (newDialogWriter != null) {
						newDialogWriter.m_dialogEventList.Add (newDialogEvent);
					}
				} else if (reader.LocalName == "TalkNode") {

					newTalkNode = new TalkNode ("");

>>>>>>> temp
					for (int i = 0; i < reader.AttributeCount; i++) {
						reader.MoveToAttribute (i);
						if (reader.Name == "Name") {
							newTalkNode.m_name = reader.Value;

						} else if (reader.Name == "NodeType") {
							newTalkNode.m_dialogType = (DialogNode.NodeType)System.Enum.Parse (typeof (DialogNode.NodeType), reader.Value);
						}
					}
<<<<<<< HEAD
=======
					//不是选择分支下的对话节点
					if (m_isTalkNode) {
						if (newDialogEvent != null) {
							newDialogEvent.m_nodeList.Add (newTalkNode);
						}
					} else {
						if (newSelectionNode != null) {
							newSelectionNode.m_selection [newSelect] = newTalkNode;
							//每次都重新设为true
							m_isTalkNode = true;
						}
					}

>>>>>>> temp
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
<<<<<<< HEAD
					if (newTalkContent != null) {
						newTalkNode.m_talkContents.Add (newTalkContent);
					}
=======

>>>>>>> temp
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
<<<<<<< HEAD
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
=======

					if (newTalkNode != null) {
						newTalkNode.m_talkContents.Add (newTalkContent);
					}
				} else if (reader.LocalName == "SelectionNode") {
					newSelectionNode = new SelectionNode ("");
					for (int i = 0; i < reader.AttributeCount; i++) {
						reader.MoveToAttribute (i);
						if (reader.Name == "Name") {
							newSelectionNode.m_name = reader.Value;
						}
					}

					if (newDialogEvent != null) {
						newDialogEvent.m_nodeList.Add (newSelectionNode);
					}
				} else if (reader.LocalName == "Select") {
					m_isTalkNode = false;

					for (int i = 0; i < reader.AttributeCount; i++) {
						reader.MoveToAttribute (i);
						if (reader.Name == "Select") {
							//把数值临时保存
							newSelect = reader.Value;
							newSelectionNode.m_selection.Add (reader.Value, new TalkNode (""));
						}
					}
				}
			}


		}


		return newDialogWriter;
	}

	static DialogWriter writer;
	static DialogEvent newWriterDialogEvent;
	static TalkNode newWriterTalkNode;
	static SelectionNode newWriterSelectionNode;
	//从hierarchy生成新的DialogWriter
	public static DialogWriter ProduceWriter ()
	{
		GameObject writerGo = GameObject.Find ("Dialog Writer");
		writer = new DialogWriter ();
		for (int i = 0; i < writerGo.transform.childCount; i++) {
			Transform t1 = writerGo.transform.GetChild (i);
			WriteDialogEvent (t1, i);

			for (int j = 0; j < t1.childCount; j++) {
				Transform t2 = t1.GetChild (j);
				if (t2.GetComponent<MonoTalkNode> () != null) {
					WriteTalkNode (t2);

					for (int k = 0; k < t2.childCount; k++) {
						Transform t3 = t2.GetChild (k);
						WriteTalkContent (t3);
					}
				} else {
					WriteSelectionNode (t2);

					for (int k = 0; k < t2.childCount; k++) {
						Transform t3 = t2.GetChild (k);
						WriteSelectionTalkNode (t3);

						for (int l = 0; l < t3.childCount; l++) {
							Transform t4 = t3.GetChild (l);
							WriteTalkContent (t4);
						}
					}
				}

>>>>>>> temp
			}
		}
		return writer;
	}

<<<<<<< HEAD
	public static void SaveXMLEvent ()
	{
		DialogWriter dialogWriter = ProduceWriter ();
=======
	//从mono中提取对象并保存到临时对象中，并加入上一级的临时对象的子对象中
	static void WriteDialogEvent (Transform monoDialogEvent, int eventOrder)
	{
		DialogEvent dialogevent = monoDialogEvent.gameObject.GetComponent<MonoDialogEvent> ().m_event;
		newWriterDialogEvent = new DialogEvent (dialogevent.m_name);
		dialogevent.m_eventOrder = eventOrder;
		writer.m_dialogEventList.Add (newWriterDialogEvent);
	}

	static void WriteTalkNode (Transform monoDialogEvent)
	{
		TalkNode talkNode = monoDialogEvent.gameObject.GetComponent<MonoTalkNode> ().m_node;
		newWriterTalkNode = new TalkNode (talkNode.m_name);
		newWriterTalkNode.m_background = talkNode.m_background;
		newWriterTalkNode.m_tachie = talkNode.m_tachie;
		newWriterTalkNode.m_name = talkNode.m_name;
		newWriterTalkNode.m_dialogType = DialogNode.NodeType.Talk;
		newWriterDialogEvent.m_nodeList.Add (newWriterTalkNode);
	}

	static void WriteTalkContent (Transform monoDialogEvent)
	{
		TalkContent talkContent = monoDialogEvent.gameObject.GetComponent<MonoTalkContent> ().m_talkContent;
		newWriterTalkNode.m_talkContents.Add (talkContent);
	}

	static void WriteSelectionNode (Transform monoDialogEvent)
	{
		SelectionNode talkNode = monoDialogEvent.gameObject.GetComponent<MonoSelectionNode> ().m_node;
		newWriterSelectionNode = new SelectionNode (talkNode.m_name);
		newWriterDialogEvent.m_nodeList.Add (newWriterSelectionNode);
	}

	static void WriteSelectionTalkNode (Transform monoDialogEvent)
	{

		TalkNode talkNode = monoDialogEvent.gameObject.GetComponent<MonoTalkNode> ().m_node;
		newWriterTalkNode = new TalkNode (talkNode.m_name);
		newWriterTalkNode.m_background = talkNode.m_background;
		newWriterTalkNode.m_tachie = talkNode.m_tachie;
		newWriterTalkNode.m_name = talkNode.m_name;
		newWriterTalkNode.m_dialogType = DialogNode.NodeType.Talk;
		newWriterSelectionNode.m_selection.Add (talkNode.m_name, newWriterTalkNode);

	}

	//保存为xml
	public static void SaveXMLEvent ()
	{
		DialogWriter dialogWriter = ProduceWriter ();
		//保存路径
>>>>>>> temp
		path = Application.dataPath + "/OutputXml/" + dialogWriter.GetType () + ".xml";
		FileInfo fi = new FileInfo (path);

		if (fi.Exists) {
<<<<<<< HEAD
			Debug.Log ("dsadasdsdasdadadasdasdasdwdw");
=======
			//Debug.Log ("dsadasdsdasdadadasdasdasdwdw");
>>>>>>> temp
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
<<<<<<< HEAD
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
=======

				//判断是否为talk节点
				if (dialogEvent.m_nodeList [j].m_dialogType == DialogNode.NodeType.Talk) {
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
				} else {
					XmlElement node = doc.CreateElement ("SelectionNode");
					SelectionNode selectionNode = (SelectionNode)dialogEvent.m_nodeList [j];
					node.SetAttribute ("Name", selectionNode.m_name);
					node.SetAttribute ("NodeType", selectionNode.m_dialogType.ToString ());

					//分为两个select节点
					foreach (string s in selectionNode.m_selection.Keys) {
						XmlElement select = doc.CreateElement ("Select");
						select.SetAttribute ("Select", s);

						XmlElement talk = doc.CreateElement ("TalkNode");
						TalkNode talkNode = selectionNode.m_selection [s];

						talk.SetAttribute ("Name", talkNode.m_name);
						talk.SetAttribute ("NodeType", talkNode.m_dialogType.ToString ());
						for (int k = 0; k < talkNode.m_background.Count; k++) {
							//node.SetAttribute ("Background"+k, );
							XmlElement background = doc.CreateElement ("Background");
							background.SetAttribute ("Name", talkNode.m_background [k]);
							talk.AppendChild (background);
						}
						for (int k = 0; k < talkNode.m_tachie.Count; k++) {
							XmlElement tachie = doc.CreateElement ("Tachie");
							tachie.SetAttribute ("Name", talkNode.m_tachie [k]);
							talk.AppendChild (tachie);
							//node.SetAttribute ("Tachie"+k, talkNode.m_tachie [k]);
						}
						for (int k = 0; k < talkNode.m_talkContents.Count; k++) {
							XmlElement content = doc.CreateElement ("TalkContent");
							TalkContent talkContent = talkNode.m_talkContents [k];
							content.SetAttribute ("Background", talkContent.m_backGround.ToString ());
							content.SetAttribute ("Tachie", talkContent.m_tachie.ToString ());
							content.SetAttribute ("Name", talkContent.m_name);
							content.SetAttribute ("Content", talkContent.m_content);
							talk.AppendChild (content);
						}

						select.AppendChild (talk);
						node.AppendChild (select);
					}

					eve.AppendChild (node);
				}

>>>>>>> temp
			}
			dialogWriterElem.AppendChild (eve);
		}
		doc.AppendChild (dialogWriterElem);

		doc.Save (path);
	}
<<<<<<< HEAD
}
=======
}

>>>>>>> temp
