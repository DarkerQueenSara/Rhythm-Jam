using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ScoreManager : MonoBehaviour
{
    #region SingleTon

    public static ScoreManager Instance { get; private set; }
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TextMeshPro scoreText;
    public TextMeshPro multiplierText;
    public int scorePerNote = 10;
    public int notesToIncreaseMultiplier = 10;
    public int multiplierIncrease = 2;
    private int _score;
    private int _scoreMultiplier = 1;
    private int _currentStreak;
    private int _maxStreak;
    private int _notesHit;
    private int _totalNotes;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    #endregion

    private void Start()
    {
        _score = 0;
        _scoreMultiplier = 1;
        UpdateText();
    }

    public void Hit()
    {
        _notesHit++;
        _totalNotes++;
        _score += scorePerNote;
        _currentStreak++;
        if (_currentStreak > _maxStreak) _maxStreak = _currentStreak;
        if (_currentStreak % notesToIncreaseMultiplier == 0) _scoreMultiplier *= multiplierIncrease;
        UpdateText();
        Instance.hitSFX.Play();
    }

    public void Miss()
    {
        _totalNotes++;
        _currentStreak = 0;
        _scoreMultiplier = 1;
        UpdateText();
        Instance.missSFX.Play();
    }

    private void UpdateText()
    {
        scoreText.text = _score.ToString();
        multiplierText.text = _scoreMultiplier + "X";
    }
}