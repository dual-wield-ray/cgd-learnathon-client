using MQTTnet;
using MQTTnet.Client;
using UnityEngine;

public class MQTTClient : WebSocketMQTTClient
{
    [SerializeField] private StringEvent messageReceived;

    protected override void OnConnect()
    {
        base.OnConnect();
    }

    protected override void OnDisconnect()
    {
        base.OnDisconnect();
    }

    protected override void OnMessage(string message)
    {
        // Sanitize string sent by companion app
        message = message.Replace("\\", "");
        message = message.Replace("\"{", "{");
        message = message.Replace("}\"", "}");

        message.TrimStart('{');
        message = message.Trim('}', '{', ' ', '\n');

        message = message.Remove(0, 4);
        
        messageReceived.Raise(message);
    }
}