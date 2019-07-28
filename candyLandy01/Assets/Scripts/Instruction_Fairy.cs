using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction_Fairy : MonoBehaviour {
    float verticalSpeed;
    float amplitude;
    Vector3 targetPosition;
    Vector3 tempPosition;
    Transform target;
    Transform player;
    //int status = 0;//0--free    1--instruction
	// Use this for initialization
	void Start () {
        target = playerManager.instance.player.transform;
        targetPosition = transform.position;
        tempPosition = transform.position;
        amplitude = 0.5f;
        verticalSpeed = 0.5f;
        player = playerManager.instance.player.transform;

    }
    Vector3 direction;
    void FaceTarget()
    {

        direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 3f);
    }
    // Update is called once per frame
    void Update () {

        if (Mathf.Abs(targetPosition.x - transform.position.x) < 0.5 && Mathf.Abs(targetPosition.z - transform.position.z) < 0.5)
        {
           // RaycastHit hit;
           // Physics.Raycast(transform.position, -transform.up, out hit);
            targetPosition = target.position + Random.onUnitSphere * 3;
            targetPosition.y = target.position.y +Random.Range(-0.5f,0.5f) ;
        }
        RaycastHit hit;
        bool some=Physics.Raycast(transform.position, -transform.up, out hit);
        //Debug.Log(hit.distance);
        if(hit.distance<0.1||some==false)
        {
            targetPosition.y = transform.position.y + 0.5f;
        }
        FaceTarget();
        tempPosition += Time.deltaTime * direction * 2f;
        //tempPosition.y = target.position.y + (Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude);
        transform.position = tempPosition;
    }
}
