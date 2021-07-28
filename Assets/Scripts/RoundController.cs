using UnityEngine;
using System;
using TMPro;

public class RoundController : MonoBehaviour
{
    #region SingleTon

    public static RoundController Instance { get; private set; }
    public Player[] players;
    public int currentPlayerIndex;
    public int currentOpponentIndex;
    public TextMeshPro phaseText;

    [Header("End Round Screens")]
    public GameObject victoryScreen;
    public GameObject defeatScreen;

    public enum RoundPhase
    {
        COMEBACK,
        ATTACK
    }

    public RoundPhase currentRoundPhase;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    #endregion

    void Start()
    {
        GameEvents.Instance.RoundPhaseOver += RoundPhaseOver;
        GameEvents.Instance.PlayerDied += PlayerDied;
        GameEvents.Instance.RoundOver += RoundOver;

        if (players.Length == 0) Debug.LogError("No players assigned");
        currentPlayerIndex = 0;
        currentOpponentIndex = 1;
        currentRoundPhase = RoundPhase.ATTACK;
        phaseText.text = currentRoundPhase.ToString();
    }

    public void RoundPhaseOver(object sender, EventArgs eventArgs)
    {
        StartNextRoundPhase();
    }

    public void StartNextRoundPhase()
    {
        int nextRoundPhase = (int) currentRoundPhase + 1;
        nextRoundPhase = nextRoundPhase % (Enum.GetValues(typeof(RoundPhase)).GetUpperBound(0) + 1);
        currentRoundPhase = (RoundPhase) nextRoundPhase;
        phaseText.text = currentRoundPhase.ToString();
        
        if (nextRoundPhase == 0)
        {
            currentOpponentIndex = currentPlayerIndex;
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
            PositionPlayers();
        }
    }

    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Space)) {
            GameEvents.Instance.OnRoundPhaseOver(EventArgs.Empty);
        }*/
    }

    void PositionPlayers()
    {
        //set players in new positions
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public Player GetCurrentOpponent()
    {
        return players[currentOpponentIndex];
    }

    void PlayerDied(object sender, PlayerDiedArgs eventArgs) {
        RoundOverArgs args = new RoundOverArgs();
        if(eventArgs.player == GetCurrentPlayer()) {
            args.winner = GetCurrentOpponent();
        } else {
            args.winner = GetCurrentPlayer();
        }
        GameEvents.Instance.OnRoundOver(args);
    }

    void RoundOver(object sender, RoundOverArgs eventArgs) {
        Debug.Log("Game Over");
        if(eventArgs.winner.playerType == Player.PlayerType.PLAYER) {
            // win
            victoryScreen.SetActive(true);
        } else {
            // lose
            defeatScreen.SetActive(true);
        }
        SongManager.Instance.StopSong();
    }
}