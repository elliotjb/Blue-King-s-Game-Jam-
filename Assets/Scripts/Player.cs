using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatePlayer
{
    IN_NODE = 0,
    ARRIVE,
    MOVING,
    RETURN,
    DYING
}

public class Player : MonoBehaviour
{
    private GameObject initial_node;
    private GameObject next_node;

    public GameObject UI_Menu;

    public GameObject camera_go;
    public NodeManager nodeManager;

    //[SerializeField] private GameObject particle_hit;
    [SerializeField] private Score score;

    [SerializeField] private float velocity = 20.0f;
    [SerializeField] private Vector3 initial_position;
    [SerializeField] private StatePlayer state = StatePlayer.IN_NODE;
    public Vector3 next_position;
    private bool isReturn = false;

    // PowerUp Variables -----------------------------
    public bool armor_active = false;
    public bool star_active = false;
    public bool slow_active = false;
    private float duration_armor = 0.0f;
    private float duration_star = 0.0f;
    private float duration_slow = 0.0f;
    private float current_time_armor = 0.0f;
    private float current_time_star = 0.0f;
    private float current_time_slow = 0.0f;

    public float save_slow_reduced = 2.0f;
    private float save_time_trail;
    public float new_time_trail = 1.0f;

    public GameObject metal_hud;
    public GameObject star_hud;
    public GameObject slowmo_hud;
    public Image powerup_bar;
    // -----------------------------------------------

    private Vector3 direction;
    private float distance_arrive = 0.0f;
    static public bool startGame = false;
    public bool tutorial_active = false;

    EvolutionController evo_controller;
    public TargetIndicator target_controller;
    public EvolutionProvider evo_provider;

    // Materials -----------------------------
    public GameObject script_star;
    public Material normal_mat;
    public Material star_mat;
    public Material metal_mat;
    //----------------------------------------

    private float time_arrive = 0.0f;

    //Sounds
    public AudioClip NodeSound;
    public AudioClip MetalSound;
    public AudioClip StarSound;
    public AudioClip SlowDown;
    public AudioClip Accelerate;
    public AudioClip defeat;
    public AudioSource source;
    public AudioSource camerasource;

    public bool debug = false;

    // Use this for initialization
    void Start()
    {
        tutorial_active = false;
        source = GetComponent<AudioSource>();
        initial_node = nodeManager.GetCurrentNode();
        next_node = nodeManager.GetNextNode();
        evo_controller = GetComponent<EvolutionController>();
        transform.position = initial_node.transform.position;
        initial_position = initial_node.transform.position;
        next_position = next_node.transform.position;

        //Update Arrow direction
        target_controller.SetTarget(next_node.transform);

        //Set camera at the initial position
        camera_go.GetComponent<CameraController>().MoveCamera(transform);

        //Reset HUD
        DisablePowerUpHUD(PoweUpType.METAL);
        DisablePowerUpHUD(PoweUpType.SLOW);
        DisablePowerUpHUD(PoweUpType.STAR);

        script_star.GetComponent<ScrollingUVs_Layers>().enabled = false;
        if (startGame)
        {
            UI_Menu.GetComponent<UI_Game>().PlayGame();
        }
    }

    public void ActiveEvoProvider()
    {
        evo_provider.gameObject.SetActive(true);
        evo_provider.StartProvider();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            debug = !debug;
        }

        if (debug)
        {
            //Test Power Ups
            if (Input.GetKeyDown(KeyCode.S))
            {
                ActivePowerUp(PoweUpType.STAR, 5.0f);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                ActivePowerUp(PoweUpType.METAL, 5.0f);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                ActivePowerUp(PoweUpType.SLOW, 5.0f, 2.0f);
                save_slow_reduced = 2.0f;
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                ActivePowerUp(PoweUpType.SLOW, 5.0f, 2.0f);
                save_slow_reduced = 2.0f;
            }
        }

