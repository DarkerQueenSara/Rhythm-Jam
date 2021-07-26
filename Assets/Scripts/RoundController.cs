using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoundController : MonoBehaviour
{
    public static RoundController Instance;
    public Player[] players;
    public int currentPlayerIndex;
    public int currentOpponentIndex;
    public enum RoundPhase {
        COMEBACK,
        ATTACK
    }

    public RoundPhase currentRoundPhase;

    void Awake() {
        Instance = this;
    }
    
    void Start() {
        GameEvents.Instance.RoundPhaseOver += RoundPhaseOver;

        if(players.Length == 0) Debug.LogError("No players assigned");
        currentPlayerIndex = 0;
        currentOpponentIndex = 1;
        currentRoundPhase = RoundPhase.ATTACK;

    }

    public void RoundPhaseOver(object sender, EventArgs eventArgs) {
        StartNextRoundPhase();
    }

    public void StartNextRoundPhase() {
        
        int nextRoundPhase = (int)currentRoundPhase + 1;
        nextRoundPhase = nextRoundPhase % (Enum.GetValues(typeof(RoundPhase)).GetUpperBound(0)+1);
        currentRoundPhase = (RoundPhase)nextRoundPhase;

        if(nextRoundPhase == 0) {
            currentOpponentIndex = currentPlayerIndex;
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
            PositionPlayers();
        }

    }

    void Update() {
        /*if(Input.GetKeyDown(KeyCode.Space)) {
            GameEvents.Instance.OnRoundPhaseOver(EventArgs.Empty);
        }*/
    }

    void PositionPlayers() {
        //set players in new positions
    }

    public Player GetCurrentPlayer() {
        return players[currentPlayerIndex];
    }

    public Player GetCurrentOpponent() {
        return players[currentOpponentIndex];
    }
}
