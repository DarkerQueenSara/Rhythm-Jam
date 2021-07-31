using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using System;

public class RoundPhaseControlLane : Lane
{
    [Header("Control Settings")]
    public int controlIndex;

    void Start() {
        /*Empty, needs to be here to override Lane Start*/
    }

    void Update() {
        if(controlIndex < timeStamps.Count) {
            double timeStamp = timeStamps[controlIndex];
            double audioTime = SongManager.GetAudioSourceTime() -
                               (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if(timeStamp <= audioTime) {

                if(controlIndex == timeStamps.Count - 1) {
                    //trigger round over
                    RoundOverArgs args = new RoundOverArgs();
                    Player currentPlayer = RoundController.Instance.GetCurrentPlayer();
                    Player currentOpponent = RoundController.Instance.GetCurrentOpponent();
                    if(currentPlayer.currentHealth > currentOpponent.currentHealth) {
                        Debug.Log("current player has more health");
                        args.winner = currentPlayer;
                    } else if(currentPlayer.currentHealth == currentOpponent.currentHealth) {
                        args.winner = currentPlayer.playerType == Player.PlayerType.PLAYER? currentPlayer: currentOpponent;
                    } else {
                        Debug.Log("current opponent has more health");
                        args.winner = currentOpponent;
                    }
                    GameEvents.Instance.OnRoundOver(args);
                    controlIndex++;
                } else {
                    //trigger phase change
                    GameEvents.Instance.OnPreRoundPhaseOver(EventArgs.Empty);
                    controlIndex++;
                }
            }
        }
    }

}
