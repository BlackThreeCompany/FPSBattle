using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : MonoBehaviour
{
    public float ThrowPower;
    public float BoomTimer;

    public bool isBoom = false;
    bool isGround = false;

    Rigidbody rbody;
    Transform tr;

    public int cnt = 1;

    public GameObject SmokeEft;
    public static SmokeGrenade instance;

    private void Awake()
    {
        instance = this;
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
        if (isBoom && cnt == 1)
        {
            isBoom = false;
            cnt = 0;
            Invoke("Boom", 5.0f);
        }
    }

    void Boom()
    {
        SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.SmokegrenadeBoom, 0, SoundManager.instance.sfxVolum * 0.8f);
        Destroy(tr.gameObject, SoundManager.instance.SmokegrenadeBoom.length);
        Instantiate(SmokeEft, tr.position, tr.rotation);
        Instantiate(SmokeEft, tr.position, tr.rotation);
        ActionController.instance.throwGrenade = false;
    }
    public void DestroyMe()
    {
        Destroy(this.gameObject);
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
