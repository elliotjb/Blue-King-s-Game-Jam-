using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScoreType
{
    ARRIVE_NODE = 0,
    POWERUP_ARMOR,
    POWERUP_STAR,
    POWERUP_SLOW,
    STAR,
    DESTROY_OBSTACLE,
    DESTROY_ENEMY
}

public class Score : MonoBehaviour {

    // Score-----------------------
    Text score;
    [SerializeField] private int score_arrive_node = 250;
    [SerializeField] private int score_powerup_armor = 100;
    [SerializeField] private int score_powerup_star = 200;
    [SerializeField] private int score_star = 70;
    [SerializeField] private int score_powerup_slowmotion = 50;
    [SerializeField] private int score_destroy_obstacle = 370;
    [SerializeField] private int score_destroy_enemy = 200;
    // ----------------------------

    // Use this for initialization
    void Start () {
        score = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddScore(ScoreType type)
    {
        int current_score = int.Parse(score.text);
        switch(type)
        {
            case ScoreType.ARRIVE_NODE:
                {
                    current_score += score_arrive_node;
                    break;
                }
            case ScoreType.POWERUP_ARMOR:
                {
                    current_score += score_powerup_armor;
                    break;
                }
            case ScoreType.POWERUP_STAR:
                {
                    current_score += score_powerup_star;
                    break;
                }
            case ScoreType.POWERUP_SLOW:
                {
                    current_score += score_powerup_slowmotion;
                    break;
                }
            case ScoreType.STAR:
                {
                    current_score += score_star;
                    break;
                }
            case ScoreType.DESTROY_OBSTACLE:
                {
                    current_score += score_destroy_obstacle;
                    break;
                }
            case ScoreType.DESTROY_ENEMY:
                {
                    current_score += score_destroy_enemy;
                    break;
                }
        }
        score.text = current_score.ToString();
    }

    public int GetScore()
    {
        return int.Parse(score.text);
    }

}
