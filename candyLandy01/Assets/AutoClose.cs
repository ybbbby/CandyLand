using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoClose : MonoBehaviour {

    Transform player;
	// Use this for initialization
	void Start () {
        player = playerManager.instance.player.transform;

	}
	
	// Update is called once per frame
	void Update () {
        float dis = Vector3.Distance(transform.position, player.position);
        if(dis<6)
        {
            this.gameObject.SetActive(false);
        }
	}
}
