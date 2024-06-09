using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class GunScript : MonoBehaviour
{
    //Gun stats
    public float damage;
    public int kills;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public float ScopeSpread;
    private float SecretSpread;
    public float ScopeSpeed;
    private float SecretSpeed;

    public string gunType;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public GameObject fpsCam;
    public Camera actualCamera;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;


    //Graphics
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public TextMeshProUGUI text;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource gunShoot;
    //[SerializeField] VoiceLines VoiceLines;
    //[SerializeField] string character;
    public float t;
    public bool check = false;

    private void Awake()
    {
        SecretSpread = spread;
        SecretSpeed = GetComponent<PlayerMovement>().speed;
        //VoiceLines = GameObject.Find(character+"Lines").GetComponent<VoiceLines>();

        bulletsLeft = magazineSize;
        readyToShoot = true;

    }
    private void Update()
    {            
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Mouse1)) {
            animator.SetBool("Scope", true);
            spread = ScopeSpread;
            GetComponent<PlayerMovement>().speed = ScopeSpeed;
            if(gunType == "Sniper" && actualCamera) {
                actualCamera.fieldOfView = Mathf.Lerp(actualCamera.fieldOfView, 30, t);
                t += 0.5f * Time.deltaTime;
            }
        }   
        else if ((!Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Mouse1)) && animator.GetBool("Scope") == true) {
            animator.SetBool("Scope", false);
            spread = SecretSpread;
            GetComponent<PlayerMovement>().speed = SecretSpeed;
            actualCamera.fieldOfView = 60;
            
            t = 0;
        }
        MyInput();

        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);   
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        if (Input.GetKeyDown(KeyCode.Mouse0) && bulletsLeft == 0 && !reloading) Reload();
        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 ){
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        gunShoot.Play();
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);
        if(bulletsShot <= 1) {
            animator.SetTrigger("Fire");
        }
        
        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
        {
            Debug.Log(rayHit.transform.name);

            if (rayHit.transform.CompareTag("Enemy")) {            

                //rayHit.transform.gameObject.GetComponent<Enemy>().sphere.material = rayHit.transform.gameObject.GetComponent<Enemy>().hurtMaterial;
                //rayHit.transform.gameObject.GetComponent<Enemy>().Invoke("HurtReturn", 0.1f);
                rayHit.transform.gameObject.GetComponent<Enemy>().TakeDamage(damage);          
                   check = true;
                //VoiceLines.StartCoroutine("RandomKill");
            }

        }


        //Graphics
        
        //Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        muzzleFlash.Play();
        if(bulletsShot <= 1) {
            bulletsLeft--;
        }
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if(bulletsShot > 0 && bulletsLeft > 0) {
            Invoke("ShootingWithTime", timeBetweenShots);
        }

        if(rayHit.transform != null) {
            if(!rayHit.transform.CompareTag("Enemy") && rayHit.transform != null && !rayHit.transform.CompareTag("Player")) {
            GameObject impactGameObject = Instantiate(impactEffect, rayHit.point, Quaternion.LookRotation(rayHit.normal));
            Destroy(impactGameObject, 1f);
            }
        }

    }
    private void ResetShot()
    {
        readyToShoot = true;
        animator.ResetTrigger("Fire");
    }
    private void Reload()
    {
        animator.SetTrigger("Reload");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        animator.ResetTrigger("Reload");
        bulletsLeft = magazineSize;
        reloading = false;
    }
    void ShootingWithTime() {
        Shoot();
    }

}