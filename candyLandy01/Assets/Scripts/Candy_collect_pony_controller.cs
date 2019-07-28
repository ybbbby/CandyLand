using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Candy_collect_pony_controller : MonoBehaviour {
    public float lookRadius = 10f;
    public float idleRadius = 8f;
    public float distance = 1f;
    Animator PonyAnimator;
    public AudioClip Yell;// 0
    public AudioClip Walk;// 1
    public AudioClip ShakeHead;// 2
    public AudioClip idleSound;// 3
    public AudioClip FlySound;// 4
    AudioSource source;
    Rigidbody rb;
    Transform player;
    int status;

    int[] IdleStates = { 1, 2, 6, 8, 10, 11, 9 };//idleA,idleB,Walk,Horn_Atk,lie,jump,fly
    float[] TimeLast = { 2, 3, 2, 3.5f, 3, 3.5f, 6 };
    float _TimeLeft = 0;
    int move;
    int lastmove;
    Vector3 FaceToward = new Vector3();
    Vector3 goRandom = new Vector3();
    bool onGround = false;
    public LayerMask hitMask;
    float TimeLeft = -100;
    int BigStatus;// 0--free 1--go for food 2--get food 3--into House
    // Use this for initialization
    Transform caidian;//the original position the unicron prepare to get into house
    int houseProcess = 1;//0 --not in the process, 1--go to origin  2-- go to house
    Transform house;
    void Start()
    {
        player = playerManager.instance.player.transform;
        rb = GetComponent<Rigidbody>();
        PonyAnimator = GetComponent<Animator>();
        BigStatus = 0;
        source = GetComponent<AudioSource>();
    }
    float DownSpeed = 1f;
    // Update is called once per frame
    void EatCandy()
    {
        if(BigStatus==0)
        {
            BigStatus = 1;
        }
    }
    void FixedUpdate()
    {
        if(BigStatus==0)
        {
            idleMove();
        }
        else if(BigStatus==1)
        {
            Collider Candy;
            Candy=GoForCandy();
            if(Candy==null)
            {
                BigStatus = 0;
            }
            else
            {
                Go(Candy.transform, Candy);
            }
        }
        else if(BigStatus==2)
        {
            FollowPeople();
        }
        else
        {
            gointoHouse();
        }
    }
    void fangsheng()
    {
        devolve();
        BigStatus = 0;
    }
    void gointoHouse()
    {
        if (houseProcess == 1)
        {
            Go(caidian, null, houseProcess);
            
        }
        else if (houseProcess == 2)
        {
            Go(house, null, houseProcess);
        }
    }
    void intoHouse(Transform[] pos)
    {
        BigStatus = 3;
        caidian = pos[0];
        house = pos[1];
       // rb.isKinematic = true;
        
    }
    void evolve()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren)
        {
            if(child.gameObject.name== "Wing00")
            {
                Behaviour halo = (Behaviour) child.gameObject.GetComponent("Halo");
                halo.enabled = true;
            }
        }
    }
    void devolve()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name == "Wing00")
            {
                Behaviour halo = (Behaviour)child.gameObject.GetComponent("Halo");
                halo.enabled = false;
            }
        }
    }
    float DestroyDelay=20;
    void Go(Transform target,Collider candy=null,int house=0)
    {
        if(house!=0)
        {
            DestroyDelay -= Time.fixedDeltaTime;
            if(DestroyDelay < 0)
            {
                Destroy(rb.gameObject);
            }
        }
        float distance = Vector3.Distance(target.position, transform.position);
        _TimeLeft = 0;
        float dis;
        if(candy !=null)
        {
            dis = 1.25f;
        }
        else if(house==0)
        {
            dis = 2.5f;
        }
        else
        {
            dis = 0.8f;
        }
        if (distance <= dis)
        {
            if (move == 9) // the unicorn is flying
            {

                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);

                rb.MovePosition(rb.position - rb.transform.up.normalized * 1 * Time.fixedDeltaTime);
                if (onGround == true)       //land first
                {
                    rb.useGravity = true;
                    move = 1;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.Sleep();
                    onGround = false;
                }

            }
            else    // the unicorn is on the ground
            {
                if(candy !=null)
                {
                    if (!actmove(7, 2f))
                    {
                        if(candy.gameObject!=null)
                        {
                            candy.SendMessage("eaten", SendMessageOptions.DontRequireReceiver);
                            player.gameObject.SendMessage("intoTame", transform);
                            evolve();
                            BigStatus = 2;
                        }
                    }
                }
                else if(house==0)
                {
                    status = 2;//prepare to become unwild
                    //FaceTarget();
                    if (!actmove(3, 2f))
                    {
                        status = 3;//in the core radius and become not wild
                    }
                }
                else if(house==1)
                {
                    if (!actmove(11, 2f))
                    {
                        houseProcess = 2;
                        rb.isKinematic = true;
                    }
                }
                else if(house==2)
                {
                    Destroy(rb.gameObject);
                    //playerManager.HousedUnicron++;
                }
            }
        }
        else
        {
            TimeLeft = -100;
            if (move == 9)
            {
                FaceToward = (target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(FaceToward.x, FaceToward.y, FaceToward.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
                rb.MovePosition(rb.position + FaceToward * 1.5f * Time.fixedDeltaTime);
            }
            else
            {
                PonyAnimator.SetInteger("animation", 5);
                GoSoundWithAnimation(5);
                FaceTarget(target);
                rb.MovePosition(rb.position + rb.transform.forward * 1.5f * Time.fixedDeltaTime);
            }
        }
    }
    Collider GoForCandy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(rb.transform.position, lookRadius+1f);
        int i = 0;
        float DisMin = float.MaxValue;
        int index = -1;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Candy")
            {
                if( Vector3.Distance(hitColliders[i].transform.position, transform.position)<DisMin)
                {
                    DisMin = Vector3.Distance(hitColliders[i].transform.position, transform.position);
                    index = i;
                }
            }
            i++;
        }
        if (index == -1)
            return null;
        else
            return hitColliders[index];
    }
    void FollowPeople()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (status <= 2)
        {
            //_TimeLeft = 0;
            status = 1;//into the lookRadius but wild
            Go(player);
            /*
            if (distance <= 2)
            {
                if (move == 9)
                {

                    Vector3 direction = (player.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);

                    rb.MovePosition(rb.position - rb.transform.up.normalized * 1 * Time.fixedDeltaTime);
                    if (onGround == true)
                    {
                        rb.useGravity = true;
                        move = 1;
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        rb.Sleep();
                        onGround = false;
                    }

                }
                else
                {
                    status = 2;//prepare to become unwild
                    //FaceTarget();
                    if (!actmove(3, 2f))
                    {
                        status = 3;//in the core radius and become not wild
                    }
                }
            }
            else
            {
                if (move == 9)
                {
                    FaceToward = (player.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(FaceToward.x, FaceToward.y, FaceToward.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
                    rb.MovePosition(rb.position + FaceToward * 1.5f * Time.fixedDeltaTime);
                }
                else
                {
                    PonyAnimator.SetInteger("animation", 5);

                    FaceTarget(player);

                    //FaceToward = (target.position - transform.position).normalized;
                    rb.MovePosition(rb.position + rb.transform.forward * 1.5f * Time.fixedDeltaTime);
                    // bool a = agent.SetDestination(target.position);
                    // a = (agent.pathEndPosition == transform.position);
                }
            }
            */
        }
        else if (status == 3 && distance > idleRadius)
        {
            status = 4;//start getting into core circle
            
            _TimeLeft = 0;
        }
        else if (status == 4)
        {
            if (distance <= ((idleRadius) / 2))
            {
                if (move == 9)
                {
                    rb.MovePosition(rb.position - rb.transform.up.normalized * 1 * Time.fixedDeltaTime);
                    if (onGround == true)
                    {
                        rb.useGravity = true;
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        rb.Sleep();
                        onGround = false;
                        move = 1;
                        _TimeLeft = 1;
                        status = 3;
                    }

                }
                else
                {
                    PonyAnimator.SetInteger("animation", 2);
                    GoSoundWithAnimation(2);
                    move = 2;
                    _TimeLeft = 1;
                    status = 3;
                }
            }
            else
            {
                if (move == 9)
                {
                    /*
                    onGround = false;
                    _TimeLeft = 1;
                    FaceToward = target.position - transform.position;
                    Quaternion lookRotation = Quaternion.LookRotation(FaceToward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 1f);
                    rb.MovePosition(rb.position + FaceToward.normalized * 1 * Time.fixedDeltaTime);
                    */

                    FaceToward = (player.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(FaceToward.x, FaceToward.y, FaceToward.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
                    rb.MovePosition(rb.position + FaceToward * 1.5f * Time.fixedDeltaTime);
                }
                else
                {
                    PonyAnimator.SetInteger("animation", 5);
                    GoSoundWithAnimation(5);
                    FaceTarget(player);
                    rb.MovePosition(rb.position + rb.transform.forward * 1.5f * Time.fixedDeltaTime);
                }

            }
        }
        else
        {
            //status is 0 or 3
            idleMove();
        }
    }

    /*
     * 
    public AudioClip Yell;// 0
    public AudioClip Walk;// 1
    public AudioClip ShakeHead;// 2
    public AudioClip idleSound;// 3
    public AudioClip FlySound;// 4
    
     */
    int remember = 0;
    void setAudio(int i)
    {
        
        if (source.clip == ShakeHead && source.isPlaying==true)
        {
            return;
        }
        if (i==0)
        {
            if (source.clip != Yell)
            {
                source.volume = 0.563f;
                source.clip = Yell;
                source.loop = false;
                source.Play();
            }
        }
        else if(i==1)
        {
            if(source.clip!=Walk)
            {
                source.volume = 1f;
                source.clip = Walk;
                source.loop = true;
                source.Play();
            }
        }
        else if(i==2)
        {
            if (source.clip != ShakeHead)
            {
                source.volume = 0.25f;
                source.clip = ShakeHead;
                source.loop = false;
                source.Play();
            }
        }
        else if(i==3)
        {
            if(source.clip!=idleSound)
            {
                source.volume = 0.563f;
                source.clip = idleSound;
                source.loop = false;
                source.Play();
            }
        }
        else if(i==4)
        {
            if (source.clip != FlySound)
            {
                source.volume = 1f;
                source.clip = FlySound;
                source.loop = true;
                source.Play();
            }
        }
        else
        {
            
                source.clip = null;
                source.loop = false;
                source.Stop();
        }
    }
    /*
     * 
        public AudioClip Yell;// 0
        public AudioClip Walk;// 1
        public AudioClip ShakeHead;// 2
        public AudioClip idleSound;// 3
        public AudioClip FlySound;// 4
     */
    void GoSoundWithAnimation(int a)
    {
        if (a == 2)
        {
            setAudio(3);
        }
        else if (a == 3||a==11)
        {
            setAudio(0);
        }
        else if (a == 4)
        {
            setAudio(2);
        }
        else if(a==5||a==8)
        {
            setAudio(1);
        }
        else if(a==9)
        {
            setAudio(4);
        }
        else
        {
            setAudio(-1);
        }
    }
    int DecideMove(double a)
    {
        int b = 0;
        if (a < 0.6)
        {
            if (a < 0.2)
            {
                b = 0;
            }
            else if (a > 0.4)
            {
                b = 2;
            }
            else
            {
                b = 1;
            }
        }
        else
        {
            if (a < 0.7)
            {
                b = 4;
            }
            else if (a < 0.8)
            {
                b = 5;
            }
            else if (a < 0.9)
            {
                b = 6;
            }
            else
            {
                b = 3;
            }
        }
        return b;
    }
    Vector2 output = new Vector2();
    Vector2 getGaussDirection()
    {
        float a = 0;
        for (int i = 0; i < 3; i++)
        {
            a += Random.value;
        }

        a = a / 3 * 3.1415926f;
        a -= a / 2;

        Vector3 asd = transform.forward;
        float b = Mathf.Atan(asd.x / asd.z);
        a = b + a;

        output.x = Mathf.Sin(a);
        output.y = Mathf.Cos(a);
        return output;
    }

    bool actmove(int a, float b)
    {
        if (TimeLeft == -100)
        {
            PonyAnimator.SetInteger("animation", a);
            GoSoundWithAnimation(a);
            TimeLeft = b;
        }
        if (TimeLeft <= 0)
        {
            TimeLeft = -100f;
            return false;
        }
        TimeLeft -= Time.fixedDeltaTime * 1f;
        return true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (move == 9 && collision.collider.tag == "landable")
        {

            onGround = true;
        }
    }
    void idleMove()
    {
        if (_TimeLeft <= 0)
        {
            lastmove = DecideMove(Random.value);
            if (lastmove == move)
            {
                move = 2;
            }
            else
            {
                move = lastmove;
            }
            _TimeLeft = TimeLast[move];
            move = IdleStates[move];
            PonyAnimator.SetInteger("animation", move);
            if (move == 6)
            {
                Vector2 b = getGaussDirection();
                FaceToward.x = b.x;
                FaceToward.y = 0;
                FaceToward.z = b.y;
            }
            else if (move == 8)
            {
                FaceToward = transform.forward;
            }
            else if (move == 9)
            {
                FaceToward = Vector3.up;
                rb.useGravity = false;
                Vector2 b = getGaussDirection();
                goRandom.x = b.x;
                goRandom.y = 0;
                goRandom.z = b.y;

            }
        }
        GoSoundWithAnimation(move);
        if (move == 6)
        {
            Vector3 asd = CorrectUpandDown(FaceToward);
            Quaternion lookRotation = Quaternion.LookRotation(asd);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 1f);
            rb.MovePosition(rb.position + asd.normalized * 1 * Time.fixedDeltaTime);

        }
        else if (move == 8 && (_TimeLeft >= 0.5f))
        {
            Vector3 asd = CorrectUpandDown(FaceToward);
            Quaternion lookRotation = Quaternion.LookRotation(asd);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 1f);
            rb.MovePosition(rb.position + rb.transform.forward * 2 * Time.fixedDeltaTime);
        }
        else if (move == 9 && _TimeLeft < TimeLast[6] / 6)
        {
            rb.MovePosition(rb.position - FaceToward.normalized * DownSpeed * Time.fixedDeltaTime);
            if (onGround == false)
            {
                _TimeLeft = TimeLast[6] / 6;
                DownSpeed += 0.1f;
            }
            else
            {
                _TimeLeft = 0f;
                onGround = false;
            }
        }
        else if (move == 9 && _TimeLeft > TimeLast[6] / 6 && _TimeLeft < TimeLast[6] * 2 / 3)
        {
            Quaternion lookRotation = Quaternion.LookRotation(goRandom);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
            rb.MovePosition(rb.position + goRandom * 1 * Time.fixedDeltaTime);
        }
        else if (move == 9 && _TimeLeft > TimeLast[6] * 2 / 3)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(transform.forward.x, 0f, transform.forward.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
            rb.MovePosition(rb.position + FaceToward.normalized * 1 * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position);
        }

        if ((move == 8 || move == 11) && (_TimeLeft < 0.5f))
        {
            PonyAnimator.SetInteger("animation", 4);
            GoSoundWithAnimation(4);
        }

        _TimeLeft -= Time.fixedDeltaTime * 1f;

        if (_TimeLeft < 0f && move != 1)
        {
            rb.useGravity = true;
            DownSpeed = 1;
            if(move!=9)
            {
                PonyAnimator.SetInteger("animation", 1);
                GoSoundWithAnimation(1);
                move = 1;
                _TimeLeft = 0.8f;
            }
            else
            {
                PonyAnimator.SetInteger("animation", 6);
                GoSoundWithAnimation(6);
                move = 6;
                _TimeLeft = 0.8f; 
                Vector2 b = getGaussDirection();
                FaceToward.x = b.x;
                FaceToward.y = 0;
                FaceToward.z = b.y;
            }
        }
    }
    float FindAngleY(Vector3 direction)
    {
        Ray ray = new Ray(transform.position + transform.forward * transform.lossyScale.z * 4 / 9 + transform.up * transform.lossyScale.y / 4, Vector3.down);
        RaycastHit hit;
        float angle;
       // Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
        if (Physics.Raycast(ray, out hit, distance, hitMask))
        {
            angle = Vector3.Angle(hit.normal, direction);
            angle = angle - 90;
            float angle2 = 90 - Vector3.Angle(transform.forward, Vector3.up);
            if (Vector3.Angle(transform.up, hit.normal) < 90)
            {
                angle = angle2;
            }

            //Debug.Log("angle " + angle);

        }
        else
        {
            angle = 90 - Vector3.Angle(transform.forward, Vector3.up);
            transform.Rotate(180, 0, 0);
        }
        return angle;
    }
    Vector3 CorrectUpandDown(Vector3 direction)
    {
        direction.y = 0;
        direction = direction.normalized;
        float angle = FindAngleY(direction);

        direction.y = Mathf.Tan(Mathf.Deg2Rad * angle);
        direction = direction.normalized;
        return direction;
    }
    Vector3 FaceTarget(Transform target)
    {

        Vector3 direction = (target.position - transform.position).normalized;
        direction = CorrectUpandDown(direction);


        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
        return direction;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
    
}
