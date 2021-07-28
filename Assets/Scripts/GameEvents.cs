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

    public event EventHandler RoundOver;
    public virtual void OnRoundOver(EventArgs args) {
        EventHandler handler = RoundOver;
        handler?.Invoke(this, args);
    }

    public event EventHandler<PlayerDiedArgs> PlayerDied;
    public virtual void OnPlayerDied(PlayerDiedArgs args) {
        EventHandler<PlayerDiedArgs> handler = PlayerDied;
        handler?.Invoke(this, args);
    }
}

public class PlayerDiedArgs : EventArgs
{
    public Player player { get; set; }
}
