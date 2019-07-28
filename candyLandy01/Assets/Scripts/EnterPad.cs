using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPad : MonoBehaviour
{

    public Camera cam;
    public GameObject root;
    public GameObject pad;
    public GameObject sub;
    public GameObject sub_child;
    public GameObject wake;
    public float rotspeed;
    public float speed;
    public float speedown;
    private bool inside;
    private bool onBridge;
    private Rect rec0;
    private int count;

    // Use this for initialization
    void Start()
    {
        inside = false;
        onBridge = false;
        rec0 = cam.rect;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (inside)
        {

            GetComponent<FPSWalkerCS>().walk_speed = 0.2f;
            GetComponent<FPSWalkerCS>().running_speed = 0.6f;
            //cam.rect.position.y >= 0.1f &&
            if (GetComponent<CharacterController>().height > 0.2)
            {
                GetComponent<CharacterController>().height -= Time.deltaTime * 0.5f;
                GetComponent<CharacterController>().radius = GetComponent<CharacterController>().height / 10;
            }


        }

        if (onBridge && inside)
        {
            //if (cam.rect.position.y < 0.1f)
            //{

            //    rec0.y = 0.1f;
            //    rec0.height = 0.8f;
            //    cam.rect = rec0;

            //}

            Debug.Log(sub.transform.localEulerAngles.y);
            if (sub.transform.localEulerAngles.y < 170)
            {
                wake.SetActive(true);
                if (sub.transform.localEulerAngles.y > 140)
                {
                    wake.GetComponent<AnimatedTextureWakeUVs>().under = true;

                }
                sub.transform.Rotate(0, 2.0f * rotspeed * Time.deltaTime, 0);
                sub.transform.Translate(Vector3.right * Time.deltaTime * speed * 0.5f);
                if (sub.transform.localEulerAngles.y > 91)
                {
                    sub_child.transform.Translate(Vector3.down * Time.deltaTime * speedown * 0.01f);

                }
            }
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TriPad")
        {
            Debug.Log("entered Pad");
            //GetComponent<CharacterController>().enabled = false;
            inside = true;
            Vector3 posi = pad.transform.position;
            posi.y = transform.position.y;
            transform.position = posi;
        }

        if (other.tag == "TriBridge")
        {
            Debug.Log("entered Bridge");
            onBridge = true;
        }
    }
}