        switch (state)
        {
            case StatePlayer.IN_NODE:
                {
                    if (Input.GetKeyDown(KeyCode.Space) && startGame && !tutorial_active && evo_provider.show == false)
                    {
                        GetComponent<TrailRenderer>().enabled = true;
                        direction = next_position - initial_position;
                        GetComponent<Rigidbody>().AddForce(direction.normalized * (velocity + evo_controller.ExtraSpeed()) * Time.deltaTime, ForceMode.Impulse);

                        //Set animation to MOVING
                        GetComponent<Animator>().SetFloat("Speed", 1.0f);

                        state = StatePlayer.MOVING;
                    }
                    break;
                }
            case StatePlayer.ARRIVE:
                {
                    // Calcul next_position
                    float distance = Vector3.Distance(transform.position, next_position);
                    time_arrive += Time.deltaTime;
                    bool force_arrive = false;
                    if(time_arrive >= 1.0f)
                    {
                        force_arrive = true;
                    }
                    if (distance_arrive >= distance && force_arrive == false)
                    {
                        distance_arrive = distance;
                    }
                    else
                    {
                        time_arrive = 0.0f;
                        transform.position = next_position;
                        state = StatePlayer.IN_NODE;
                        GetComponent<Rigidbody>().velocity = Vector3.zero;
                        camera_go.GetComponent<CameraController>().MoveCamera(transform);
                        source.PlayOneShot(NodeSound);
                        //Call ReserchNextNode
                        if (isReturn == false)
                        {
                            nodeManager.UpdateSectableNodes();

                            //Restart Color Charges
                            evo_controller.RestartColorCharges();

                            score.AddScore(ScoreType.ARRIVE_NODE);
                        }

                        isReturn = false;
                        initial_node = nodeManager.GetCurrentNode();
                        next_node = nodeManager.GetNextNode();
                        initial_position = initial_node.transform.position;
                        next_position = next_node.transform.position;

                        //Update Arrow direction
                        target_controller.SetTarget(next_node.transform);

                        //Set animation to Idle
                        GetComponent<Animator>().SetFloat("Speed", 0.0f);

                    }
                    break;
                }
            case StatePlayer.MOVING:
                {

                    break;
                }
            case StatePlayer.RETURN:
                {
                    GetComponent<Rigidbody>().AddForce((initial_node.transform.position - transform.position).normalized * velocity * 2 * Time.deltaTime, ForceMode.Force);
                    break;
                }
            case StatePlayer.DYING:
                {
                    //......
                    if (transform.position.y <= -5)
                    {
                        initial_node = nodeManager.GetCurrentNode();
                        next_node = nodeManager.GetNextNode();
                        initial_position = initial_node.transform.position;
                        next_position = next_node.transform.position;
                        GetComponent<Rigidbody>().useGravity = false;
                        GetComponent<Rigidbody>().velocity = Vector3.zero;
                        transform.position = initial_position;
                        //Update Arrow direction
                        target_controller.SetTarget(next_node.transform);

                        //Set animation to Idle
                        GetComponent<Animator>().SetBool("Hit", false);
                        UI_Menu.GetComponent<UI_Game>().Score_window(score.GetScore());
                        state = StatePlayer.IN_NODE;
                        gameObject.SetActive(false);

                    }
                    break;
                }
        }

        // METALLIC BALL POWER UP -------------------
        if (armor_active)
        {
            if (evo_provider.show == false && tutorial_active == false)
            {
                current_time_armor += Time.deltaTime;

                //Update HUD
                powerup_bar.fillAmount = 1 - (current_time_armor / duration_armor);
            }

            if (current_time_armor >= duration_armor)
            {
                current_time_armor = 0.0f;
                armor_active = false;

                script_star.GetComponent<Renderer>().material = normal_mat;
                script_star.GetComponent<ScrollingUVs_Layers>().enabled = false;
                //Disable HUD
                DisablePowerUpHUD(PoweUpType.METAL);           
            }
        }
        // -------------------------------------------

        // STAR POWER UP -----------------------------
        if (star_active)
        {
            if (evo_provider.show == false && tutorial_active == false)
            {
                current_time_star += Time.deltaTime;

                //Update HUD
                powerup_bar.fillAmount = 1 - (current_time_star / duration_star);
            }

            if (current_time_star >= duration_star)
            {
                current_time_star = 0.0f;
                star_active = false;

                script_star.GetComponent<Renderer>().material = normal_mat;
                script_star.GetComponent<ScrollingUVs_Layers>().enabled = false;
                //Disable HUD
                DisablePowerUpHUD(PoweUpType.STAR);
            }
        }
        // --------------------------------------------

