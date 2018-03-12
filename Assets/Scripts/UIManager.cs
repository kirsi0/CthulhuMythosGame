using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

static public class DialogPlayer
{

	static DialogWriter dialogWriter;
	//临时保存
	static DialogEvent m_dialogEvent;
	//现在选择的node
	static int m_currentNode;


	//保存背景和立绘的文件保存
	static List<Sprite> m_background = new List<Sprite> ();
	static List<Sprite> m_tachie = new List<Sprite> ();
	static List<TalkContent> m_content = new List<TalkContent> ();
	//保存选择节点的多个选项
	static List<string> m_select = new List<string> ();
	//当前显示内容的序号
	static int m_currentContent;

	//保存所选的选项内容，作为暂时的保存处
	public static string m_selected;

	//初始化
	static void Init ()
	{
		dialogWriter = new DialogWriter ();
		dialogWriter = XMLUtility.LoadXMLEvent ();
	}


	//装入事件
	static public void Load (string name)
	{
		//第一次执行时
		if (dialogWriter == null) {
			Init ();
		}
		m_dialogEvent = null;

		for (int i = 0; i < dialogWriter.m_dialogEventList.Count; i++) {
			Console.Log (dialogWriter.m_dialogEventList [i].m_name);
		}
		for (int i = 0; i < dialogWriter.m_dialogEventList.Count; i++) {
			if (dialogWriter.m_dialogEventList [i].m_name == name) {

				Camera.main.GetComponent<RayCastDetection> ().CloseDetection ();
				GameObject.Find ("RigidBodyFPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController> ().mouseLook.SetCursorLock (false);

				m_dialogEvent = dialogWriter.m_dialogEventList [i];

				m_currentNode = 0;

			}
		}

		if (m_dialogEvent == null) {
			Console.Log ("Cant find Story!");

			GameObject.Find ("RigidBodyFPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController> ().mouseLook.SetCursorLock (true);
			Camera.main.GetComponent<RayCastDetection> ().OpenDetection ();
			return;
		} else {
			Console.Log ("yes, data is exist!");
			Play ();
		}

	}
	static int i = 0;
	static void Play ()
	{


		if (m_dialogEvent.m_nodeList [m_currentNode].m_dialogType == DialogNode.NodeType.Talk) {

			LoadTalk ();
			UIManager.Instance ().ShowPanel<UIDialogPanel> ();
			GameObject.Find ("RigidBodyFPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController> ().mouseLook.SetCursorLock (false);

			if (i != 0) {
				GameObject.Find ("UIDialogPanel").GetComponent<UIDialogPanel> ().InitDialog ();
			}
			i++;

		} else {
			LoadSelection ();
			UIManager.Instance ().ShowPanel<UISelectPanel> ();
			GameObject.Find ("RigidBodyFPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController> ().mouseLook.SetCursorLock (false);


			GameObject.Find ("UISelectPanel").GetComponent<UISelectPanel> ().InitSelect (m_select);
		}
	}
	static public void PlaySelection ()
	{


		UIManager.Instance ().ClosePanel<UISelectPanel> ();


		Clear ();
		//检查的时候会自增，因此一开始先减去自增的数值
		m_currentContent = -1;

		SelectionNode m_selectNode = (SelectionNode)m_dialogEvent.m_nodeList [m_currentNode];
		Debug.Log ("SelectedOption : " + m_selected);
		TalkNode m_node = m_selectNode.m_selection [m_selected];
		m_content = m_node.m_talkContents;
		GetSpriteAsset (m_node.m_background, m_background);
		GetSpriteAsset (m_node.m_tachie, m_tachie);

		GameObject.Find ("RigidBodyFPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController> ().mouseLook.SetCursorLock (false);

		UIManager.Instance ().ShowPanel<UIDialogPanel> ();
		GameObject.Find ("UIDialogPanel").GetComponent<UIDialogPanel> ().InitDialog ();

	}
	static void LoadTalk ()
	{
		Clear ();
		//检查的时候会自增，因此一开始先减去自增的数值
		m_currentContent = -1;

		TalkNode m_node = (TalkNode)m_dialogEvent.m_nodeList [m_currentNode];
		m_content = m_node.m_talkContents;
		GetSpriteAsset (m_node.m_background, m_background);
		GetSpriteAsset (m_node.m_tachie, m_tachie);
	}
	static void LoadSelection ()
	{
		SelectionNode m_node = (SelectionNode)m_dialogEvent.m_nodeList [m_currentNode];
		foreach (string s in m_node.m_selection.Keys) {
			m_select.Add (s);
		}
	}

	static public string LoadContent ()
	{
		string content = m_content [m_currentContent].m_content;

		return content;
	}

	static public Sprite LoadTachie ()
	{

		//if (m_content [currentContent].m_tachie == 0) {
		//	return null;
		//}
		Sprite sprite = m_tachie [m_content [m_currentContent].m_tachie];

		return sprite;
	}

	static public Sprite LoadBackground ()
	{

		//if (m_content [currentContent].m_backGround == 0) {
		//	return null;
		//}
		Sprite background = m_background [m_content [m_currentContent].m_backGround];

		return background;
	}

	static public string LoadName ()
	{

		string name = m_content [m_currentContent].m_name;

		return name;
	}

	static public bool IsReading ()
	{

		Debug.Log ("m_currentContent : " + m_currentContent);
		Debug.Log ("m_currentNode " + m_currentNode);
		Debug.Log (" m_content.Count" + m_content.Count);
		if (m_currentContent == m_content.Count - 1) {

			m_currentNode++;
			if (m_currentNode < m_dialogEvent.m_nodeList.Count) {

				Play ();
			}
			m_currentContent = -1;

			return false;
		} else {
			m_currentContent++;
			return true;
		}
	}

	static public void NextNode ()
	{
		m_currentNode++;
		if (m_currentNode < m_dialogEvent.m_nodeList.Count) {

			Play ();
		} else {
			return;
		}

	}

	////目前运行的事件是否结束
	//bool IsReadable ()
	//{
	//	if (currentNode == m_node.Count) {
	//		if (currentContent == m_content.Count) {

	//		}
	//	}
	//}

	//TalkContent Read ()
	//{

	//}

	static void GetSpriteAsset (List<string> name, List<Sprite> asset)
	{
		foreach (string str in name) {

			//Debug.Log (str);
			//Sprite s = (Sprite)Resources.Load ("UI/" + str, typeof (Sprite));
			//Debug.Log (s);
			asset.Add (Resources.Load ("UI/" + str, typeof (Sprite)) as Sprite);

		}
	}

	static void Clear ()
	{
		m_tachie.Clear ();

		m_background.Clear ();

	}
}

public class DialogEvent
{
	public List<DialogNode> m_nodeList = new List<DialogNode> ();
	//事件名，应该具有意义；
	public string m_name;
	//对话的序号
	public int m_eventOrder;

	public DialogEvent (string name)
	{
		m_name = name;
	}

	public DialogEvent (DialogEvent dialog)
	{
		m_nodeList = dialog.m_nodeList;
	}

	void AddNode (DialogNode newNode)
	{
		switch (newNode.m_dialogType) {
		case DialogNode.NodeType.Selection:

			break;

		case DialogNode.NodeType.Talk:

			break;
		}
	}
}
public class MonoDialogEvent : MonoBehaviour
{
	public DialogEvent m_event;

	public void Init (DialogEvent e)
	{
		m_event = e;
	}

}

public class TalkContent
{

	public int m_backGround;
	public int m_tachie;
	public string m_name;
	public string m_content;


}

public class MonoTalkContent : MonoBehaviour
{
	public TalkContent m_talkContent;

	public void Init (TalkContent talkContent)
	{
		m_talkContent = talkContent;
	}
}
//对话节点基类
public class DialogNode
{

	//对话节点类型
	public enum NodeType
	{
		Talk,
		Selection,

	}
	public NodeType m_dialogType;
	//对话名
	public string m_name;
	//对话所隶属的章节
	int m_chapter;
	//对话的序号
	int m_order;
	//

	public DialogNode (int order)
	{
		m_name = "DialogNode_" + m_order;
		m_order = order;
	}

	public DialogNode (string name)
	{
		m_name = name;
	}

	public DialogNode (int order, string name)
	{
		m_name = name;
		m_order = order;
	}

	public void LoadDialog ()
	{

	}
	public void SaveDialog ()
	{

	}
}
//战斗场景的对话事件
public class TalkNode : DialogNode
{
	public List<TalkContent> m_talkContents = new List<TalkContent> ();
	//reource
	public List<string> m_tachie = new List<string> ();
	//background
	public List<string> m_background = new List<string> ();

	//对话类型,决定了什么时候播放(应该由逻辑控制,可能废除)
	public enum BattleDialogType
	{
		BeforeBattle,
		Afterbattle,
		InBattle,

	}
	//BattleDialogType m_battleDialogType;

	//List<string> m_talkNodeList = new List<string> ();

	public TalkNode (string name) : base (name)
	{

		m_dialogType = NodeType.Talk;

	}
}

public class MonoTalkNode : MonoBehaviour
{
	public TalkNode m_node;

	public void Init (TalkNode node)
	{
		m_node = node;
	}
}

public class SelectionNode : DialogNode
{

	public Dictionary<string, TalkNode> m_selection = new Dictionary<string, TalkNode> ();


	public SelectionNode (string name) : base (name)
	{
		m_dialogType = NodeType.Selection;
	}
}

public class MonoSelectionNode : MonoBehaviour
{
	public SelectionNode m_node;

	public void Init (SelectionNode node)
	{
		m_node = node;
	}
}

public class DialogWriter
{
	public List<DialogEvent> m_dialogEventList = new List<DialogEvent> ();

	int EventNum;
}


public class MonoDialogWriter : MonoBehaviour
{

	public DialogWriter m_dialogWriter = new DialogWriter ();

	public void Init (DialogWriter DialogWriter)
	{
		m_dialogWriter = DialogWriter;
	}

}