using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StringEvent", menuName = "Learnathon/StringEvent")]
public class StringEvent : ScriptableObject
{
    public event Action<string> OnEventRaised;

    public void Raise(string value)
    {
        OnEventRaised?.Invoke(value);
    }
}
