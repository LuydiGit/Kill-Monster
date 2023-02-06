using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : EnemyController
{
    // Start is called before the first frame update
    void Start()
    {
        health = 2;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update ();
    }

    void FixedUpdate () {
        if (isMoving) {
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        }
    }
}
