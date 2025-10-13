using System;
using System.Collections.Generic;
using System.Text;
using M2MqttUnity;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SendMessage : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_InputField gameInputField;

    [FormerlySerializedAs("_mqttClient")] [SerializeField] private MQTTClient mqttClient;
    

    public void Send()
    {
        var topic = $"learnathon/{gameInputField.text}/api";
        Dictionary<string, string> dict = new Dictionary<string, string>
        {
            { "msg", inputField.text },
        };

        mqttClient.Publish(topic, JsonConvert.SerializeObject(dict));
    }
}
