using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Player_Motor : MonoBehaviour {

    public AudioClip JumpSound;
    AudioSource source;
    [SerializeField]
    private Camera cam;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;

    public bool ifMove=true;

    [SerializeField]
    private float cameraRotationLimit = 75f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponents<AudioSource>()[1];

    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void RotateCamera(float _cameraRotation)
    {
        cameraRotationX = _cameraRotation;
    }
    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }
    private void FixedUpdate()
    {
        if(ifMove)
        {
            PerformMovement();
        }
        PerformRotation();
    }
    void PerformMovement()
    {
        if(velocity!=Vector3.zero)
        {
           // Debug.Log("velocity"+velocity);
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if(thrusterForce!=Vector3.zero)
        {
            if(!source.isPlaying)
            {

                source.PlayOneShot(JumpSound);
            }
           // Debug.Log("thrust"+ thrusterForce);
            rb.AddForce(thrusterForce*Time.fixedDeltaTime,ForceMode.Acceleration);
        }
    }
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if(cam!=null)
        {
            
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }
}

