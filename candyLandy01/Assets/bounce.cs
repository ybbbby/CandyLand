using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounce : MonoBehaviour {
    Rigidbody rb;
    Vector3 dir;
    public float ForwardSpeedUp = 4;
    public float backVelocity = 10;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        dir = new Vector3(-0.9939408f, -0.0007806213f, -0.1099157f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == false)
        {
            return;
        }
        if(other.name.Contains("Gumball_Machine_Model"))
        {
            Vector3 asd = new Vector3();
            asd =transform.up;
            asd = Vector3.RotateTowards(asd, dir, Mathf.Deg2Rad * 90, 0);

            // rb.velocity = rb.velocity+asd * 1* ForwardSpeedUp;
            if (Vector3.Dot(asd, rb.velocity) > 0)
            {
                rb.velocity = rb.velocity + rb.velocity.normalized * 1 * ForwardSpeedUp;
            }
            else
            {
                //Debug.Log("back");
                rb.velocity = rb.velocity.normalized * -1 ;
            }
           
            other.SendMessage("TurnUp");
        }
        else if(other.name.Contains("Ballgum"))
        {
            Vector3 asd = new Vector3();
            asd = transform.up;
            asd = Vector3.RotateTowards(asd, dir, Mathf.Deg2Rad * 90, 0);
            rb.velocity = asd * -1 * backVelocity;

            //rb.velocity = backVelocity * -1*rb.velocity.normalized;
            other.SendMessage("TurnUp");
        }
        else if(other.name=="FinishLine")
        {
            RainbowGravity rg = GetComponent<RainbowGravity>();
            rg.backToNormal();
            rb.velocity = Vector3.zero;
            rg.enabled = false;
            this.enabled = false;
        }
    }
}
