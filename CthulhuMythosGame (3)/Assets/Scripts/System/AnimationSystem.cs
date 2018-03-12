using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : MonoBehaviour {
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
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
