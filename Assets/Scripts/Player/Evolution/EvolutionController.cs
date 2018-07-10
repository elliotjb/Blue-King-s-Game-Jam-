using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EvolutionType
{
    NONE = -1,
    SIZE,
    SPEED,
    ORANGE_COLOR, 
    GREEN_COLOR,
    PINK_COLOR
}

public class EvolutionController : MonoBehaviour
{
    //PROGRESSION FIELDS -------------------------------------
    [SerializeField] float speed_progression = 0.0f;
    [SerializeField] float size_progression = 0.0f;
    [SerializeField] int orange_charges = 0;
    [SerializeField] int green_charges = 0;
    [SerializeField] int pink_charges = 0;
    // -------------------------------------------------------

    //CURRENT PROGRESSION VALUES -----------------------------
    [SerializeField] float curr_speed = 0.0f;
    [SerializeField] float curr_size = 0.0f;
    [SerializeField] int curr_orange = 0;
    [SerializeField] int curr_green = 0;
    [SerializeField] int curr_pink = 0;
    // -------------------------------------------------------

    //MAX PROGRESSION VALUES ---------------------------------
    public float max_speed = 300.0f;
    public float min_size = 0.5f;
    public int max_orange_charges = 5;
    public int max_green_charges = 5;
    public int max_pink_charges = 5;
    // -------------------------------------------------------

    //HUD VARIABLES -----------------------------------------
    public GameObject size_hud;
    public GameObject speed_hud;
    public GameObject green_hud;
    public GameObject orange_hud;
    public GameObject pink_hud;
    // -------------------------------------------------------
    public GameObject color_ball;


    
    GameObject temp_object;

    //Sounds
    public AudioClip evolvesound;
    public AudioSource source;

    public Player player;

    // Use this for initialization
    void Start ()
    {
		curr_size = 0.0f;
        player = GetComponent<Player>();
        source = GetComponent<AudioSource>();
        //RESET HUD -------------------------------------------------------
        /*Speed*/
        temp_object = speed_hud.transform.Find("SpeedBar").gameObject;
        temp_object.GetComponent<Image>().fillAmount = 0.0f;
        
        /*Size*/
        temp_object = size_hud.transform.Find("SizeBar").gameObject;
        temp_object.GetComponent<Image>().fillAmount = size_progression * 0.1f;

        /* Colors */
        green_hud.transform.Find("Image").gameObject.SetActive(false);
        orange_hud.transform.Find("Image").gameObject.SetActive(false);
        pink_hud.transform.Find("Image").gameObject.SetActive(false);

        //-----------------------------------------------------------------
    }

