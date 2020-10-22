using UnityEngine;

public class Hp : MonoBehaviour
{
    public int level;
    private int currentHealth;

    public HealthBar healthbar;

    void Start()
    {
        currentHealth = level * 10;
        healthbar.SetLevel(level);
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
            TakeDamage(12);
        }
    }


}
