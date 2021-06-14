using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    [SerializeField]private List<EnemyBase> enemiesToActivatedOnTrigger;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var enemy in enemiesToActivatedOnTrigger)
            {
                enemy.Activated = true;
            }
        }
    }
}
