using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CandyLeft : MonoBehaviour {
    public int Number = 3;
    public Text left;

	// Update is called once per frame
	void Update () {
        left.text = Number + "/3";
        
	}
}
