using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    Walk,
    Dead
}

public class AudioDemo : MonoBehaviour {

    private AudioSource walk;
    private AudioSource bgm;
	// Use this for initialization
	void Start () {
        walk = this.gameObject.AddComponent<AudioSource>();
        bgm = this.gameObject.AddComponent<AudioSource>();
        AudioClip walkac = Resources.Load<AudioClip>("Music/walk");
        AudioClip bgmac = Resources.Load<AudioClip>("Music/BGM");
        walk.clip = walkac;
        bgm.clip = bgmac;
        bgm.loop = true;
        bgm.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void playVoice(AudioType type)
    {
        switch (type)
        {
            case AudioType.Walk:
                walk.Play();
                break;
        }
    }
    public void pauseVoice(AudioType type)
    {
        switch (type)
        {
            case AudioType.Walk:
                walk.Pause();
                break;
        }
    }
}
