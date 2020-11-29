﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseInteractor : MonoBehaviour
{
    new Camera camera;
    private float lastAttackTime = 0f;
    private GameObject player;

    [Range(0.01f, 2.00f)]
    [SerializeField] private float DeleteTime;
    [SerializeField] private Texture2D[] cursor_circle;

    public Tilemap tilemap;

    private Coroutine deletion;
    private bool destroying;
    private Vector3Int location;

    public float timeAttackRate = 1f;
    public float interactRange = 2.5f;
    public int damageAmount = 40;
    public float knockBackAmount = 100f;

    public LayerMask enemiesLayer;

    public AudioClip[] mineSounds;
    public AudioSource mineSound;
    private float mineSoundTime = -1;

    public AudioClip[] swingSounds;
    public AudioSource swingSound;
    private float swingSoundTime = -1;


    private Animator animator; // for changing players swing animations
    IEnumerator Delete()
    {
        if (!GameController.GameIsPaused)
        {

            for (int i = 0; i < 17; i += 1)
            {
                Cursor.SetCursor(cursor_circle[i], new Vector2(16, 16), CursorMode.Auto); // Animate cursor (has 7 frames)
                yield return new WaitForSeconds(DeleteTime / 17.0f); // Pause for 1/17th of desletion time

            }

            try
            {
                Wall_Tile tile = (Wall_Tile)tilemap.GetTile(location);
                tile.DropItems(location);
                tilemap.SetTile(location, null);
                tilemap.RefreshTile(location + new Vector3Int(-1, 0, 0));
                tilemap.RefreshTile(location + new Vector3Int(0, -1, 0));
                tilemap.RefreshTile(location + new Vector3Int(1, 0, 0));
                tilemap.RefreshTile(location + new Vector3Int(0, 1, 0));
            }
            catch { }
        }

        Cursor.SetCursor(cursor_circle[17], new Vector2(8, 8), CursorMode.Auto);
        destroying = false;
        StopCoroutine(deletion);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camera = Camera.main;
        Cursor.SetCursor(cursor_circle[17], new Vector2(16, 16), CursorMode.Auto);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("action", true);   // set player animation to action
            // get the interact point
            Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPos = player.transform.position;

            Vector2 worldPnt = mousePos;
            if (Vector2.Distance(worldPnt, playerPos) > interactRange) // check if the we clicked outside our range
                worldPnt = playerPos + (mousePos - playerPos).normalized * interactRange; // then limit the point we can interact at

            Debug.DrawLine(playerPos, worldPnt);

            if (Time.time - swingSoundTime > 2)
            {
                swingSound.clip = swingSounds[Random.Range(0, swingSounds.Length)];
                swingSound.Play(0);

                swingSoundTime = Time.time;
            }

            bool didAttack = false;

            // check if can attack
            if (!destroying && Time.time - lastAttackTime > timeAttackRate)
            {
                // check if hit anything

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(worldPnt, 0.5f, enemiesLayer);

                foreach(Collider2D enemy in hitEnemies)
                {
                    var slime = enemy.gameObject.GetComponent<Slime>();

                    Vector2 slimePos = slime.transform.position;

                    slime.doDamage(damageAmount, (slimePos - playerPos).normalized * knockBackAmount);
                    didAttack = true;
                }

                if (didAttack)
                {
                    lastAttackTime = Time.time;
                    return;
                }
            }

            location = tilemap.WorldToCell(worldPnt);
            if (tilemap.HasTile(location))
            {
                if (!destroying)
                {
                    deletion = StartCoroutine(Delete());
                    destroying = true;
                }
            }
            else
                destroying = false;

            if (destroying && Time.time - mineSoundTime > 0.25)
            {
                mineSound.clip = mineSounds[Random.Range(0, mineSounds.Length)];
                mineSound.Play(0);

                mineSoundTime = Time.time;
            }
        }
        else
        {
            animator.SetBool("action", false);  // stop player animation
            if (destroying)
            { // Cancel destruction of a tile
                StopCoroutine(deletion);
                Cursor.SetCursor(cursor_circle[17], new Vector2(8, 8), CursorMode.Auto);
                destroying = false;
            }
        }

        if (destroying && Vector3Int.Distance(tilemap.WorldToCell( // Destroying a tile, but dragged to new tile
            camera.ScreenToWorldPoint(
                Input.mousePosition)), location) >= 1)
        {
            animator.SetBool("action", false); // stop player animation
            StopCoroutine(deletion);
            destroying = false;
        }
    }

    public void IncreaseDamage (int amount = 0)
    {
        damageAmount += amount;
        GameController.StartGame("next floor", GameController.FloorLevel++);
    }
}
