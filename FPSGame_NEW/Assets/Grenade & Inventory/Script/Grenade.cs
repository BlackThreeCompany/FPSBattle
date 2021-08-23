using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float ThrowPower;
    public float BoomTimer;


    bool isGround = false;

    Rigidbody rbody;
    Transform tr;

    public GameObject explosionGroundEft;
    public GameObject explosionEft;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rbody.AddForce(tr.forward * ThrowPower);
    }

    // Update is called once per frame
    void Update()
    {
        if (ActionController.instance.throwGrenade)
        {
            Invoke("Boom", BoomTimer);
        }

    }

    void Boom()
    {
        SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.grenadeBoom, 0, SoundManager.instance.sfxVolum);
        if (isGround)
        {
            Instantiate(explosionGroundEft, tr.position, tr.rotation);
        }
        else
        {
            Instantiate(explosionEft, tr.position, tr.rotation);
        }

        RaycastHit[] rayHits = Physics.SphereCastAll(tr.position, 10, Vector3.up, 0f, LayerMask.GetMask("Player"));

        foreach(RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<PlayerState>().HitByGrenade(transform.position);
        }

        Destroy(tr.gameObject);
        ActionController.instance.throwGrenade = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        int collisionLayer = collision.gameObject.layer;

        if (collisionLayer == 9)
        {
            SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.ImpactGrenade, 0, SoundManager.instance.sfxVolum);
            isGround = true;
        }
        else isGround = false;
    }
}
