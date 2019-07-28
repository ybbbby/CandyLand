using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderToRainbow : MonoBehaviour {
    private Transform plate;
    public GameObject plateTrue;
    Vector3 Origine;
    float grow=1,maximum;
    int state;//0 stay still 1 grow 2 shrink
    private void Start()
    {
        plate = plateTrue.transform;
        Origine = plate.localScale;
        state = 0;
        //Debug.Log(plate.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            state = 1;
        }
    }
    private void Update()
    {
        if(state==1)
        {
            if (plate.localScale.y <= 0.22f)
            {
                plate.localScale += Origine * Time.deltaTime * 0.5f;
            }
        }
        else if(state ==2)
        {
            plate.localScale -= Origine * Time.deltaTime;
            if(plate.localScale.x<=Origine.x)
            {
                state = 0;
                plate.localScale = Origine;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Player")
        {
            state = 2;
        }
    }

}
