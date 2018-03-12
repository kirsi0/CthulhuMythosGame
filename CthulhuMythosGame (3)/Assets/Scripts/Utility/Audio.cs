using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Audio : MonoBehaviour {

    private AudioSource audioSource;
	// Use this for initialization
	void Start () {
        audioSource = this.gameObject.AddComponent<AudioSource>();

        AudioClip audioClip = Resources.Load<AudioClip>("walk");

        audioSource.loop = true;
        audioSource.clip = audioClip;
        audioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPlayerConnected(NetworkPlayer player)
    {
        
    }

}
