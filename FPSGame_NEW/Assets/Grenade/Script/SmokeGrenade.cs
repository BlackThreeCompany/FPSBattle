using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : MonoBehaviour
{
    public float ThrowPower;
    public float Timer;
    public float BoomTimer;


    bool isGround = false;
    bool isBoom = false;

    Rigidbody rbody;
    Transform tr;

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
        if (ActionController.instance.throwGrenade)
        {
            if (Timer < BoomTimer)
            {
                StartCoroutine(BoomCnt());
            }
            else if (Timer >= BoomTimer)
            {
                Boom();
            }
        }

    }

    IEnumerator BoomCnt()
    {
        if (!isBoom)
        {
            isBoom = true;
            yield return new WaitForSeconds(0.5f);
            Timer += 0.5f;
            isBoom = false;
        }
    }

    void Boom()
    {
        Timer = 0;
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
