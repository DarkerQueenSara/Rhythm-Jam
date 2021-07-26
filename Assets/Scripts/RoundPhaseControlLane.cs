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
        if(controlIndex < timeStamps.Count) {
            double timeStamp = timeStamps[controlIndex];
            double audioTime = SongManager.GetAudioSourceTime() -
                               (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if(timeStamp <= audioTime) {
                //trigger phase change
                GameEvents.Instance.OnRoundPhaseOver(EventArgs.Empty);
                controlIndex++;
            }
        }
    }

}
