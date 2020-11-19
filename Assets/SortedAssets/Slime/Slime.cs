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
    private Collider2D cc;
    private Transform player;
    private AIPath aIPath;
    private AIDestinationSetter ai;
    private GameObject roamDest;
    public HealthBarSlime hb;

    [Tooltip("When the slime collides with the player.")]
    public UnityEvent OnCollidePlayer;

    [Tooltip("When the slime is damged.")]
    public UnityEvent OnDamaged;

    [Tooltip("When the slime dies.")]
    public UnityEvent OnDeath;

    [Tooltip("How much health does the slime have?")]
    public int health = 100;

    [Tooltip("How close does the player need to be before it starts chasing")]
    public float chaseDist = 20f;

    [Tooltip("How far will the slime roam from its current position")]
    public float roamDist = 5f;

    [Tooltip("How often will the slime roam to a new location?")]
    public float roamChangeRate = 10f;

    [Tooltip("How fast the slime is when chasing a player")]
    public float chaseSpeed = 5f;

    [Tooltip("How fast the slime is when roaming")]
    public float roamSpeed = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<Collider2D>();

        if (OnCollidePlayer == null)
            OnCollidePlayer = new UnityEvent();

        if (OnDamaged == null)
            OnDamaged = new UnityEvent();

        if (OnDeath == null)
            OnDeath = new UnityEvent();

        ai = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        roamDest = new GameObject();

        InvokeRepeating("UpdateRoamDest", 0f, roamChangeRate);

        hb.SetMaxHealth(health);
    }

    void UpdateRoamDest()
    {
        roamDest.transform.Translate(
            new Vector2(
                transform.position.x + Random.Range(-roamDist, roamDist),
                transform.position.y + Random.Range(-roamDist, roamDist)
            )
        );
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player is close enough to the slime for it to path find
        if (Vector2.Distance(transform.position, player.position) > chaseDist || health <= 0)
        {
            aIPath.maxSpeed = roamSpeed;
            ai.target = roamDest.transform;
        }
        else
        {
            aIPath.maxSpeed = chaseSpeed;
            ai.target = player;
            roamDest.transform.Translate(player.position);
        }

        // check if we need to flip the enemie's sprite depending on which way it wants to move
        if (aIPath.desiredVelocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            hb.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            hb.transform.localScale = new Vector3(-0.01f, 0.01f, 1);
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
        hb.SetHealth(health);

        OnDeath.Invoke();

        animator.Play("Slime_Death");
    }

    public void doDamage(int amount, Vector2 knockback)
    {
        // inflict damage to the slime, check if it should be dead
        health -= amount;
        hb.SetHealth(health);

        animator.Play("Slime_Damage");

        rb.AddForce(knockback);

        OnDamaged.Invoke();

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
