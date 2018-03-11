using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestigateBoxPanel : UIMenu
{

	private Scene m_scene;

	private string m_name;      //当前显示的物体名称

	private Text m_itemName;    //UI上显示的名称
	private Text m_obsResult;
	private Text m_ObsValue;
	private GameObject m_ObsButton;

	Dictionary<string, InvestigateInfo> m_itemInvestigateInfo;

	// Use this for initialization
	void InitPanel ()
	{
		m_itemName = transform.Find ("ItemName").GetComponent<Text> ();
		m_obsResult = transform.Find ("Panel").Find ("ObsResult").GetComponent<Text> ();
		m_ObsValue = transform.Find ("ObsValue").GetComponent<Text> ();
		m_ObsButton = transform.Find ("ObsButton").gameObject;

		m_scene = GameObject.Find ("Scene").GetComponent<Scene> ();

		m_itemInvestigateInfo = new Dictionary<string, InvestigateInfo> ();
		foreach (string s in m_scene.m_observableObj.Keys) {
			m_itemInvestigateInfo.Add (s, new InvestigateInfo ());
		}


		AddButtonClick ("ObsButton", ObsCheck);
		AddButtonClick ("BoxClose", CloseBox);
	}

	// Update is called once per frame
	void Update ()
	{

	}
	//显示各种数据
	public void ShowInfo (string aName)
	{
		Camera.main.GetComponent<RayCastDetection> ().CloseDetection ();

		if (m_itemName == null) {
			InitPanel ();
		}

		m_name = aName;
		m_itemName.text = m_scene.m_observableObjDate [m_name].m_name;
		m_obsResult.text = m_scene.m_observableObjDate [m_name].m_normalInfo;
		m_ObsValue.text = m_scene.m_obsName + " : " + m_scene.m_obs;

		//是否调查过？
		if (m_itemInvestigateInfo [m_name].m_investigated == false) {
			m_ObsButton.SetActive (true);
		} else {
			//调查过了
			m_ObsButton.SetActive (false);

			if (m_itemInvestigateInfo [m_name].m_pass == true) {
				m_obsResult.text += m_scene.m_observableObjDate [m_name].m_hardInfo;
			}
		}
	}

	//d20 + obsAdjust ？ diff
	void ObsCheck ()
	{
		bool pass = ValueCalculate.Check (20,
						  ValueCalculate.CalculateAdjust (m_scene.m_obs),
						  m_scene.m_observableObjDate [m_name].m_difficult);

		m_ObsButton.SetActive (false);
		m_obsResult.text += "检定结果：" + ValueCalculate.m_result + " ";

		TmpDate.m_noteContent += "\n" + m_scene.m_observableObjDate [m_name].m_normalInfo + "\n";

		m_itemInvestigateInfo [m_name].m_investigated = true;
		m_itemInvestigateInfo [m_name].m_pass = pass;

		if (pass == true) {
			TmpDate.m_investigatePoint += 2;

			m_obsResult.text += m_scene.m_observableObjDate [m_name].m_hardInfo;
			TmpDate.m_noteContent += m_scene.m_observableObjDate [m_name].m_hardInfo + "\n";
		} else {
			TmpDate.m_investigatePoint += 1;
		}

	}

	void CloseBox ()
	{

		DialogPlayer.Load (m_name);
		Skyunion.UIManager.Instance ().ClosePanel<InvestigateBoxPanel> ();
	}

}

public class InvestigateInfo
{

	public bool m_investigated = false;        //是否调查过
	public bool m_pass = false;                //是否通过
}

static class TmpDate
{
	static public string m_noteContent;
	static public int m_investigatePoint;
}