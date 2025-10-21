mergeInto(LibraryManager.library, {
  MqttConnect: function (clientIdPtr) {
    var clientId = UTF8ToString(clientIdPtr);
    window.mqttClient = new Paho.MQTT.Client("broker.emqx.io", 8084, clientId);

    window.mqttClient.onConnectionLost = function (responseObject) {
      if (responseObject.errorCode !== 0) {
        console.log("Connection lost: " + responseObject.errorMessage);
      }
    };

    window.mqttClient.onMessageArrived = function (message) {
      SendMessage("MQTTClient", "OnMessage", "");
    };
    
    window.mqttClient.connect({
      useSSL: true,
      onSuccess: function () {
        console.log("MQTT connected.");
        SendMessage("MQTTClient", "OnConnect");
      },
      onFailure: function (error) {
        console.log("MQTT connection failed: " + error.errorMessage);
        SendMessage("MQTTClient", "OnDisconnect");
      }
    });
  },

  MqttSubscribe: function (topicPtr) {
    var topic = UTF8ToString(topicPtr);
    window.mqttClient.subscribe(topic);
    console.log("Subscribed to topic: " + topic);
  },

  MqttPublish: function (topicPtr, messagePtr) {
    var topic = UTF8ToString(topicPtr);
    var message = UTF8ToString(messagePtr);
    var mqttMessage = new Paho.MQTT.Message(message);
    mqttMessage.destinationName = topic;
    window.mqttClient.send(mqttMessage);
    console.log("Published to topic: " + topic + ", message: " + message);
  }
});
