using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_Motor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class Player_Controller : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [SerializeField]
    private float thrusterFuelBurnSpeed = 5f;
    //[SerializeField]
   // private float thrusterFuelRegenSpeed = 0.1f;
    [SerializeField]
    private float thrusterFuelTotal = 1f;
    private float thrusterFuelAmount;

    [Header("Spring settings:")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private Player_Motor motor;
    private ConfigurableJoint joint;
    int ifFirst=0;
    public int ifJump=0;
    void Start()
    {
        motor = GetComponent<Player_Motor>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
        thrusterFuelAmount = thrusterFuelTotal;

    }
    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }
    private void FixedUpdate()
    {
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _yMov = Input.GetAxisRaw("Vertical");
        float _yRot = Input.GetAxisRaw("Mouse X");
        float _xRot = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRot * lookSensitivity;
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _yMov;
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

        motor.Move(_velocity);
        motor.Rotate(_rotation);
        
        motor.RotateCamera(_cameraRotationX);

        //jump
        Vector3 _thrusterForce = Vector3.zero;

        // The avatar has the strengthh to jump
        if(Input.GetButton("Jump")&&thrusterFuelAmount>0f&& ifJump==1)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.fixedDeltaTime;
            _thrusterForce = transform.up * thrusterForce;
            SetJointSettings(0f);
            ifFirst = 1;
        } 
        else if(Input.GetButton("Jump"))    // The avatar has no strength to jump
        {
            SetJointSettings(jointSpring);
        }
        else         // The avatar stay to recover strength
        {
            thrusterFuelAmount += thrusterFuelBurnSpeed * Time.fixedDeltaTime;
            if(ifFirst==1)
            {
                ifFirst = 0;
                SetJointSettings(jointSpring);
            }
        }
        SetJointAnchor();
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, thrusterFuelTotal);
        motor.ApplyThruster(_thrusterForce);
    }
    private void SetJointAnchor()
    {
        RaycastHit _b;
        Ray _asd=new Ray(transform.position,Vector3.up*-1);
        Physics.Raycast(_asd,out _b, LayerMask.GetMask("Ground"));
        Vector3 a = new Vector3(transform.position.x, (float)(transform.position.y - _b.distance + 0.5), transform.position.z);
        joint.connectedAnchor =a;
    }
}