    // Update is called once per frame
    void Update ()
    {
        if (player.debug)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                EvolveSpeed(100.0f);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                EvolveSize(0.02f);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                EvolveColor(2);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                EvolveColor(3);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                EvolveColor(4);
            }
        }
    }

    public bool IsFullEvolved()
    {
        if(SizeMaxed() && SpeedMaxed() && OrangeMaxed() && GreenMaxed() && PinkMaxed())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //SPEED EVOLUTION -----------------------------------------
    public void EvolveSpeed(float speed_factor)
    {
        if (speed_progression < 100.0f) 
        {
            curr_speed += speed_factor;

            //Update progression
            speed_progression = curr_speed / max_speed * 100.0f;

            //Modify HUD
            temp_object = speed_hud.transform.Find("SpeedBar").gameObject;
            temp_object.GetComponent<Image>().fillAmount = speed_progression * 0.01f;
            source.PlayOneShot(evolvesound);
        }

    }

    public float ExtraSpeed()
    {
        return curr_speed;
    }

    public bool SpeedMaxed()
    {
        return speed_progression >= 100.0f;
    }
    // ---------------------------------------------------------

    //SIZE EVOLUTION -----------------------------------------
    public void EvolveSize(float size_factor)
    {
        if (size_progression < 100.0f && transform.localScale.x > 0.12)
        {
            curr_size += size_factor;

            // size factor < 0 = REDUCE
            // size factor > 0 = AUGMENT
            transform.localScale += new Vector3(-size_factor, -size_factor, -size_factor);

            GetComponent<TrailRenderer>().widthMultiplier -= size_factor;

            //Update progression
            size_progression = curr_size / min_size * 100.0f;
            if (size_progression > 99.9f)
            {
                size_progression = 100.0f;
            }

            //Modify HUD
            temp_object = size_hud.transform.Find("SizeBar").gameObject;
            temp_object.GetComponent<Image>().fillAmount = size_progression * 0.01f;
            source.PlayOneShot(evolvesound);
        }

    }

    public bool SizeMaxed()
    {
        return size_progression >= 100.0f;
    }
    // ---------------------------------------------------------

    //COLOR EVOLUTION -----------------------------------------
    public void EvolveColor(int color)
    {
        //INCREASE TOTAL CHARGES of the specified color
        if ((EvolutionType)color == EvolutionType.ORANGE_COLOR)
        {
            if (orange_charges < max_orange_charges)
            {
                orange_charges += 1;

                // Activate Color Ball
                color_ball.GetComponent<ColorBall>().ActivateColor(ColorType.ORANGE_COLOR);

                //Set the current charges to max
                curr_orange = orange_charges;
            }
            
        }
        else if ((EvolutionType)color == EvolutionType.GREEN_COLOR)
        {
            if (green_charges < max_green_charges)
            {
                green_charges += 1;

                // Activate Color Ball
                color_ball.GetComponent<ColorBall>().ActivateColor(ColorType.GREEN_COLOR);

                //Set the current charges to max
                curr_green = green_charges;
            }
        }
        else if ((EvolutionType)color == EvolutionType.PINK_COLOR)
        {
            if (pink_charges < max_pink_charges)
            {
                pink_charges += 1;

                // Activate Color Ball
                color_ball.GetComponent<ColorBall>().ActivateColor(ColorType.PINK_COLOR);

                //Set the current charges to max
                curr_pink = pink_charges;
            }
        }
        source.PlayOneShot(evolvesound);
        //Modify HUD
        ModifyColorHUD((EvolutionType)color);
    }

    //UPDATE HUD VALUES OF COLORS
    public void ModifyColorHUD(EvolutionType color)
    {
        if (color == EvolutionType.ORANGE_COLOR)
        {
            temp_object = orange_hud.transform.Find("charges").gameObject;
            temp_object.GetComponent<Text>().text = curr_orange.ToString();

            temp_object = orange_hud.transform.Find("Image").gameObject;

            if (curr_orange > 0)
            {
                temp_object.SetActive(true);
            }
            else
            {
                temp_object.SetActive(false);
            }
        }
        else if (color == EvolutionType.GREEN_COLOR)
        {
            temp_object = green_hud.transform.Find("charges").gameObject;
            temp_object.GetComponent<Text>().text = curr_green.ToString();

            temp_object = green_hud.transform.Find("Image").gameObject;
            if (curr_green > 0)
            {
                temp_object.SetActive(true);
            }
            else
            {
                temp_object.SetActive(false);
            }
        }
        else if (color == EvolutionType.PINK_COLOR)
        {
            temp_object = pink_hud.transform.Find("charges").gameObject;
            temp_object.GetComponent<Text>().text = curr_pink.ToString();

            temp_object = pink_hud.transform.Find("Image").gameObject;
            if (curr_pink > 0)
            {
                temp_object.SetActive(true);
            }
            else
            {
                temp_object.SetActive(false);
            }
        }
    }

    //CHECK COLLISIONS WITH COLOR OBSTACLES
    public bool CheckCollisionObstacle(string color_obstacle)
    {
        if (color_obstacle == "Obstacle_Green") 
        {
            //Check if you have at least 1 color charge to break that obstacle
            if (curr_green > 0) 
            {
                curr_green -= 1;
                ModifyColorHUD(EvolutionType.GREEN_COLOR);
                if (curr_green <= 0)
                {
                    // Desactivate Color Ball
                    color_ball.GetComponent<ColorBall>().DesactivateColor(ColorType.GREEN_COLOR);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        else if (color_obstacle == "Obstacle_Orange")
        {
            //Check if you have at least 1 color charge to break that obstacle
            if (curr_orange > 0)
            {
                curr_orange -= 1;
                ModifyColorHUD(EvolutionType.ORANGE_COLOR);
                if (curr_orange <= 0)
                {
                    // Desactivate Color Ball
                    color_ball.GetComponent<ColorBall>().DesactivateColor(ColorType.ORANGE_COLOR);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        else if (color_obstacle == "Obstacle_Pink")
        {
            //Check if you have at least 1 color charge to break that obstacle
            if (curr_pink > 0)
            {
                curr_pink -= 1;
                ModifyColorHUD(EvolutionType.PINK_COLOR);
                if (curr_pink <= 0)
                {
                    // Desactivate Color Ball
                    color_ball.GetComponent<ColorBall>().DesactivateColor(ColorType.PINK_COLOR);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    //Restart Color charges when arriving to the next node
    public void RestartColorCharges()
    {
        //Update Charges
        curr_green = green_charges;
        curr_orange = orange_charges;
        curr_pink = pink_charges;

        //Update HUD
        ModifyColorHUD(EvolutionType.GREEN_COLOR);
        ModifyColorHUD(EvolutionType.ORANGE_COLOR);
        ModifyColorHUD(EvolutionType.PINK_COLOR);

        // Activate Color Ball
        if (curr_green > 0)
        {
            color_ball.GetComponent<ColorBall>().ActivateColor(ColorType.GREEN_COLOR);
        }
        if (curr_orange > 0)
        {
            color_ball.GetComponent<ColorBall>().ActivateColor(ColorType.ORANGE_COLOR);
        }
        if (curr_pink > 0)
        {
            color_ball.GetComponent<ColorBall>().ActivateColor(ColorType.PINK_COLOR);
        }
    }

    public bool OrangeMaxed()
    {
        return orange_charges == max_orange_charges;
    }

    public bool GreenMaxed()
    {
        return green_charges == max_green_charges;
    }

    public bool PinkMaxed()
    {
        return pink_charges == max_pink_charges;
    }
    // ---------------------------------------------------------

}
