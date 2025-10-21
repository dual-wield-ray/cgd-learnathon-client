using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using UnityEngine;
using System.Runtime.InteropServices;

public class WebSocketMQTTClient : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void MqttConnect(string clientId);

    [DllImport("__Internal")]
    private static extern void MqttSubscribe(string topic);

    [DllImport("__Internal")]
    private static extern void MqttPublish(string topic, string message);
#endif
    
    public string brokerAddress = "127.0.0.1";

    protected IMqttClient Client;
    protected IMqttClientOptions ClientOptions;
    
    protected Queue<string> messageQueue = new();

    [SerializeField] private List<string> topicsToSubscribe = new();

    private void Awake()
    {
        topicsToSubscribe.Add($"learnathon/{Application.productName}/api");
    }

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        MqttConnect("unity_webgl_client_" + UnityEngine.Random.Range(1, 10000));

        Invoke("SubscribeToTopics", 3f); // Wait a bit before subscribing
#else
        
        var factory = new MqttFactory();
        Client = factory.CreateMqttClient();

        ClientOptions = new MqttClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithWebSocketServer(brokerAddress)
            .WithCleanSession()
            .WithKeepAlivePeriod(new TimeSpan(0, 1, 0))
            .Build();
        
        Client.ConnectAsync(ClientOptions);
        
        Client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
        {
            EnqueueMessage(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
        });

        Client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(e =>
        {
            OnConnect();
            return Task.CompletedTask;
        });

        Client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(e =>
        {
            OnDisconnect();
            return Task.CompletedTask;
        });
#endif
    }

    private void Update()
    {
        while (messageQueue.Count > 0)
        {
            var msg = messageQueue.Dequeue();
            OnMessage(msg);
        }
    }

    public void EnqueueMessage(string message)
    {
        messageQueue.Enqueue(message);
    }

    public void PublishMessage(string message, string topic, int qos)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        MqttPublish(topic, message);
        #else
        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(message);
        if (qos == 0)
        {
            applicationMessage = applicationMessage.WithAtMostOnceQoS();
        }
        else if (qos == 1)
        {
            applicationMessage = applicationMessage.WithAtLeastOnceQoS();
        }
        else if (qos == 2)
        {
            applicationMessage = applicationMessage.WithExactlyOnceQoS();
        }
        
        Client.PublishAsync(applicationMessage.Build());
        #endif
    }

    protected virtual void OnConnect()
    {
        SubscribeTopics();
    }
    
    protected virtual void OnDisconnect()
    {
        UnsubscribeTopics();
    }
    
    protected virtual void OnMessage(string message)
    {
        
    }

    protected virtual void SubscribeTopics()
    {
        foreach (string topic in topicsToSubscribe)
        {
            Debug.Log($"Subscribing to topic: {topic}");
        
#if UNITY_WEBGL && !UNITY_EDITOR
            MqttSubscribe(topic);
#else
            Client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
#endif
        }
    }
    
    protected virtual void UnsubscribeTopics()
    {
        
    }
}
