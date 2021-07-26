using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using System;

public class RoundPhaseControlLane : Lane
{
    [Header("Control Settings")]
    public int controlIndex;

    void Update() {
        Debug.Log(timeStamps.Count);
        if(controlIndex < timeStamps.Count) {
            Debug.Log("entered");
            double timeStamp = timeStamps[controlIndex];
            double audioTime = SongManager.GetAudioSourceTime() -
                               (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if(timeStamp <= audioTime) {
                //trigger phase change
                Debug.Log("change phase");
                GameEvents.Instance.OnRoundPhaseOver(EventArgs.Empty);
                controlIndex++;
            }
        }
    }

}
