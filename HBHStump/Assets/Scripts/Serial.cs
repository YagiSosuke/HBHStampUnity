using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class Serial : MonoBehaviour
{
    public string portName = "COM8";
    public int baurate = 115200;
    public string stampPartsName = "頭";
    
    public SerialPort serial;    //変更
    bool isLoop = true;
    
    string NowWordButton = "endz";      //現在の言葉ボタンの選択状態を示す
    Text[] IDGroup = new Text[10];                     //IDを表示するテキストたち

    public static bool[,] PushF = new bool[5, 3];
    public static bool cardReadF = false;

    [SerializeField] StumpImageScript stumpImageScript;     //現在のパーツを視覚的に表示するスクリプト
    [SerializeField] GameObject disconnectPanel;            //切断されたことを示すパネル

    void Start()
    {
        //シリアルを開く
        if (SerialCheck.instance)
        {
            portName = SerialCheck.instance.comNumber.ToString();
        }
        while (serial == null)
        {
            Open();
        }
        Debug.Log("Open done");
        SerialReadWordAndParts();

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                PushF[i, j] = false;
            }
        }

        ConnectCheck().Forget();
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N)){
            Debug.Log("呼び出し");
            serial.Write("callz");
        }
        if (cardReadF)
        {
            StartCoroutine(CardReadFlagDown());
        }
    }

    //シリアル通信でデータを受け取った時
    public void ReadData()
    {
        while (this.isLoop)
        {
            string message = serial.ReadLine();
            Debug.Log("message:" + message);
            

            if (NowWordButton == "endz")
            {
                //文字、座標入力をセット
                switch (message)
                {
                    //接続済みか確認
                    case "connect":
                        break;

                    //カードの読み込み
                    case "CardRead":
                        cardReadF = true;
                        break;

                    //カードやパネルが読み込まれていないとき
                    #region
                    case "notNewPanelRead":
                        for (int i = 0; i < 5; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                if (PushF[i, j])
                                {
                                    PushF[i, j] = false;
                                }
                            }
                        }
                        break;
                    #endregion

                    //パーツの設定
                    #region
                    case "Head":
                        setParts("頭");
                        break;
                    case "Body":
                        setParts("体");
                        break;
                    case "Hip":
                        setParts("尻");
                        break;
                    #endregion

                    //文字の判定
                    #region
                    case "a.png":
                        ButtonNameChange.TempWord = "あ";
                        break;
                    case "i.png":
                        ButtonNameChange.TempWord = "い";
                        break;
                    case "u.png":
                        ButtonNameChange.TempWord = "う";
                        break;
                    case "e.png":
                        ButtonNameChange.TempWord = "え";
                        break;
                    case "o.png":
                        ButtonNameChange.TempWord = "お";
                        break;
                    case "ka.png":
                        ButtonNameChange.TempWord = "か";
                        break;
                    case "ki.png":
                        ButtonNameChange.TempWord = "き";
                        break;
                    case "ku.png":
                        ButtonNameChange.TempWord = "く";
                        break;
                    case "ke.png":
                        ButtonNameChange.TempWord = "け";
                        break;
                    case "ko.png":
                        ButtonNameChange.TempWord = "こ";
                        break;
                    case "sa.png":
                        ButtonNameChange.TempWord = "さ";
                        break;
                    case "si.png":
                        ButtonNameChange.TempWord = "し";
                        break;
                    case "su.png":
                        ButtonNameChange.TempWord = "す";
                        break;
                    case "se.png":
                        ButtonNameChange.TempWord = "せ";
                        break;
                    case "so.png":
                        ButtonNameChange.TempWord = "そ";
                        break;
                    case "ta.png":
                        ButtonNameChange.TempWord = "た";
                        break;
                    case "ti.png":
                        ButtonNameChange.TempWord = "ち";
                        break;
                    case "tu.png":
                        ButtonNameChange.TempWord = "つ";
                        break;
                    case "te.png":
                        ButtonNameChange.TempWord = "て";
                        break;
                    case "to.png":
                        ButtonNameChange.TempWord = "と";
                        break;
                    case "na.png":
                        ButtonNameChange.TempWord = "な";
                        break;
                    case "ni.png":
                        ButtonNameChange.TempWord = "に";
                        break;
                    case "nu.png":
                        ButtonNameChange.TempWord = "ぬ";
                        break;
                    case "ne.png":
                        ButtonNameChange.TempWord = "ね";
                        break;
                    case "no.png":
                        ButtonNameChange.TempWord = "の";
                        break;
                    case "ha.png":
                        ButtonNameChange.TempWord = "は";
                        break;
                    case "hi.png":
                        ButtonNameChange.TempWord = "ひ";
                        break;
                    case "hu.png":
                        ButtonNameChange.TempWord = "ふ";
                        break;
                    case "he.png":
                        ButtonNameChange.TempWord = "へ";
                        break;
                    case "ho.png":
                        ButtonNameChange.TempWord = "ほ";
                        break;
                    case "ma.png":
                        ButtonNameChange.TempWord = "ま";
                        break;
                    case "mi.png":
                        ButtonNameChange.TempWord = "み";
                        break;
                    case "mu.png":
                        ButtonNameChange.TempWord = "む";
                        break;
                    case "me.png":
                        ButtonNameChange.TempWord = "め";
                        break;
                    case "mo.png":
                        ButtonNameChange.TempWord = "も";
                        break;
                    case "ya.png":
                        ButtonNameChange.TempWord = "や";
                        break;
                    case "yu.png":
                        ButtonNameChange.TempWord = "ゆ";
                        break;
                    case "yo.png":
                        ButtonNameChange.TempWord = "よ";
                        break;
                    case "ra.png":
                        ButtonNameChange.TempWord = "ら";
                        break;
                    case "ri.png":
                        ButtonNameChange.TempWord = "り";
                        break;
                    case "ru.png":
                        ButtonNameChange.TempWord = "る";
                        break;
                    case "re.png":
                        ButtonNameChange.TempWord = "れ";
                        break;
                    case "ro.png":
                        ButtonNameChange.TempWord = "ろ";
                        break;
                    case "wa.png":
                        ButtonNameChange.TempWord = "わ";
                        break;
                    case "wo.png":
                        ButtonNameChange.TempWord = "を";
                        break;
                    case "nn.png":
                        ButtonNameChange.TempWord = "ん";
                        break;
                    case "ga.png":
                        ButtonNameChange.TempWord = "が";
                        break;
                    case "gi.png":
                        ButtonNameChange.TempWord = "ぎ";
                        break;
                    case "gu.png":
                        ButtonNameChange.TempWord = "ぐ";
                        break;
                    case "ge.png":
                        ButtonNameChange.TempWord = "げ";
                        break;
                    case "go.png":
                        ButtonNameChange.TempWord = "ご";
                        break;
                    case "za.png":
                        ButtonNameChange.TempWord = "ざ";
                        break;
                    case "zi.png":
                        ButtonNameChange.TempWord = "じ";
                        break;
                    case "zu.png":
                        ButtonNameChange.TempWord = "ず";
                        break;
                    case "ze.png":
                        ButtonNameChange.TempWord = "ぜ";
                        break;
                    case "zo.png":
                        ButtonNameChange.TempWord = "ぞ";
                        break;
                    case "da.png":
                        ButtonNameChange.TempWord = "だ";
                        break;
                    case "di.png":
                        ButtonNameChange.TempWord = "ぢ";
                        break;
                    case "du.png":
                        ButtonNameChange.TempWord = "づ";
                        break;
                    case "de.png":
                        ButtonNameChange.TempWord = "で";
                        break;
                    case "do.png":
                        ButtonNameChange.TempWord = "ど";
                        break;
                    case "ba.png":
                        ButtonNameChange.TempWord = "ば";
                        break;
                    case "bi.png":
                        ButtonNameChange.TempWord = "び";
                        break;
                    case "bu.png":
                        ButtonNameChange.TempWord = "ぶ";
                        break;
                    case "be.png":
                        ButtonNameChange.TempWord = "べ";
                        break;
                    case "bo.png":
                        ButtonNameChange.TempWord = "ぼ";
                        break;
                    case "pa.png":
                        ButtonNameChange.TempWord = "ぱ";
                        break;
                    case "pi.png":
                        ButtonNameChange.TempWord = "ぴ";
                        break;
                    case "pu.png":
                        ButtonNameChange.TempWord = "ぷ";
                        break;
                    case "pe.png":
                        ButtonNameChange.TempWord = "ぺ";
                        break;
                    case "po.png":
                        ButtonNameChange.TempWord = "ぽ";
                        break;
                    #endregion

                    //スタンプ位置の判定
                    #region
                    case "0,0":
                        PushF[0, 0] = true;
                        break;
                    case "1,0":
                        PushF[1, 0] = true;
                        break;
                    case "2,0":
                        PushF[2, 0] = true;
                        break;
                    case "3,0":
                        PushF[3, 0] = true;
                        break;
                    case "4,0":
                        PushF[4, 0] = true;
                        break;
                    case "0,1":
                        PushF[0, 1] = true;
                        break;
                    case "1,1":
                        PushF[1, 1] = true;
                        break;
                    case "2,1":
                        PushF[2, 1] = true;
                        break;
                    case "3,1":
                        PushF[3, 1] = true;
                        break;
                    case "4,1":
                        PushF[4, 1] = true;
                        break;
                    case "0,2":
                        PushF[0, 2] = true;
                        break;
                    case "1,2":
                        PushF[1, 2] = true;
                        break;
                    case "2,2":
                        PushF[2, 2] = true;
                        break;
                    case "3,2":
                        PushF[3, 2] = true;
                        break;
                    case "4,2":
                        PushF[4, 2] = true;
                        break;
                    #endregion
                }

                StumpScript.stampPartsWord[StumpScript.TempStump] = ButtonNameChange.TempWord;  //現在設定されているパーツに対応する文字を登録
            }
        }
    }

    //アプリケーション終了時呼び出し
    private void OnApplicationQuit()
    {
        if (serial != null) Close();
    }

    public void Open()
    {
        serial = new SerialPort(portName, baurate, Parity.None, 8, StopBits.One);

        try
        {
            serial.Open();
            isLoop = true;
            serial.ReadTimeout = 1000;          //タイムアウトするまでの時間(ms) - 終了時に必要
                                                //操作しないと勝手にタイムアウトする
            Scheduler.ThreadPool.Schedule(() => ReadData()).AddTo(this);
            Debug.Log("port open correct!");

            disconnectPanel.SetActive(false);
        }
        catch (Exception e)
        {
            serial = null;
            Debug.Log("can not open serial port");
            Debug.LogException(e);
        }
    }
    public void OpenCheck()
    {
        serial = new SerialPort(portName, baurate, Parity.None, 8, StopBits.One);

        serial.Open();
        isLoop = true;
        serial.ReadTimeout = 1000;          //タイムアウトするまでの時間(ms) - 終了時に必要
                                            //操作しないと勝手にタイムアウトする
        Scheduler.ThreadPool.Schedule(() => ReadData()).AddTo(this);
        Debug.Log("port open correct!");
        SerialReadWordAndParts();

        disconnectPanel.SetActive(false);
    }

    public void Close()
    {
        try
        {
            this.isLoop = false;
            this.serial.Close();
            this.serial = null;
            Debug.Log("port close correct!");
        }
        catch (Exception e)
        {
            Debug.Log("can not close serial port");
            Debug.LogException(e);
        }
    }

    public void setParts(string parts)
    {
        Debug.Log("実行" + parts);
        StumpScript.TempStump = parts;
    }


    //タイムアウトしないように定期的に接続されているか確認する処理
    public async UniTask ConnectCheck()
    {
        while (true)
        {
            await UniTask.Delay(500, cancellationToken: this.GetCancellationTokenOnDestroy());
            try
            {
                serial.Write("Checkz");
            }
            catch
            {
                disconnectPanel.SetActive(true);
                Debug.Log("noConnect");
            }
        }
    }

    //現在デバイスに設定されている文字と部位をを取得する
    public void SerialReadWordAndParts()
    {
        serial.Write("NowDataz");
    }

    //言葉を設定したとき
    public void SetWordSub(string message)
    {
        message = message.Replace("list:", "");
        string[] M = message.Split(',');
        for(int i = 0; i < 10; i++)
        {
            IDGroup[i].text = M[i];
        }
    }

    //カードで文字を読み込んだフラグを下げたいとき
    private IEnumerator CardReadFlagDown()
    {
        yield return null;
        cardReadF = false;
    }
    //スタンプを押したフラグを下げたいとき
    private IEnumerator PushFlagDown(int x, int y)
    {
        yield return null;
        PushF[x, y] = false;
    }

    public bool pushCheck()
    {
        for (int i = 0; i < 15; i++)
        {
            if (Serial.PushF[i % 5, i / 5])
            {
                return true;
            }
        }
        return false;
    }
}