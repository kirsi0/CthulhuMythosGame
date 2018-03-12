﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;

public static class XMLUtility
{
	public static List<DialogEvent> LoadXMLEvent ()
	{
		return new List<DialogEvent> ();
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

		FileInfo fi = new FileInfo (Application.dataPath + "/OutputXml/" + dialogWriter.GetType () + ".xml");

		if (fi.Exists) {
			Debug.Log ("dsadasdsdasdadadasdasdasdwdw");
			//fi.MoveTo ("./backup");
			fi.Delete ();
		}

		List<DialogEvent> dialogEventLsit = dialogWriter.m_dialogEventList;

		XmlDocument doc = new XmlDocument ();
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
					node.SetAttribute ("Background", talkNode.m_background [k]);
				}
				for (int k = 0; k < talkNode.m_tachie.Count; k++) {
					node.SetAttribute ("Tachie", talkNode.m_tachie [k]);
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
			doc.AppendChild (eve);
		}

		doc.Save (Application.dataPath + "/OutputXml/" + dialogWriter.GetType () + ".xml");
	}
}

[CustomEditor (typeof (MonoDialogWriter))]
public class DialogWriterEditor : Editor
{
	public MonoDialogWriter m_monoDialogWriter;

	public DialogWriter m_dialogWriter;

	string eventName = "";
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("LoadXmlDialog")) {

		}
		if (GUILayout.Button ("SaveXmlDialog")) {
			XMLUtility.SaveXMLEvent ();
		}
		EditorGUILayout.EndHorizontal ();

		eventName = (string)EditorGUILayout.TextField ("New Event Name", eventName);
		if (GUILayout.Button ("Add New Event")) {
			if (eventName != "") {
				m_dialogWriter.m_dialogEventList.Add (new DialogEvent (eventName));
				UpdateWriterTree (m_dialogWriter.m_dialogEventList);
			} else {
				EditorUtility.DisplayDialog ("Add New Event Fail", "Event name is empty", "Redo");
			}
		}

		EditorGUILayout.TextField ("Event Number ", m_dialogWriter.m_dialogEventList.Count.ToString ());
		EditorGUILayout.EndVertical ();
	}

	private void OnEnable ()
	{
		m_monoDialogWriter = target as MonoDialogWriter;

		m_dialogWriter = m_monoDialogWriter.m_dialogWriter;

		UpdateWriterTree (m_dialogWriter.m_dialogEventList);
	}

	private void UpdateWriterTree (List<DialogEvent> eventList)
	{
		CleanAllEvent ();

		foreach (DialogEvent e in eventList) {

			MonoDialogEvent monoEvent = ShowEvent (e);
			foreach (DialogNode n in monoEvent.m_event.m_nodeList) {

				MonoTalkNode monoTalkNode = ShowNode (n, monoEvent);
				foreach (TalkContent content in monoTalkNode.m_node.m_talkContents) {

					MonoTalkContent monoTalkContent = ShowContent (content, monoTalkNode);
				}
			}
		}
	}

	private MonoDialogEvent ShowEvent (DialogEvent e)
	{

		GameObject go = new GameObject (e.m_name + "-" + e.GetType ());
		go.transform.SetParent (m_monoDialogWriter.transform);
		go.transform.position = Vector3.zero;
		MonoDialogEvent monoEvent = go.AddComponent<MonoDialogEvent> ();
		monoEvent.Init (e);
		return monoEvent;
	}


	private MonoTalkNode ShowNode (DialogNode node, MonoDialogEvent monoEvent)
	{

		GameObject go = new GameObject (node.m_name + "-" + node.m_dialogType);
		go.transform.SetParent (monoEvent.transform);
		go.transform.position = Vector3.zero;

		switch (node.m_dialogType) {
		case DialogNode.NodeType.Selection:
			MonoSelectionNode selectionNode = go.AddComponent<MonoSelectionNode> ();
			selectionNode.Init ((SelectionNode)node);
			//todo
			return null;


		case DialogNode.NodeType.Talk:
			MonoTalkNode talkNode = go.AddComponent<MonoTalkNode> ();
			talkNode.Init ((TalkNode)node);
			return talkNode;
		}
		return null;
	}

	MonoTalkContent ShowContent (TalkContent talkNode, MonoTalkNode monoTalkNode)
	{
		GameObject go = new GameObject (talkNode.m_name + "-" + talkNode.GetType ());
		go.transform.SetParent (monoTalkNode.transform);
		go.transform.position = Vector3.zero;

		MonoTalkContent monoCont = go.AddComponent<MonoTalkContent> ();
		monoCont.Init (talkNode);

		return monoCont;
	}



	void CleanAllEvent ()
	{
		for (int i = 0; i < m_monoDialogWriter.transform.childCount; i++) {
			Transform t = m_monoDialogWriter.transform.GetChild (i);
			DestroyImmediate (t.gameObject);
			i--;
		}
	}
}


