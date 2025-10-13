using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StringEvent))]
public class StringEventEditor : Editor
{
    private string _text = "";
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.Label("---------------------------------------");
        GUILayout.Label("Use this section to simulate various messages to your game.");
        GUILayout.Label("These messages are *not* going to be sent over the network!");
        GUILayout.Label("This is just for testing; the real messages will be sent from other devices!");
        
        
        EditorGUILayout.Space();
        _text = EditorGUILayout.TextField("Message", _text);
        if (GUILayout.Button("Send!"))
        {
            (target as StringEvent)?.Raise(_text);
        }
        GUILayout.Label("---------------------------------------");
    }
}
