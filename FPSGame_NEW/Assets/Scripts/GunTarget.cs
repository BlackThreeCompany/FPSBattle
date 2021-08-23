using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTarget : MonoBehaviour
{
    public GameObject Cam;
    RaycastHit hit1;
    RaycastHit hit2;
    public GameObject HitPoint;

    public GameObject GunHole;

    public Vector3 Gundir;
    public float GunHitDist;
    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit1))
        {

            HitPoint.SetActive(true);


            Debug.DrawRay(Cam.transform.position, Cam.transform.forward * hit1.distance, Color.green);
            HitPoint.transform.position = hit1.point;

            Gundir = (hit1.point - GunHole.transform.position).normalized;
            GunHitDist = Vector3.Distance(GunHole.transform.position, HitPoint.transform.position);

            Debug.DrawRay(GunHole.transform.position, Gundir * GunHitDist, Color.red);

            //if(Physics.Raycast(GunHole.transform.position,))
        }
        else
        {
            Debug.DrawRay(Cam.transform.position, Cam.transform.forward * 1000f, Color.green);
            HitPoint.SetActive(false);
        }
        


    }
}
