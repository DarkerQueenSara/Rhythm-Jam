using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerType {
        PLAYER,
        AI
    }

    public PlayerType playerType;
    public Sprite frontSprite;
    public Sprite backSprite;
}
