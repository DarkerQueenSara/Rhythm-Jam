using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Text;

[CustomEditor(typeof(SongManager))]
[CanEditMultipleObjects]
public class SongManagerEditor : Editor 
{

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        if (GUILayout.Button("Update Lyrics"))
            UpdateLyricsFromFile();

        EditorGUILayout.Space();

        DrawDefaultInspector();
    }

    void UpdateLyricsFromFile() {
        Debug.Log("updated lyrics");
        SongManager songManager = target as SongManager;
        songManager.lyrics = songManager.lyricFile.text.Split(new char[] {' ', '\n', ',', '-', '!', '?', '\t'}, StringSplitOptions.RemoveEmptyEntries);

        //clean remaining empties
        songManager.lyrics = (songManager.lyrics).Where(word => word[0] >= '<' && word [0] <= 'z').ToArray();
    }
}
