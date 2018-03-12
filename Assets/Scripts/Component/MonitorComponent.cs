using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorComponent : BasicComponent
{
	public int m_SightArea;
	public List<Vector3> m_view;
	public List<BasicEntity> m_enemy = new List<BasicEntity> ();
	public List<Vector3> m_voice = new List<Vector3> ();
}
