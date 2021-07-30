using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


public class CanvasAnimationController : MonoBehaviour
{
    [Header("UI")] public TextMeshProUGUI phaseText;
    public Image textBubble;
    public Color playerColor;
    public Color opponentColor;
    
    public TextMeshProUGUI lyricsText;
    private Animator animator;
    void Start() {
        GameEvents.Instance.RoundPhaseOver += RoundPhaseOver;
        animator = GetComponent<Animator>();
        textBubble.color = playerColor;
        phaseText.text = RoundController.Instance.currentRoundPhase.ToString();
    }

    void RoundPhaseOver(object sender, EventArgs args) {
        if(RoundController.Instance.currentRoundPhase != RoundController.RoundPhase.ATTACK) return;
        
        if(RoundController.Instance.GetCurrentPlayer().playerType == Player.PlayerType.PLAYER) {
            animator.Play("player_out");
            textBubble.color = opponentColor;
        } else {
            animator.Play("ai_out");
            textBubble.color = playerColor;
        }
        phaseText.text = RoundController.Instance.currentRoundPhase.ToString();
    }

    public void UpdatePlayerSprites() {
        RoundController.Instance.UpdatePlayerSprites();
    }

    public void CleanLyricsText() {
        if(lyricsText) lyricsText.text = "";
    }
}
