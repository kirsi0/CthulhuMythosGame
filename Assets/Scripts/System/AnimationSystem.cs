using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : BasicSystem
{

	private ComponentType m_linkedType = ComponentType.Animation;
	//覆盖父类的m_linkedType
	public override ComponentType M_LinkedType {
		set {
			m_linkedType = value;
		}
		get {
			return m_linkedType;
		}
	}
	public enum AniType
	{
		Move,
		Atack,
		Cross,
		BreakWindow,
		Die,
		Shoot,
		Opendoor
	}
	public enum PlayType
	{
		Loop,
		None
	}
	public struct Ani
	{
		GameObject obj;
		AnimationSystem.AniType aniType;
		AnimationSystem.PlayType playType;
		float playTime;
	}
	public override void Init (List<BasicEntity> entities)
	{
		foreach (BasicEntity e in entities) {
			Animator animator = e.gameObject.AddComponent<Animator> ();

		}
	}

	public override void Execute (List<BasicEntity> entities)
	{

	}
}
