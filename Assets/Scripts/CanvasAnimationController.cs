using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


public class CanvasAnimationController : MonoBehaviour
{
    [Header("UI")] public Image primaryTextBubble;
    public Image secondaryTextBubble;
    public Image gameWindow;
    public TextMeshProUGUI lyricsText;

    [Header("Colors")] public Color playerWindowColor;
    public Color opponentWindowColor;
    public Color playerPrimaryColor;
    public Color playerSecondaryColor;
    public Color opponentPrimaryColor;
    public Color opponentSecondaryColor;

    private Animator _animator;

    private void Start()
    {
        GameEvents.Instance.RoundPhaseOver += RoundPhaseOver;
        _animator = GetComponent<Animator>();
        gameWindow.color = playerWindowColor;
        primaryTextBubble.color = playerPrimaryColor;
        secondaryTextBubble.color = playerSecondaryColor;
    }

    private void RoundPhaseOver(object sender, EventArgs args)
    {
        if (RoundController.Instance.currentRoundPhase != RoundController.RoundPhase.COMEBACK) return;

        if (RoundController.Instance.GetCurrentPlayer().playerType == Player.PlayerType.AI)
        {
            _animator.Play("player_out");
        }
        else
        {
            _animator.Play("ai_out");
        }
    }

    public void UpdatePlayerSprites()
    {
        RoundController.Instance.UpdatePlayerSprites();

        //update player colors in bubble
        if (RoundController.Instance.GetCurrentPlayer().playerType == Player.PlayerType.PLAYER)
        {
            primaryTextBubble.color = playerPrimaryColor; //change primary color
            secondaryTextBubble.color = playerSecondaryColor;
            gameWindow.color = playerWindowColor;
        }
        else
        {
            primaryTextBubble.color = opponentPrimaryColor; //change primary color
            secondaryTextBubble.color = opponentSecondaryColor;
            gameWindow.color = opponentWindowColor;
        }
    }

    public void CleanLyricsText()
    {
        if (lyricsText) lyricsText.text = "";
    }
}