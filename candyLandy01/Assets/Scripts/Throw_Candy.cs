using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw_Candy : MonoBehaviour {
    public GameObject[] CandyPrefab;
    public Transform head;
    public GameObject sun;
    public GameObject rocket;
    public GameObject guidUn;
    public AudioClip successThrow;
    public AudioClip failThrow;
    AudioSource source;
    public float throwForce = 0.1f;
    public Transform house;
    public Transform[] x;
    Transform[] tamedUn=new Transform[5];
    Rigidbody rb;
    Player_Controller pc;
    int index = 0;
    int k;
    public AudioClip BGM2;
    public AudioClip BGM3;
    AudioSource BGMSource;
	// Use this for initialization
  
	void Start () {
        rb = GetComponent<Rigidbody>();
        source = GetComponents<AudioSource>()[1];
        BGMSource = GetComponents<AudioSource>()[0];
        pc = GetComponent<Player_Controller>();

    }
    // Update is called once per frame
    void Update () {
        
        if(Input.GetButtonDown("Reload")||Input.GetButtonDown("Throw"))//loadingCandy
        {
           // Debug.Log("reloaded");
            Collider[] hitColliders = Physics.OverlapSphere(rb.transform.position, 3f);
            int i = 0;
           // Debug.Log(hitColliders);
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "GingerbreadMan")
                {
                  //  Debug.Log("jieshoudaole ");
                    GameObject[] asd=GameObject.FindGameObjectsWithTag("Candy");
                    for(int j=0;j<asd.Length;j++)
                    {
                        Destroy(asd[j]);
                    }
                    playerManager.candyleft = 5;

                    k = 0;
                    for(int j=0;j<index;j++)
                    {
                        if (tamedUn[j].name.Contains("UnicornCholoate"))
                        {
                            Transform[] ss = new Transform[2];
                            ss[0] = x[k];
                            ss[1] = house;
                            tamedUn[j].gameObject.SendMessage("intoHouse",ss);
                            k++;
                        }
                        else
                        {
                            tamedUn[j].gameObject.SendMessage("fangsheng");
                        }
                    }
                    playerManager.HousedUnicron += k;
                    index = 0;
                    break;
                }
                i++;
            }
            if(i==hitColliders.Length)
            {
                if ( playerManager.candyleft > 0)
                {
                    source.PlayOneShot(successThrow);
                    int a = (int)Mathf.Floor(Random.Range(0, 5));
                    GameObject Candy = Instantiate(CandyPrefab[a], head.position + head.forward.normalized, head.rotation) as GameObject;
                    Candy.GetComponent<Rigidbody>().AddForce(head.forward * throwForce, ForceMode.Impulse);
                    playerManager.candyleft--;
                }
                else if ( playerManager.candyleft <= 0)
                {
                    Debug.Log("asd");
                    source.PlayOneShot(failThrow);
                }
            }
        }
        if(playerManager.HousedUnicron>=5)
        {
            GameObject sdf = GameObject.Find("CandyGrende");
            if (sdf != null)
            {
                sdf.SetActive(false);
            }
            playerManager.candyleft = 0;
            int z = 0;
            for(int i=0;i<5;i++)
            {
                if (tamedUn[i] != null)
                {
                    if (tamedUn[i].gameObject.name.Contains("UnicornCholoate"))
                    {
                        z = 1;
                        break;
                    }
                }
            }
            if(z==0)
            {
                rocket.SetActive(true);
                sun.SetActive(false);
                guidUn.SetActive(true);
                Camera asd = this.GetComponentInChildren<Camera>();
                asd.clearFlags = CameraClearFlags.SolidColor;
                this.enabled = false;
                BGMSource.loop = true;
                BGMSource.clip = BGM2;
                BGMSource.Play();
                pc.ifJump = 1;
            }
        }

    }
    void intoTame(Transform asd)
    {
        tamedUn[index] = asd;
        index++;
    }
    public void backToNormal()
    {
        sun.SetActive(true);
        Camera asd = this.GetComponentInChildren<Camera>();
        asd.clearFlags = CameraClearFlags.Skybox;

        BGMSource.loop = true;
        BGMSource.clip = BGM3;
        BGMSource.Play();

    }
    
}
