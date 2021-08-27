using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GunController : MonoBehaviourPunCallbacks
{

    public LayerMask IgnoreMe;


    public GameObject Cam;
    RaycastHit hit1;
    RaycastHit hit2;
    public GameObject HitPoint;

    public GameObject[] GunHole;

    public GameObject AR1;
    public GameObject AR2;
    public GameObject HandGun;
    public GameObject Grenade;
    public GameObject SmokeGrenade;

    public GameObject ThrowGrenade;
    public GameObject ThrowSmokeGrenade;

    public Vector3 Gundir;
    public float GunHitDist;

    public GameObject Bullet;

    public int CurrentHand; // Hand : 0      AR : 1    AR : 2    Pistol : 3       Grenade : 4        SmokeGrenade : 5

    public float ShootDelay = 0.1f;
    public float ShootDebugTime = 1f;

    public PhotonView pv;
    Transform tr;


    bool isSingle;

    public int GunHoleNum=0;

    private void Awake()
    {
        if (pv.IsMine)
        {
            Cam = Camera.main.gameObject;
            HitPoint = GameObject.Find("hitpoint");
        }

        tr = GetComponent<Transform>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GunShootUpdate()
    {
        GunChange();
        guntarget();
        Shoot();
    }
    void guntarget()
    {
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit1,IgnoreMe))
        {

            HitPoint.SetActive(true);


            Debug.DrawRay(Cam.transform.position, Cam.transform.forward * hit1.distance, Color.green);
            HitPoint.transform.position = hit1.point;

            Gundir = (hit1.point - GunHole[GunHoleNum].transform.position).normalized;
            GunHitDist = Vector3.Distance(GunHole[GunHoleNum].transform.position, HitPoint.transform.position);

            Debug.DrawRay(GunHole[GunHoleNum].transform.position, Gundir * GunHitDist, Color.red);

            //if(Physics.Raycast(GunHole.transform.position,))
        }
        else
        {
            Debug.DrawRay(Cam.transform.position, Cam.transform.forward * 1000f, Color.green);
            HitPoint.SetActive(false);

            Gundir = Cam.transform.forward;
        }
    }

    void Shoot()
    {
        if(CurrentHand == 4 || CurrentHand == 5)
        {
            if (Input.GetMouseButton(0) && !ActionController.instance.throwGrenade)
            {
                if(CurrentHand == 4)
                {
                    ActionController.instance.throwGrenade = true;
                    SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.ThrowGrenade, 0, SoundManager.instance.sfxVolum);
                    GameObject CloneGrenade = ThrowGrenade;
                    //CloneGrenade = Instantiate(Smokegrenade, tr.position, tr.rotation);
                    CloneGrenade = Instantiate(ThrowGrenade, tr.position, tr.rotation);
                    CloneGrenade.GetComponent<Grenade>().isBoom = true;
                    WeaponManager.instance.GrenadeCnt--;
                    Inventory.instnace.DeleteGrenade();
                    if (WeaponManager.instance.GrenadeCnt == 0)
                    {
                        TurnOff();
                        return;
                    }
                    CloneGrenade.layer = 7;
                }
                else if(CurrentHand == 5)
                {
                    ActionController.instance.throwGrenade = true;
                    SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.ThrowGrenade, 0, SoundManager.instance.sfxVolum);
                    GameObject CloneGrenade = ThrowSmokeGrenade;
                    //CloneGrenade = Instantiate(grenade, tr.position, tr.rotation);
                    CloneGrenade = Instantiate(ThrowSmokeGrenade, tr.position, tr.rotation);
                    CloneGrenade.GetComponent<SmokeGrenade>().isBoom = true;
                    WeaponManager.instance.Smoke_GrenadeCnt--;
                    Inventory.instnace.DeleteSmokeGrenade();
                    if (WeaponManager.instance.Smoke_GrenadeCnt == 0)
                    {
                        TurnOff();
                        return;
                    }
                    CloneGrenade.layer = 7;
                }

            }
        }

        else
        {
            if (!isSingle)
            {
                if (Input.GetMouseButton(0) )
                {
                    if (ShootDebugTime >= WeaponManager.instance.FireSpeed && WeaponManager.instance.isCanFireAR)
                    {
                        ShootDebugTime = 0;
                        //Instantiate(Bullet, GunHole.transform.position, Quaternion.LookRotation(Gundir));
                        PhotonNetwork.Instantiate("Bullet", GunHole[GunHoleNum].transform.position, Quaternion.LookRotation(Gundir));
                        //pv.RPC("PlayerShoot", RpcTarget.All, GunHole.transform.position, Quaternion.LookRotation(Gundir));

                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (ShootDebugTime >= WeaponManager.instance.FireSpeed && WeaponManager.instance.isCanFirePistol)
                    {
                        ShootDebugTime = 0;
                        //Instantiate(Bullet, GunHole.transform.position, Quaternion.LookRotation(Gundir));

                        PhotonNetwork.Instantiate("Bullet", GunHole[GunHoleNum].transform.position, Quaternion.LookRotation(Gundir));
                        //pv.RPC("PlayerShoot", RpcTarget.All, GunHole.transform.position, Quaternion.LookRotation(Gundir));

                    }
                }
            }
        }

       
        
        ShootDebugTime += Time.deltaTime;
    }

    [PunRPC]
    private void PlayerShoot(Vector3 Pos, Quaternion Rot)
    {
        Instantiate(Bullet, Pos, Rot);
    }

    void GunChange()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            TurnOff();
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && WeaponManager.instance.WeaponSloat.item != null)
        {
            WeaponChnage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && WeaponManager.instance.WeaponSloat2.item != null)
        {

            ChangeWeapon2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && WeaponManager.instance.PistolSloat.item != null)
        {
            HandGun.SetActive(true);
            AR1.SetActive(false);
            AR2.SetActive(false);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            GunHoleNum = 1;
            //
            CurrentHand = 3;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 10;
            WeaponManager.instance.FireSpeed = 0f;
            //
            StatManager.instance.PlayerMoveSpeed = 5f;
            //
            isSingle = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(WeaponManager.instance.isCanThrowGrenade)
            {
                HandGun.SetActive(false);
                AR1.SetActive(false);
                AR2.SetActive(false);
                Grenade.SetActive(true);
                SmokeGrenade.SetActive(false);
                //
                CurrentHand = 4;
                Inventory.instnace.CurrentHand = CurrentHand;
                //
                WeaponManager.instance.damage = 10;
                WeaponManager.instance.FireSpeed = 0f;
                //
                StatManager.instance.PlayerMoveSpeed = 5f;
                //
                isSingle = true;
            }
            else if(WeaponManager.instance.isCanThrowSmokeGrenade)
            {
                HandGun.SetActive(false);
                AR1.SetActive(false);
                AR2.SetActive(false);
                Grenade.SetActive(false);
                SmokeGrenade.SetActive(true);
                //
                CurrentHand = 5;
                Inventory.instnace.CurrentHand = CurrentHand;
                //
                WeaponManager.instance.damage = 10;
                WeaponManager.instance.FireSpeed = 0f;
                //
                StatManager.instance.PlayerMoveSpeed = 5f;
                //
                isSingle = true;
            }
            
        }
    }

    void TurnOff()
    {
        Grenade.SetActive(false);
        SmokeGrenade.SetActive(false);
        //
        CurrentHand = 0;
        Inventory.instnace.CurrentHand = CurrentHand;
        //
        WeaponManager.instance.damage = 10;
        WeaponManager.instance.FireSpeed = 0f;
        //
        StatManager.instance.PlayerMoveSpeed = 6f;
        //
        isSingle = false;
    }

    public void WeaponChnage()
    {
        if (WeaponManager.instance.WeaponName == "AK-47")
        {
            HandGun.SetActive(false);
            AR1.SetActive(true);
            AR2.SetActive(false);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            GunHoleNum = 0;
            //
            CurrentHand = 1;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 25;
            WeaponManager.instance.FireSpeed = 0.25f;
            //
            StatManager.instance.PlayerMoveSpeed = 4f;
            //
            isSingle = false;
        }
        else if (WeaponManager.instance.WeaponName == "AKM")
        {
            HandGun.SetActive(false);
            AR1.SetActive(false);
            AR2.SetActive(true);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            GunHoleNum = 0;
            //
            CurrentHand = 1;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 35;
            WeaponManager.instance.FireSpeed = 0.35f;
            //
            StatManager.instance.PlayerMoveSpeed = 4f;
            //
            isSingle = false;
        }
    }


    public void ChangeWeapon2()
    {
        if (WeaponManager.instance.WeaponName2 == "AK-47")
        {

            HandGun.SetActive(false);
            AR1.SetActive(true);
            AR2.SetActive(false);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            GunHoleNum = 0;
            //
            CurrentHand = 2;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 25;
            WeaponManager.instance.FireSpeed = 0.25f;
            //
            StatManager.instance.PlayerMoveSpeed = 4f;
            //
            isSingle = false;
        }
        else if (WeaponManager.instance.WeaponName2 == "AKM")
        {
            HandGun.SetActive(false);
            AR1.SetActive(false);
            AR2.SetActive(true);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            GunHoleNum = 0;
            //

            CurrentHand = 2;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 35;
            WeaponManager.instance.FireSpeed = 0.35f;
            //
            StatManager.instance.PlayerMoveSpeed = 4f;
            //
            isSingle = false;
        }

    }

}
