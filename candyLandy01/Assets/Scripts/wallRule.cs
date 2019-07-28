using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallRule : MonoBehaviour {

    public GameObject OuterCylinder;

    private MeshCollider OuterMesh;
    private void Start()
    {
        OuterMesh = OuterCylinder.GetComponent<MeshCollider>();
    }
    // Use this for initialization
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit"+other.tag);
        if (other.tag == "Player")
        {
            OuterMesh.enabled = false;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("leave");
        if (other.tag == "Player")
        {
            OuterMesh.enabled = true;
        }
    }
}
