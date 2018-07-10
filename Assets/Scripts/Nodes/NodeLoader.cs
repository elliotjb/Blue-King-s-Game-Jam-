using UnityEngine;
using System.Linq;
public class NodeLoader : MonoBehaviour {

    [SerializeField] private static GameObject[] node_prefabs;
    // Use this for initialization
    void Start()
    {
        node_prefabs = Resources.LoadAll("Nodes", typeof(GameObject)).Cast<GameObject>().ToArray();
    }
    
    public static GameObject GetNode(int index)
    {
        int temp1 = 0;
        if (index < 0 || index > node_prefabs.Length - 1)
        {
            temp1 = Random.Range(6, (node_prefabs.Length - 1));
            if (temp1 <= 5)
            {
                temp1 += 5;
                return node_prefabs[temp1];
            }
            return node_prefabs[temp1];
        }

        return node_prefabs[index];
    }
    public static GameObject GetNodeRandom()
    {
            return node_prefabs[Random.Range(0,node_prefabs.Length)];

    }

}
