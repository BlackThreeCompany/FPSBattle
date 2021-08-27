using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SmokeGrenade : MonoBehaviourPunCallbacks
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

    public PhotonView pv;
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
        if (!pv.IsMine)
        {

            return;
        }

        if (isBoom && cnt == 1)
        {
            isBoom = false;
            cnt = 0;
            pv.RPC("BoomReady_RPC", RpcTarget.OthersBuffered);
            Invoke("Boom", 5.0f);
        }
    }

    void Boom()
    {
        if (pv.IsMine)
        {
            pv.RPC("Boom_RPC", RpcTarget.OthersBuffered);
        }

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

    [PunRPC]
    private void BoomReady_RPC()
    {
        this.gameObject.layer = 7;
    }

    [PunRPC]
    private void Boom_RPC()
    {
        Boom();
    }
}

