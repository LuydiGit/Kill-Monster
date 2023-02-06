using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float maxSpeed;
    public float jumpForce;

    public int health;
    public bool invunerable = false;
    public bool isAlive = true;
    
    public bool grounded;
    private bool jumping;

    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer sprite;
    private AudioSource soundFx;

    public Transform groundCheck;

    //Variaveis do tiro
    public Transform bulletSpawn;
    public float fireRate;
    private float nextFire;
    public GameObject bulletObject;

    void Awake (){
        rb2d = GetComponent<Rigidbody2D> ();
        sprite = GetComponent<SpriteRenderer> ();
        anim = GetComponent<Animator> ();
        soundFx = GetComponent<AudioSource> ();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

       if (Input.GetButtonDown ("Jump") && grounded)
       {
            jumping = true;
            soundFx.Play();
       } 

    }

    void FixedUpdate(){

        if(isAlive){

                float move = Input.GetAxis ("Horizontal");

                anim.SetFloat("Speed", Mathf.Abs(move));

                rb2d.velocity = new Vector2 (move * maxSpeed, rb2d.velocity.y);
                
                if((move > 0f && sprite.flipX) || (move < 0f && !sprite.flipX)){
                    Flip ();
                }


            if(jumping){

                rb2d.AddForce (new Vector2 (0f, jumpForce));
                anim.SetBool("JumpFall", rb2d.velocity.y == 0f );
                jumping = false;
            } 

           






            if(Input.GetButton ("Fire1")  && Time.time > nextFire){
            Fire();
        }
        }
    }

    void Flip (){
        sprite.flipX = !sprite.flipX;

         if(!sprite.flipX) 
         bulletSpawn.position = new Vector3 (this.transform.position.x + 0.65f, bulletSpawn.position.y, bulletSpawn.position.z);
         else
         bulletSpawn.position = new Vector3 (this.transform.position.x - 0.65f, bulletSpawn.position.y, bulletSpawn.position.z);
    }   

    void Fire (){
        anim.SetTrigger ("Fire");

        nextFire = Time.time + fireRate;

        GameObject cloneBullet = Instantiate (bulletObject, bulletSpawn.position, bulletSpawn.rotation);

        if (sprite.flipX)
            cloneBullet.transform.eulerAngles = new Vector3 (0, 0, 180);

    }

    IEnumerator Damage(){

        for(float i = 0f; i < 1f; i += 0.1f) {
            sprite.enabled = false;
            yield return new WaitForSeconds (0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds (0.1f);
         }

         invunerable = false;

    }

    public void DamagePlayer (){

        if(isAlive){
                invunerable = true;
                health--;
                StartCoroutine (Damage ());

            if (health < 1){
                isAlive = false;
                anim.SetTrigger ("Death");
                Invoke ("ReloadLevel", 3f);
            }

        }
    }

    void ReloadLevel(){
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

}