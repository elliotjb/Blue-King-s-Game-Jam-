using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpace : MonoBehaviour {


    Button button = null;
	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();

    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
            button.onClick.Invoke();


    }
}
