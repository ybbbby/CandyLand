using UnityEngine;
using System.Collections;

public class GemSimpleRotate : MonoBehaviour {

    public float gemRotationSpeed = 5;

    void Start () {

	}
	
	void Update () {
		transform.Rotate(Vector3.up,gemRotationSpeed*Time.deltaTime,0);
	}
}