using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public int health;
    private int currentHealth;

    public HealthBar healthbar;

    void Start()
    {
        currentHealth = health;
        healthbar.SetMaxHealth(health);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        healthbar.SetHealth(currentHealth);

        
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TakeDamage(4);
        }
    }


}
