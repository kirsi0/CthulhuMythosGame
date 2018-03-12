using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可调查物品初始化根据游戏中Obs对象下自对象的名字与数据库中保存的名字相对应来获取数据。

//1.游戏中物品的名字必须与数据库中的名字相对应
public class Scene : MonoBehaviour
{
	GameObject m_sceneUI;
	//GameInvestigateMark的prefab
	GameObject perfab;

	Team m_team;
	public int m_obs = 0;      //观察属性最高的调查员
	public string m_obsName;    //他的名字

	public Dictionary<string, ObservableObj> m_observableObjDate;

	public Dictionary<string, GameObject> m_observableObj;
	public List<string> m_obsGoName;
	Dictionary<string, UIFollow> m_observableScript;


	private void Awake ()
	{


		m_observableObjDate = new Dictionary<string, ObservableObj> ();
		m_observableObj = new Dictionary<string, GameObject> ();
		m_observableScript = new Dictionary<string, UIFollow> ();
		m_obsGoName = new List<string> ();
		//轻轻松松可以观察到的



		ObservableObj photoFrame = new ObservableObj ("PhotoFrame", 13,
													  "木质框架边缘镶嵌着破碎的玻渣，照片中有四个人的合影，应该是安第斯一家的合影，合影中拉着父亲手的应该是埃斯·安第斯，他的脸被撕掉了。",
													  "只有埃斯·安第斯的外貌模糊不清，似乎是特地销毁掉的。");

		m_observableObjDate.Add ("PhotoFrame", photoFrame);

		ObservableObj candleStick = new ObservableObj ("CandleStick", 11,
													   "一个银制的，刻画着多张充满着痛苦的面孔，向举着灯托的赤裸少女伸出手，构成了灯的主要结构。",
													   "认真查看的话，可以看到上面隐隐约约沾着一些早已干涸的血迹。");
		m_observableObjDate.Add ("CandleStick", candleStick);


		ObservableObj noteBook = new ObservableObj ("NoteBook", 12,
													"这是一本老旧的记事本，被巧妙地隐藏在柜子的暗层中，里面用密码记录着信息。",
													"似乎和一个自上古就被阿克汉姆人信仰的怪异宗教有关，这种宗教崇拜的是一种来自远古的神秘力量，这种力量比宇宙还要古老。");

		m_observableObjDate.Add ("NoteBook", noteBook);


		ObservableObj lockedDoor = new ObservableObj ("LockedDoor", 11,
													  "一个巨大的锁挂在房门上，似乎已经很久没有用过了。",
													  "仔细地看，似乎上面写着埃斯·安第斯。名字被剥去的方式像是使用爪子磨的。");

		m_observableObjDate.Add ("LockedDoor", lockedDoor);


		ObservableObj portraiture = new ObservableObj ("Portraiture", 10,
													   "埃斯·安第斯的肖像画，他穿着一袭红衣，绘制于1990年。",
													   "画像上的他用的是右手握住酒杯");

		m_observableObjDate.Add ("Portraiture", portraiture);


		ObservableObj diary = new ObservableObj ("Diary", 11,
												 "日记写着埃斯·安第斯的名字，第一行就是“我恨红色衣服，死也不穿”，上面的日期只记录到1989年，也就埃斯25岁的时候，他似乎和自己的哥哥有了很大的争执，似乎是他认为达克的计划太疯狂了。",
												 "日记的最后一部分没有写完，书写似乎是被突然中断的，还被撕扯掉了一部分。");

		m_observableObjDate.Add ("Diary", diary);


		ObservableObj acientBook = new ObservableObj ("AcientBook", 14,
													 "一堆看上去已经破破烂烂的书籍，似乎已经很有年代了。",
													  "书中的内容十分杂乱，但是其中提到的令某些沉睡的力量觉醒的方法让人从内心产生一些糟糕的预感。");

		m_observableObjDate.Add ("AcientBook", acientBook);

	}




	//最好不要用Mono的生命周期，调用顺序无法确定，依赖顺序和执行顺序不同会导致错误
	//SceneInit让UIInvestigateScenePanel去做，之后的函数是依赖他的
	public void SceneInit ()
	{
		GetSceneDate ();

		CollectObsObj ();

		InitObservableItemUI ();
	}

