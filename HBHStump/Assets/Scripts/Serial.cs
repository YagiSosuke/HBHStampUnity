using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Serial : MonoBehaviour
{
    public string portName = "COM8";
    public int baurate = 115200;
    public string stampPartsName = "頭";

    //public static SerialPort serial;    //変更
    public SerialPort serial;    //変更
    bool isLoop = true;
    
    string NowWordButton = "endz";      //現在の言葉ボタンの選択状態を示す
    Text[] IDGroup = new Text[10];                     //IDを表示するテキストたち

    public static bool[,] PushF = new bool[5, 3];
    public static bool cardReadF = false;

    [SerializeField] StumpImageScript stumpImageScript;     //現在のパーツを視覚的に表示するスクリプト

    void Start()
    {
        Debug.Log(stampPartsName + "の実行");
        if(serial == null) Open();
        
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                PushF[i, j] = false;
            }
        }
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
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if (PushF[i, j])
                {
                    IEnumerator coroutine = PushFlagDown(i, j);
                    StartCoroutine(coroutine);
                }
            }
        }
    }

    //シリアル通信でデータを受け取った時
    public void ReadData()
    {
        while (this.isLoop)
        {
            string message = serial.ReadLine();
            Debug.Log("message:" + message);
            

            //設定 - おそらくいらない
            #region
            if (true) {
                switch (NowWordButton)
                {
                    case "az":
                        SetWordSub(message);
                        break;
                    case "kz":
                        SetWordSub(message);
                        break;
                    case "sz":
                        SetWordSub(message);
                        break;
                    case "tz":
                        SetWordSub(message);
                        break;
                    case "nz":
                        SetWordSub(message);
                        break;
                    case "hz":
                        SetWordSub(message);
                        break;
                    case "mz":
                        SetWordSub(message);
                        break;
                    case "yz":
                        SetWordSub(message);
                        break;
                    case "rz":
                        SetWordSub(message);
                        break;
                    case "wz":
                        SetWordSub(message);
                        break;
                }
            }
            #endregion

            if (NowWordButton == "endz")
            {
                //文字、座標入力をセット
                switch (message)
                {
                    //カードの読み込み
                    case "CardRead":
                        cardReadF = true;
                        break;

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
    //void OnDestroy()
    {
        this.isLoop = false;
        Close();
        //this.serial.Close();
    }

    public void Open()
    {
        Debug.Log(stampPartsName + "のポートを開く");
        serial = new SerialPort(portName, baurate, Parity.None, 8, StopBits.One);
        Debug.Log("serial = " + serial);

        try
        {
            serial.Open();
            Scheduler.ThreadPool.Schedule(() => ReadData()).AddTo(this);
        }
        catch (Exception e)
        {
            Debug.Log("can not open serial port");
            Debug.LogException(e);
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

    public void setParts(string parts)
    {
        Debug.Log("実行" + parts);
        StumpScript.TempStump = parts;
    }



    //設定スクリプト
    //たぶんいらない
    #region
    public void AButton()
    {
        if (GameObject.Find("AToggle").GetComponent<Toggle>().isOn)
        {
            if (NowWordButton != "az")
            {
               serial.Write("az");
            }
            NowWordButton = "az";
        }
    }
    public void KButton()
    {
        if (GameObject.Find("KToggle").GetComponent<Toggle>().isOn)
        {
            if (NowWordButton != "kz")
            {
                serial.Write("kz");
            }
            NowWordButton = "kz";
        }
    }
    public void SButton()
    {
        if (GameObject.Find("SToggle").GetComponent<Toggle>().isOn)
        {
            if (NowWordButton != "sz")
            {
                serial.Write("sz");
            }
            NowWordButton = "sz";
        }
    }
    public void TButton()
    {
        if (GameObject.Find("TToggle").GetComponent<Toggle>().isOn)
        {
            if (NowWordButton != "tz")
            {
                serial.Write("tz");
            }
            NowWordButton = "tz";
        }
    }
    public void NButton()
    {
        if (GameObject.Find("NToggle").GetComponent<Toggle>().isOn)
        {
            if (NowWordButton != "nz")
            {
                serial.Write("nz");
            }
            NowWordButton = "nz";
        }
    }
    public void HButton()
    {
        if (GameObject.Find("HToggle").GetComponent<Toggle>().isOn)
        {
            if (NowWordButton != "hz")
            {
                serial.Write("hz");
            }
            NowWordButton = "hz";
        }
    }
    public void MButton()
    {
        if (GameObject.Find("MToggle").GetComponent<Toggle>().isOn)
        {
            if (NowWordButton != "mz")
            {
                serial.Write("mz");
            }
            NowWordButton = "mz";
        }
    }
    public void YButton()
    {
        if (GameObject.Find("YToggle").GetComponent<Toggle>().isOn)
        {
            if (NowWordButton != "yz")
            {
                serial.Write("yz");
            }
            NowWordButton = "yz";
        }
    }
    public void RButton()
    {
        if (GameObject.Find("RToggle").GetComponent<Toggle>().isOn)
        {
            if (NowWordButton != "rz")
            {
                serial.Write("rz");
            }
            NowWordButton = "rz";
        }
    }
    public void WButton()
    {
        if (GameObject.Find("WToggle").GetComponent<Toggle>().isOn)
        {
            if (NowWordButton != "wz")
            {
                serial.Write("wz");
            }
            NowWordButton = "wz";
        }
    }
    public void EndButton()
    {
        NowWordButton = "endz";
        serial.Write("wz");
    }
    #endregion

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