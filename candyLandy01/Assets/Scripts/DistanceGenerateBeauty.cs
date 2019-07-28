using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DistanceGenerateBeauty : MonoBehaviour {
    //public float range=1;
    public float forceStrength = 0.3f;
    Rigidbody rb;
    public float gap = 0.1f;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        
        Collider[] hitColliders = Physics.OverlapSphere(rb.transform.position, forceStrength);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Unicorn")
            {
                float factor = Vector3.Distance(hitColliders[i].transform.position, rb.transform.position);
                Vector3 dir = (hitColliders[i].transform.position - rb.transform.position).normalized;
                rb.transform.position = hitColliders[i].transform.position - dir * (forceStrength+gap);
            }
            i++;
        }
    }
}
