using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class trytry : MonoBehaviour
{
    Rigidbody rb;
   // Transform target;
    NavMeshAgent agent;
    BoxCollider bc;

    public float flybegin=1;
    public float flydown = 3;
    float _timeleft;
    public bool fly;
    int first=0;
    // Use this for initialization
    void Start()
    {
       // target = playerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();




    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(fly==false)
        {
            first = 0;
        }
        if (fly == true)
        {
            if(first==0)
            {
                bc.enabled = true;
                rb.useGravity = false;
                agent.updatePosition = false;
                _timeleft = 5;
                first = 1;
                rb.velocity = Vector3.zero;
            }
           
            Vector3 FaceToward = rb.transform.up;
            if (_timeleft < 6-flydown)
            {
                rb.MovePosition(rb.position - FaceToward.normalized * 1 * Time.deltaTime);
            }
            else if (_timeleft > 6-flybegin)
            {
                rb.MovePosition(rb.position + FaceToward.normalized * 1 * Time.deltaTime);
            }
            _timeleft -= Time.deltaTime * 1f;
            if (_timeleft < 0)
            {
                bc.enabled = false;
                agent.updatePosition = true;
                rb.useGravity = true;
                fly = false;
            }
        }
    }
}
