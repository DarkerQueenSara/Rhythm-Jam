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
        } else {
            animator.Play("ai_out");
        }
        phaseText.text = RoundController.Instance.currentRoundPhase.ToString();
    }

    public void UpdatePlayerSprites() {
        RoundController.Instance.UpdatePlayerSprites();

        //update player colors in bubble
        if(RoundController.Instance.GetCurrentPlayer().playerType == Player.PlayerType.PLAYER) {
            textBubble.color = playerColor; //change primary color
            //FIXME: missing secondary color
        } else {
            textBubble.color = opponentColor; //change primary color
            //FIXME: missing secondary color
        }
    }

    public void CleanLyricsText() {
        if(lyricsText) lyricsText.text = "";
    }
}
