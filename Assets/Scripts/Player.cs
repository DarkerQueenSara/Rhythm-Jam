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
        playerSpriteInitialPosition = Vector3.zero;
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
        //frontHealthBar.fillAmount -= (float) damage / maxHealth;
        DamageAnimation(damage);
        HealthBarDamageAnimation();

    }

    public void DealDamage()
    {
        //Debug.Log("Health to remove " + healthToRemove);
        currentHealth -= healthToRemove;
        healthToRemove = 0;
        //se calhar tenho que usar lerp
        //backHealthBar.fillAmount = frontHealthBar.fillAmount;
        HealthBarFinishDamageAnimation();

        if(currentHealth <= 0) {
            Die();
        }
    }

    public void RestoreHealth(int health)
    {
        currentHealth += health;
        backHealthBar.color = Color.yellow;
        //se calhar tenho que usar lerp
        //backHealthBar.fillAmount += (float) health / maxHealth;
        HealthBarHealAnimation();
        Invoke(nameof(FinishHeal), healTime);
    }

    private void FinishHeal()
    {
        //se calhar tenho que usar lerp
        //frontHealthBar.fillAmount = backHealthBar.fillAmount;
        HealthBarFinishHealAnimation();
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
        if (RoundController.Instance.GetCurrentPlayer() == this && RoundController.Instance.currentRoundPhase == RoundController.RoundPhase.COMEBACK) {
            //finish damage at the beginning of comeback round
            DealDamage();
        }
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
        DamageAnimationX(damage);
    }

    void DamageAnimationX(float damage) {
        //Debug.Log(damageAnimationDistance * damage/9f);
        LeanTween.cancel(playerSprite);
        LeanTween.moveX(playerSprite, playerSprite.transform.position.x + damageAnimationDistance * damage/9f, damageAnimationTime/2).setOnComplete(DamageAnimationReset);
    }

    void DamageAnimationReset() {
        LeanTween.moveLocal(playerSprite,  playerSpriteInitialPosition, damageAnimationTime/2);
    }

    void HealthBarDamageAnimation() {
        LeanTween.cancel(frontHealthBar.gameObject);
        LeanTween.value(frontHealthBar.gameObject, UpdateValueFrontHealthBar, frontHealthBar.fillAmount, ((float)currentHealth-healthToRemove)/maxHealth, healthBarDamageAnimationTime);
    }

    void HealthBarFinishDamageAnimation() {
        LeanTween.cancel(backHealthBar.gameObject);
        LeanTween.value(backHealthBar.gameObject, UpdateValueBackHealthBar, backHealthBar.fillAmount, ((float)currentHealth)/maxHealth, healthBarDamageAnimationTime);
    }

    void HealthBarHealAnimation() {
        LeanTween.cancel(backHealthBar.gameObject);
        LeanTween.value(backHealthBar.gameObject, UpdateValueBackHealthBar, backHealthBar.fillAmount, ((float)currentHealth)/maxHealth, healthBarHealAnimationTime);
    }

    void HealthBarFinishHealAnimation() {
        LeanTween.cancel(frontHealthBar.gameObject);
        LeanTween.value(frontHealthBar.gameObject, UpdateValueFrontHealthBar, frontHealthBar.fillAmount, ((float)currentHealth)/maxHealth, healthBarHealAnimationTime);
    }

    void UpdateValueFrontHealthBar(float val, float ratio) {
        frontHealthBar.fillAmount = val;
    }

    void UpdateValueBackHealthBar(float val, float ratio) {
        backHealthBar.fillAmount = val;
    }

}