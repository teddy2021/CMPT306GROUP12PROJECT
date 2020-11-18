using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

public class Slime : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private Transform player;
    private AIPath aIPath;
    private AIDestinationSetter ai;

    public UnityEvent OnCollidePlayer;
    public int health = 100;
    public float chaseDist = 100f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();

        if (OnCollidePlayer == null)
            OnCollidePlayer = new UnityEvent();

        ai = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player is close enough to the slime for it to path find
        if (Vector2.Distance(transform.position, player.position) > chaseDist || health <= 0)
        {
            ai.target = null;
        }
        else
        {
            ai.target = player;
        }

        // check if we need to flip the enemie's sprite depending on which way it wants to move
        if (aIPath.desiredVelocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void OnDeathAnimComplete()
    {
        // when the death anim completes, delete this object.
        Destroy(gameObject); // remove us
    }

    public void Death()
    {
        // freeze and play the death anim

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        cc.enabled = false;

        health = 0;

        animator.Play("Slime_Death");
    }

    public void doDamage(int amount)
    {
        // inflict damage to the slime, check if it should be dead
        health -= amount;

        if (health <= 0)
            Death();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if collison on a player

        if (collision.gameObject.tag == "Player")
        {
            OnCollidePlayer.Invoke();
        }
    }
}
