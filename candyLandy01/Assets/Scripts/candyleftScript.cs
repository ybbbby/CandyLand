using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class candyleftScript : MonoBehaviour {
    Image asd;
   
    public int which;
	// Use this for initialization
	void Start () {
        asd = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (which > playerManager.candyleft)
        {
            asd.color = Color.black;
        }
        else
        {
            asd.color = Color.white;
        }
	}
}
