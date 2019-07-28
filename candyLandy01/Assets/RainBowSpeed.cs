using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainBowSpeed : MonoBehaviour {
    Rigidbody rb;
    Vector3 Dir = Vector3.zero;
    Vector3 GDir = Vector3.zero;
    public float ForceStrength;
    public float sideStrength;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
    public void SetDir(Vector3 _Dir)
    {
        Dir = _Dir;
    }
    public void SetGDir(Vector3 _GDir)
    {
        GDir = _GDir;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.AddForce(-1 * GDir * 10f);
        rb.AddForce(Dir*ForceStrength);
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _yMov = Input.GetAxisRaw("Vertical");

        if (_xMov>0)
        {
            
            rb.AddForce(rb.transform.right* sideStrength * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        if (_xMov<0)
        {
            rb.AddForce(-1*rb.transform.right* sideStrength*Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
