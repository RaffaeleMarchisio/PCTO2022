/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
/*
 * Simple Jump
 * */
public class Player : MonoBehaviour
{
    [SerializeField]
    float jumpVelocity = 20f;
    public float moveSpeed = 7f;
    [SerializeField] 
    private LayerMask platformsLayerMask;
    private bool isplaying = false;
    public static Player inst;
    private Rigidbody2D rigidbody2d;
    private CircleCollider2D boxCollider2d;
    private SpriteRenderer sr;
    private Animator anim;
    public GameObject up_arrow;
    public GameObject door_open;
    public GameObject Pause_screen;
    public GameObject Final_screen;
    private bool on_the_house1;
    public Text final_score;
    private Vector2 cord_player;
    public AudioSource base_audio;
    public AudioSource sound_coin;
    public AudioSource door_sound;
    public AudioSource portal_sound;

    private void Start()
    {
        Pause_screen.SetActive(false);
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<CircleCollider2D>();
        sr = transform.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!isplaying)
        {
            base_audio.Play();
            isplaying = true;
        }
        if (IsGrounded() && FindObjectOfType<UDP_reciver>().getyPolsoDx() > -350)
        {
            cord_player = rigidbody2d.transform.position;
            rigidbody2d.velocity = Vector2.up * jumpVelocity;
        }
        if(on_the_house1)
        {
            if (FindObjectOfType<UDP_reciver>().getyPolsoDx() < -600)
            {
                door_sound.Play();
                rigidbody2d.gameObject.transform.position = new Vector2(-42, -29);
            }
            up_arrow.SetActive(true);
        }
        else
        {
            up_arrow.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause_screen.SetActive(true);
            base_audio.Stop();
        }
        HandleMovement_FullMidAirControl();
    }
    public void resume()
    {
        base_audio.Play();
        Pause_screen.SetActive(false);
    }
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 1f, platformsLayerMask);
        return raycastHit2d.collider != null;
    }

    private void HandleMovement_FullMidAirControl()
    {
        Debug.Log("y:" + FindObjectOfType<UDP_reciver>().getyPolsoDx() + " x:" + FindObjectOfType<UDP_reciver>().getxPolsoDx());
        if (FindObjectOfType<UDP_reciver>().getxPolsoDx()>710)
        {
            anim.SetFloat("speed", 1);
            rigidbody2d.velocity = new Vector2(-moveSpeed, rigidbody2d.velocity.y);
            sr.flipX = true;
        }
        else
        {
            if (FindObjectOfType<UDP_reciver>().getxPolsoDx() < 500)
            {
                
                anim.SetFloat("speed", 1);
                rigidbody2d.velocity = new Vector2(+moveSpeed, rigidbody2d.velocity.y);
                sr.flipX = false;
            }
            else
            {
                rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
            }
            
        }
        if (FindObjectOfType<UDP_reciver>().getxPolsoDx() > 500 && FindObjectOfType<UDP_reciver>().getxPolsoDx()< 710)
        {
            anim.SetFloat("speed", 0);
        }
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            Destroy(collision.gameObject);
            Score_manager.instance.Changescore(100);
            up_arrow.SetActive(false);
            sound_coin.Play();

        }
        else if (collision.tag == "Portal")
        {
            FindObjectOfType<UDP_reciver>().isGameOver = true;
            up_arrow.SetActive(false);
            Final_screen.SetActive(true);
            final_score.text = "Your Score:" + Score_manager.instance.punteggio;
            Destroy(rigidbody2d.gameObject);
            portal_sound.Play();
            base_audio.Stop();
        }
        else if (collision.tag == "die")
        {
            if (Score_manager.instance.punteggio != 0 && Score_manager.instance.punteggio>10) {
                Score_manager.instance.Changescore(-10);
            }
            rigidbody2d.gameObject.transform.position = cord_player;
            up_arrow.SetActive(false);
        }
        else if(collision.tag == "house1")
        {
            on_the_house1 = true;
            
        }
        else if(collision.tag == "ex_house1")
        {
            door_sound.Play();
            rigidbody2d.gameObject.transform.position = new Vector2(19, -5);
        }
        else if (collision.tag == "door")
        {
            door_open.SetActive(true);
            door_sound.Play();
        }
        else if (collision.tag == "ex_house2"){
            door_sound.Play();
            rigidbody2d.gameObject.transform.position = new Vector2(60, -4.5f);
        }
        else if (collision.tag == "ex_house3")
        {
            door_sound.Play();
            rigidbody2d.gameObject.transform.position = new Vector2(113.5f, -5f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "house1")
        {
            on_the_house1 = false;
        }
        else if (collision.tag == "door")
        {
            door_open.SetActive(false);
        }
    }
}
