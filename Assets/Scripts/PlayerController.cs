using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GunController theGun;
    public Transform Camera;

    private float moveSpeed = 5f;
    private Camera mainCamera;
    private Rigidbody myRigidbody;

    private bool playerOnGround = true;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            theGun.isFiring = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            theGun.isFiring = false;
        }
    }

    private void FixedUpdate()
    {

        /*transform.
            Translate( Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 
                        0f, 
                        Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime );
        */

        myRigidbody.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidbody.velocity.y, Input.GetAxisRaw("Vertical") * moveSpeed);

        if (Input.GetKey(KeyCode.Space) && playerOnGround)
        {
            myRigidbody.AddForce(new Vector3(0, 5 * moveSpeed, 0), ForceMode.Impulse);
            playerOnGround = false;
        }

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "World")
        {
            playerOnGround = true;
        }
    }


}
