using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{


    private Rigidbody myRB;
    private float moveSpeed = 4f;

    [SerializeField] private PlayerController thePlayer;
    private HealthBarCamera HealthBarCamera;
    NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        HealthBarCamera = this.GetComponentInChildren<HealthBarCamera>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        myRB = GetComponent<Rigidbody>();
        thePlayer = FindObjectOfType<PlayerController>();

        navMeshAgent.speed = moveSpeed;

        HealthBarCamera.cam = thePlayer.Camera;

    }
    private void Update()
    {
        if (navMeshAgent == null)
        {
            Debug.LogError("Error NavMeshAgent");
        }
        else
        {
            if (thePlayer != null)
            {
                transform.LookAt(thePlayer.transform.position);

                /*Vector2 dir = transform.position - thePlayer.transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));*/

                navMeshAgent.SetDestination(thePlayer.transform.position);
            }
        }
    }

}
