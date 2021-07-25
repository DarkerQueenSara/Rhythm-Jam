using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Text;

[CustomEditor(typeof(Player))]
[CanEditMultipleObjects]
public class PlayerEditor : Editor 
{

    private string[] fieldsToAvoid = {"Base", "m_Script", "AIMissChance", "AIMissPenalty"};
    private string[] AIFields = {"AIMissChance", "AIMissPenalty"};

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var p = serializedObject.GetIterator();
        do {
            
            if (!fieldsToAvoid.Any(p.name.Contains)) {
                EditorGUILayout.PropertyField(p);
            }else if (AIFields.Any(p.name.Contains)) {
                Player player = target as Player;
                if(player.playerType == Player.PlayerType.AI)
                    EditorGUILayout.PropertyField(p);
            }
        } while (p.NextVisible(true));
        serializedObject.ApplyModifiedProperties();
        
    }

}
