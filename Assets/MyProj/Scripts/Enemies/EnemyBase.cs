using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
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
    
    private Animator _anim;
    private Rigidbody _rb;
    private NavMeshAgent _agent;
    private Transform _target;
    
    private bool _alive;
    private bool _attacking;

    private int _health;

    private Vector3 _dir;
    
    #endregion
    
    #region Pistol

    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject MuzzlePrefab;

    public bool EnemiesFocussingOn =false;
    
    private float _timeBetweenShots = 0.3F;
    
    #endregion

    public bool Activated = false;
    
    private void Awake()
    {
        _anim = this.GetComponent<Animator>();
        _rb = this.GetComponent<Rigidbody>();
        _agent = this.GetComponent<NavMeshAgent>();

        _target = GameObject.FindWithTag("Player").transform;

        _agent.updateRotation = false;
        _agent.stoppingDistance = attackDistance;
        
        _anim.SetFloat("InputX", -1);
        _anim.SetBool("Walking", true);
        _anim.SetFloat("InputX", 1F);
        
        _health = startingHealth;

        _alive = true;

        if (!isMeleeEnemy)
        {
            // _agent.stoppingDistance = 
        }
        

    }

    private void Update()
    {
        if (Activated)
        {
            var distToPlayer = Vector3.Distance(this.transform.position, _target.transform.position);

            _dir = _target.position - _rb.position;
            _dir.Normalize();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dir),
                turnSpeed * Time.deltaTime);

            _timeBetweenShots -= Time.deltaTime;

            if (_alive && distToPlayer > attackDistance)
            {
                _agent.SetDestination(_target.position);

                _anim.SetBool("Walking", true);
            }
            else
            {
                _anim.SetBool("Walking", false);
            }

            if (distToPlayer < attackDistance && !_attacking)
            {
                if (isMeleeEnemy)
                    StartCoroutine(MeleeAttack());
                if (!isMeleeEnemy && _timeBetweenShots < 0)
                    Shoot();

            }
        }
    }

    private IEnumerator MeleeAttack()
    {
        _attacking = true;
        
        _anim.SetTrigger("Attack");
        
        Collider[] hitColliders = Physics.OverlapSphere(meleeAttackCenter.position, meleeHitRadius);
        
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                hitCollider.GetComponent<PlayerHealth>().TakeDamage(4);
                break;
            }
        }
        
        yield return new WaitForSeconds(1F);
        
        _attacking = false;
        
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

        var gameManager = GameObject.FindWithTag("Player").GetComponent<GameManager>();
        gameManager.AddKill();
        gameManager.Enemies.Remove(this);

        Destroy(this.GetComponent<CapsuleCollider>());
        _agent.isStopped = true;
        Destroy(_agent);
        _anim.SetBool("Dead", true);
        Destroy(this);
    }

    private void OnDrawGizmosSelected()
    {
        if (meleeAttackCenter != null)
        {
            Gizmos.DrawWireSphere(meleeAttackCenter.position, meleeHitRadius);
        }
    }
}