        // STAR POWER UP -----------------------------
        if (slow_active)
        {
            if (evo_provider.show == false && tutorial_active == false)
            {
                current_time_slow += Time.deltaTime;

                //Update HUD
                powerup_bar.fillAmount = 1 - (current_time_slow / duration_slow);
            }

            if (current_time_slow >= duration_slow)
            {
                source.PlayOneShot(Accelerate);
                camerasource.pitch = 1.0f;
                slow_active = false;
                current_time_slow = 0.0f;
                script_star.GetComponent<Renderer>().material = normal_mat;
                script_star.GetComponent<ScrollingUVs_Layers>().enabled = false;
                //Disable HUD
                nodeManager.GetComponent<NodeManager>().ResetObstaclesSpeed();
                DisablePowerUpHUD(PoweUpType.SLOW);
            }
        }
        // --------------------------------------------
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && state != StatePlayer.IN_NODE && state != StatePlayer.DYING)
        {
            // Trigger enter Node Arrive
            distance_arrive = Vector3.Distance(transform.position, next_position);
            state = StatePlayer.ARRIVE;
        }
        if (other.gameObject.layer == 12 && state != StatePlayer.IN_NODE && state != StatePlayer.DYING && isReturn == false)
        {
            // Trigger enter Obstacle
            CollisionObstacle(other.gameObject);
        }
        if (other.gameObject.layer == 13 && state != StatePlayer.IN_NODE && state != StatePlayer.DYING)
        {
            // Trigger enter PowerUp
            other.gameObject.GetComponent<PowerUp>().ApplyPowerUp(gameObject);
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.layer == 14 && state != StatePlayer.IN_NODE && state != StatePlayer.DYING && isReturn == false)
        {
            CollisionEnemy(other.gameObject);
        }
    }

    public void EnablePowerUpHud(PoweUpType type)
    {
        if (type == PoweUpType.METAL)
        {
            //Enable Metal Sprite
            metal_hud.SetActive(true);

            slowmo_hud.SetActive(false);
            star_hud.SetActive(false);
        }
        else if (type == PoweUpType.STAR)
        {
            //Enable Star Sprite
            star_hud.SetActive(true);

            metal_hud.SetActive(false);
            slowmo_hud.SetActive(false);
        }
        else if (type == PoweUpType.SLOW)
        {
            //Enable Slow-Mo Sprite
            slowmo_hud.SetActive(true);

            star_hud.SetActive(false);
            metal_hud.SetActive(false);
        }

        powerup_bar.fillAmount = 1.0f;
    }

    public void DisablePowerUpHUD(PoweUpType type)
    {
        if (type == PoweUpType.METAL)
        {
            //Enable Metal Sprite
            metal_hud.SetActive(false);
        }
        else if (type == PoweUpType.STAR)
        {
            //Enable Star Sprite
            star_hud.SetActive(false);
        }
        else if (type == PoweUpType.SLOW)
        {
            //Enable Slow-Mo Sprite
            slowmo_hud.SetActive(false);
        }

        //Reset Time Bar
        powerup_bar.fillAmount = 0.0f;
    }

    public void ActivePowerUp(PoweUpType type, float duration, float speed_slow = 0.0f)
    {
        if (type == PoweUpType.METAL)
        {
            if (star_active)
            {
                current_time_star = 0.0f;
                star_active = false;
            }
            else if (slow_active)
            {
                slow_active = false;
                current_time_slow = 0.0f;
                //Call function Normal Speed
                nodeManager.GetComponent<NodeManager>().ResetObstaclesSpeed();
            }
            script_star.GetComponent<Renderer>().material = metal_mat;
            script_star.GetComponent<ScrollingUVs_Layers>().enabled = true;
            score.AddScore(ScoreType.POWERUP_ARMOR);
            armor_active = true;
            duration_armor = duration;
            current_time_armor = 0.0f;
            source.PlayOneShot(MetalSound);
        }
        else if (type == PoweUpType.STAR)
        {
            if(armor_active)
            {
                current_time_armor = 0.0f;
                armor_active = false;
            }
            else if (slow_active)
            {
                slow_active = false;
                current_time_slow = 0.0f;
                nodeManager.GetComponent<NodeManager>().ResetObstaclesSpeed();
            }
            script_star.GetComponent<Renderer>().material = star_mat;
            script_star.GetComponent<ScrollingUVs_Layers>().enabled = true;
            score.AddScore(ScoreType.POWERUP_STAR);
            star_active = true;
            duration_star = duration;
            current_time_star = 0.0f;
        }
        else if (type == PoweUpType.SLOW)
        {
            if (armor_active)
            {
                current_time_armor = 0.0f;
                armor_active = false;
            }
            else if (star_active)
            {
                current_time_star = 0.0f;
                star_active = false;
            }
            source.PlayOneShot(SlowDown);
            camerasource.pitch = 0.6f;
            script_star.GetComponent<Renderer>().material = normal_mat;
            script_star.GetComponent<ScrollingUVs_Layers>().enabled = false;
            score.AddScore(ScoreType.POWERUP_SLOW);
            slow_active = true;
            duration_slow = duration;
            current_time_slow = 0.0f;
            //Call function
            save_time_trail = GetComponent<TrailRenderer>().time;
            GetComponent<TrailRenderer>().time = new_time_trail;
            nodeManager.GetComponent<NodeManager>().ChangeSpeed(speed_slow);
            save_slow_reduced = speed_slow;
        }

        EnablePowerUpHud(type);
    }

    public void CollisionEnemy(GameObject enemy)
    {
        if (star_active)
        {
            // Destroy Enemy and continue
            enemy.GetComponent<ObstacleScript>().Leave();
            score.AddScore(ScoreType.DESTROY_ENEMY);
        }
        else if (armor_active)
        {
            // Return 
            //state = StatePlayer.RETURN;
            //particle_hit.transform.position = collision.contacts[0].point;
            //particle_hit.GetComponent<ParticleSystem>().Play();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(-direction.normalized * velocity * Time.deltaTime, ForceMode.Impulse);
            next_position = initial_position;
            isReturn = true;
        }
        else
        {
            // Die?
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<TrailRenderer>().enabled = false;
            source.PlayOneShot(defeat);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 5);

            //Set animation to Idle
            GetComponent<Animator>().SetBool("Hit", true);
            GetComponent<Rigidbody>().useGravity = true;
            state = StatePlayer.DYING;

            // TODO All power Up desactivate
        }
    }

    public void ForceShoot()
    {
        //GetComponent<TrailRenderer>().enabled = true;
        //direction = next_position - initial_position;
        //GetComponent<Rigidbody>().AddForce(direction.normalized * (velocity + evo_controller.ExtraSpeed()) * Time.deltaTime, ForceMode.Impulse);
        //
        ////Set animation to MOVING
        //GetComponent<Animator>().SetFloat("Speed", 1.0f);
        startGame = true;
        //state = StatePlayer.MOVING;
    }
    public void TutorialClose()
    {
        tutorial_active = false;

    }
    public void CollisionObstacle(GameObject obstacle)
    {
        if (star_active)
        {
            obstacle.GetComponent<ObstacleScript>().Leave();
            score.AddScore(ScoreType.DESTROY_OBSTACLE);
        }
        else
        {
            if (evo_controller.CheckCollisionObstacle(obstacle.tag))
            {
                obstacle.GetComponent<ObstacleScript>().Leave();
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().AddForce(-direction.normalized * velocity * Time.deltaTime, ForceMode.Impulse);
                next_position = initial_position;
                score.AddScore(ScoreType.DESTROY_OBSTACLE);
                isReturn = true;
            }
            else if (armor_active)
            {
                // Return 
                //state = StatePlayer.RETURN;
                //particle_hit.transform.position = collision.contacts[0].point;
                //particle_hit.GetComponent<ParticleSystem>().Play();
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().AddForce(-direction.normalized * velocity * Time.deltaTime, ForceMode.Impulse);
                next_position = initial_position;

                isReturn = true;
            }
            else
            {
                // Die?
                source.PlayOneShot(defeat);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<TrailRenderer>().enabled = false;
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 5);

                //Set animation to Idle
                GetComponent<Animator>().SetBool("Hit", true);
                GetComponent<Rigidbody>().useGravity = true;
                state = StatePlayer.DYING;
            }
        }
    }

    public StatePlayer GetState()
    {
        return state;
    }
}