[CustomEditor (typeof (MonoDialogEvent))]
public class DialogEventEditor : Editor
{
	MonoDialogEvent m_monoDialogEvent;
	DialogEvent m_dialogEvent;

	DialogNode.NodeType m_nodeType = DialogNode.NodeType.Talk;
	string m_nodeName = "";

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		EditorGUILayout.BeginVertical ();
		//添加文本节点
		EditorGUILayout.BeginHorizontal ();
		m_nodeType = (DialogNode.NodeType)EditorGUILayout.EnumPopup ("New Node Type", m_nodeType);
		m_nodeName = (string)EditorGUILayout.TextField ("New Node Name", m_nodeName);
		EditorGUILayout.EndHorizontal ();

		if (GUILayout.Button ("Add New Node")) {
			if (m_nodeName != "") {
				switch (m_nodeType) {
				case DialogNode.NodeType.Selection:
					m_dialogEvent.m_nodeList.Add (new SelectionNode (m_nodeName));
					UpdateEventTree ();
					break;

				case DialogNode.NodeType.Talk:
					m_dialogEvent.m_nodeList.Add (new TalkNode (m_nodeName));
					UpdateEventTree ();
					break;
				}
			} else {
				EditorUtility.DisplayDialog ("Add New Node Fail", "Node name is empty", "Redo");

			}

		}
		//添加选择节点
		EditorGUILayout.TextField ("Node Number ", m_dialogEvent.m_nodeList.Count.ToString ());

		EditorGUILayout.EndVertical ();

	}

	private void OnEnable ()
	{
		m_monoDialogEvent = target as MonoDialogEvent;

		m_dialogEvent = m_monoDialogEvent.m_event;

		UpdateEventTree ();
	}

	//重新更新事件内部
	void UpdateEventTree ()
	{
		ClearAllNode ();
		foreach (DialogNode n in m_dialogEvent.m_nodeList) {
			ShowNode (n, m_monoDialogEvent);
		}
	}

	private void ShowNode (DialogNode node, MonoDialogEvent monoEvent)
	{

		GameObject go = new GameObject (node.m_name + "-" + node.m_dialogType);
		go.transform.SetParent (monoEvent.transform);
		go.transform.position = Vector3.zero;

		switch (node.m_dialogType) {
		case DialogNode.NodeType.Selection:
			MonoSelectionNode selectionNode = go.AddComponent<MonoSelectionNode> ();
			selectionNode.Init ((SelectionNode)node);
			break;

		case DialogNode.NodeType.Talk:
			MonoTalkNode talkNode = go.AddComponent<MonoTalkNode> ();
			talkNode.Init ((TalkNode)node);
			break;


		}

	}

	void ClearAllNode ()
	{
		for (int i = 0; i < m_monoDialogEvent.transform.childCount; i++) {
			Transform t = m_monoDialogEvent.transform.GetChild (i);
			DestroyImmediate (t.gameObject);
			i--;
		}
	}
}

[CustomEditor (typeof (MonoTalkNode))]
public class TalkNodeEditor : Editor
{
	MonoTalkNode m_monoTalkNode;
	TalkNode m_talkNode;

