using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUp : MonoBehaviour {
    public AudioClip back;
    public AudioClip forward;
    AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    void TurnUp()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name == "SPHERE")
            {
                Behaviour halo = (Behaviour)child.gameObject.GetComponent("Halo");
                halo.enabled = true;
            }
        }
        if(this.name.Contains("Ballgum"))
        {
            source.PlayOneShot(back);
        }
        else
        {
            source.PlayOneShot(forward);
        }
    }
	
}
