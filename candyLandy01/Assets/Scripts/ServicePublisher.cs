using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServicePublisher : MonoBehaviour {

    public delegate void PlayerDelegate();
    private int state = 0;
    private Vector3 Tposition;
    private Vector3 OriginPlace;
    public int UpOrDown;//0--Up, 1--Down
    private float speed = 1f;
    float z;
    // Use this for initialization
    void Start () {
        state = 0;
        OriginPlace = transform.position;
        GameObject house = GameObject.Find("House");
        z = house.transform.position.z - transform.position.z;


    }
    private void OnDisable()
    {
        ServiceProvider.OnGrabBallon1 -= BalloonFlyService1;
        ServiceProvider.OnGrabBallon2 -= BalloonFlyService2;
    }
    private void OnEnable()
    {
        ServiceProvider.OnGrabBallon1 += BalloonFlyService1;
        ServiceProvider.OnGrabBallon2 += BalloonFlyService2;
    }
    // Update is called once per frame
    void FixedUpdate () {
        if(UpOrDown==0)
        {
            if (state == 1)
            {
                Tposition = transform.position;
                speed += 0.001f;
                Tposition.y += Time.fixedDeltaTime * speed;
                transform.position = Tposition;
                if (Tposition.y >= 38f)
                {
                    transform.position = OriginPlace - Vector3.up * 2;
                }
                if (transform.position.y < OriginPlace.y && ((OriginPlace.y - transform.position.y) < 0.1f))
                {
                    transform.position = OriginPlace;
                    state = 0;
                }
            }
        }
        else
        {
            if(state==1)
            {
                Tposition = transform.position;
               // speed += 0.001f;
                Tposition.z += Time.fixedDeltaTime * speed;
                transform.position = Tposition;
                if (Mathf.Abs(Tposition.z-OriginPlace.z)>=z)
                {
                    state = 2;
                    speed = 2;

                }
            }
            else if(state==2)
            {
                Tposition = transform.position;
               // speed += 0.001f;
                Tposition.y -= Time.fixedDeltaTime * speed;
                transform.position = Tposition;
                if (Tposition.y <= 5)
                {
                    transform.position = OriginPlace + Vector3.up * 2;

                }
                if (transform.position.y > OriginPlace.y && ((OriginPlace.y - transform.position.y) < 0.1f))
                {
                    transform.position = OriginPlace;
                    state = 0;
                }
            }
            
        }
	}
    void BalloonFlyService1()
    {
        if(UpOrDown==0)
        {
            state = 1;
            speed = 1;
        }
    }
    void BalloonFlyService2()
    {
        if(UpOrDown==1)
        {
            state = 1;
            speed = 2;
        }
    }
}
