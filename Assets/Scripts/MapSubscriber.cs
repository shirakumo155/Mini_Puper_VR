using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Robotics.ROSTCPConnector;
using MapMsg = RosMessageTypes.Nav.OccupancyGridMsg;

/// <summary>
/// LiDARスキャンデータ（LaserScanMsg）を受信するためのクラス
/// 主にスキャンデータの取得および座標変換に使い、描画は別のスクリプトを用意することを想定
/// </summary>
public class MapSubscriber : MonoBehaviour
{
    // 受信するROSのトピック名
    [SerializeField] string rosTopicName = "map";
    // デバッグモードとするかどうか（デバッグモードではコンソールにログを出力）
    [SerializeField] bool isDebugMode = true;
    // カメラデータを貼り付けるRawImageオブジェクト
    [SerializeField] RawImage rawImage;

    private Texture2D texture2D;
    private sbyte[] imageData;
    private bool isMessageReceived;

    ROSConnection ros;

    /// <summary>
    /// 初期化用のイベント関数
    /// https://docs.unity3d.com/ja/2020.3/Manual/ExecutionOrder.html
    /// </summary>
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<MapMsg>(rosTopicName, MapMsgUpdate);
        //ROSConnection.instance.Subscribe<CompressedImageMsg>(rosTopicName, ImageMsgUpdate);
        texture2D = new Texture2D(1, 1, TextureFormat.R8, true);
        texture2D.wrapMode = TextureWrapMode.Clamp;
        texture2D.filterMode = FilterMode.Point;
    }

    /// <summary>
    /// 1フレーム毎に呼び出されるイベント関数
    /// https://docs.unity3d.com/ja/2020.3/Manual/ExecutionOrder.html
    /// </summary>
    void Update()
    {
        if (isMessageReceived)
        {
            ProcessMessage();
        }
    }

    /// <summary>
    /// ROSトピックを受け取った際に呼ばれるコールバック関数
    /// </summary>
    void MapMsgUpdate(MapMsg MapData)
    {
        imageData = MapData.data;
        if (MapData.info.width != texture2D.width || MapData.info.height != texture2D.height)
        {
            texture2D.Resize((int)MapData.info.width, (int)MapData.info.height);
        }
        isMessageReceived = true;

        
        if (isDebugMode)
        {
            
            Debug.Log("imageData :" + imageData);
            Debug.Log("rawImage info. width :" + MapData.info.width);
            Debug.Log("rawImage info. height :" + MapData.info.height);
        }
    }

    /// <summary>
    /// ImageMsgのデータをTextureに反映する関数
    /// </summary>
    void ProcessMessage()
    {
        texture2D.SetPixelData(imageData,0);
        //texture2D.SetPixelData(rawImage.data, 0);
        //texture2D.filterMode = FilterMode.Point;
        texture2D.Apply();
        rawImage.texture = texture2D;
        isMessageReceived = false;
    }
}