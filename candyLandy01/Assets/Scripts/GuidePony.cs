using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidePony : MonoBehaviour {

    Animator PonyAnimator;
    Transform player;
    Rigidbody rb;
    public Transform target;
    public Transform target1;
    public float FloorDisStandard=5;
    public float CeilDisStandard=8;
    public float heightStandard=5.24f;
    public LayerMask hitMask;
    int step = 0;
    int heightStatus = 0; // 0--stable, 1and2--unstable
    int status=0;// 0-- GoAway, 1-- ceil 
    Transform currentTarget;
    // Use this for initialization
    void Start () {
        PonyAnimator = GetComponent<Animator>();
        player = playerManager.instance.player.transform;
        rb = GetComponent<Rigidbody>();
        PonyAnimator.SetInteger("animation", 9);
        currentTarget = target1;
    }
	
	void FixedUpdate() {
        
        Vector3 TempDir = (currentTarget.position - player.position).normalized;
        TempDir.y = 0;
        TempDir=TempDir.normalized;
        float dis = getDistance();
        if (Vector3.Distance(currentTarget.position, rb.transform.position) < 6f)
        {
            if (currentTarget == target1)
            {
                currentTarget = target;
            }
            else
            {
                Destroy(gameObject);
                GameObject.Find("Rocket").SetActive(false);
            }
        }
        int HeiDec=keepHeight();
        if(HeiDec==0)
        {
            if (status == 0)
            {
                if (dis < CeilDisStandard)
                {
                    FaceTarget(currentTarget);
                    rb.MovePosition(rb.position + rb.transform.forward *2f * Time.fixedDeltaTime);
                }
                else
                {
                    status = 1;
                }
            }
            else if (status == 1)
            {
                if (dis < FloorDisStandard)
                {
                    status = 0;
                }
                else if (dis > CeilDisStandard)
                {
                    FaceTarget(player);
                    rb.MovePosition(rb.position + rb.transform.forward * 2f * Time.fixedDeltaTime);
                }
                else
                {
                    FaceTarget(player);
                    rb.MovePosition(rb.position);
                }

            }
        }
        else if(HeiDec==1)
        {
            rb.MovePosition(rb.position + rb.transform.up * 0.75f * Time.fixedDeltaTime);
        }
        else if(HeiDec==2)
        {
            rb.MovePosition(rb.position - rb.transform.up * 0.75f * Time.fixedDeltaTime);
        }
       
        //rb.MovePosition(rb.position + rb.transform.forward * 0.75f * Time.fixedDeltaTime);


    }
    void FaceTarget(Transform target)
    {

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
        return;
    }
    float getDistance()
    {
        Vector3 flatRb = rb.position;
        flatRb.y = 0;
        Vector3 flatPlayer = player.position;
        flatPlayer.y = 0;
        return Vector3.Distance(flatRb, flatPlayer);
    }
    int keepHeight()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, hitMask))
        {
            if (heightStatus == 0)
            {
                if (hit.distance < heightStandard - 1)
                {
                    heightStatus = 1;
                }
                else if(hit.distance > heightStandard + 1)
                {
                    heightStatus = 2;
                }
            }
            else if(heightStatus == 1)
            {
                if(hit.distance<heightStandard)
                {
                    return 1;
                }
                else
                {
                    heightStatus = 0;
                }
            }
            else if(heightStatus == 2)
            {
                if(hit.distance>heightStandard)
                {
                    return 2;
                }
                else
                {
                    heightStatus = 0;
                }
            }
        }
        return 0;
    }
}
