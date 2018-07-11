using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstaclePool", menuName = "Obstacles/List", order = 1)]
public class ObstaclePool : ScriptableObject {

    [SerializeField]private GameObject[] obstacle_pool;

    public GameObject[] ObstaclesPool
    {
        get { return obstacle_pool;}
    }
}
