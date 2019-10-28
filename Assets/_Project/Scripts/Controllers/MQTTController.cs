using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using System;

/// <summary>
/// Classe de controle sobre comunicacao MQTT
/// </summary>
public class MQTTController : MonoBehaviour
{
    public string topicSubscribe;
    public string topicMessage;
    public string publishMessage;
    private MqttClient client;

	private void Start ()
    {
        try
        {
            // create client instance 
            client = new MqttClient("broker.automab.com.br", 1883, false, null);
            //client = new MqttClient("broker.mqttdashboard.com", 8000, false, null);

            // register to message received 
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            //string clientId = "clientId-R6uVusCBve";
            //client.Connect(clientId);
            client.Connect(clientId, "ismael", "123456");

            // subscribe to the topic "/home/temperature" with QoS 2 
            //client.Subscribe(new string[] { "hello/world" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            //client.Subscribe(new string[] { "hello/world" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            print("MQTT Connected");
        }
        catch(Exception e)
        {
            print("Error connection:: " + e);
        }
	}
	void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{ 
		Debug.Log("Received: " + Encoding.UTF8.GetString(e.Message));
	} 

	void OnGUI()
    {
		//if ( GUI.Button (new Rect (20,40,80,20), "Level 1")) {
		//	Debug.Log("sending...");
		//	//client.Publish("hello/world", System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
		//	client.Publish("BLEKFIEFF/#", System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
		//	Debug.Log("sent");
		//}
	}

    [ContextMenu("SubscribeNow")]
    public void SubscribeNow()
    {
        client.Subscribe(new string[] { topicSubscribe }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        print("Subscribe on:: " + topicSubscribe);
    }

    [ContextMenu("PublishMessage")]
    public void PublishMessage()
    {
        client.Publish(topicMessage, Encoding.UTF8.GetBytes(publishMessage), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        print("Message: " + publishMessage + " send");
        print("Topic: " + topicMessage);
    }
}