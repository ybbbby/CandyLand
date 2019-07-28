using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceProvider : MonoBehaviour {
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnGrabBallon1;
    public static event PlayerDelegate OnGrabBallon2;
    private Vector3 Difference;

    private ConfigurableJoint joint;
    Player_Motor pm;
    GameObject balloon;
    Vector3 OriginPlace;
    private int state=0;
    RainbowGravity rg;
    int endGame = 0;
    // Use this for initialization
    float timeStay = -1f;
    public AudioClip Click;
    AudioSource source;
    void Start () {
        pm = GetComponent<Player_Motor>();
        state = 0;
        joint = GetComponent<ConfigurableJoint>();
        rg = GetComponent<RainbowGravity>();
        source = GetComponents<AudioSource>()[1];
    }
	
	// Update is called once per frame
	void Update () {
        if (state == 1)
        {
            if(balloon.transform.position.y<OriginPlace.y)
            {
                state = 0;
                timeStay = -1f;
                SetJointSettings(20f);
                pm.ifMove = true;
                return;
            }
            transform.position = balloon.transform.position - Difference;
            if (Input.GetButton("Jump"))
            {
                
                state = 0;
                timeStay = -1f;
                SetJointSettings(20f);
                pm.ifMove = true;
            }
        }
        else if(state==2)
        {
            if(balloon.transform.position.y<7f)
            {
                state = 0;
                timeStay = -1f;
                SetJointSettings(20f);
                pm.ifMove = true;
                return;
            }
            transform.position = balloon.transform.position - Difference;
            if (Input.GetButton("Jump"))
            {

                state = 0;
                timeStay = -1f;
                SetJointSettings(20f);
                pm.ifMove = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="balloon")
        {
            balloon = other.gameObject;
            timeStay = 2f;
        }
        else if(other.name=="end")
        {
            if(endGame==1)
            {
                Application.Quit();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "balloon")
        {
            Debug.Log("leave");
            timeStay = -1f;
            SetJointSettings(20f);
            pm.ifMove = true;
            state = 0;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "balloon"&&timeStay>0)
        {
            timeStay -= Time.deltaTime;
        }

        if(other.tag == "balloon" && timeStay <= 0&&timeStay>-1&&state==0)
        {
            ServicePublisher sp = other.GetComponentInParent<ServicePublisher>();
            if (sp.UpOrDown == 0)
            {
                OnGrabBallon1();
                state = 1;
                rg.enabled = true;

                OriginPlace = balloon.transform.position;
                pm.ifMove = false;
                Difference = other.transform.position - transform.position;
                SetJointSettings(0f);
            }
            else
            {
                OnGrabBallon2();
                state = 2;

                OriginPlace = balloon.transform.position;
                pm.ifMove = false;
                Difference = other.transform.position - transform.position;
                SetJointSettings(0f);

                Throw_Candy asd = GetComponent<Throw_Candy>();
                asd.backToNormal();
                startEndGame();
                GameObject dfg=GameObject.Find("Gingerbread Man");
                JumpJump sdf = dfg.GetComponent<JumpJump>();
                sdf.enabled = true;
                GameObject ff = GameObject.Find("House");
                ff.SendMessage("Change");
                source.PlayOneShot(Click,1);


            }
            

        }
        
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = _jointSpring
        };
    }
    public void startEndGame()
    {
        endGame = 1;
    }
}
