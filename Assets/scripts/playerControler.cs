using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//checks that the object in   unity  that the script is connected to has a ridged body 
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class script : MonoBehaviour
{


    //variables

    //ground movement
    [SerializeField, Range(1, 20)] private float speed;

    //create ridged body
    Rigidbody2D rb;

    //variable to  check if player is on ground
    private Transform groundCheck;

    //jump variable 
    [SerializeField, Range(1, 20)] private float jumpForce = 10;

    //the   0.01f converts 0.01 to float
    //the radius of the ground  check 
    [SerializeField, Range(0.01f, 1)] private float groundCheckRadius = 0.02f;

    //use layer mask to check the ground  layer for the  floor objects 
    [SerializeField] private LayerMask isGroundLayer;

    SpriteRenderer sr;

    Animator anim;


    private bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {



        //create ridged body and renderer and connect it to mario obj ridged body set in unity 
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        //  checks that speed is not set to 0
        if (speed <= 0)
        {
            speed = 5;
            Debug.Log("Speed was set incorrectly");
        }

        //checks that jumpForce is set correctly
        if (jumpForce <= 0)
        {
            jumpForce = 5;
            Debug.Log("Speed was set incorrectly");
        }

        //checks that we  have a groundCheck obj and creates it
        if (!groundCheck)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            groundCheck = obj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        //checks if the player is hitting the ground
        if (!isGrounded)
        {
            if (rb.velocity.y <= 0)
            {
                isGrounded = IsGrounded();
            }

        }
        else
        {
            isGrounded = IsGrounded();
        }

        //horizontal input handler
        float hInput = Input.GetAxis("Horizontal");


        rb.velocity = new Vector2(hInput * speed, rb.velocity.y);

        //jump input handler
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //if(hInput > 0 &&  sr.flipX ||hInput > 0 && !sr.flipX) sr.flipX = !sr.flipX;}
        //flips the sprite when   hInput changes from  positive to negative and vise versa 
        if (hInput != 0)
        {
            sr.flipX = (hInput < 0);
        }



        //tells the animation editor to flip the animations 
        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);

        if (isGrounded && Input.GetButtonDown("Fire1"))
        {

            anim.SetTrigger("isAttacking");

        }

        if (!isGrounded && Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("isAirAttacking");

        }


        //else{isAttacking = false; anim.SetBool("isAttacking", isAttacking);}



    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("collect"))
        {
            Destroy(other.gameObject);
        }
    }





    //function to check of  the player is on the ground
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
    }
}
