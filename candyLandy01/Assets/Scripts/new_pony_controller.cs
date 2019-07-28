using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class new_pony_controller : MonoBehaviour {

    public float lookRadius = 10f;
    public float idleRadius = 8f;
    public float distance = 1f;
    Animator PonyAnimator;

    Rigidbody rb;
    Transform target;
    int status;

    int[] IdleStates = { 1, 2, 6, 8, 10, 11, 9 };//idleA,idleB,Walk,Horn_Atk,loll,jump,fly
    float[] TimeLast = { 2, 3, 2, 3.5f, 3, 3.5f, 6 };
    float _TimeLeft = 0;
    int move;
    int lastmove;
    Vector3 FaceToward = new Vector3();
    Vector3 goRandom = new Vector3();
    float TimeLeft = -100;
    bool onGround = false;
    public LayerMask hitMask;
    // Use this for initialization
    void Start()
    {
        target = playerManager.instance.player.transform;
        rb = GetComponent<Rigidbody>();
        PonyAnimator = GetComponent<Animator>();
    }
    float DownSpeed = 1f;
    // Update is called once per frame
    void FixedUpdate()
    {

        //rb.velocity = Vector3.zero;
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance >= lookRadius)
        {
            status = 0;//wild
        }
        if (distance <= lookRadius && status <= 2)
        {
            _TimeLeft = 0;
            status = 1;//into the lookRadius but wild
            if (distance <= 2)
            {
                if (move == 9)
                {

                    Vector3 direction = (target.position - transform.position).normalized;
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
                    FaceToward = (target.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(FaceToward.x, FaceToward.y, FaceToward.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
                    rb.MovePosition(rb.position + FaceToward * 1.5f * Time.fixedDeltaTime);
                }
                else
                {
                    PonyAnimator.SetInteger("animation", 5);

                    FaceTarget();
                    
                    //FaceToward = (target.position - transform.position).normalized;
                    rb.MovePosition(rb.position + rb.transform.forward * 1.5f * Time.fixedDeltaTime);
                    // bool a = agent.SetDestination(target.position);
                    // a = (agent.pathEndPosition == transform.position);
                }
            }
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
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1f);
                    rb.MovePosition(rb.position + FaceToward.normalized * 1 * Time.deltaTime);
                    */

                    FaceToward = (target.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(FaceToward.x, FaceToward.y, FaceToward.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
                    rb.MovePosition(rb.position + FaceToward * 1.5f * Time.fixedDeltaTime);
                }
                else
                {
                    PonyAnimator.SetInteger("animation", 5);
                    FaceTarget();
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
        //Debug.Log(output);
        return output;
    }

    bool actmove(int a, float b)
    {
        if (TimeLeft == -100)
        {
            PonyAnimator.SetInteger("animation", a);
            TimeLeft = b;
        }
        if (TimeLeft <= 0)
        {
            PonyAnimator.SetInteger("animation", 2);
            TimeLeft = -100f;
            return false;
        }
        TimeLeft -= Time.deltaTime * 1f;
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
        /*
        if (rb.velocity != Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.Sleep();
        }
        */
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
                //FaceToward=CorrectUpandDown(FaceToward);
            }
            else if (move == 8)
            {
                FaceToward = transform.forward;
                //FaceToward = CorrectUpandDown(transform.forward);
            }
            else if (move == 9)
            {
                FaceToward = Vector3.up;
                rb.useGravity = false;
                Vector2 b = getGaussDirection();
                goRandom.x = b.x;
                goRandom.y = 0;
                goRandom.z = b.y;
                //goRandom = CorrectUpandDown(goRandom);

            }
        }

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
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(transform.forward.x,0f, transform.forward.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
            rb.MovePosition(rb.position + FaceToward.normalized * 1 * Time.fixedDeltaTime);
        }
        else
        {
            //Vector3 direction = CorrectUpandDown(transform.forward);
            //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);

            rb.MovePosition(rb.position);
        }

        if ((move == 8 || move == 11) && (_TimeLeft < 0.5f))
        {
            PonyAnimator.SetInteger("animation", 4);
        }

        _TimeLeft -= Time.fixedDeltaTime * 1f;

        if (_TimeLeft < 0f && move != 1)
        {
            if (move == 9)
            {
                rb.useGravity = true;
                DownSpeed = 1;
            }
            PonyAnimator.SetInteger("animation", 1);
            move = 1;
            _TimeLeft = 0.8f;
        }
    }
    float FindAngleY(Vector3 direction)
    {
        Ray ray = new Ray(transform.position + transform.forward * transform.lossyScale.z*4/9+ transform.up * transform.lossyScale.y / 4, Vector3.down);
        RaycastHit hit;
        float angle;
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
        if (Physics.Raycast(ray, out hit, distance, hitMask))
        {
            //Debug.Log("Hit collider " + hit.collider + ", at " + hit.point + ", normal " + hit.normal);
            Debug.DrawRay(hit.point, hit.normal * 2f, Color.blue);

            angle = Vector3.Angle(hit.normal, direction);

            angle = angle - 90;
            float angle2 = 90 - Vector3.Angle(transform.forward, Vector3.up);
            if (Vector3.Angle(transform.up, hit.normal) >= 90)
            {
                Debug.Log("modify" + Mathf.Abs(angle2 - angle) + "  forward:" + angle2 + "suppose" + angle);

                //angle = angle2;
            }
            else
            {
                angle = angle2;
            }
            
            Debug.Log("angle " + angle);

        }
        else
        {
            angle = 90 - Vector3.Angle(transform.forward, Vector3.up);
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
    Vector3 FaceTarget()
    {

        Vector3 direction = (target.position - transform.position).normalized;
        direction = CorrectUpandDown(direction);


        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
        return direction;
        // Vector3 direction = (target.position - transform.position).normalized;
        // Quaternion lookRotation = Quaternion.LookRotation(direction);
        ///fix me!
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position, lookRadius);
        //Gizmos.DrawWireSphere(transform.position, idleRadius);
    }

}
