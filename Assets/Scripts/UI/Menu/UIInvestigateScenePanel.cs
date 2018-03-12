using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Skyunion;

public class UIInvestigateScenePanel : UIPanel
{

	// Use this for initialization
	void Start ()
	{
		AddClickEvent ("InvestigatorBook", showBook);

		UIManager.Instance ().ClosePanel<UINoteBookPanel> ();

		//初始化Scene
		GameObject.Find ("Scene").GetComponent<Scene> ().SceneInit ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void showBook (BaseEventData eventDate)
	{
		UIManager.Instance ().ShowPanel<UINoteBookPanel> ();

	}
}

public class Character
{
	public PropertyComponent.CharacterType m_characterType = PropertyComponent.CharacterType.Null;
	public string m_name;

	public int m_str;   //强韧strength
	public int m_obs;   //观察observation
	public int m_agi;   //敏捷agility
	public int m_spd;   //速度speed
	public int m_hrz;   //视野horizon

	public int m_hp;
	public int m_san;
	public int m_ap;



	public Character (PropertyComponent.CharacterType type, string name, int str, int agi, int obs, int spd, int hrz)
	{
		m_characterType = type;

		m_str = str;
		m_obs = obs;
		m_agi = agi;
		m_spd = spd;
		m_hrz = hrz;

		//todo complete detail date calculate
		m_hp = m_str * +15;
		m_san = (m_str - m_obs) + 20;
		m_ap = (int)m_agi / 4;
	}
}

public class Team
{
	//team name and team member
	public List<Character> m_members = new List<Character> ();

	public List<string> m_item = new List<string> ();

	public int itemLimit;
}
//静态数据
public static class InvestigateSceneDate
{
	//all team
	public static List<Team> m_teams = new List<Team> ();
	//default Team 1;
	public static int m_currentTeam = 0;
	public static int m_TeamNum = 0;

	public static void AddNewTeam (Team team)
	{
		m_teams.Add (team);
		m_TeamNum++;
	}

}

public class ObservableObj
{
	public string m_name;
	public int m_difficult;

	public string m_normalInfo;
	public string m_hardInfo;


	public ObservableObj (string name, int diff, string normalInfo, string hardInfo)
	{
		m_name = name;
		m_difficult = diff;
		m_normalInfo = normalInfo;
		m_hardInfo = hardInfo;
	}
}

public class OptionalObj
{
	public string m_name;

}