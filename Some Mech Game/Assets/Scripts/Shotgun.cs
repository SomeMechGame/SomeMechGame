using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour{

    public int numPellets = 8;
    public float dmgPerPellet = 10;
    public float fireRate = 0.25f;
    private float nextFire;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    private LineRenderer laserLine;
    public Camera fpsCam;
    private AudioSource gunAudio;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);   

    public GameObject cannonPoint;
    private void Start() {
        laserLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
    }
    void Update(){
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            StartCoroutine (ShotEffect());
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;
            laserLine.SetPosition (0, cannonPoint.transform.position);
            int layerMask = 1 << 7;
            layerMask = ~layerMask;

            if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange, layerMask)){
                //Debug.Log(hit.transform.gameObject.name);
                laserLine.SetPosition (1, hit.point);
                HP health = hit.collider.GetComponent<HP>();
                if (health != null){
                    health.Damage(dmgPerPellet);
                }

                if (hit.rigidbody != null){
                    hit.rigidbody.AddForce (-hit.normal * hitForce);
                }
            }else{
                laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }

    private IEnumerator ShotEffect(){
        gunAudio.Play ();

        // Turn on our line renderer
        laserLine.enabled = true;

        //Wait for .07 seconds
        yield return shotDuration;

        // Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }
}
