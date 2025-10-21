using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SendMessage : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_InputField gameInputField;

    [SerializeField] private MQTTClient mqttClient;

    public void Send()
    {
        var topic = $"learnathon/{gameInputField.text}/api";

        // Note: simple hardcoded JSON string because WebGL builds don't support Reflection,
        // which is what the common JSON libraries use (like Newtonsoft)
        // If needing more, could try using https://github.com/SaladLab/Json.Net.Unity3D
        string jsonStr = "" +
                         "{\n" +
                         $"    \"msg\": \"{inputField.text}\"\n" +
                         "}";
        
        mqttClient.PublishMessage(jsonStr, topic, 2);
    }
}
