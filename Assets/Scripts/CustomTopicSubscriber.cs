using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using MyMessageMsg = RosMessageTypes.MiniPupperInterfaceV0.MyMessageMsg;
public class CustomTopicSubscriber : MonoBehaviour
{
    ROSConnection ros;

    [SerializeField] GameObject mobileRobot;
    Vector3 UnityPos;
    Quaternion UnityQuaternion;

    private int numRobotLinks = 13;

    private Transform[] robotLinkPositions;
    // 初期化時に呼ばれる
    void Start()
    {
        // ROSコネクションの取得
        ros = ROSConnection.GetOrCreateInstance();
        // サブスクライバーの登録
        ros.Subscribe<MyMessageMsg>("custom_topic", OnSubscribe);

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
    }
    // サブスクライブ時に呼ばれる
    void OnSubscribe(MyMessageMsg msg)
    {
        //Debug.Log("Subscribe : " + msg.x + ", " + msg.y + ", " + msg.z + ", " + msg.qx + ", " + msg.qy + ", " + msg.qz);
        UnityPos.x = -msg.y;
        UnityPos.y = msg.z;
        UnityPos.z = msg.x;
        UnityQuaternion.x = -msg.qy;
        UnityQuaternion.y = msg.qz;
        UnityQuaternion.z = msg.qx;
        UnityQuaternion.w = -msg.qw;
        robotLinkPositions[0].position = UnityPos;
        robotLinkPositions[0].rotation = UnityQuaternion;

    }
}
