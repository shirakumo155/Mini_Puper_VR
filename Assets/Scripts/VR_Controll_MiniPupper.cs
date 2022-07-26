using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TwistMsg = RosMessageTypes.Geometry.TwistMsg;
using Unity.Robotics.ROSTCPConnector;

public class VR_Controll_MiniPupper : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteritics;
    private InputDevice targetDevice;

    // Variables required for ROS communication
    // Topic name in ROS to send
    [SerializeField] string topicName = "cmd_vel";


    // ROS Connector
    private ROSConnection ros;

    private TwistMsg cmdVelMessage = new TwistMsg();


    // Start is called before the first frame update
    void Start()
    {
        // Get ROS connection static instance
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistMsg>(topicName);
        PushUp();
        Send();

        // Set Up VR controller
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteritics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            Debug.Log("Target: " + targetDevice);
        }
    }



    /// <summary>
    /// Setting stop status
    /// </summary>
    public void PushUp()
    {
        cmdVelMessage.linear.x = 0;
        cmdVelMessage.angular.z = 0;
    }

    /// <summary>
    /// Publishing speed and angular vel 
    /// </summary>
    public void Send()
    {
        ros.Publish(topicName, cmdVelMessage);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtunValue) && primaryButtunValue)
        {
            Debug.Log("Pressing Primary Button");
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
        {
            Debug.Log("trigger Value: " + triggerValue);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue))
        {
            Debug.Log("Primary TouchPad: " + primary2DAxisValue);
            cmdVelMessage.linear.x = primary2DAxisValue[1];
            cmdVelMessage.angular.z = -primary2DAxisValue[0];
            Send();
        }
    }
}
