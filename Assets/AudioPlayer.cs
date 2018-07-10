using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

	// Use this for initialization
    public AudioSource audiosoruce;
    void Start() {
        audiosoruce = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Space))
        {
            PlaySound();
        }
	}
    public void PlaySound()
    {
        Debug.Log("before");
        GetComponent<AudioSource>().Play();
        Debug.Log("After");
    }
}
