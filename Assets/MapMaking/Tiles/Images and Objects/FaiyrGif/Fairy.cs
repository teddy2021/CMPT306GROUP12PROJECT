using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Fairy : MonoBehaviour
{

    private GameObject player;
    private AudioSource pickupSound;
    public AudioClip[] pickupSounds;
    public GameObject parent;

    private AIPath aIPath;
    private AIDestinationSetter ai;
    private GameObject roamDest;


    [Tooltip("How far will roam from its current position")]
    public float roamDist = 5f;

    [Tooltip("How often will roam to a new location?")]
    public float roamChangeRate = 5f;

    [Tooltip("How fast is when roaming")]
    public float roamSpeed = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        pickupSound = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();

        ai = GetComponentInParent<AIDestinationSetter>();
        aIPath = GetComponentInParent<AIPath>();

        roamDest = new GameObject();
        InvokeRepeating("UpdateRoamDest", roamChangeRate, roamChangeRate);

        aIPath.maxSpeed = roamSpeed;
        ai.target = roamDest.transform;
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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject;

            player.GetComponent<PlayerDamage>().doHeal(35);
            player.GetComponentInChildren<HeadLamp>().doPower(20f);

            pickupSound.clip = pickupSounds[Random.Range(0, pickupSounds.Length)];
            pickupSound.Play(0);

            Destroy(parent);

        }
    }
}
