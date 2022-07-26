using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
// 座標変換用
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

// 各種ROSトピック形式
using RosMessageTypes.Geometry;
using Tf = RosMessageTypes.Tf2.TFMessageMsg;
using RosPosVector3 = Unity.Robotics.ROSTCPConnector.ROSGeometry.Vector3<Unity.Robotics.ROSTCPConnector.ROSGeometry.FLU>;
using RosQuaternion = Unity.Robotics.ROSTCPConnector.ROSGeometry.Quaternion<Unity.Robotics.ROSTCPConnector.ROSGeometry.FLU>;

/// <summary>
/// ロボットの位置姿勢データ（TFMessageMsg）を受信するためのクラス
/// 位置姿勢データの取得、座標変換を行い、指定したGameObjectの位置姿勢を指定する用途を想定
/// </summary>
public class TfSubscriber : MonoBehaviour
{
    // Variables required for ROS communication
    [SerializeField] string rosTopicName = "tf";

    [SerializeField] GameObject mobileRobot;

    [SerializeField] bool isDebugMode = false;

    private int numRobotLinks = 13;

    private Transform[] robotLinkPositions;

    ROSConnection ros;
    Vector3 unityOdomPos;
    Quaternion unityOdomQuaternion;
    Vector3 unityMapPos;
    Quaternion unityMapQuaternion;


    /// <summary>
    /// 初期化用のイベント関数
    /// https://docs.unity3d.com/ja/2020.3/Manual/ExecutionOrder.html
    /// </summary>
    void Start()
    {
        // // Get ROS connection static instance
        
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<Tf>(rosTopicName, TfUpdate);
        // ROSConnection.instance.Subscribe<Tf>(rosTopicName, TfUpdate);

        robotLinkPositions = new Transform[numRobotLinks];

        string body_link = "base_link";
        robotLinkPositions[0] = mobileRobot.transform.Find(body_link).transform;

        string rb_hip_link = body_link + "/rb1/rpb1";
        robotLinkPositions[1] = mobileRobot.transform.Find(rb_hip_link).transform;

        string rb_u_leg_link = body_link + "/rb1/rb2/rpb2";
        robotLinkPositions[2] = mobileRobot.transform.Find(rb_u_leg_link).transform;

        string rb_l_leg_link = body_link + "/rb1/rb2/rb3";
        robotLinkPositions[3] = mobileRobot.transform.Find(rb_l_leg_link).transform;

        string lb_hip_link = body_link + "/lb1/lpb1";
        robotLinkPositions[4] = mobileRobot.transform.Find(lb_hip_link).transform;

        string lb_u_leg_link = body_link + "/lb1/lb2/lpb2";
        robotLinkPositions[5] = mobileRobot.transform.Find(lb_u_leg_link).transform;

        string lb_l_leg_link = body_link + "/lb1/lb2/lb3";
        robotLinkPositions[6] = mobileRobot.transform.Find(lb_l_leg_link).transform;

        string rf_hip_link = body_link + "/rf1/rpf1";
        robotLinkPositions[7] = mobileRobot.transform.Find(rf_hip_link).transform;

        string rf_u_leg_link = body_link + "/rf1/rf2/rpf2";
        robotLinkPositions[8] = mobileRobot.transform.Find(rf_u_leg_link).transform;

        string rf_l_leg_link = body_link + "/rf1/rf2/rf3";
        robotLinkPositions[9] = mobileRobot.transform.Find(rf_l_leg_link).transform;

        string lf_hip_link = body_link + "/lf1/lpf1";
        robotLinkPositions[10] = mobileRobot.transform.Find(lf_hip_link).transform;

        string lf_u_leg_link = body_link + "/lf1/lf2/lpf2";
        robotLinkPositions[11] = mobileRobot.transform.Find(lf_u_leg_link).transform;

        string lf_l_leg_link = body_link + "/lf1/lf2/lf3";
        robotLinkPositions[12] = mobileRobot.transform.Find(lf_l_leg_link).transform;


        if (isDebugMode)
        {
            Debug.Log(robotLinkPositions[1].position + " " + robotLinkPositions[1].rotation);
        }
    }

    /// <summary>
    /// デバッグ時の文字列生成用関数
    /// </summary>
    string Vector3ToString(Vector3 vec, string titile = "")
    {
        return titile + " (" + vec[0] + ", " + vec[1] + ", " + vec[2] + ")";
    }

    /// <summary>
    /// デバッグ時の文字列生成用関数
    /// </summary>
    string RosPosVector3ToString(RosPosVector3 vec, string titile = "")
    {
        return titile + " (" + vec.x + ", " + vec.y + ", " + vec.z + ")";
    }

    /// <summary>
    /// ROSトピックを受け取った際に呼ばれるコールバック関数
    /// </summary>
    void TfUpdate(Tf tfMessage)
    {
        for (int i = 0; i < tfMessage.transforms.Length; i++)
        {
            if (tfMessage.transforms[i].child_frame_id == "base_link")
            {
                Vector3Msg rosOdomPosMsg = tfMessage.transforms[i].transform.translation;
                QuaternionMsg rosOdomQuaternionMsg = tfMessage.transforms[i].transform.rotation;
                RosPosVector3 rosOdomPos = rosOdomPosMsg.As<FLU>();
                unityOdomPos = rosOdomPos.toUnity;
                RosQuaternion rosOdomQuaternion = rosOdomQuaternionMsg.As<FLU>();
                unityOdomQuaternion = rosOdomQuaternion.toUnity;

                if (isDebugMode)
                {
                    Debug.Log(RosPosVector3ToString(rosOdomPos, "tf(ros)") + " --> " + Vector3ToString(unityOdomPos, "tf(unity)"));
                    Debug.Log(Vector3ToString(unityOdomPos, "pos"));
                }
            }

            if (tfMessage.transforms[i].child_frame_id == "odom")
            {
                Vector3Msg rosMapPosMsg = tfMessage.transforms[i].transform.translation;
                QuaternionMsg rosMapQuaternionMsg = tfMessage.transforms[i].transform.rotation;
                RosPosVector3 rosMapPos = rosMapPosMsg.As<FLU>();
                unityMapPos = rosMapPos.toUnity;
                RosQuaternion rosMapQuaternion = rosMapQuaternionMsg.As<FLU>();
                unityMapQuaternion = rosMapQuaternion.toUnity;

                if (isDebugMode)
                {
                    Debug.Log(RosPosVector3ToString(rosMapPos, "tf(ros)") + " --> " + Vector3ToString(unityMapPos, "tf(unity)"));
                    Debug.Log(Vector3ToString(unityOdomPos, "pos"));
                }
            }
            //if (tfMessage.transforms[i].child_frame_id == "odom")
            //{
            //    rosMapPosMsg = tfMessage.transforms[i].transform.translation;
            //    rosMapQuaternionMsg = tfMessage.transforms[i].transform.rotation;
            //}



            //RosPosVector3 rosMapPos = rosMapPosMsg.As<FLU>();
            //Vector3 unityMapPos = rosMapPos.toUnity;
            //RosQuaternion rosMapQuaternion = rosMapQuaternionMsg.As<FLU>();
            //Quaternion unityMapQuaternion = rosMapQuaternion.toUnity;




            //robotLinkPositions[0].position = unityOdomPos + unityMapPos;
            robotLinkPositions[0].position = unityOdomPos;
            robotLinkPositions[0].rotation = unityOdomQuaternion;
        }
    }
}
