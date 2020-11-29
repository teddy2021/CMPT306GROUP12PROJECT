﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDamage : MonoBehaviour
{
    public int health = 100;
    public int maxHealth;
    public float invisibilityTime = 1.0f;

    private float lastDamagedTime = 0f;

    [Tooltip("When the player is damged.")]
    public UnityEvent OnDamaged;

    [Tooltip("When the player dies.")]
    public UnityEvent OnDeath;

    [Tooltip("When the player dies.")]
    public UnityEvent OnDeathAnimComplete;

    public HealthBar hb;

    public AudioSource playerSound;
    public AudioClip[] playerHurtSounds;
    public AudioClip[] playerDeathSounds;

    private void Start()
    {
        if (OnDamaged == null)
        {
            OnDamaged = new UnityEvent();
        }

        if (OnDeath == null)
        {
            OnDeath = new UnityEvent();
        }

        if (OnDeathAnimComplete == null)
        {
            OnDeathAnimComplete = new UnityEvent();
        }

        hb.SetMaxHealth(health);

        maxHealth = health;
    }

    void OnDeathAnimationComplete()
    {
        gameObject.SetActive(false);
        OnDeathAnimComplete.Invoke();
    }

    void doDeath()
    {
        health = 0;
        hb.SetHealth(health);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        OnDeath.Invoke();

        playerSound.clip = playerDeathSounds[Random.Range(0, playerDeathSounds.Length)];
        playerSound.Play(0);

        GetComponent<Animator>().Play("Player_Death");
    }

    public void doDamage(int amount, Vector2 knockback)
    {
        health -= amount;
        hb.SetHealth(health);

        GetComponent<Rigidbody2D>().AddForce(knockback);
        GetComponent<Animator>().Play("Player_Damage");

        OnDamaged.Invoke();

        if (health <= 0)
            doDeath();
        else
        {
            playerSound.clip = playerHurtSounds[Random.Range(0, playerHurtSounds.Length)];
            playerSound.Play(0);
        }
    }

    public void tryDoDamage(int amount, Vector2 knockback)
    {
        if (Time.time - lastDamagedTime < invisibilityTime)
            return;

        lastDamagedTime = Time.time;

        doDamage(amount, knockback);
    }

    public void IncreaseHealth(int amount = 0)
    {
        health += amount;
        maxHealth += amount;

        GameController.StartGame("next floor", GameController.FloorLevel++);
    }
}
