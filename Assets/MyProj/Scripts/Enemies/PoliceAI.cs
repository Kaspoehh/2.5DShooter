using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class PoliceAI : MonoBehaviour
{
    #region changeable variables

    [SerializeField] private float turnSpeed;
    [SerializeField] private int startingHealth;
    [SerializeField] private Transform meleeAttackCenter;
    [SerializeField] private float meleeHitRadius = 1;
    [SerializeField] private bool isMeleeEnemy;
    [SerializeField] private float attackDistance;
    
    #endregion

    #region Shared variables

    private GameManager _gameManager;
    
    private Animator _anim;
    private Rigidbody _rb;
    private NavMeshAgent _agent;
    [SerializeField]private Transform target;
    private Transform _enemyTarget;
    
    private bool _alive;
    private bool _attacking;

    private int _health;

    private Vector3 _dir;
    
    #endregion
    
    #region Pistol

    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject MuzzlePrefab;
    
    private float _timeBetweenShots = 0.3F;
    
    #endregion

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        
        _anim = this.GetComponent<Animator>();
        _rb = this.GetComponent<Rigidbody>();
        _agent = this.GetComponent<NavMeshAgent>();

        _enemyTarget = PickEnemy().transform;
        
        _agent.updateRotation = false;
        _agent.stoppingDistance = attackDistance;
        
        _anim.SetFloat("InputX", -1);
        _anim.SetBool("Walking", true);
        _anim.SetFloat("InputX", 1F);
        
        
        
        _health = startingHealth;

        _alive = true;

    }

    private void Update()
    {
        _dir = _enemyTarget.position - _rb.position;
        
        _dir.Normalize();
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dir),
            turnSpeed * Time.deltaTime);

        _timeBetweenShots -= Time.deltaTime;

        _agent.SetDestination(target.position);

    }

    private void Shoot()
    {
        _anim.SetTrigger("Shoot");

        if (MuzzlePrefab != null)
        {
            var flash = Instantiate(MuzzlePrefab, muzzlePoint.position, Quaternion.identity);

            flash.transform.parent = muzzlePoint.transform;
            flash.transform.localPosition = new Vector3(0, 0, 0);
            flash.transform.localRotation = Quaternion.Euler(0, 0, 0);

            //Debug.DrawLine(shootPoint.position, shootPoint.TransformDirection(Vector3.forward) * 100, Color.red, 2f);
            
            StartCoroutine(DestroyObject(flash));
        }

        if (Physics.Linecast(shootPoint.position, shootPoint.TransformDirection(Vector3.forward) * 100,out RaycastHit hit))
        {
            if(hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerHealth>().TakeDamage(15);
                // var bld = Instantiate(bloodSplatterPrefab, hit.point, Quaternion.identity);
                // bld.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);;
            }   
                
        }

        _timeBetweenShots = 0.4F;
        
    }
    
    private IEnumerator DestroyObject(GameObject goToDestroy)
    {
        if (!_alive)
        {
            Destroy(goToDestroy);
            yield break;
        }
        
        yield return new WaitForSeconds(0.2F);
        
        Destroy(goToDestroy);
    }
    
    public void TakeDamage(int damageAmount)
    {
        _health -= damageAmount;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _alive = false;
        _rb.isKinematic = true;
        _rb.useGravity = false;
        
        if(_anim.layerCount > 0)
            _anim.SetLayerWeight(1, 0);
        
        if(shootPoint != null)
            Destroy(shootPoint.gameObject);
        
        GameObject.FindWithTag("Player").GetComponent<GameManager>().AddKill();
        
        
        Destroy(this.GetComponent<CapsuleCollider>());
        _agent.isStopped = true;
        Destroy(_agent);
        _anim.SetBool("Dead", true);
        Destroy(this);
    }

    private EnemyBase PickEnemy()
    {
        EnemyBase closestEnemy = null;
        float closestEnemyDistance = 5000;
        
        for (int i = 0; i < _gameManager.Enemies.Count; i++)
        {
            float distance = Vector2.Distance(this.transform.position, _gameManager.Enemies[i].transform.position);
            
            if (distance < closestEnemyDistance)
            {
                closestEnemy = _gameManager.Enemies[i];
            }
            
        }

        if (closestEnemy == null)
        {
            Debug.Log("Not Found an enemy wtf");
        }
        return closestEnemy;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (meleeAttackCenter != null)
        {
            Gizmos.DrawWireSphere(meleeAttackCenter.position, meleeHitRadius);
        }
    }
}
