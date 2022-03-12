using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

/*
デバイスと通信可能かチェックするクラス 
シングルトンで作成
*/

public class SerialCheck : MonoBehaviour
{
    public static SerialCheck instance;

    [SerializeField] ConnectionTextControler connectionTextControler;
    [SerializeField] LoadImageAnimation loadImageAnimation;

    [SerializeField] GameObject LoadingImage;
    [SerializeField] Button connectionButton;
    [SerializeField] Button disconnectionButton;
    [SerializeField] Button startButton;


    public COMNumber comNumber = COMNumber.COM0;
    [SerializeField] Dropdown comNumDropdown;       //COM番号を指定するドロップダウンメニュー

    [SerializeField] GameObject connectingPanel;    //接続中に表示するパネル

    public int baurate = 115200;

    public SerialPort serial;
    bool isLoop = true;
    
    //シリアル通信でデータを受け取った時
    public void ReadData()
    {
        while (this.isLoop)
        {
            string message = serial.ReadLine();         //ReadLine()が不具合? 次回起動時ができない
            Debug.Log("message:" + message);

            switch (message)
            {
                case "a.png":
                    break;
            }
        }
    }

    //アプリケーション終了時呼び出し
    void OnDestroy()
    {
        if (serial != null) Close();
    }

    public async UniTask Open()
    {
        connectingPanel.SetActive(true);

        await UniTask.DelayFrame(1, cancellationToken: this.GetCancellationTokenOnDestroy());

        string portName = comNumber.ToString();
        serial = new SerialPort(portName, baurate, Parity.None, 8, StopBits.One);

        try
        {
            serial.Open();
            isLoop = true;
            serial.ReadTimeout = 2000;          //タイムアウトするまでの時間(ms) - 終了時に必要
                                                //操作しないと勝手にタイムアウトする
            Debug.Log($"serial.ReadTimeout = {serial.ReadTimeout}");
            connectionTextControler.Clear();
            connectionTextControler.DisplayConnectionDevice(comNumber);
            Scheduler.ThreadPool.Schedule(() => ReadData()).AddTo(this);        
            Debug.Log("port open correct!");
        }
        catch (Exception e)
        {
            serial = null;
            connectionTextControler.Clear();
            connectionTextControler.DisplayErrorMessage(e);
            Debug.Log("can not open serial port");
            Debug.LogException(e);
        }
        connectingPanel.SetActive(false);
    }
    public void Close()
    {
        try
        {
            //serial.ReadTimeout = 5000;
            this.isLoop = false;
            this.serial.Close();
            this.serial.Dispose();
            this.serial = null;
            Debug.Log("port close correct!");
        }
        catch (Exception e)
        {
            Debug.Log("can not close serial port");
            Debug.LogException(e);
        }
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadingImage.SetActive(false);
        }
    }
    private void Start()
    {
        if (connectionButton)
            connectionButton.interactable = true;
        if (disconnectionButton)
            disconnectionButton.interactable = false;
        if (startButton)
            startButton.interactable = false;
    }


    //ボタンクリック時接続、切断、スタート
    public void OnConnection()
    {
        UniTask.Void(async () =>
        {
            if (serial == null)
            {
                await Open();
                if (serial != null)
                {
                    connectionButton.interactable = false;
                    disconnectionButton.interactable = true;
                    startButton.interactable = true;
                }
            }
        });
    }
    public void OnDisconnection()
    {
        if (serial != null)
        {
            Close();
            connectionTextControler.Clear();
            connectionButton.interactable = true;
            disconnectionButton.interactable = false;
            startButton.interactable = false;
        }
    }
    public void OnStartButtonClick()
    {
        if (serial != null)
        {
            Close();
        }
        SceneManager.LoadScene("ManyChangeScene");
    }

    //COM番号切り替え
    public void OnCOMNumberChange()
    {
        comNumber = (COMNumber)comNumDropdown.value;
    }
}
public enum COMNumber
{
    COM0,
    COM1,
    COM2,
    COM3,
    COM4,
    COM5,
    COM6,
    COM7,
    COM8,
    COM9,
}
