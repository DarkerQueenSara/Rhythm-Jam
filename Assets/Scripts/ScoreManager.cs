using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region SingleTon

    public static ScoreManager Instance { get; private set; }
    public AudioSource hitSFX;
    public AudioSource missSFX;

    [Header("UI")] public TextMeshPro scoreText;
    public TextMeshPro multiplierText;
    public Image multiplierBar;

    [Header("Score Values")] public int scorePerFineNote = 10;
    public int scorePerGoodNote = 20;
    public int scorePerGreatNote = 30;
    public int scorePerPerfectNote = 40;
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
        multiplierBar.fillAmount = 0;

        UpdateText();
    }

    //TODO meter aqui a passar que tipo de hit
    public void Hit()
    {
        _notesHit++;
        _totalNotes++;
        _score += scorePerFineNote;
        _currentStreak++;
        if (_currentStreak > _maxStreak) _maxStreak = _currentStreak;
        multiplierBar.fillAmount += 1.0f / notesToIncreaseMultiplier;
        if (_currentStreak % notesToIncreaseMultiplier == 0)
        {
            _scoreMultiplier *= multiplierIncrease;
            multiplierBar.fillAmount = 0;
        }

        UpdateText();
        Instance.hitSFX.Play();
    }

    public void Miss()
    {
        _totalNotes++;
        _currentStreak = 0;
        _scoreMultiplier = 1;
        multiplierBar.fillAmount = 0;
        UpdateText();
        Instance.missSFX.Play();
    }

    private void UpdateText()
    {
        scoreText.text = _score.ToString();
        multiplierText.text = _scoreMultiplier + "X";
    }
}