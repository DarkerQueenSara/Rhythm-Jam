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
    public float healthBarDamageAnimationTime;
    public float healthBarHealAnimationTime;
    public float healTime = 2f;
    public float damageAnimationDistance;
    public float damageAnimationTime;

    private float AIMissPenaltyEndTime = 0f;

    private GameObject playerSprite;
    private Vector3 playerSpriteInitialPosition;

    private void Start()
    {
        GameEvents.Instance.RoundPhaseOver += RoundPhaseOver;
        currentHealth = maxHealth;
        playerSprite = transform.Find("Sprite").gameObject;
        playerSpriteInitialPosition = playerSprite.transform.position;
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
        DamageAnimation(damage);
    }

    public void DealDamage()
    {
        currentHealth -= healthToRemove;
        healthToRemove = 0;
        //se calhar tenho que usar lerp
        backHealthBar.fillAmount = frontHealthBar.fillAmount;

        if(currentHealth <= 0) {
            Die();
        }
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

    void RoundPhaseOver(object sender, EventArgs eventArgs) {
        if (RoundController.Instance.GetCurrentOpponent() == this && RoundController.Instance.currentRoundPhase == RoundController.RoundPhase.ATTACK) {
            DealDamage();
        }/* else if(RoundController.Instance.GetCurrentPlayer() == this && RoundController.Instance.currentRoundPhase == RoundController.RoundPhase.COMEBACK) {
            FinishHeal();
        }*/
    }

    void Die() {
        PlayerDiedArgs args = new PlayerDiedArgs();
        args.player = this;

        GameEvents.Instance.OnPlayerDied(args);
    }

    public void SetSprite(Sprite sprite) {
        GetComponentInChildren<Image>().sprite = sprite;
    }

    public void SetSpriteFront() {
        SetSprite(frontSprite);
    }

    public void SetSpriteBack() {
        SetSprite(backSprite);
    }

    void DamageAnimation(float damage) {
        Debug.Log("damage animation");
        DamageAnimationX(damage);
    }

    void DamageAnimationX(float damage) {
        LeanTween.cancel(playerSprite);
        LeanTween.moveX(playerSprite, playerSprite.transform.position.x + damageAnimationDistance * damage/9f, damageAnimationTime/2).setOnComplete(DamageAnimationReset);
    }

    void DamageAnimationReset() {
        LeanTween.move(playerSprite,  playerSpriteInitialPosition, damageAnimationTime/2);
    }

}