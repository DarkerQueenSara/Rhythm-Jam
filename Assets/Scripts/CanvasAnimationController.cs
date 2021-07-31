using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


public class CanvasAnimationController : MonoBehaviour
{
    [Header("UI")] 
    public Image primaryTextBubble;
    public Image secondaryTextBubble;
    public Color playerPrimaryColor;
    public Color playerSecondaryColor;
    public Color opponentPrimaryColor;
    public Color opponentSecondaryColor;

    public TextMeshProUGUI lyricsText;
    private Animator animator;
    void Start() {
        GameEvents.Instance.RoundPhaseOver += RoundPhaseOver;
        animator = GetComponent<Animator>();
        primaryTextBubble.color = playerPrimaryColor;
        secondaryTextBubble.color = playerSecondaryColor;
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

        //update player colors in bubble
        if(RoundController.Instance.GetCurrentPlayer().playerType == Player.PlayerType.PLAYER) {
            primaryTextBubble.color = playerPrimaryColor; //change primary color
            secondaryTextBubble.color = playerSecondaryColor;
        } else {
            primaryTextBubble.color = opponentPrimaryColor; //change primary color
            secondaryTextBubble.color = opponentSecondaryColor;
        }
    }

    public void CleanLyricsText() {
        if(lyricsText) lyricsText.text = "";
    }
}
