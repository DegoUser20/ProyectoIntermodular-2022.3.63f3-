using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vidaMaxima : MonoBehaviour
{
    public int maxVida = 100;
    int currentHealth;
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxVida;
    }

    public void TakeDamage (int amount)
    {
        currentHealth -=amount;
        if (currentHealth <= 0)
        {
            Debug.Log("soldado muerto");
            Destroy(gameObject);
        }
    }
}
