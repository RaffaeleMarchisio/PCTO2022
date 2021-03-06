
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    // dichiarazione di variabili
    
    [SerializeField]
    float jumpVelocity = 20f;
    
    public float moveSpeed = 7f;
    [SerializeField] 
    private LayerMask platformsLayerMask;
    
    private bool isplaying = false;
    
    public static Player inst;
    
    private Rigidbody2D rigidbody2d; //variabile che rappresenta il personaggio principale
    
    private CircleCollider2D boxCollider2d;  // variabile che rappresenta il collider del personaggio principale
    private SpriteRenderer sr;             // variabile per la gestione dello sprite del personaggio principale 
    private Animator anim;         // variabile per la gestione delle animazioni
    //oggetti vari
    public GameObject up_arrow;
    public GameObject door_open;
    public GameObject Pause_screen;
    public GameObject Final_screen;
    private bool on_the_house1;
    public Text final_score;
    private Vector2 cord_player;
    //variabili per la gestione dei suoni
    public AudioSource base_audio;
    public AudioSource sound_coin;
    public AudioSource door_sound;
    public AudioSource portal_sound;

    private void Start()
    {
        Pause_screen.SetActive(false);
        rigidbody2d = transform.GetComponent<Rigidbody2D>(); //vado a cercare il componente Rigidbody2D all'interno del personaggio e la metto all'interno della variabile rigidbody2d
        boxCollider2d = transform.GetComponent<CircleCollider2D>(); //vado a cercare il componente CircleCollider2D all'interno del personaggio e la metto all'interno della variabile boxCollider2d
        sr = transform.GetComponent<SpriteRenderer>(); //vado a cercare il componente SpriteRenderer all'interno del personaggio e la metto all'interno della variabile sr
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //if per far partire la musica ad inizio gioco
        if(!isplaying)
        {
            base_audio.Play();
            isplaying = true;
        }
        
        if (IsGrounded() && FindObjectOfType<UDP_reciver>().getyPolsoDx() > -350) //se ?? atterrato e la coordinata y del polso destro e sopra ad un certo valore
        {
            cord_player = rigidbody2d.transform.position;      //mi salvo le coordinate del personaggio quando salta
            rigidbody2d.velocity = Vector2.up * jumpVelocity;  // faccio saltare il personaggio
        }
        if(on_the_house1)  // se ?? sulla porta della casa 
        {
            if (FindObjectOfType<UDP_reciver>().getyPolsoDx() < -600)  // se la coordinata x del polso destro ?? minori di un certo valore
            {
                door_sound.Play();      //parte il suono della porta
                rigidbody2d.gameObject.transform.position = new Vector2(-42, -29); // teletrasporto il personaggio dentro la casa
            }
            up_arrow.SetActive(true);
        }
        else
        {
            up_arrow.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))// se il tasto premuto ?? uguale a esc
        {
            Pause_screen.SetActive(true);  //mostra la schermata di pausa
            base_audio.Stop();  // ferma il suono di base
        }
        HandleMovement_FullMidAirControl(); 
    }
    public void resume()
    {
        base_audio.Play();
        Pause_screen.SetActive(false);
    }
    private bool IsGrounded()//funzione per capire quando il personaggio ?? atterrato
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 1f, platformsLayerMask);
        return raycastHit2d.collider != null;
    }

    private void HandleMovement_FullMidAirControl()
    {
        if (FindObjectOfType<UDP_reciver>().getxPolsoDx()>710)//se la coordinata x del polso destro ?? maggiore di un certo valore
        {
            anim.SetFloat("speed", 1);  // avvia l'animazione della camminata
            rigidbody2d.velocity = new Vector2(-moveSpeed, rigidbody2d.velocity.y); //muovo il personaggio di movespeed(verso sinistra)
            sr.flipX = true;  //gira lo sprite del personaggio
        }
        else
        {
            if (FindObjectOfType<UDP_reciver>().getxPolsoDx() < 500) //se la coordinata x del polso destro ?? minore di un certo valore
            {
                
                anim.SetFloat("speed", 1); // avvia l'animazione della camminata
                rigidbody2d.velocity = new Vector2(+moveSpeed, rigidbody2d.velocity.y); //muovo il personaggio di movespeed(verso destra)
                sr.flipX = false;
            }
            else
            {
                rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y); 
            }
            
        }
        if (FindObjectOfType<UDP_reciver>().getxPolsoDx() > 500 && FindObjectOfType<UDP_reciver>().getxPolsoDx()< 710) // mentre il personaggio sta fermo
        {
            anim.SetFloat("speed", 0); //avvia l'animazione da fermo
        }
    }

   
    private void OnTriggerEnter2D(Collider2D collision)  //funzione di unity per la gestione delle collisioni 
    {
        if (collision.tag == "Coin")      //quando il personaggio entra nella collisione della moneta
        {
            Destroy(collision.gameObject);  //distruggi la moneta
            Score_manager.instance.Changescore(100);  //aggiungi 100 al punteggio
            up_arrow.SetActive(false); 
            sound_coin.Play(); //riproduco il suono delle monete

        }
        else if (collision.tag == "Portal")  //quando il personaggio entra nella collisione del portale
        {
            FindObjectOfType<UDP_reciver>().isGameOver = true;      //chiusura del socket per la gestione di openpose
            up_arrow.SetActive(false);
            Final_screen.SetActive(true);    //mostra la schermata finale
            final_score.text = "Your Score:" + Score_manager.instance.punteggio;  //aggiorna il testo del punteggio finale
            Destroy(rigidbody2d.gameObject); //distrugge l'oggetto del personaggio
            portal_sound.Play(); //suono del portale
            base_audio.Stop(); // suono di base si ferma
        }
        else if (collision.tag == "die")   //quando il personaggio entra nella collisione della collisione dell'oggetto die_line(un oggetto invisibile con un collider)
        {
            if (Score_manager.instance.punteggio != 0 && Score_manager.instance.punteggio>10) {
                Score_manager.instance.Changescore(-10);
            }
            rigidbody2d.gameObject.transform.position = cord_player;  //quando il personaggio cade viene teletrasportato alle coordinate salvate durante il salto prima della caduta
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
