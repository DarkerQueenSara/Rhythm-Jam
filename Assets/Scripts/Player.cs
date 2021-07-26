using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public enum PlayerType
    {
        PLAYER,
        AI
    }

    public PlayerType playerType;

    public int maxHealth = 100;
    [HideInInspector] public int currentHealth;
    [HideInInspector] public int healthToRemove;

    [Header("AI Options")] [Range(0.0f, 1.0f)]
    public float AIMissChance;

    public float AIMissPenalty;

    [Header("Sprites")] public Sprite frontSprite;
    public Sprite backSprite;

    [Header("UI")]  public Image backHealthBar;
    public Image frontHealthBar;
    public float healTime = 2f;

    private float AIMissPenaltyEndTime = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void AddDamage(int damage)
    {
        healthToRemove += damage;
        backHealthBar.color = Color.red;
        //se calhar tenho que usar lerp
        frontHealthBar.fillAmount -= (float) damage / maxHealth;
    }

    public void DealDamage()
    {
        currentHealth -= healthToRemove;
        healthToRemove = 0;
        //se calhar tenho que usar lerp
        backHealthBar.fillAmount = frontHealthBar.fillAmount;
    }

    public void RestoreHealth(int health)
    {
        currentHealth += health;
        backHealthBar.color = Color.yellow;
        //se calhar tenho que usar lerp
        backHealthBar.fillAmount += (float) health / maxHealth;
        Invoke(nameof(FinishHeal), healTime);
    }

    private void FinishHeal()
    {
        //se calhar tenho que usar lerp
        frontHealthBar.fillAmount = backHealthBar.fillAmount;
    }


    public bool AIHit()
    {
        if (playerType == PlayerType.PLAYER) return false;

        float hit = Random.Range(0f, 1f);
        if (hit > AIMissChance && Time.time > AIMissPenaltyEndTime)
        {
            //hit
            return true;
        }
        else
        {
            //miss
            AIMissPenaltyEndTime = Time.time + AIMissPenalty;
            return false;
        }
    }
}