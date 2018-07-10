using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EvolutionPlace
{
    NONE = -1,
    LEFT,
    CENTER,
    RIGHT
}

// 0 = size
// 1 = speed
// 2 = orange
// 3 = green
// 4 = pink

public class EvolutionProvider : MonoBehaviour
{
    public GameObject[] all_evolutions;

    List<int> to_show_index;

    //PLACE VARIABLES 
    [SerializeField] GameObject left_evo;
    [SerializeField] GameObject center_evo;
    [SerializeField] GameObject right_evo;

    int index = -1;

    public bool show = false;

    public EvolutionController player_evos;

	// Use this for initialization
	void Start ()
    {
        to_show_index = new List<int> { 0, 1, 2, 3, 4 };

        //StartProvider();
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void StartProvider()
    {
        //Clear show evolutions list and fill with all possible evolutions
        to_show_index.Clear();

        if (player_evos.SizeMaxed() == false)
        {
            to_show_index.Add(0);
        }
        if (player_evos.SpeedMaxed() == false)
        {
            to_show_index.Add(1);
        }
        if (player_evos.OrangeMaxed() == false)
        {
            to_show_index.Add(2);
        }
        if (player_evos.GreenMaxed() == false)
        {
            to_show_index.Add(3);
        }
        if (player_evos.PinkMaxed() == false)
        {
            to_show_index.Add(4);
        }
    
        SetEvolution(EvolutionPlace.LEFT);
        SetEvolution(EvolutionPlace.CENTER);
        SetEvolution(EvolutionPlace.RIGHT);

        show = true;
    }

    void SetEvolution(EvolutionPlace place)
    {
        //Show possible evolutions to upgrade only
        if (to_show_index.Count > 0)
        {
            //Get a random evo from the sow list
            index = Random.Range(0, to_show_index.Count);

            //Get Left evolution element
            GetEvolution(place);

            //Remove from the show list
            to_show_index.RemoveAt(index);

            //Set Element at the correct place
            SetEvoPosition(place);
        }
    }

    void GetEvolution(EvolutionPlace place)
    {
        if(place == EvolutionPlace.LEFT)
        {
            left_evo = all_evolutions[to_show_index[index]];
        }
        else if (place == EvolutionPlace.CENTER)
        {
            center_evo = all_evolutions[to_show_index[index]];
        }
        else if (place == EvolutionPlace.RIGHT)
        {
            right_evo = all_evolutions[to_show_index[index]];
        }
    }

    void SetEvoPosition(EvolutionPlace place)
    {
        if (place == EvolutionPlace.LEFT)
        {
            left_evo.transform.Translate(new Vector3(-220f, 0, 0));
            left_evo.SetActive(true);
        }
        else if (place == EvolutionPlace.CENTER)
        {
            left_evo.transform.Translate(new Vector3(0, 0, 0));
            center_evo.SetActive(true);
        }
        else if (place == EvolutionPlace.RIGHT)
        {
            right_evo.transform.Translate(new Vector3(220.0f, 0, 0));
            right_evo.SetActive(true);
        }
    }

    public void CloseProvider()
    {
        //Reset positions
        if (left_evo != null)
        {
            left_evo.transform.Translate(new Vector3(220.0f, 0, 0));
            left_evo.SetActive(false);

        }
        if (center_evo != null)
        {
            center_evo.SetActive(false);
        }
        if (right_evo != null)
        {
            right_evo.transform.Translate(new Vector3(-220.0f, 0, 0));
            right_evo.SetActive(false);
        }

        show = false;

        transform.gameObject.SetActive(false);

        //StartProvider();
    }
}
