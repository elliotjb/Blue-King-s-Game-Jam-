using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObstacleLayer : System.Object
{
    [Range(1.0f, 360.0f)] [SerializeField] private float obstaclesepparation = 1.0f;

    [Range(1.0f, 60.0f)] [SerializeField] private float base_speed = 1.0f;
    [SerializeField] AnimationCurve speed_curve = AnimationCurve.Linear(0.0f, 1.0f, 5.0f, 1.0f);

    [Range(1.0f, 10.0f)] [SerializeField] private float offset_to_node = 1.0f;
    [SerializeField] AnimationCurve offset_curve = AnimationCurve.Linear(0.0f, 1.0f, 5.0f, 1.0f);


    private List<GameObject> obstacle_list;
    [SerializeField] private string obstacles_string;
    public bool clock_direction;

    public string obstaclesString
    {
        get { return obstacles_string; }

    }
    public List<GameObject> obstacleList
    {
        get { return obstacle_list; }
        set { obstacle_list = value; }
    }
    public float obstacleSepparation
    {
        get { return obstaclesepparation; }
    }
    public float rotationSpeed
    {
        get { return base_speed; }
    }
    public AnimationCurve speedCurve
    {
        get { return speed_curve; }
    }
    public bool useSpeedCurve
    {
        get
        {
            float value = speedCurve[0].value;
            for (int i = 1; i < speedCurve.length; i++)
            {
                if (value != speedCurve[i].value)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public float offsetToNode
    {
        get { return offset_to_node; }
    }
    public AnimationCurve offsetCurve
    {
        get { return offset_curve; }
    }
    public bool useoffsetCurve
    {
        get
        {
            float value = offsetCurve[0].value;
            for (int i = 1; i < offsetCurve.length; i++)
            {
                if (value != offsetCurve[i].value)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
public class NodeClass : MonoBehaviour
{

    [SerializeField] ObstaclePool ObstaclesPool;
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
            obj.obstacleList = new List<GameObject>();
        }

        ClearObstacles();
        CreateObstacles();
    }

    // Update is called once per frame
    void Update()
    {
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
            obj.obstacleList = new List<GameObject>();
        }
    }
    public void ClearObstacles()
    {
        foreach (ObstacleLayer item in ObstaclesArray)
        {
            foreach (GameObject obj in item.obstacleList)
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
            item.obstacleList.Clear();
            //Create obstacles
        }
    }
    public void ChangeSpeed(float speed)
    {
        foreach (ObstacleLayer obj in ObstaclesArray)
        {
            foreach (GameObject item in obj.obstacleList)
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
            foreach (GameObject item in obj.obstacleList)
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
        float offset_acumulation = 0.0f;
        foreach (ObstacleLayer item in ObstaclesArray)
        {

            item.obstacleList.Clear();
            offset_acumulation += item.offsetToNode;
            Vector3 trans = new Vector3(transform.position.x, transform.position.y + offset_acumulation, transform.position.z);
            string[] prefabnumber = item.obstaclesString.Split(',');

            int i = 0;
            foreach (string obstacle in prefabnumber)
            {
                int obsvalue = 0;
                int.TryParse(obstacle, out obsvalue);


                if (!(obsvalue <= 0 || obsvalue >= ObstaclesPool.ObstaclesPool.Length))
                {
                    Quaternion rot = Quaternion.identity;
                    if (obsvalue == 4) //star and shield
                    {
                        rot = Quaternion.Euler(new Vector3(0, -90, 0));
                    }
                    else if (obsvalue == 9)
                    {
                        rot = Quaternion.Euler(new Vector3(90, 0, 0));
                    }
                    GameObject Obstacle = Instantiate(ObstaclesPool.ObstaclesPool[obsvalue], trans, rot);
                    float angle = (i * item.obstacleSepparation);
                    Obstacle.transform.RotateAround(transform.position, Vector3.forward, angle);
                    Obstacle.GetComponent<ObstacleScript>().nodePosition = transform.position;
                    Obstacle.GetComponent<ObstacleScript>().SetOffset(item.offsetToNode,(item.useoffsetCurve) ? item.offsetCurve : null);

                    if (item.clock_direction)
                    {
                        Obstacle.GetComponent<ObstacleScript>().SetAngle(-item.rotationSpeed, (item.useSpeedCurve)?item.speedCurve:null);
                    }
                    else
                    {
                        Obstacle.GetComponent<ObstacleScript>().SetAngle(item.rotationSpeed, (item.useSpeedCurve) ? item.speedCurve : null);
                    }

                    item.obstacleList.Add(Obstacle);
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
            string[] prefabnumber = item.obstaclesString.Split(',');
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

    private void OnDrawGizmos()
    {
        float offset_acumulation = 0.0f;

        foreach (ObstacleLayer item in ObstaclesArray)
        {

            offset_acumulation += item.offsetToNode;
            Vector3 trans = new Vector3(transform.position.x, transform.position.y + offset_acumulation, transform.position.z);
            string[] prefabnumber = item.obstaclesString.Split(',');

            int i = 0;
            foreach (string obstacle in prefabnumber)
            {
                int obsvalue = 0;
                int.TryParse(obstacle, out obsvalue);
                if (obsvalue != 0)
                {

                    float size = (ObstaclesPool.ObstaclesPool[obsvalue] == null) ? 0.5f : ObstaclesPool.ObstaclesPool[obsvalue].transform.localScale.x;//star and shield
                    float angle = (i * item.obstacleSepparation);
                    Vector3 dir = trans - transform.position; // get point direction relative to pivot
                    dir = Quaternion.Euler(0, 0, angle) * dir; // rotate it
                    trans = dir + transform.position; // calculate rotated point
                    Gizmos.DrawSphere(trans, size/*ObstaclesPool[obsvalue].transform.localScale.x*/);
                }
                i++;
            }
        }
    }


}
