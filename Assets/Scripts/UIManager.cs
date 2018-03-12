using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

static public class DialogPlayer
{
	//文件保存
	static List<Sprite> m_background = new List<Sprite> ();
	static List<Sprite> m_tachie = new List<Sprite> ();

	static List<TalkContent> m_content = new List<TalkContent> ();
	static int currentContent;

	static List<TalkNode> m_node = new List<TalkNode> ();
	static int currentNode;
	//装入事件
	static public void LoadDialogEvent (DialogEvent e)
	{

		for (int i = 0; i < e.m_nodeList.Count; i++) {

			switch (e.m_nodeList [i].m_dialogType) {
			case DialogNode.NodeType.Talk:

				TalkNode node = (TalkNode)e.m_nodeList [i];
				m_node.Add (node);


				break;
			}
		}
		currentNode = 0;
		currentContent = -1;

		m_content = m_node [currentNode].m_talkContents;
		GetSpriteAsset (m_node [currentNode].m_background, m_background);
		GetSpriteAsset (m_node [currentNode].m_tachie, m_tachie);

		UIManager.Instance ().ShowPanel<UIDialog> ();

	}



	static public string LoadContent ()
	{
		string content = m_content [currentContent].m_content;
		return content;
	}

	static public Sprite LoadTachie ()
	{
		if (m_content [currentContent].m_tachie == 0) {
			return null;
		}
		Sprite sprite = m_tachie [m_content [currentContent].m_tachie];
		return sprite;
	}

	static public Sprite LoadBackground ()
	{
		if (m_content [currentContent].m_backGround == 0) {
			return null;
		}
		Sprite background = m_background [m_content [currentContent].m_backGround];
		return background;
	}

	static public string LoadName ()
	{
		string name = m_content [currentContent].m_name;
		return name;
	}

	static public bool IsReading ()
	{
		if (currentContent == m_content.Count - 1) {

			return false;
		} else {
			currentContent++;
			return true;
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
			asset.Add (Resources.Load (str) as Sprite);
		}
	}

	static void Clear ()
	{
		m_tachie.Clear ();
		m_content.Clear ();
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
	Dictionary<string, List<TalkContent>> m_selection = new Dictionary<string, List<TalkContent>> ();

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