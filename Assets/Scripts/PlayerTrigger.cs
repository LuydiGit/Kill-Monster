using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTrigger : MonoBehaviour
{
    private Player player;

    void Awake () {
        player = GameObject.Find ("Player").GetComponent<Player> ();
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.CompareTag ("Enemy")) {

            if(!player.invunerable) {
                player.DamagePlayer ();
            }
        }

        if(other.CompareTag("Portal")){
            Invoke ("NextLevel", 1f);
        }
    }

    void NextLevel(){
       SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1); 
    }

}
