using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // nuova voce script per regolazione velocità Player in Unity

    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private float verticalInput;
    //private bool grounded;

    [Header ("SFX")]
    [SerializeField] private AudioClip jumpSound;
    //[SerializeField] private AudioClip runSound;
    

    public GameManager theGM;

    private void Awake()

    //Riferimenti alle animazioni e Rigidbody2D 
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }


    private void Update()
    {
        ///// SCRIPT MOVIMENTO PLAYER /////

        horizontalInput = Input.GetAxis("Horizontal"); //COMANDI TEST MOVIMENTO CON TASTIERA PC
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        verticalInput = Input.GetAxis("Vertical");
        //horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal"); //UTILIZZARE QUESTO PER GIOCARE AL TELEFONO

        // inserimento "flip" del Player quando cambia direzione

        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(0.2f, 0.2f, 1);
            //SoundManager.instance.PlaySound(runSound);
        }
        else
            if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-0.2f, 0.2f, 1);
            //SoundManager.instance.PlaySound(runSound);
        }
        //if (Input.GetKey(KeyCode.Space) && isGrounded())
        //Jump();           


        //Impostare i parametri per animator
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

       
        // Salto sul muro e ancoraggio ad esso

        if (wallJumpCooldown > 0.2f)
       {
            
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())      //staccaggio dal muro
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 6;

            if (Input.GetKey(KeyCode.Space))
                Jump();

            //if(CrossPlatformInputManager.GetButtonDown("Jump"))
            //{
            //  Jump();
            //
            //if ((CrossPlatformInputManager.GetButtonDown("Jump")) && isGrounded())
            //  SoundManager.instance.PlaySound(jumpSound);
            //}
            // if (CrossPlatformInputManager.GetButtonDown("Jump")) UTILIZZARE PER SALTARE BOTTONE TELEFONO


        }
        else
            wallJumpCooldown += Time.deltaTime;
    }
    // JUMP 
    private void Jump()
    {
        
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("Jump");
            SoundManager.instance.PlaySound(jumpSound);
        }
        else if (onWall() && !isGrounded())
        {
            if(horizontalInput == 0)
            {
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
            transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x)/5, transform.localScale.y, transform.localScale.z);  //Flip del giocatore durante il salto su lato opposto
            }
             else
                 body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 25, 15);
            
            wallJumpCooldown = 0;
            
        }
    }


        // Salto normale in contatto con oggetti
     private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
     }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null; 
    }
    
    public bool canAttack3()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
        
    }

    public bool canAttack1()
    {
        return horizontalInput != 0 && isGrounded() && !onWall();
      
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //   if(other.gameObject.tag == "Spikes")
    //{
    //  Debug.Log("ouch!");
    //} 
    //}
}
