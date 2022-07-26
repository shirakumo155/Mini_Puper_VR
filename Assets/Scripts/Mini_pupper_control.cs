using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
// Specifies ROS topic type
// You can check the type by "rostopic list" and "rostopic info /cmd_vel"
// /cmd_vel has the type of geometry_msgs/Twist
using TwistMsg = RosMessageTypes.Geometry.TwistMsg;


/// <summary>
/// The class for sending CmdVel (TwistMsg)
/// Assuming UI that can call functions in the class
/// </summary>

public class Mini_pupper_control : MonoBehaviour
{
    // Variables required for ROS communication
    // Topic name in ROS to send
    [SerializeField] string topicName = "cmd_vel";

    // Standard velocity value to send velocity signal
    [SerializeField] float linearVel;

    // Standard angular vel value to send angular signal
    [SerializeField] float angularVel;

    // ROS Connector
    private ROSConnection ros;

    private TwistMsg cmdVelMessage = new TwistMsg();
    private bool push = false;

    /// <summary>
    /// Seriese of event functions to initialize
    /// https://docs.unity3d.com/ja/2020.3/Manual/ExecutionOrder.html
    /// </summary>
    void Start()
    {
        // Get ROS connection static instance
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistMsg>(topicName);
        PushUp();
        Send();
    }

    /// <summary>
    /// Series of event function called by UI input
    /// </summary>
    /// <param name="ratio"></param> factor ratio
    public void PushDown_Forward(float ratio = 1.0f)
    {
        push = true;
        cmdVelMessage.linear.x = linearVel * ratio;
    }

    public void PushDown_Backward(float ratio = 1.0f)
    {
        push = true;
        cmdVelMessage.linear.x = -linearVel * ratio;
        
    }

    public void PushDown_RightTurn(float ratio = 1.0f)
    {
        push = true;
        cmdVelMessage.angular.z = -angularVel * ratio;
        
    }

    public void PushDown_LeftTurn(float ratio = 1.0f)
    {
        push = true;
        cmdVelMessage.angular.z = angularVel * ratio;
        
    }

    /// <summary>
    /// Setting stop status
    /// </summary>
    public void PushUp()
    {
        push = false;
        cmdVelMessage.linear.x = 0;
        cmdVelMessage.angular.z = 0;
    }

    private void Update()
    {
        if (push)
        {
            Send();
        }
    }

    /// <summary>
    /// Publishing speed and angular vel 
    /// </summary>
    public void Send()
    {
        ros.Publish(topicName, cmdVelMessage);
    }


}
