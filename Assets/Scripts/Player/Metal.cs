using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : PowerUp {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void ApplyPowerUp(GameObject player)
    {
        player.GetComponent<Player>().ActivePowerUp(type, duration_armor);
    }
}
