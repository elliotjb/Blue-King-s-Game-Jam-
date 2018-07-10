 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour {

    public GameObject metal_group = null;
    public GameObject clock_group = null;
    public GameObject start_group = null;
    public GameObject evolution_group = null;
    static bool metal_tutorial_done = false;
    static bool clock_tutorial_done = false;
    static bool star_tutorial_done = false;
    static bool evolution_tutorial_done = false;
    public Player player = null;
    // Use this for initialization
    private void Awake()
    {
        if (!metal_tutorial_done)
            EventManager.StartListening("Metal Spawn", ShowMetalTutorial);
        if (!clock_tutorial_done)
            EventManager.StartListening("Clock Spawn", ShowClockTutorial);
        if (!star_tutorial_done)
            EventManager.StartListening("Start Spawn", ShowStarTutorial);
        if (!evolution_tutorial_done)
            EventManager.StartListening("Evolution Spawn", ShowEvolutionTutorial);
        
    }
    private void OnDestroy()
    {
   
        EventManager.StopListening("Metal Spawn", ShowMetalTutorial);
        EventManager.StopListening("Clock Spawn", ShowClockTutorial);
        EventManager.StopListening("Start Spawn", ShowStarTutorial);
        EventManager.StopListening("Evolution Spawn", ShowEvolutionTutorial);

    }
    void Start () {
        metal_group.SetActive(false);
        clock_group.SetActive(false);
        start_group.SetActive(false);
        evolution_group.SetActive(false);

    }

    // Update is called once per frame
    void Update () {
		
	}
    public void TutorialCancel()
    {
        player.TutorialClose();
    }

    void ShowMetalTutorial()
    {
        metal_group.SetActive(true);
        metal_tutorial_done = true;
        player.tutorial_active = true;
        EventManager.StopListening("Metal Spawn", ShowMetalTutorial);

    }
    void ShowClockTutorial()
    {
        clock_group.SetActive(true);
        clock_tutorial_done = true;
        player.tutorial_active = true;

        EventManager.StopListening("Clock Spawn", ShowClockTutorial);

    }
    void ShowStarTutorial()
    {
        start_group.SetActive(true);
        star_tutorial_done = true;
        player.tutorial_active = true;

        EventManager.StopListening("Start Spawn", ShowStarTutorial);

    }
    void ShowEvolutionTutorial()
    {
        evolution_group.SetActive(true);
        evolution_tutorial_done = true;
        player.tutorial_active = true;

        EventManager.StopListening("Evolution Spawn", ShowEvolutionTutorial);


    }
}
