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
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    public List<int> lyricIndex = new List<int>();

    int spawnIndex = 0;
    int inputIndex = 0;

    public TextMeshPro lyricsText;
    private bool _lineBreak;
    
    private Player currentPlayer;

    void Start() {
        currentPlayer = RoundController.Instance.GetCurrentPLayer();
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
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float) timeStamps[spawnIndex];
                spawnIndex++;
            }
        } 

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() -
                               (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                Debug.Log("Miss on " + inputIndex);
                lyricsText.text += "uh... ";
                inputIndex++;
                if(inputIndex < timeStamps.Count) {
                    timeStamp = timeStamps[inputIndex];
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
                    if(IsPlayer() || (!IsPlayer() && currentPlayer.AIHit())) {
                        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);

                        //hit note
                        Debug.Log("Hit on " + inputIndex + " " + SongManager.Instance.lyrics[lyricIndex[inputIndex]]);
                        Hit();
                        Destroy(notes[inputIndex].gameObject);

                        String word = SongManager.Instance.lyrics[lyricIndex[inputIndex]];
                        char last = word.ToCharArray()[word.Length - 1];
                        if (last == '\r' || last == '\n' || last == '\t' || last == '\v')
                        {
                            lyricsText.text += word.Substring(0, word.Length - 2) + " ";
                            _lineBreak = true;
                        }
                        else
                        {
                            lyricsText.text += word.Substring(0, word.Length - 1) + " ";
                        }

                        inputIndex++;
                    }
                }
                else
                {
                    //innacurate note
                    /*Debug.Log("Innacurate on " + inputIndex);

                    if (inputIndex < timeStamps.Count - 1)
                    timeStamp = timeStamps[inputIndex+1];
                    if (Mathf.Abs((float)(audioTime - timeStamp)) < marginOfError) {
                        //hit note
                        Debug.Log("Hit on " + inputIndex+1);
                        Hit();
                        Destroy(notes[inputIndex+1].gameObject);
                        inputIndex+=2;
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
        return currentPlayer.playerType == Player.PlayerType.PLAYER;
    }

    private void Hit() {
        ScoreManager.Hit();
    }

    private void Miss()
    {
        //ScoreManager.Miss();
    }
}