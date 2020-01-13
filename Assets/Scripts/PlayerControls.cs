using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float movementSpeed = 7.0f;

    public float shotTime;
    public static string bulletName = "bullet";

    public LayerMask mask;

    public Vector3 pos, oldpos;
    
    public float time;

    private Rigidbody rb;

    public SphereCollider colliderVar;
    public BoxCollider boxColliderVar;

    public Vector3 tempPosition;

    

    int counter = 0;


    // Start is called before the first frame update
    void Start()
    {
        shotTime = Time.time;
        rb = GetComponent<Rigidbody>();
        colliderVar = GetComponent<SphereCollider>();

        Camera.main.transform.rotation = new Quaternion(0, 90, 0,0);
    }

    void movement()
    {
        pos = transform.position;


        if (Input.GetKey(KeyCode.W))
        {
            //pos -= Vector3. (Camera.main.transform.forward, new Vector3(1,0,1)) * movementSpeed * time;

            rb.AddForce(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * movementSpeed * time, ForceMode.Impulse);
            //pos += new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * movementSpeed * time;
        }

        if (Input.GetKey(KeyCode.S))
        {
            //pos -= new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * movementSpeed * time;
            rb.AddForce(-new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * movementSpeed * time, ForceMode.Impulse);

        }
        if (Input.GetKey(KeyCode.A))
        {
            //pos -= Camera.main.transform.right * movementSpeed * time;
            rb.AddForce(-Camera.main.transform.right * movementSpeed * time, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Camera.main.transform.right * movementSpeed * time, ForceMode.Impulse);
            //pos += Camera.main.transform.right * movementSpeed * time;
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            rb.AddForce(Vector3.up * movementSpeed /2 , ForceMode.Impulse);
        }


        //if(!Input.anyKey)
        //{
        //    rb.velocity = Vector3.zero;
        //    //rb.angularVelocity = Vector3.zero;
        //}

        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    pos.y -= movementSpeed * time / 2;
        //}

        //Debug.Log(pos);
        //tempPosition = pos;

        //if(Physics.CheckBox(colliderVar.bounds.center,
        //    new Vector3(colliderVar.bounds.center.x, colliderVar.bounds.min.y, colliderVar.bounds.center.z), boxColliderVar.bounds * .9f, mask))

        transform.position = pos;

        //if (new Vector3(pos.x - oldpos.x, pos.y - oldpos.y, pos.z - oldpos.z).magnitude > .2)
        //{
        //    oldpos = pos;

        //}
            Camera.main.transform.position = pos + new Vector3(0, 4, 0);

        //Debug.Log("Camera: " + Camera.main.transform.forward);
        //Debug.Log("Pos: " + pos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.GetContact(0).point != null)
        {
            pos = oldpos;
        }
    }

    public bool isGrounded()
    {
        //Debug.Log("Checking grounded");
        //Debug.Log("Grounded was: " + Physics.CheckCapsule(colliderVar.bounds.center,
        //    new Vector3(colliderVar.bounds.center.x, colliderVar.bounds.min.y, colliderVar.bounds.center.z), colliderVar.radius, mask));

        return Physics.CheckCapsule(colliderVar.bounds.center,
            new Vector3(colliderVar.bounds.center.x, colliderVar.bounds.min.y, colliderVar.bounds.center.z), colliderVar.radius * .9f, mask);
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.deltaTime;

        movement();

        //Debug.Log("shot time: " + shotTime);
        //Debug.Log("Time.time: " + Time.time) ;

        if (Input.GetMouseButton(0))
        {
            if (Time.time - shotTime > 0.4f)
            {
                shotTime = Time.time;
                GameObject currentBullet = GameObject.Instantiate<GameObject>(GameObject.Find("OGBullet"));
                currentBullet.transform.rotation = Camera.main.transform.rotation;
                currentBullet.transform.position = Camera.main.transform.position;
                currentBullet.transform.name = bulletName + counter;
                counter++;
                //BulletScript.bulletList.Add(currentBullet);

                Debug.Log("spawned bullet");

            }

        }

        

        if (Input.GetMouseButtonDown(1))
        {

            Cursor.visible = !Cursor.visible;
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;

        }

    }


}
