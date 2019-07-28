using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpJump : MonoBehaviour {
    public int status = 0;
    public float speed;
    Vector3 ss;
    public float acc = 0.2f;
    float Rspeed;
    Transform player;
    // Use this for initialization
    void Start () {
        ss = new Vector3();
        Rspeed = speed;
        player= playerManager.instance.player.transform;

    }
	
	// Update is called once per frame
	void Update () {
		
                FaceTarget(player);
                ss = transform.position;
                ss.y += Rspeed * Time.deltaTime;
                Rspeed -= acc;
                transform.position = ss;
                if(transform.position.y<0.3f)
                {
                    Rspeed = speed;
                }
	}
    Vector3 FaceTarget(Transform target)
    {

        Vector3 direction = (target.position - transform.position-Vector3.up).normalized;
      
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
        return direction;
    }
}
