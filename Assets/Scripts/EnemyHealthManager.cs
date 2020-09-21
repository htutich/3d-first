using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{


    private int health = 100;
    private int curHealth;

    public HealthBarController healthBar;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = health;
        healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        if(curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void HurtEnemy(int damage)
    {
        curHealth -= damage;
        healthBar.SetHealth(curHealth);
    }
}
