using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SerialTest : MonoBehaviour
{/*

    public string portName = "COM8";
    public int baurate = 115200;

    public static SerialPort serial;    //変更

    string NowWordButton = "endz";      //現在の言葉ボタンの選択状態を示す
    bool isLoop = true;

    void Start()
    {
        if (serial == null) Open();
    }

    void Update()
    {

    }

    //シリアル通信でデータを受け取った時
    public void ReadData()
    {
        string message = serial.ReadLine();
        Debug.Log("message;" + message);

        //送信された言葉を調べる
        if (message == "Hoge")
        {
            //言葉が送信されたときの処理
        }
    }

    //アプリケーション終了時呼び出し
    //private void OnApplicationQuit()
    //オブジェクト破棄時呼び出し
    void OnDestroy()
    {
        this.isLoop = false;
        Close();
        this.serial.Close();
    }

    public void Open()
    {
        serial = new SerialPort(portName, baurate, Parity.None, 8, StopBits.One);

        try
        {
            serial.Open();
            Scheduler.ThreadPool.Schedule(() => ReadData()).AddTo(this);
        }
        catch (Exception e)
        {
            Debug.Log("can not open serial port");
        }
    }

    public void Close()
    {
        if (serial != null && serial.IsOpen)
        {
            serial.Close();
            serial.Dispose();
        }
    }
    */
}