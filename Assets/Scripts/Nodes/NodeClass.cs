using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleLayer : System.Object
{
    [Range(1.0f, 360.0f)] public float obstaclesepparation = 1.0f;
    [Range(1.0f, 10.0f)] public float offset_to_node = 1.0f;
    [Range(1.0f, 60.0f)] public float rotation_speed = 1.0f;
    public List<GameObject> obstacle_list;
    public string obstaclesstring;
    public bool clock_direction;

}
public class NodeClass : MonoBehaviour {

    public GameObject[] ObstaclesPool = new GameObject[1];
    public bool evolution_active = false;

    public int NumofLayers = 1;
    public ObstacleLayer[] ObstaclesArray;
    private GameObject player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        foreach (ObstacleLayer obj in ObstaclesArray)
        {
            obj.obstacle_list = new List<GameObject>();
        }

        ClearObstacles();
        CreateObstacles();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K))
        {
            CreateObstacles();

        }
	}
    void ResizeObjLayer()
    {
        ObstaclesArray = new ObstacleLayer[NumofLayers];
        foreach (ObstacleLayer obj in ObstaclesArray)
        {
            obj.obstacle_list = new List<GameObject>();
        }
    }
    public void ClearObstacles()
    {
        foreach (ObstacleLayer item in ObstaclesArray)
        {
            foreach (GameObject obj in item.obstacle_list)
            {
                if (obj.activeInHierarchy == true)
                {
                    if (obj.GetComponent<ObstacleScript>().leaving == false)
                    {
                        obj.GetComponent<ObstacleScript>().Explode();
                        obj.GetComponent<Collider>().enabled = false;
                    }
                    else
                    {
                        Destroy(obj);
                    }
                }
                else
                {
                    Destroy(obj);
                }
            }
            item.obstacle_list.Clear();
            //Create obstacles
        }
    }
    public void ChangeSpeed(float speed)
    {
        foreach (ObstacleLayer obj in ObstaclesArray)
        {
            foreach (GameObject item in obj.obstacle_list)
            {
                item.GetComponent<ObstacleScript>().LastAngle = item.GetComponent<ObstacleScript>().Angle;
                item.GetComponent<ObstacleScript>().Angle = item.GetComponent<ObstacleScript>().Angle / speed;
            }
        }
    }

    public void ResetObstaclesSpeed()
    {
        foreach (ObstacleLayer obj in ObstaclesArray)
        {
            foreach (GameObject item in obj.obstacle_list)
            {
                item.GetComponent<ObstacleScript>().Angle = item.GetComponent<ObstacleScript>().LastAngle;
            }
        }
    }
    public void CloneParameters(NodeClass node)
    {
        evolution_active = node.evolution_active;

        ObstaclesArray = node.ObstaclesArray;
    }

    public void CreateObstacles()
    {
        
        foreach (ObstacleLayer item in ObstaclesArray)
        {
           
            item.obstacle_list.Clear();
            Vector3 trans = new Vector3(transform.position.x, transform.position.y + item.offset_to_node, transform.position.z);
            string[] prefabnumber = item.obstaclesstring.Split(',');

            int i = 0;
            foreach (string obstacle in prefabnumber)
            {
                int obsvalue = 0;
                int.TryParse(obstacle, out obsvalue);


                if (!(obsvalue <= 0 || obsvalue >= ObstaclesPool.Length))
                    {
                        Quaternion rot = Quaternion.identity;
                        if(obsvalue == 4) //star and shield
                        {
                            rot = Quaternion.Euler(new Vector3(0, -90, 0));
                        }
                        else if(obsvalue == 9)
                        {
                        rot = Quaternion.Euler(new Vector3(90, 0, 0));
                    }
                        GameObject Obstacle = Instantiate(ObstaclesPool[obsvalue], trans, rot);
                        float angle = (i * item.obstaclesepparation);
                        Obstacle.transform.RotateAround(transform.position, Vector3.forward, angle);
                        Obstacle.GetComponent<ObstacleScript>().nodePosition = transform.position;

                        if (item.clock_direction)
                        {
                            Obstacle.GetComponent<ObstacleScript>().Angle = -Obstacle.GetComponent<ObstacleScript>().Angle * item.rotation_speed;
                        }
                        else
                        {
                            Obstacle.GetComponent<ObstacleScript>().Angle = Obstacle.GetComponent<ObstacleScript>().Angle * item.rotation_speed;
                        }
                        item.obstacle_list.Add(Obstacle);
                    }
                i++;
            }
        }
        if (player.GetComponent<Player>().slow_active)
        {
            ChangeSpeed(player.GetComponent<Player>().save_slow_reduced);
        }
        
    }
    public void LookContent()
    {
     
        if (evolution_active)
        {
            EventManager.TriggerEvent("Evolution Spawn");
            GetComponentInChildren<EvolutionNode>().enabled = true;
            GetComponentInChildren<MeshRenderer>().enabled = true;
            GetComponentInChildren<SphereCollider>().enabled = true;

        }
        else
        {
            GetComponentInChildren<EvolutionNode>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponentInChildren<SphereCollider>().enabled = true;

        }
        foreach (ObstacleLayer item in ObstaclesArray)
        {
            string[] prefabnumber = item.obstaclesstring.Split(',');
            foreach (string obstacle in prefabnumber)
            {
                int obsvalue = 0;
                int.TryParse(obstacle, out obsvalue);
                if (obsvalue == 4)
                    EventManager.TriggerEvent("Metal Spawn");
                else if (obsvalue == 8)
                    EventManager.TriggerEvent("Clock Spawn");
                else if (obsvalue == 9)
                    EventManager.TriggerEvent("Start Spawn");
            }
        }
    }
}
