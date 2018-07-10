using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Game : MonoBehaviour {

    public GameObject score;
    public GameObject evolution;
    public GameObject powerup;
    public GameObject pause_button;
    public GameObject pause;
    public GameObject mainmenu;
    public GameObject dead;
    public Text score_dead;
    public int actual_score;
    public static int max_score;

    // Use this for initialization
    void Start () {
        score.SetActive(false);
        evolution.SetActive(false);
        powerup.SetActive(false);
        pause.SetActive(false);
        pause_button.SetActive(false);
        dead.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void LateUpdate()
    {
        if (dead.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("ScenePlayer");
            }
        }
    }

    public void PlayGame()
    {
        score.SetActive(true);
        evolution.SetActive(true);
        powerup.SetActive(true);
        pause_button.SetActive(true);
        mainmenu.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ForceShoot();
    }

    public void Pause()
    {
        pause.SetActive(true);
    }

    public void Score_window(int score_player)
    {
        dead.SetActive(true);
        score.SetActive(false);
        score_dead.text = score_player.ToString();
        evolution.SetActive(false);
        powerup.SetActive(false);
    }

    // Pause Buttons --------------------
    public void Continue()
    {
        pause.SetActive(false);
    }

    public void Restart()
    {
        //
        SceneManager.LoadScene("ScenePlayer");
    }

    public void Exit()
    {
        Application.Quit();
    }

}
