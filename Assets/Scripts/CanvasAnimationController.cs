using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class CanvasAnimationController : MonoBehaviour
{
    public TextMeshProUGUI lyricsText;
    private Animator animator;
    void Start() {
        GameEvents.Instance.RoundPhaseOver += RoundPhaseOver;
        animator = GetComponent<Animator>();
    }

    void RoundPhaseOver(object sender, EventArgs args) {
        if(RoundController.Instance.currentRoundPhase != RoundController.RoundPhase.ATTACK) return;
        
        if(RoundController.Instance.GetCurrentPlayer().playerType == Player.PlayerType.PLAYER) {
            animator.Play("player_out");

        } else {
            animator.Play("ai_out");

        }
    }

    public void UpdatePlayerSprites() {
        RoundController.Instance.UpdatePlayerSprites();
    }

    public void CleanLyricsText() {
        if(lyricsText) lyricsText.text = "";
    }
}
