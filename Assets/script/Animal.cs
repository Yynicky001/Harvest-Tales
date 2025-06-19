using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;
    [SerializeField] float currentHealth;
    [SerializeField] int maxHealth;
    public bool isDead;
    private Animator animator;
   

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerbody.transform.position, transform.position);
        if (distance < 10f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }
    public void TakeDamage(float damage)
    {
        if (isDead == false)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                animator.SetTrigger("DIE");
                isDead = true;
            }
            else
            {
                animator.SetTrigger("HURT");
            }

        }
    }
    public delegate void OnDestroyedEventHandler();
    public event OnDestroyedEventHandler OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
