using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Grenade : MonoBehaviourPunCallbacks
{
    public float ThrowPower;
    public float BoomTimer;

    public bool isBoom=false;
    bool isGround = false;

    Rigidbody rbody;
    Transform tr;

    public GameObject explosionGroundEft;
    public GameObject explosionEft;

    public PhotonView pv;
    public int MyViewId;

    public GameObject Player;
    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        MyViewId = StatManager.instance.MyViewId;
        Player = NetWorkManager.instance.Player;
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
        if (isBoom)
        {
            isBoom = false;
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

        int hitcnt = 0;
        foreach(RaycastHit hitObj in rayHits)
        {
            if (!pv.IsMine) continue;
            //hitObj.transform.GetComponent<PlayerState>().HitByGrenade(transform.position);

            int Damage = 0;
            float distance = Vector3.Distance(transform.position, hitObj.transform.gameObject.transform.position);
            
            if (distance >= 0 && distance <= 3.7)
            {
                Damage = 100;
                
            }
            else
            {
                Damage = Mathf.FloorToInt((1 / distance) * 300);

            }
            
            Debug.Log("µ¥¹ÌÁö " + (int)Damage);
            pv.RPC("Boom_Damage", RpcTarget.AllBuffered,StatManager.instance.MyViewId,hitObj.transform.gameObject.GetPhotonView().ViewID, Damage);
            Debug.Log(hitObj);
            hitcnt++;
        }
        if(hitcnt == 0 && pv.IsMine)
        {
            pv.RPC("DestoryGrenade_RPC", RpcTarget.AllBuffered);
        }
        //Destroy(tr.gameObject);
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

    [PunRPC]
    private void Boom_Damage(int From_id, int To_id,int Damage)
    {



        if (To_id == MyViewId)
        {
            GameManager.instance.KillLog(From_id, To_id, Damage, 5);
        }

        else if (From_id == MyViewId)
        {
            GameManager.instance.KillLog(From_id, To_id, Damage, 4);
        }
        
        else
        {
            GameManager.instance.KillLog(From_id, To_id, Damage, 6);
        }
        Debug.Log("###");
        Destroy(tr.gameObject);

    }

    [PunRPC]
    private void DestoryGrenade_RPC()
    {
        Destroy(tr.gameObject);
    }
}
