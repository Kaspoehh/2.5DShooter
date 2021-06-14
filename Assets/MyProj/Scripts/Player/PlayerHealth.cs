using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField]private Slider healthSlider;

    [Header("Stats")] 
    [SerializeField] private int startingHealth;
    [SerializeField] private float currentHealth;

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateUI();
        CheckHealth();
    }

    private void UpdateUI()
    {
        healthSlider.value = currentHealth;
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("You dead bruh");
    }
}