	Sprite m_tachie;
	Sprite m_background;
	public override void OnInspectorGUI ()
	{
		//base.OnInspectorGUI ();

		EditorGUILayout.BeginVertical ();
		//Debug.Log (GUI.);
		GUI.backgroundColor = Color.red;
		if (GUILayout.Button ("Add New Talk Content")) {
			m_talkNode.m_talkContents.Add (new TalkContent ());
			UpdateNodeTree ();
		}
		GUI.backgroundColor = Color.white;
		m_tachie = (Sprite)EditorGUILayout.ObjectField ("New Tachie", m_tachie, typeof (Sprite), false);
		if (GUILayout.Button ("Add New Tachie")) {
			m_talkNode.m_tachie.Add (m_tachie.name);
		}

		m_background = (Sprite)EditorGUILayout.ObjectField ("New Background", m_background, typeof (Sprite), false);
		if (GUILayout.Button ("Add New Background")) {
			m_talkNode.m_background.Add (m_background.name);
		}
		if (m_talkNode != null) {
			EditorGUILayout.TextField ("Content Number", m_talkNode.m_talkContents.Count.ToString ());

			for (int i = 0; i < m_talkNode.m_tachie.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Tachie-" + i, m_talkNode.m_tachie [i]);
				if (GUILayout.Button ("Delete Tachie")) {
					m_talkNode.m_tachie.RemoveAt (i);
				}
				EditorGUILayout.EndHorizontal ();
			}
			for (int i = 0; i < m_talkNode.m_background.Count; i++) {
				EditorGUILayout.BeginHorizontal ();

				EditorGUILayout.LabelField ("Background-" + i, m_talkNode.m_background [i]);
				if (GUILayout.Button ("Delete Background")) {
					m_talkNode.m_background.RemoveAt (i);
				}
				EditorGUILayout.EndHorizontal ();

			}
		}
		EditorGUILayout.EndVertical ();
	}

	private void OnSceneGUI ()
	{
		m_monoTalkNode = target as MonoTalkNode;

		m_talkNode = m_monoTalkNode.m_node;

		UpdateNodeTree ();
	}

	void UpdateNodeTree ()
	{
		ClearContent ();

		foreach (TalkContent content in m_talkNode.m_talkContents) {
			ShowContent (content, m_monoTalkNode);
		}
	}

	void ShowContent (TalkContent talkNode, MonoTalkNode monoTalkNode)
	{
		GameObject go = new GameObject (talkNode.m_name + "-" + talkNode.GetType ());
		go.transform.SetParent (monoTalkNode.transform);
		go.transform.position = Vector3.zero;

		MonoTalkContent monoCont = go.AddComponent<MonoTalkContent> ();
		monoCont.Init (talkNode);

	}

	void ClearContent ()
	{
		for (int i = 0; i < m_monoTalkNode.transform.childCount; i++) {
			Transform t = m_monoTalkNode.transform.GetChild (i);
			DestroyImmediate (t.gameObject);
			i--;
		}
	}
}

[CustomEditor (typeof (MonoTalkContent))]
public class TalkContentEditor : Editor
{
	MonoTalkContent m_monoTalkConten;

	TalkContent m_talkContent;

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		EditorGUILayout.BeginVertical ();
		if (m_talkContent != null) {
			m_talkContent.m_backGround = EditorGUILayout.IntField ("Background Order", m_talkContent.m_backGround);
			m_talkContent.m_tachie = EditorGUILayout.IntField ("Tachie Order", m_talkContent.m_tachie);
			m_talkContent.m_name = EditorGUILayout.TextField ("name", m_talkContent.m_name);
			m_talkContent.m_content = EditorGUILayout.TextField ("content", m_talkContent.m_content);

		}
		EditorGUILayout.EndVertical ();
	}

	private void OnSceneGUI ()
	{
		m_monoTalkConten = target as MonoTalkContent;
		m_talkContent = m_monoTalkConten.m_talkContent;

		ShowContent ();
	}

	void ShowContent ()
	{
		m_monoTalkConten.gameObject.name = m_talkContent.m_name + m_talkContent.GetType ();
	}
}