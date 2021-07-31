using System;
using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public List<KeyCode> inputs;

    public GameObject notePrefab;

    private List<Note> _notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    public List<int> lyricIndex = new List<int>();

    private int _spawnIndex = 0;
    private int _inputIndex = 0;

    public TextMeshProUGUI lyricsText;
    private Image _image;

    private bool _generatedLists;
    private bool _lineBreak;
    private bool _stop;
    private int _lastHitType;

    private Player _currentPlayer;
    private Player _currentOpponent;
    public float arrowScaleUnfocused;
    public float arrowScaleFocused;
    public float arrowFocusTime;
    private RectTransform _arrowIndicator;


    private void Start()
    {
        GameEvents.Instance.RoundPhaseOver += RoundPhaseOver;
        _image = GetComponent<Image>();
        _arrowIndicator = transform.Find("Arrow").GetComponent<RectTransform>();
        UpdatePlayers();
        lyricsText.text = "";
        _stop = false;
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        int index = 0;
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan =
                    TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double) metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                               (double) metricTimeSpan.Milliseconds / 1000f);

                lyricIndex.Add(index);
            }

            index++;
        }

        _generatedLists = true;
        Debug.Log(gameObject.name + " generated " + timeStamps.Count + " timestamps and " + lyricIndex.Count +
                  " lyrics");
    }

    private void Update()
    {
        if (!_stop)
        {
            string[] lyrics = SongManager.Instance.lyrics;

            if (_spawnIndex < timeStamps.Count)
            {
                if (SongManager.GetAudioSourceTime() >= timeStamps[_spawnIndex] - SongManager.Instance.noteTime)
                {
                    var note = Instantiate(notePrefab, transform);
                    Note aux = note.GetComponent<Note>();
                    _notes.Add(aux);
                    aux.assignedTime = (float) timeStamps[_spawnIndex];
                    _spawnIndex++;
                }
            }

            if (IsPlayer() && CheckInput())
                FocusArrow();

            if (_inputIndex < timeStamps.Count)
            {
                double timeStamp = timeStamps[_inputIndex];
                //double marginOfError = SongManager.Instance.marginOfError;
                double audioTime = SongManager.GetAudioSourceTime() -
                                   (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

                if (timeStamp + ScoreManager.Instance.fineTiming < audioTime)
                {
                    if (_lineBreak)
                    {
                        lyricsText.text = "";
                        _lineBreak = false;
                    }

                    Miss();
                    //Debug.Log("Miss on " + _inputIndex);
                    lyricsText.text += "uh... ";
                    if (_inputIndex < timeStamps.Count)
                    {
                        timeStamp = timeStamps[_inputIndex];
                    }

                    if (_generatedLists && _inputIndex < lyricIndex.Count && lyricIndex[_inputIndex] < lyrics.Length)
                    {
                        String w = lyrics[lyricIndex[_inputIndex]];
                        char l = w.ToCharArray()[w.Length - 1];
                        if (l == '\r' || l == '\n' || l == '\t' || l == '\v') _lineBreak = true;
                    }

                    _inputIndex++;
                }

                if (IsPlayer() && CheckInput() || !IsPlayer())
                {
                    if (CheckHit(audioTime, timeStamp))
                    {
                        if (IsPlayer() || (!IsPlayer() && _currentPlayer.AIHit()))
                        {
                            if (!IsPlayer())
                                FocusArrow();

                            //hit note
                            //Debug.Log("Hit on " + _inputIndex + " " + SongManager.Instance.lyrics[lyricIndex[_inputIndex]]);
                            if (_inputIndex < _notes.Count && _notes[_inputIndex] != null &&
                                _notes[_inputIndex].gameObject != null)
                                Destroy(_notes[_inputIndex].gameObject);

                            if (_generatedLists && _inputIndex < lyricIndex.Count &&
                                lyricIndex[_inputIndex] < lyrics.Length)
                            {
                                if (_lineBreak)
                                {
                                    lyricsText.text = "";
                                    _lineBreak = false;
                                }

                                String word = lyrics[lyricIndex[_inputIndex]];
                                char last = word.ToCharArray()[word.Length - 1];
                                if (last == '\r' || last == '\n' || last == '\t' || last == '\v')
                                {
                                    lyricsText.text += word.Substring(0, word.Length - 2) + " ";
                                    _lineBreak = true;
                                    last = word.ToCharArray()[word.Length - 2];
                                }
                                else
                                {
                                    lyricsText.text += word.Substring(0, word.Length - 1) + " ";
                                }

                                Hit((int) Char.GetNumericValue(last));

                                _inputIndex++;
                            }
                        }
                    }
                }
            }
        }
    }

    private bool IsPlayer()
    {
        return _currentPlayer.playerType == Player.PlayerType.PLAYER;
    }

    private bool CheckInput()
    {
        foreach (KeyCode k in inputs)
        {
            if (Input.GetKeyDown(k)) return true;
        }

        return false;
    }

    private bool CheckHit(double audioTime, double timeStamp)
    {
        if (Mathf.Abs((float) (audioTime - timeStamp)) <= ScoreManager.Instance.perfectTiming)
        {
            _lastHitType = 3;
            return true;
        }

        if (Mathf.Abs((float) (audioTime - timeStamp)) <= ScoreManager.Instance.greatTiming)
        {
            _lastHitType = 2;
            return true;
        }

        if (Mathf.Abs((float) (audioTime - timeStamp)) <= ScoreManager.Instance.goodTiming)
        {
            _lastHitType = 1;
            return true;
        }

        if (Mathf.Abs((float) (audioTime - timeStamp)) <= ScoreManager.Instance.fineTiming)
        {
            _lastHitType = 0;
            return true;
        }

        return false;
    }

    private void Hit(int hitPointAmount)
    {
        if (RoundController.Instance.currentRoundPhase == RoundController.RoundPhase.COMEBACK)
        {
            _currentPlayer.RestoreHealth(hitPointAmount);
        }
        else if (RoundController.Instance.currentRoundPhase == RoundController.RoundPhase.ATTACK)
        {
            _currentOpponent.AddDamage(hitPointAmount);
        }

        ScoreManager.Instance.Hit(_lastHitType, IsPlayer());
    }

    private void Miss()
    {
        ScoreManager.Instance.Miss(IsPlayer());
    }

    private void RoundPhaseOver(object sender, EventArgs eventArgs)
    {
        UpdatePlayers();
    }

    private void UpdatePlayers()
    {
        _currentPlayer = RoundController.Instance.GetCurrentPlayer();
        _currentOpponent = RoundController.Instance.GetCurrentOpponent();
    }

    public void StopLane()
    {
        _stop = true;

        foreach (var note in _notes)
        {
            note.StopNote();
        }
    }

    void FocusArrow()
    {
        LeanTween.cancel(_arrowIndicator);
        LeanTween.scale(_arrowIndicator, new Vector2(arrowScaleFocused, arrowScaleFocused), arrowFocusTime / 2)
            .setOnComplete(UnfocusArrow);
    }

    void UnfocusArrow()
    {
        LeanTween.cancel(_arrowIndicator);
        LeanTween.scale(_arrowIndicator, new Vector2(arrowScaleUnfocused, arrowScaleUnfocused), arrowFocusTime / 2);
    }
}