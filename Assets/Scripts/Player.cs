using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerType {
        PLAYER,
        AI
    }

    public PlayerType playerType;

    [Header("AI Options")]
    [Range(0.0f, 1.0f)]
    public float AIMissChance;
    public float AIMissPenalty;

    [Header("Sprites")]
    public Sprite frontSprite;
    public Sprite backSprite;

    private float AIMissPenaltyEndTime = 0f;
    public bool AIHit() {
        if(playerType == PlayerType.PLAYER) return false;

        float hit = Random.Range(0f, 1f);
        if(hit > AIMissChance && Time.time > AIMissPenaltyEndTime) {
            //hit
            return true;
        } else {
            //miss
            AIMissPenaltyEndTime = Time.time + AIMissPenalty;
            return false;
        }
    }
}
