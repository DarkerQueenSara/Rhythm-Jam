using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    void Awake() {
        Instance = this;
    }

    public event EventHandler RoundPhaseOver;
    public virtual void OnRoundPhaseOver(EventArgs args) {
        EventHandler handler = RoundPhaseOver;
        handler?.Invoke(this, args);
    }

    public event EventHandler<RoundOverArgs> RoundOver;
    public virtual void OnRoundOver(RoundOverArgs args) {
        EventHandler<RoundOverArgs> handler = RoundOver;
        handler?.Invoke(this, args);
    }

    public event EventHandler<PlayerDiedArgs> PlayerDied;
    public virtual void OnPlayerDied(PlayerDiedArgs args) {
        EventHandler<PlayerDiedArgs> handler = PlayerDied;
        handler?.Invoke(this, args);
    }
}

public class RoundOverArgs : EventArgs
{
    public Player winner { get; set; }
}

public class PlayerDiedArgs : EventArgs
{
    public Player player { get; set; }
}
