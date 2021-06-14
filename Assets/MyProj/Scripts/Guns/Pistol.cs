using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform shootPoint1;
    [SerializeField] private Transform shootPoint2;
    [SerializeField] private GameObject MuzzlePrefab;
    [SerializeField] private int maxAmmoInClip = 20;
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private Transform meleeAttackCenter;
    [SerializeField] private float meleeHitRadius = 1;

    private GameManager _gameManager;
    private int _ammoInClip;
    private float _timeBetweenShots = 0.3F;
        
    //[SerializeField] private GameObject bloodSplatterPrefab;
    
    private Animator _anim;

    private void Start()
    {
        _gameManager = this.GetComponent<GameManager>();
        _anim = this.GetComponent<Animator>();
        _ammoInClip = maxAmmoInClip;
        _ammoText.text = "Ammo: " + _ammoInClip + "/∞";
    }

    private void Update()
    {
        _timeBetweenShots -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_timeBetweenShots <= 0)
            {
                Shoot();
                _timeBetweenShots = .3F;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(_timeBetweenShots <= 0)
            {
                MeleeAttack();
                _timeBetweenShots = 1F;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public void Shoot()
    {
        if (_ammoInClip <= 0)
            return;
        
        _anim.SetTrigger("Shoot"); 
        
        var flash = Instantiate(MuzzlePrefab, muzzlePoint.position, Quaternion.identity);
        flash.transform.parent = muzzlePoint.transform;
        flash.transform.localPosition = new Vector3(0, 0, 0);
        flash.transform.localRotation = Quaternion.Euler(0,0,0);
        bool hittedenemy = false;
 
        
        _ammoInClip--;
        _ammoText.text = "Ammo: " + _ammoInClip + "/∞";
        
        StartCoroutine(DestroyObject(flash));
        
        //Debug.DrawLine(shootPoint.position, shootPoint.TransformDirection(Vector3.forward) * 100, Color.red, 2f);
        if (Physics.Linecast(shootPoint.position, shootPoint.TransformDirection(Vector3.forward) * 100,out RaycastHit hit))
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                if (hittedenemy)
                    return;
                hittedenemy = true;
                hit.collider.GetComponent<EnemyBase>().TakeDamage(15);
                // var bld = Instantiate(bloodSplatterPrefab, hit.point, Quaternion.identity);
                // bld.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);;
            }   
                
        }
        
        if (Physics.Linecast(shootPoint1.position, shootPoint1.TransformDirection(Vector3.forward) * 100,out RaycastHit hit1))
        {
            if(hit1.collider.CompareTag("Enemy"))
            {
                
                if (hittedenemy)
                    return;
                
                hittedenemy = true;
                
                hit1.collider.GetComponent<EnemyBase>().TakeDamage(15);
                // var bld = Instantiate(bloodSplatterPrefab, hit.point, Quaternion.identity);
                // bld.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);;
            }   
                
        }
        
        if (Physics.Linecast(shootPoint2.position, shootPoint2.TransformDirection(Vector3.forward) * 100,out RaycastHit hit2))
        {
            if(hit2.collider.CompareTag("Enemy"))
            {
                if (hittedenemy)
                    return;
                
                hittedenemy = true;
                hit2.collider.GetComponent<EnemyBase>().TakeDamage(15);
                // var bld = Instantiate(bloodSplatterPrefab, hit.point, Quaternion.identity);
                // bld.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);;
            }   
                
        }

    }

    public void MeleeAttack()
    {
        _anim.SetTrigger("Attack");
        
        Collider[] hitColliders = Physics.OverlapSphere(meleeAttackCenter.position, meleeHitRadius);
        
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<EnemyBase>().TakeDamage(10);
            }
        }
    }

    public void Reload()
    {
        _ammoInClip = maxAmmoInClip;
        _ammoText.text = "Ammo: " + _ammoInClip + "/∞";
    }

    private IEnumerator DestroyObject(GameObject goToDestroy)
    {
        yield return new WaitForSeconds(0.2F);
        Destroy(goToDestroy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(meleeAttackCenter.position,meleeHitRadius);
    }
}
