using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowGravity : MonoBehaviour {
    Rigidbody rb;
    public float distance=1f;
    public float gravity = 10f;
    public LayerMask hitMask;
    int IsFirst = 0;
    public float ForceStrength=2f;
    bounce a;
    Player_Motor b;
    Vector3 dir;
    BoxCollider bc;
    RainBowSpeed rs;
    Vector3 OldSize;
    int firstHit = 0;
    private Camera cam;
    public AudioClip Victory;
    AudioSource source;
    AudioSource BGMSource;
    public AudioClip BGMRace;
    public AudioClip BGM2;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        a = GetComponent<bounce>();
        dir = new Vector3(-0.9939408f, -0.0007806213f, -0.1099157f);
        bc = GetComponent<BoxCollider>();
        b = GetComponent<Player_Motor>();
        OldSize=bc.size;
        rs = GetComponent<RainBowSpeed>();
        cam = GetComponentInChildren<Camera>();
        source = GetComponents<AudioSource>()[1];
        BGMSource = GetComponents<AudioSource>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
       // Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
        //Debug.Log("height" + transform.position.y);
        if (Physics.Raycast(ray, out hit, distance, hitMask) && transform.position.y >= 2)// when the player jump on the rainbow
        {
            if(BGMSource.clip!=BGMRace)    // if it is the first time player jump on the rainbow, then change the back ground music
            {
                BGMSource.clip = BGMRace;
                BGMSource.Play();
            }
            rs.enabled = true;// the script that control the rainbow gravity
            b.enabled = false;// Player_Motor b; disable the old control system.
            a.enabled = true;// the script that control the bounce force
            rs.SetGDir(hit.normal); // tranfer the augument so that the script knows the direction of rainbow gravity.
            float currentX=cam.transform.localEulerAngles.x; // fixed the camera angle
            if(Mathf.Abs(currentX)<1)
            {
                cam.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {

                cam.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            }
            bc.size = new Vector3(1.17f,0.17f,1.17f); // the global forward direction of the rainbow

            // the code below are for modifying the posture of the player
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f * Time.deltaTime); 
            IsFirst = 1;
            Vector3 asd = new Vector3();
            asd = hit.normal.normalized;
            asd=Vector3.RotateTowards(asd, dir, Mathf.Deg2Rad * 90, 0);
            rs.SetDir(asd);
            Quaternion lookRotation = Quaternion.LookRotation(asd);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1.5f);
            Debug.DrawRay(transform.position, asd,Color.red);
        }
        else if(IsFirst==1)
        {
            if(Vector3.Angle(transform.up,Vector3.up)<1)
            {
                if(BGMSource.clip!=BGM2)
                {
                    BGMSource.clip = BGM2;
                    BGMSource.Play();
                }
                rs.enabled = false;
                b.enabled = true;
                a.enabled = false;
                IsFirst = 0;
                bc.size = OldSize;
               // rb.isKinematic = false;
                transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
                firstHit = 0;
            }
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3 * Time.deltaTime);
        }
    }
    public void backToNormal()
    {
        b.enabled = true;
        rs.enabled = false;
        bc.size = OldSize;
        source.PlayOneShot(Victory);
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }
    
}
