using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class BulletMove : MonoBehaviourPunCallbacks
{
    public GameObject Bullet;
    public Rigidbody rb;
    Vector3 dir = new Vector3(0, 0, 1);

    public float BulletTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(StatManager.instance.BulletSpeed * transform.forward);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        BulletTime += Time.deltaTime;
        if (BulletTime >= 10)
        {
            Destroy(this.gameObject);
        }
    }
    void Move()
    {
        //transform.position += transform.forward * Time.deltaTime * StatManager.instance.BulletSpeed;
        //rb.velocity = Time.fixedDeltaTime * StatManager.instance.BulletSpeed * transform.forward;
        
    }
}
