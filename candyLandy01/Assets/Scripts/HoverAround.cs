using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAround : MonoBehaviour {
    float verticalSpeed;
    float amplitude;
    Vector3 CenterPosition;
    Vector3 targetPosition;
    Vector3 tempPosition;
	// Use this for initialization
	void Start () {
        CenterPosition = transform.position;
        targetPosition = transform.position;
        tempPosition = transform.position;
        amplitude = 0.5f;
        verticalSpeed = 0.5f;


    }
    Vector3 direction;
    void FaceTarget()
    {

        direction = (targetPosition-transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime*1f);
    }
    // Update is called once per frame
    void FixedUpdate () {
        if(Mathf.Abs(targetPosition.x-transform.position.x) <0.1&& Mathf.Abs(targetPosition.z - transform.position.z) < 0.1)
        {
            targetPosition = CenterPosition+Random.onUnitSphere*3;
            targetPosition.y = 0;
        }
        FaceTarget();
        tempPosition += Time.fixedDeltaTime * direction*2;
        tempPosition.y = CenterPosition.y+ Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude;
        transform.position = tempPosition;
	}
}
