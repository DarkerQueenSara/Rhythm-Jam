using System;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;

    public GameObject notePrefab;

    //public GameObject wordPrefab;
    //private GameObject currentWord = null;
    private List<Note> _notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    public List<int> lyricIndex = new List<int>();

    private int _spawnIndex = 0;
    private int _inputIndex = 0;

    public TextMeshPro lyricsText;
    private bool _lineBreak;
    
    private Player _currentPlayer;
    private Player _currentOpponent;

    void Start() {
        GameEvents.Instance.RoundPhaseOver += RoundPhaseOver;
        UpdatePlayers();
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array) {
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
    }

    void Update()
    {
        if (_spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[_spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                _notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float) timeStamps[_spawnIndex];
                _spawnIndex++;
            }
        } 

        if (_inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[_inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() -
                               (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                Debug.Log("Miss on " + _inputIndex);
                lyricsText.text += "uh... ";
                _inputIndex++;
                if(_inputIndex < timeStamps.Count) {
                    timeStamp = timeStamps[_inputIndex];
                }
            }

            if ((IsPlayer() && Input.GetKeyDown(input)) || !IsPlayer()) {

                if (_lineBreak)
                {
                    lyricsText.text = "";
                    _lineBreak = false;
                }

                if(IsPlayer())
                    GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);

                if (Mathf.Abs((float)(audioTime - timeStamp)) < marginOfError) {
                    if(IsPlayer() || (!IsPlayer() && _currentPlayer.AIHit())) {
                        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);

                        //hit note
                        Debug.Log("Hit on " + _inputIndex + " " + SongManager.Instance.lyrics[lyricIndex[_inputIndex]]);
                        Hit();
                        Destroy(_notes[_inputIndex].gameObject);

                        String word = SongManager.Instance.lyrics[lyricIndex[_inputIndex]];
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
                        
                        _currentOpponent.AddDamage((int) Char.GetNumericValue(last));
                        
                        _inputIndex++;
                    }
                }
                else
                {
                    //innacurate note
                    /*Debug.Log("Innacurate on " + _inputIndex);

                    if (_inputIndex < timeStamps.Count - 1)
                    timeStamp = timeStamps[_inputIndex+1];
                    if (Mathf.Abs((float)(audioTime - timeStamp)) < marginOfError) {
                        //hit note
                        Debug.Log("Hit on " + _inputIndex+1);
                        Hit();
                        Destroy(_notes[_inputIndex+1].gameObject);
                        _inputIndex+=2;
                    }*/
                }
            }
        }

        if (!Input.GetKey(input))
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
    }

    bool IsPlayer() {
        return _currentPlayer.playerType == Player.PlayerType.PLAYER;
    }

    private void Hit() {
        ScoreManager.Hit();
    }

    private void Miss()
    {
        //ScoreManager.Miss();
    }

    void RoundPhaseOver(object sender, EventArgs eventArgs) {
        UpdatePlayers();
    }

    void UpdatePlayers() {
        _currentPlayer = RoundController.Instance.GetCurrentPlayer();
        _currentOpponent = RoundController.Instance.GetCurrentOpponent();
    }
}