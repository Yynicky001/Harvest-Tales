using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerState : MonoBehaviour
{
    public GameObject playerbody;
    public int timeinterval;
    public float currentHealth;
    public float maxHealth;
    public RespawnLocation registeredRespawnLocation;
    private bool isPlayerDead;
    public float Temporaryhealth;
    public event Action OnRespawnRegistered;
    public static PlayerState Instance { get; set; }

    [Header("Regeneration Settings")]
    [Tooltip("Delay in seconds before regeneration starts after taking damage")]
    public float regenDelay = 3f;

    [Tooltip("Amount of health restored per second during regeneration")]
    public float regenRate = 2f;

    [Header("Regeneration Settings")]


    [Tooltip("Interval between each regeneration tick")]
    public float regenInterval = 0.5f;
    private float lastDamageTime;
    private Coroutine regenCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // 移除原有回血逻辑
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        lastDamageTime = Time.time;

        // 停止当前的回血协程
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }

        if (currentHealth <= 0 && !isPlayerDead)
        {
            PlayerDead();
        }
        else
        {
            // 只有受伤但未死亡时才开始回血协程
            if (currentHealth > 0 && currentHealth < maxHealth)
            {
                regenCoroutine = StartCoroutine(RegenerateHealth());
            }
        }
    }

    public void PlayerDead()
    {
        isPlayerDead = true;
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    public IEnumerator RespawnCoroutine()
    {
        playerbody.GetComponent<PlayerMovement>().enabled = false;
        playerbody.GetComponent<MouseMovement>().enabled = false;

        if (registeredRespawnLocation != null)
        {
            Vector3 position = registeredRespawnLocation.transform.position;
            position.y += 5f;
            position.z += 5f;
            playerbody.transform.position = position;
            currentHealth = maxHealth;
        }

        yield return new WaitForSeconds(0.2f);
        isPlayerDead = false;
        playerbody.GetComponent<PlayerMovement>().enabled = true;
        playerbody.GetComponent<MouseMovement>().enabled = true;
    }

    internal void SetRegisteredLocation(RespawnLocation respawnLocation)
    {
        registeredRespawnLocation = respawnLocation;
        OnRespawnRegistered?.Invoke();
    }

    // 改进的回血方法 - 使用协程实现平滑回血
    private IEnumerator RegenerateHealth()
    {
        // 等待回血延迟时间
        yield return new WaitForSeconds(regenDelay);

        // 计算每次回血量（整数）
        int regenAmount = Mathf.Max(1, Mathf.FloorToInt(regenRate * regenInterval));

        // 计算需要多少次回血才能达到最大生命值
        int healthToRegen = Mathf.CeilToInt(maxHealth - currentHealth);
        int regenCycles = Mathf.CeilToInt((float)healthToRegen / regenAmount);

        // 持续回血直到达到最大生命值
        for (int i = 0; i < regenCycles && !isPlayerDead; i++)
        {
            // 最后一次可能不需要回满，防止超过最大生命值
            if (i == regenCycles - 1)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += regenAmount;
            }

            yield return new WaitForSeconds(regenInterval);
        }
    }

    public static implicit operator PlayerState(int v)
    {
        throw new NotImplementedException();
    }
}