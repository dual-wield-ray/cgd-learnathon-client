using System;
using UnityEngine;

/// <summary>
/// A script to move an object in different directions based on messages received through MQTT
/// </summary>
public class MoveThroughMQTT : MonoBehaviour
{
    [SerializeField] private StringEvent messageReceived;

    private void Awake()
    {
        if (messageReceived == null)
        {
            Debug.LogError($"Event to receive message was not assigned to '{name}'.");
        }
    }

    private void OnEnable()
    {
        messageReceived.OnEventRaised += OnMessageReceived;
    }

    private void OnDisable()
    {
        messageReceived.OnEventRaised -= OnMessageReceived;
    }

    private void OnMessageReceived(string msg)
    {
        if (msg == "up")
        {
            transform.Translate(Vector3.up, Space.World);
        }
        else if (msg == "down")
        {
            transform.Translate(Vector3.down, Space.World);
        }
        else if (msg == "left")
        {
            transform.Translate(Vector3.left, Space.World);
        }
        else if (msg == "right")
        {
            transform.Translate(Vector3.right, Space.World);
        }
    }
}
