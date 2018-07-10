using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : PowerUp {

    private GameObject player;
	// Use this for initialization
	void Start ()
    {
        type = PoweUpType.SLOW;
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void ApplyPowerUp(GameObject player)
    {
        player.GetComponent<Player>().ActivePowerUp(type, duration_slow, speed_reduced_slow);
    }
}