	private void Update ()
	{

	}

	//获得场景的信息，团队信息
	void GetSceneDate ()
	{


		Character alice = new Character (PropertyComponent.CharacterType.Veteran,
									 "Alice",
									 17, 10, 16, 11, 5);

		Character hamser = new Character (PropertyComponent.CharacterType.Hacker,
									 "Hamser",
									 10, 18, 9, 8, 6);


		Team team = new Team ();
		team.m_members.Add (alice);
		team.m_members.Add (hamser);

		InvestigateSceneDate.AddNewTeam (team);
		m_team = InvestigateSceneDate.m_teams [InvestigateSceneDate.m_currentTeam];
		foreach (Character c in m_team.m_members) {
			if (c.m_obs > m_obs) {
				m_obs = c.m_obs;

				m_obsName = c.m_name;

			}

		}
		Debug.Log ("obs is:" + m_obs);
	}
	//初始化可以观察到的Item的UI
	void InitObservableItemUI ()
	{


		m_sceneUI = GameObject.Find ("UIInvestigateScenePanel");
		if (m_sceneUI != null) {
			for (int i = 0; i < m_observableObj.Keys.Count; i++) {

				InitItemUI (m_obsGoName [i]);
			}



		} else {
			Debug.Log ("fuck!");
		}
	}


	//收集场景中的可交互物品信息，以便和数据库比对
	void CollectObsObj ()
	{

		GameObject go = GameObject.Find ("Obs");
		for (int i = 0; i < go.transform.childCount; i++) {

			m_observableObj.Add (go.transform.GetChild (i).name, go.transform.GetChild (i).gameObject);
			m_obsGoName.Add (go.transform.GetChild (i).name);
		}

	}

	void InitItemUI (string aName)
	{
		string perfabName = "UI/Mark/InvestigableMark";

		if (perfab == null) {
			perfab = Skyunion.AssetsManager.LoadPrefabs<GameObject> (perfabName);
		}

		GameObject UIObject = Instantiate (perfab);
		UIObject.name = "UI" + aName;

		UIFollow uiFollow = UIObject.AddComponent<UIFollow> ();
		m_observableScript.Add (aName, uiFollow);

		uiFollow.m_followName = aName;
		uiFollow.m_follow = GameObject.Find ("Obs/" + aName);

		uiFollow.m_rectTransform = UIObject.GetComponent<RectTransform> ();
		uiFollow.m_offsetPos = new Vector3 (0, 10, 0);

		UIObject.transform.SetParent (m_sceneUI.transform);

	}

	//显示可调查物品物品的ui
	public void ShowItemUI (string aName)
	{

		if (m_observableScript.ContainsKey (aName)) {

			m_observableScript [aName].Show ();
		} else {
			return;
		}
	}

	public void ClsoeItemUI (string aName)
	{
		if (m_observableScript.ContainsKey (aName)) {

			m_observableScript [aName].Close ();
		} else {
			return;
		}

	}
	//显示物品的信息
	public void ShowItemInfo (string aName)
	{
		if (m_observableScript.ContainsKey (aName)) {

			Skyunion.UIManager.Instance ().ShowPanel<InvestigateBoxPanel> ();
			GameObject.Find ("RigidBodyFPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController> ().mouseLook.SetCursorLock (false);
			//显示对应物品的信息
			GameObject.Find ("InvestigateBoxPanel").GetComponent<InvestigateBoxPanel> ().ShowInfo (aName);

		}
	}

	public void CloseItemInfo (string aName)
	{

	}
}
//骰子计算
static class ValueCalculate
{
	public static int m_result = -1;

	//鉴定检查（骰子，调整数值，难度数值）
	public static bool Check (int dice, int adjust, int diff)
	{
		//[1,7)return int
		m_result = Random.Range (1, dice + 1);

		if (m_result + adjust >= diff) {
			return true;
		} else {
			return false;
		}
	}

	//计算属性调整值
	public static int CalculateAdjust (int property)
	{
		return property / 2 - 5;

	}

}