using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GunController : MonoBehaviourPunCallbacks
{

    //public LayerMask IgnoreMe;

    public Player_Pun playerPun;

    public GameObject Cam;
    RaycastHit hit1;
    RaycastHit hit2;
    public GameObject HitPoint;

    public GameObject[] GunHole;

    public GameObject AR1;
    public GameObject AR2;
    public GameObject ShotGun1;
    public GameObject Sniper1;
    public GameObject HandGun;
    public GameObject Grenade;
    public GameObject SmokeGrenade;

    public GameObject ThrowGrenade;
    public GameObject ThrowSmokeGrenade;


    public Vector3 Gundir;

    public float GunHitDist;

    public GameObject Bullet;

    public int CurrentHand; // Hand : 0      AR : 1    AR : 2    Pistol : 3       Grenade : 4        SmokeGrenade : 5       ShotGun : 6         Sniper : 7

    public float ShootDelay = 0.1f;
    public float ShootDebugTime = 1f;

    public PhotonView pv;
    Transform tr;
    Animator sniperAni;

    bool isSingle;

    bool isLift1 = false; //주무기 첫번째 슬롯 들고있을때(?)
    bool isLift2 = false; //주무기 두번째 슬롯 들고있을때(?)
    bool isLift3 = false; //권총 슬롯 들고있을때(?)

    //샷건
    public int pelletCount;
    public float spreadAngle;
    public float pelletFireVel;
    List<Quaternion> pellets;

    private void Awake()
    {
        pellets = new List<Quaternion>(pelletCount);
        for (int i = 0; i < pelletCount; i++)
        {
            pellets.Add(Quaternion.Euler(Vector3.zero));
        }

        if (pv.IsMine)
        {
            Cam = Camera.main.gameObject;
            HitPoint = GameObject.Find("hitpoint");
        }

        //
        tr = GetComponent<Transform>();
        sniperAni = Sniper1.GetComponent<Animator>();

        

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
        Aim();
    }
    void guntarget()
    {
        if (CurrentHand == 0 || CurrentHand == 4 || CurrentHand == 5)
        {
            return;
        }
       

        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit1))
        {

            HitPoint.SetActive(true);
            Debug.Log(hit1.transform.gameObject);
            if(hit1.transform.gameObject.layer == 12 || hit1.transform.gameObject.layer == 17)
            {
                Debug.DrawRay(Cam.transform.position, Cam.transform.forward * 1000f, Color.green);
                HitPoint.SetActive(false);

                Gundir = Cam.transform.forward;

                return;
            }

            Debug.DrawRay(Cam.transform.position, Cam.transform.forward * hit1.distance, Color.green);
            HitPoint.transform.position = hit1.point;
            Gundir = (hit1.point - GunHole[CurrentHand].transform.position).normalized;
            GunHitDist = Vector3.Distance(GunHole[CurrentHand].transform.position, HitPoint.transform.position);

            Debug.DrawRay(GunHole[CurrentHand].transform.position, Gundir * GunHitDist, Color.red);

            //if(Physics.Raycast(GunHole.transform.position,))
        }
        else
        {
            Debug.DrawRay(Cam.transform.position, Cam.transform.forward * 1000f, Color.green);
            HitPoint.SetActive(false);

            Gundir = Cam.transform.forward;
        }

    }

    void Aim()
    {
        if(Input.GetMouseButton(1))
        {
            if (isLift1)
            {
                if (CurrentHand == 7)//스나이퍼
                {
                    sniperAni.SetBool("Aim", true);
                    GameManager.instance.ScopeAimEnable();
                    
                }
            }
            else if (isLift2)
            {

            }
            else if (isLift3)
            {

            }
        }
        else
        {
            sniperAni.SetBool("Aim", false);
            GameManager.instance.ScopeAimDisable();
        }
        
    }

    void Shoot()
    {
        if (CurrentHand == 4 || CurrentHand == 5)
        {
            if (Input.GetMouseButton(0) && !ActionController.instance.throwGrenade)
            {
                if (CurrentHand == 4 && WeaponManager.instance.isCanThrowGrenade)
                {
                    ActionController.instance.throwGrenade = true;
                    SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.ThrowGrenade, 0, SoundManager.instance.sfxVolum);
                    GameObject CloneGrenade = ThrowGrenade;
                    //CloneGrenade = Instantiate(ThrowGrenade, GunHole[GunHoleNum].transform.position, GunHole[GunHoleNum].transform.rotation);
                    CloneGrenade = PhotonNetwork.Instantiate("Grenade", GunHole[CurrentHand].transform.position, GunHole[CurrentHand].transform.rotation);
                    CloneGrenade.layer = 7;
                    CloneGrenade.GetComponent<Grenade>().isBoom = true;
                    WeaponManager.instance.GrenadeCnt--;
                    Inventory.instnace.DeleteGrenade();
                    if (WeaponManager.instance.GrenadeCnt == 0)
                    {
                        TurnOff();
                        return;
                    }

                }
                else if (CurrentHand == 5 && WeaponManager.instance.isCanThrowSmokeGrenade)
                {
                    ActionController.instance.throwGrenade = true;
                    SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.ThrowGrenade, 0, SoundManager.instance.sfxVolum);
                    GameObject CloneGrenade = ThrowSmokeGrenade;
                    //CloneGrenade = Instantiate(ThrowSmokeGrenade, GunHole[GunHoleNum].transform.position, GunHole[GunHoleNum].transform.rotation);
                    CloneGrenade = PhotonNetwork.Instantiate("Smoke Grenade", GunHole[CurrentHand].transform.position, GunHole[CurrentHand].transform.rotation);
                    CloneGrenade.layer = 7;
                    CloneGrenade.GetComponent<SmokeGrenade>().isBoom = true;
                    WeaponManager.instance.Smoke_GrenadeCnt--;
                    Inventory.instnace.DeleteSmokeGrenade();
                    if (WeaponManager.instance.Smoke_GrenadeCnt == 0)
                    {
                        TurnOff();
                        return;
                    }
                }

            }
        }

        else
        {
            if (!isSingle)
            {
                if (Input.GetMouseButton(0) && CurrentHand != 0)
                {
                    
                    if (ShootDebugTime >= WeaponManager.instance.FireSpeed && WeaponManager.instance.isCanFireAR)
                    {

                        if (isLift1)
                        {
                            if (WeaponManager.instance.CurrentAmmo > 0)
                            {
                                ShootDebugTime = 0;
                                
                                if(CurrentHand == 6)
                                {
                                    ShotGunFire();
                                }
                                else if (CurrentHand == 7)
                                {
                                    PhotonNetwork.Instantiate("Bullet", GunHole[CurrentHand].transform.position, Quaternion.LookRotation(Gundir));
                                }
                                else
                                {
                                    PhotonNetwork.Instantiate("Bullet", GunHole[CurrentHand].transform.position, Quaternion.LookRotation(Gundir + new Vector3(Random.Range(-WeaponManager.instance.SpreadBullet.x, WeaponManager.instance.SpreadBullet.x),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.y, WeaponManager.instance.SpreadBullet.y),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.z, WeaponManager.instance.SpreadBullet.z))));
                                }
                                Recoil.instance.Fire();
                                WeaponManager.instance.CurrentAmmo--;
                            }

                        }
                        else if (isLift2)
                        {
                            if (WeaponManager.instance.CurrentAmmo2 > 0)
                            {
                                ShootDebugTime = 0;

                                if(CurrentHand == 6)
                                {
                                    ShotGunFire();
                                }
                                else if(CurrentHand == 7)
                                {
                                    PhotonNetwork.Instantiate("Bullet", GunHole[CurrentHand].transform.position, Quaternion.LookRotation(Gundir));
                                }
                                else
                                {
                                    PhotonNetwork.Instantiate("Bullet", GunHole[CurrentHand].transform.position, Quaternion.LookRotation(Gundir + new Vector3(Random.Range(-WeaponManager.instance.SpreadBullet.x, WeaponManager.instance.SpreadBullet.x),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.y, WeaponManager.instance.SpreadBullet.y),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.z, WeaponManager.instance.SpreadBullet.z))));
                                }
                                Recoil.instance.Fire();
                                WeaponManager.instance.CurrentAmmo2--;
                            }
                        }
                        else if (isLift3)
                        {
                            if (WeaponManager.instance.CurrentAmmo3 > 0)
                            {
                                ShootDebugTime = 0;
                                PhotonNetwork.Instantiate("Bullet", GunHole[CurrentHand].transform.position, Quaternion.LookRotation(Gundir + new Vector3(Random.Range(-WeaponManager.instance.SpreadBullet.x, WeaponManager.instance.SpreadBullet.x),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.y, WeaponManager.instance.SpreadBullet.y),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.z, WeaponManager.instance.SpreadBullet.z))));
                                Recoil.instance.Fire();
                                WeaponManager.instance.CurrentAmmo3--;
                            }
                        }
                        //pv.RPC("PlayerShoot", RpcTarget.All, GunHole.transform.position, Quaternion.LookRotation(Gundir));

                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (ShootDebugTime >= WeaponManager.instance.FireSpeed && (WeaponManager.instance.isCanFirePistol || WeaponManager.instance.isCanFireAR) )
                    {
                        if (isLift1)
                        {
                            if (WeaponManager.instance.CurrentAmmo > 0)
                            {
                                ShootDebugTime = 0;
                                if (CurrentHand == 6)
                                {
                                    ShotGunFire();
                                }
                                else if (CurrentHand == 7)
                                {
                                    PhotonNetwork.Instantiate("Bullet", GunHole[CurrentHand].transform.position, Quaternion.LookRotation(Gundir));
                                }
                                else
                                {
                                    PhotonNetwork.Instantiate("Bullet", GunHole[CurrentHand].transform.position, Quaternion.LookRotation(Gundir + new Vector3(Random.Range(-WeaponManager.instance.SpreadBullet.x, WeaponManager.instance.SpreadBullet.x),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.y, WeaponManager.instance.SpreadBullet.y),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.z, WeaponManager.instance.SpreadBullet.z))));
                                }
                                Debug.Log("1");
                                Recoil.instance.Fire();
                                WeaponManager.instance.CurrentAmmo--;
                            }

                        }
                        else if (isLift2)
                        {
                            if (WeaponManager.instance.CurrentAmmo2 > 0)
                            {
                                ShootDebugTime = 0;
                                if (CurrentHand == 6)
                                {
                                    ShotGunFire();
                                }
                                else if (CurrentHand == 7)
                                {
                                    PhotonNetwork.Instantiate("Bullet", GunHole[CurrentHand].transform.position, Quaternion.LookRotation(Gundir));
                                }
                                else PhotonNetwork.Instantiate("Bullet", GunHole[CurrentHand].transform.position, Quaternion.LookRotation(Gundir + new Vector3(Random.Range(-WeaponManager.instance.SpreadBullet.x, WeaponManager.instance.SpreadBullet.x),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.y, WeaponManager.instance.SpreadBullet.y),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.z, WeaponManager.instance.SpreadBullet.z))));
                                Debug.Log("1");
                                Recoil.instance.Fire();
                                WeaponManager.instance.CurrentAmmo2--;
                            }
                        }
                        else if (isLift3)
                        {
                            if (WeaponManager.instance.CurrentAmmo3 > 0)
                            {
                                ShootDebugTime = 0;
                                PhotonNetwork.Instantiate("Bullet", GunHole[CurrentHand].transform.position, Quaternion.LookRotation(Gundir + new Vector3(Random.Range(-WeaponManager.instance.SpreadBullet.x, WeaponManager.instance.SpreadBullet.x),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.y, WeaponManager.instance.SpreadBullet.y),
                                                                                                                                                        Random.Range(-WeaponManager.instance.SpreadBullet.z, WeaponManager.instance.SpreadBullet.z))));
                                Recoil.instance.Fire();
                                WeaponManager.instance.CurrentAmmo3--;
                            }
                        }
                        //Debug.Log(CurrentHand);
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            TurnOff();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && WeaponManager.instance.WeaponSloat.item != null)
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
            ShotGun1.SetActive(false);
            Sniper1.SetActive(false);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            //
            isLift1 = false;
            isLift2 = false;
            isLift3 = true;
            Inventory.instnace.isLift1 = false;
            Inventory.instnace.isLift2 = false;
            Inventory.instnace.isLift3 = true;
            //
            CurrentHand = 3;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 10;
            WeaponManager.instance.FireSpeed = 0f;
            WeaponManager.instance.Ammo = 8;
            //
            StatManager.instance.PlayerMoveSpeed = 5f;
            //
            isSingle = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (WeaponManager.instance.isCanThrowGrenade)
            {
                HandGun.SetActive(false);
                AR1.SetActive(false);
                AR2.SetActive(false);
                ShotGun1.SetActive(false);
                Sniper1.SetActive(false);
                Grenade.SetActive(true);
                SmokeGrenade.SetActive(false);
                //
                isLift1 = false;
                isLift2 = false;
                isLift3 = false;
                Inventory.instnace.isLift1 = false;
                Inventory.instnace.isLift2 = false;
                Inventory.instnace.isLift3 = false;
                //
                CurrentHand = 4;
                Inventory.instnace.CurrentHand = CurrentHand;
                //
                WeaponManager.instance.damage = 10;
                WeaponManager.instance.FireSpeed = 0f;
                WeaponManager.instance.Ammo = 0;
                //
                StatManager.instance.PlayerMoveSpeed = 5f;
                //
                isSingle = true;

            }
            else if (WeaponManager.instance.isCanThrowSmokeGrenade)
            {
                HandGun.SetActive(false);
                AR1.SetActive(false);
                AR2.SetActive(false);
                ShotGun1.SetActive(false);
                Sniper1.SetActive(false);
                Grenade.SetActive(false);
                SmokeGrenade.SetActive(true);
                //
                isLift1 = false;
                isLift2 = false;
                isLift3 = false;
                Inventory.instnace.isLift1 = false;
                Inventory.instnace.isLift2 = false;
                Inventory.instnace.isLift3 = false;
                //
                CurrentHand = 5;
                Inventory.instnace.CurrentHand = CurrentHand;
                //
                WeaponManager.instance.damage = 10;
                WeaponManager.instance.FireSpeed = 0f;
                WeaponManager.instance.Ammo = 0;
                //
                StatManager.instance.PlayerMoveSpeed = 5f;
                //
                isSingle = true;
            }

        }
    }

    public void TurnOff()
    {
        HandGun.SetActive(false);
        AR1.SetActive(false);
        AR2.SetActive(false);
        ShotGun1.SetActive(false);
        Sniper1.SetActive(false);
        Grenade.SetActive(false);
        SmokeGrenade.SetActive(false);
        //
        isLift1 = false;
        isLift2 = false;
        isLift3 = false;
        Inventory.instnace.isLift1 = false;
        Inventory.instnace.isLift2 = false;
        Inventory.instnace.isLift3 = false;
        //
        CurrentHand = 0;
        Inventory.instnace.CurrentHand = CurrentHand;
        //
        WeaponManager.instance.damage = 0;
        WeaponManager.instance.FireSpeed = 0;
        WeaponManager.instance.Ammo = 0;
        //
        StatManager.instance.PlayerMoveSpeed = 6f;
        //
        isSingle = false;


    }

    public void WeaponChnage()
    {
        if (WeaponManager.instance.WeaponSloat.item.itemName == "AK-47")
        {
            HandGun.SetActive(false);
            AR1.SetActive(true);
            AR2.SetActive(false);
            ShotGun1.SetActive(false);
            Sniper1.SetActive(false);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            //
            isLift1 = true;
            isLift2 = false;
            isLift3 = false;
            Inventory.instnace.isLift1 = true;
            Inventory.instnace.isLift2 = false;
            Inventory.instnace.isLift3 = false;
            //
            CurrentHand = 1;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 25;
            WeaponManager.instance.FireSpeed = 0.1f;
            WeaponManager.instance.Ammo = 30;
            //
            StatManager.instance.PlayerMoveSpeed = 4f;
            //
            isSingle = false;
        }
        else if (WeaponManager.instance.WeaponSloat.item.itemName == "AKM")
        {
            HandGun.SetActive(false);
            AR1.SetActive(false);
            AR2.SetActive(true);
            ShotGun1.SetActive(false);
            Sniper1.SetActive(false);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            //
            isLift1 = true;
            isLift2 = false;
            isLift3 = false;
            Inventory.instnace.isLift1 = true;
            Inventory.instnace.isLift2 = false;
            Inventory.instnace.isLift3 = false;
            //
            CurrentHand = 2;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 35;
            WeaponManager.instance.FireSpeed = 0.35f;
            WeaponManager.instance.Ammo = 25;
            //
            StatManager.instance.PlayerMoveSpeed = 4f;
            //
            isSingle = false;
        }
        else if (WeaponManager.instance.WeaponSloat.item.itemName == "ShotGun")
        {
            HandGun.SetActive(false);
            AR1.SetActive(false);
            AR2.SetActive(false);
            ShotGun1.SetActive(true);
            Sniper1.SetActive(false);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            //
            isLift1 = true;
            isLift2 = false;
            isLift3 = false;
            Inventory.instnace.isLift1 = true;
            Inventory.instnace.isLift2 = false;
            Inventory.instnace.isLift3 = false;
            //
            CurrentHand = 6;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 4;
            WeaponManager.instance.FireSpeed = 1f;
            WeaponManager.instance.Ammo = 8;
            //
            StatManager.instance.PlayerMoveSpeed = 4f;
            //
            isSingle = true;
        }
        else if (WeaponManager.instance.WeaponSloat.item.itemName == "Sniper")
        {
            HandGun.SetActive(false);
            AR1.SetActive(false);
            AR2.SetActive(false);
            ShotGun1.SetActive(false);
            Sniper1.SetActive(true);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            //
            isLift1 = true;
            isLift2 = false;
            isLift3 = false;
            Inventory.instnace.isLift1 = true;
            Inventory.instnace.isLift2 = false;
            Inventory.instnace.isLift3 = false;
            //
            CurrentHand = 7;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 50;
            WeaponManager.instance.FireSpeed = 1.5f;
            WeaponManager.instance.Ammo = 5;
            //
            StatManager.instance.PlayerMoveSpeed = 3f;
            //
            isSingle = true;
        }
    }


    public void ChangeWeapon2()
    {
        if (WeaponManager.instance.WeaponSloat2.item.itemName == "AK-47")
        {

            HandGun.SetActive(false);
            AR1.SetActive(true);
            AR2.SetActive(false);
            ShotGun1.SetActive(false);
            Sniper1.SetActive(false);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            //
            isLift1 = false;
            isLift2 = true;
            isLift3 = false;
            Inventory.instnace.isLift1 = false;
            Inventory.instnace.isLift2 = true;
            Inventory.instnace.isLift3 = false;
            //
            CurrentHand = 1;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 25;
            WeaponManager.instance.FireSpeed = 0.1f;
            WeaponManager.instance.Ammo = 30;
            //
            StatManager.instance.PlayerMoveSpeed = 4f;
            //
            isSingle = false;
        }
        else if (WeaponManager.instance.WeaponSloat2.item.itemName == "AKM")
        {
            HandGun.SetActive(false);
            AR1.SetActive(false);
            AR2.SetActive(true);
            ShotGun1.SetActive(false);
            Sniper1.SetActive(false);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            //
            isLift1 = false;
            isLift2 = true;
            isLift3 = false;
            Inventory.instnace.isLift1 = false;
            Inventory.instnace.isLift2 = true;
            Inventory.instnace.isLift3 = false;
            //
            CurrentHand = 2;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 35;
            WeaponManager.instance.FireSpeed = 0.35f;
            WeaponManager.instance.Ammo = 25;
            //
            StatManager.instance.PlayerMoveSpeed = 4f;
            //
            isSingle = false;
        }
        else if (WeaponManager.instance.WeaponSloat2.item.itemName == "ShotGun")
        {
            HandGun.SetActive(false);
            AR1.SetActive(false);
            AR2.SetActive(false);
            ShotGun1.SetActive(true);
            Sniper1.SetActive(false);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            //
            isLift1 = false;
            isLift2 = true;
            isLift3 = false;
            Inventory.instnace.isLift1 = false;
            Inventory.instnace.isLift2 = true;
            Inventory.instnace.isLift3 = false;
            //
            CurrentHand = 6;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 4;
            WeaponManager.instance.FireSpeed = 1f;
            WeaponManager.instance.Ammo = 8;
            //
            StatManager.instance.PlayerMoveSpeed = 4f;
            //
            isSingle = true;
        }
        else if (WeaponManager.instance.WeaponSloat2.item.itemName == "Sniper")
        {
            HandGun.SetActive(false);
            AR1.SetActive(false);
            AR2.SetActive(false);
            ShotGun1.SetActive(false);
            Sniper1.SetActive(true);
            Grenade.SetActive(false);
            SmokeGrenade.SetActive(false);
            //
            isLift1 = false;
            isLift2 = true;
            isLift3 = false;
            Inventory.instnace.isLift1 = false;
            Inventory.instnace.isLift2 = true;
            Inventory.instnace.isLift3 = false;
            //
            CurrentHand = 7;
            Inventory.instnace.CurrentHand = CurrentHand;
            //
            WeaponManager.instance.damage = 50;
            WeaponManager.instance.FireSpeed = 1.5f;
            WeaponManager.instance.Ammo = 5;
            //
            StatManager.instance.PlayerMoveSpeed = 3f;
            //
            isSingle = true;
        }

    }

    //void ShotgunFire()
    //{
    //    float[,] spread = new float[100,2];
    //    int idx = 0;
    //    float spdir = -0.02f;
    //    for(int i=0;i< 20; i++)
    //    {
    //        spread[i,0] = Mathf.Abs( spdir + ((0.002f) * i));
    //        spread[i, 1] = Mathf.Sqrt(0.0004f - (spread[i, 0] * spread[i, 0]));
            
    //    }
    //    for(int i = 0; i < 20; i++)
    //    {
    //        PhotonNetwork.Instantiate("Bullet", GunHole[CurrentHand].transform.position, Quaternion.LookRotation(new Vector3(Gundir.x + spread[i,0]- 0.02f, Gundir.y + spread[i, 1] - 0.02f, Gundir.z+ spread[i, 1] - 0.02f)));
            
    //    }
        
    //}

    void ShotGunFire()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            pellets[i] = Random.rotation;
            PhotonNetwork.Instantiate("Bullet 1", GunHole[CurrentHand].transform.position, Quaternion.RotateTowards(transform.rotation, pellets[i], spreadAngle));
            i++;
        }
    }

}
