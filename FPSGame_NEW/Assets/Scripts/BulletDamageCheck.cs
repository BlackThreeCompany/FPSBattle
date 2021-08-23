using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class BulletDamageCheck : MonoBehaviourPunCallbacks
{
    public PhotonView pv;
    public int MyViewId;

    public GameObject BulletObj;
    public Rigidbody Bulletrb;
    // Start is called before the first frame update
    void Awake()
    {
        MyViewId = StatManager.instance.MyViewId;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!pv.IsMine)
        {
            return;
        }
        Bulletrb.isKinematic = true;

        if (collision.gameObject.layer == 8)
        {
            Bulletrb.isKinematic = true;
            pv.RPC("BulletHit", RpcTarget.AllBuffered, MyViewId, collision.gameObject.GetComponent<PlayerViewId>().ViewId, 10);
        }

        if (collision.gameObject.layer == 9)
        {
            Bulletrb.isKinematic = true;
            pv.RPC("BulletDestory", RpcTarget.AllBuffered);
        }
        Debug.Log(collision.gameObject.name);
    }

    

    private void OnTriggerStay(Collider other)
    {
        if (!pv.IsMine)
        {
            return;
        }
        

        if (other.gameObject.layer == 8)
        {

            pv.RPC("BulletHit", RpcTarget.AllBuffered, MyViewId, other.gameObject.GetComponent<PlayerViewId>().ViewId,10);
        }
        
        Debug.Log(other.gameObject.name);
    }

   [PunRPC]
   private void BulletDestory()
    {
        
    
       Destroy(BulletObj);
    }

    [PunRPC]
    private void BulletHit(int From_id, int To_id,int Damage)
    {

        if(From_id == MyViewId)
        {
            GameManager.instance.KillLog(From_id, To_id, Damage, 1);
        }
        else if(To_id == MyViewId)
        {
            GameManager.instance.KillLog(From_id, To_id, Damage, 2);
        }
        else
        {
            GameManager.instance.KillLog(From_id, To_id, Damage, 3);
        }
        Destroy(BulletObj);
    }
    
}
