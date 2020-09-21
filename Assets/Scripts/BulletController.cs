using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 10f;
    private float lifeTime = 1f;
    private int damageToGive;

    private void Start()
    {
        damageToGive = Random.Range(20, 30);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Destroy(gameObject, lifeTime);
    }

    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHealthManager>().HurtEnemy(damageToGive);
            Destroy(gameObject);
        }

        Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag == "World")
        {
            Debug.Log("!!");
            Destroy(gameObject);
        }
    }
}
