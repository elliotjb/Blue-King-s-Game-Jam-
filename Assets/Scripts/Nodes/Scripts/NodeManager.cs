using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

    [SerializeField] private List<GameObject> NodeList = null;
    [SerializeField] private GameObject node = null;
    [SerializeField] private GameObject target = null;
    [SerializeField] private EvolutionController evolution_controller = null;

    [Range(2, 10)] [SerializeField] private int num_nodes = 4;
    [Range(0.0f, 10.0f)] [SerializeField] private float spawn_distance = 10.0f;

    [Range(0.0f, 1.0f)] [SerializeField] private float spawn_y_offset = 0.0f;
    [Range(0.0f, 50.0f)]  [SerializeField] private float node_x_offset = 5.0f;
    private GameObject current_node = null;
    private GameObject next_node = null;
    private int current_node_index = 0;
    private int previus_node_index = 3;

    private int node_lvl_count = 0;
    
    void Start()
    {
        node_lvl_count = 0;
        for(int i =0; i<num_nodes;i++)
        {
            float y = Random.Range(0.0f + spawn_y_offset, 1.0f - spawn_y_offset);
            Vector3 position = new Vector3(0.5f, y, spawn_distance);
            position = Camera.main.ViewportToWorldPoint(position);
            position.x += node_x_offset * i;
            GameObject item = Instantiate(NodeLoader.GetNode(i), position,Quaternion.identity,gameObject.transform);
            NodeList.Add(item);
            node_lvl_count++;
        }
        node_lvl_count--;

        current_node = NodeList[0];
        current_node.GetComponent<NodeClass>().ClearObstacles();
        next_node = NodeList[1];
        target.transform.position = next_node.transform.position;
    }
    public GameObject GetCurrentNode()
    {
        return current_node;
    }
    public GameObject GetNextNode()
    {
        return next_node;
    }
    public void UpdateSectableNodes()
    {
        node_lvl_count++;
           previus_node_index = current_node_index;
        if (++current_node_index > NodeList.Count-1)
        {
            current_node_index = 0;
            previus_node_index = 3;
        }

        current_node = NodeList[current_node_index];
        current_node.GetComponent<NodeClass>().ClearObstacles();

        next_node = (current_node_index + 1 >= NodeList.Count)?NodeList[0] : next_node = NodeList[current_node_index + 1];
        next_node.GetComponent<NodeClass>().LookContent();

        target.transform.position = next_node.transform.position;

        UpdateNodePosition();
    }
    void UpdateNodePosition()
    {
        float pos_y = Random.Range(0.0f + spawn_y_offset, 1.0f - spawn_y_offset);
        Vector3 camera_spawn = new Vector3(0.5f, pos_y, spawn_distance);
        camera_spawn = Camera.main.ViewportToWorldPoint(camera_spawn);
        
        Vector3 position = new Vector3(NodeList[current_node_index].transform.position.x + node_x_offset * 3, camera_spawn.y, NodeList[current_node_index].transform.position.z);
        if(NodeList[previus_node_index].GetComponentInChildren<EvolutionNode>()!=null)
        {
            NodeList[previus_node_index].GetComponentInChildren<EvolutionNode>().used = false;
        }
        NodeList[previus_node_index].GetComponent<NodeClass>().CloneParameters(NodeLoader.GetNode(node_lvl_count).GetComponent<NodeClass>());
        if (NodeList[previus_node_index].GetComponent<NodeClass>().evolution_active && !evolution_controller.IsFullEvolved())
        {
            NodeList[previus_node_index].GetComponentInChildren<EvolutionNode>().enabled = true;
            NodeList[previus_node_index].GetComponentInChildren<MeshRenderer>().enabled = true;
            NodeList[previus_node_index].GetComponentInChildren<Collider>().enabled = true;
        }
        
        NodeList[previus_node_index].transform.position = position;

        NodeList[previus_node_index].GetComponent<NodeClass>().CreateObstacles();


    }

    public void ChangeSpeed(float speed)
    {
        for (int i = 0; i < NodeList.Count; i++)
        {
            NodeList[i].GetComponent<NodeClass>().ChangeSpeed(speed);
        }
    }
    public void ResetObstaclesSpeed()
    {
        for (int i = 0; i < NodeList.Count; i++)
        {
            NodeList[i].GetComponent<NodeClass>().ResetObstaclesSpeed();
        }
    }
}

