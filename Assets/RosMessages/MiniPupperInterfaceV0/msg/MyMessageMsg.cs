//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.MiniPupperInterfaceV0
{
    [Serializable]
    public class MyMessageMsg : Message
    {
        public const string k_RosMessageName = "custom_topic/MyMessage";
        public override string RosMessageName => k_RosMessageName;

        public float x;
        public float y;
        public float z;
        public float qx;
        public float qy;
        public float qz;
        public float qw;

        public MyMessageMsg()
        {
            this.x = 0.0f;
            this.y = 0.0f;
            this.z = 0.0f;
            this.qx = 0.0f;
            this.qy = 0.0f;
            this.qz = 0.0f;
            this.qw = 0.0f;
        }

        public MyMessageMsg(float x, float y, float z, float qx, float qy, float qz, float qw)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.qx = qx;
            this.qy = qy;
            this.qz = qz;
            this.qw = qw;
        }

        public static MyMessageMsg Deserialize(MessageDeserializer deserializer) => new MyMessageMsg(deserializer);

        private MyMessageMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.x);
            deserializer.Read(out this.y);
            deserializer.Read(out this.z);
            deserializer.Read(out this.qx);
            deserializer.Read(out this.qy);
            deserializer.Read(out this.qz);
            deserializer.Read(out this.qw);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.x);
            serializer.Write(this.y);
            serializer.Write(this.z);
            serializer.Write(this.qx);
            serializer.Write(this.qy);
            serializer.Write(this.qz);
            serializer.Write(this.qw);
        }

        public override string ToString()
        {
            return "MyMessageMsg: " +
            "\nx: " + x.ToString() +
            "\ny: " + y.ToString() +
            "\nz: " + z.ToString() +
            "\nqx: " + qx.ToString() +
            "\nqy: " + qy.ToString() +
            "\nqz: " + qz.ToString() +
            "\nqw: " + qw.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}
