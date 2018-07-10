using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoweUpType
{
    NONE = -1,
    SLOW = 0,
    STAR,
    METAL
}   

public class PowerUp : MonoBehaviour
{
    [SerializeField] protected PoweUpType type = PoweUpType.NONE;

    // PowerUp Metal (Armor) --------------------------------
    [SerializeField] protected float duration_armor = 8.0f;

    // PowerUp Star (Invencible) --------------------------------
    [SerializeField] protected float duration_star = 12.0f;
    [SerializeField] protected float aument_speed = 20.0f;
    
    // PowerUp SlowMotion (:D) --------------------------------
    [SerializeField] protected float duration_slow = 8.0f;
    [SerializeField] protected float speed_reduced_slow = 7.0f;


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    virtual public void ApplyPowerUp(GameObject player)
    {

    }

    public PoweUpType GetTypePowerUp()
    {
        return type;
    }
}
