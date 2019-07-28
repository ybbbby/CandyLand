using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDoor : MonoBehaviour {
    MeshRenderer mr;
    public Material ss;
    Material[] asd;
	// Use this for initialization
	void Start () {
        mr = GetComponent<MeshRenderer>();
        asd = mr.materials;
        asd[13] = ss;
	}
	void Change()
    {
        mr.materials= asd;
    }
}
