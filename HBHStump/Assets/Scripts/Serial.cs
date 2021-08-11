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

    }

    //シリアル通信でデータを受け取った時
    public void ReadData()
    {
        while (this.isLoop)
        {

            string message = serial.ReadLine();
            Debug.Log("message;" + message);

            if (message == "Head")
            {
                setParts("頭");
            }
            else if (message == "Body")
            {
                setParts("体");
            }
            else if (message == "Hip")
            {
                setParts("尻");
            }

            //設定
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
                    //文字の判定
                    #region
                    case "a.png":
                        ButtonNameChange.TempWord = "あ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "i.png":
                        ButtonNameChange.TempWord = "い";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "u.png":
                        ButtonNameChange.TempWord = "う";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "e.png":
                        ButtonNameChange.TempWord = "え";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録                        
                        break;
                    case "o.png":
                        ButtonNameChange.TempWord = "お";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ka.png":
                        ButtonNameChange.TempWord = "か";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ki.png":
                        ButtonNameChange.TempWord = "き";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ku.png":
                        ButtonNameChange.TempWord = "く";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ke.png":
                        ButtonNameChange.TempWord = "け";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ko.png":
                        ButtonNameChange.TempWord = "こ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "sa.png":
                        ButtonNameChange.TempWord = "さ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "si.png":
                        ButtonNameChange.TempWord = "し";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "su.png":
                        ButtonNameChange.TempWord = "す";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "se.png":
                        ButtonNameChange.TempWord = "せ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "so.png":
                        ButtonNameChange.TempWord = "そ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ta.png":
                        ButtonNameChange.TempWord = "た";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ti.png":
                        ButtonNameChange.TempWord = "ち";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "tu.png":
                        ButtonNameChange.TempWord = "つ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "te.png":
                        ButtonNameChange.TempWord = "て";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "to.png":
                        ButtonNameChange.TempWord = "と";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "na.png":
                        ButtonNameChange.TempWord = "な";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ni.png":
                        ButtonNameChange.TempWord = "に";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "nu.png":
                        ButtonNameChange.TempWord = "ぬ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ne.png":
                        ButtonNameChange.TempWord = "ね";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "no.png":
                        ButtonNameChange.TempWord = "の";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ha.png":
                        ButtonNameChange.TempWord = "は";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "hi.png":
                        ButtonNameChange.TempWord = "ひ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "hu.png":
                        ButtonNameChange.TempWord = "ふ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "he.png":
                        ButtonNameChange.TempWord = "へ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ho.png":
                        ButtonNameChange.TempWord = "ほ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ma.png":
                        ButtonNameChange.TempWord = "ま";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "mi.png":
                        ButtonNameChange.TempWord = "み";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "mu.png":
                        ButtonNameChange.TempWord = "む";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "me.png":
                        ButtonNameChange.TempWord = "め";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "mo.png":
                        ButtonNameChange.TempWord = "も";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ya.png":
                        ButtonNameChange.TempWord = "や";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "yu.png":
                        ButtonNameChange.TempWord = "ゆ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "yo.png":
                        ButtonNameChange.TempWord = "よ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ra.png":
                        ButtonNameChange.TempWord = "ら";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ri.png":
                        ButtonNameChange.TempWord = "り";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ru.png":
                        ButtonNameChange.TempWord = "る";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "re.png":
                        ButtonNameChange.TempWord = "れ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ro.png":
                        ButtonNameChange.TempWord = "ろ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "wa.png":
                        ButtonNameChange.TempWord = "わ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "wo.png":
                        ButtonNameChange.TempWord = "を";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "nn.png":
                        ButtonNameChange.TempWord = "ん";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ga.png":
                        ButtonNameChange.TempWord = "が";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "gi.png":
                        ButtonNameChange.TempWord = "ぎ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "gu.png":
                        ButtonNameChange.TempWord = "ぐ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ge.png":
                        ButtonNameChange.TempWord = "げ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "go.png":
                        ButtonNameChange.TempWord = "ご";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "za.png":
                        ButtonNameChange.TempWord = "ざ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "zi.png":
                        ButtonNameChange.TempWord = "じ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "zu.png":
                        ButtonNameChange.TempWord = "ず";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ze.png":
                        ButtonNameChange.TempWord = "ぜ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "zo.png":
                        ButtonNameChange.TempWord = "ぞ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "da.png":
                        ButtonNameChange.TempWord = "だ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "di.png":
                        ButtonNameChange.TempWord = "ぢ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "du.png":
                        ButtonNameChange.TempWord = "づ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "de.png":
                        ButtonNameChange.TempWord = "で";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "do.png":
                        ButtonNameChange.TempWord = "ど";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "ba.png":
                        ButtonNameChange.TempWord = "ば";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "bi.png":
                        ButtonNameChange.TempWord = "び";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "bu.png":
                        ButtonNameChange.TempWord = "ぶ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "be.png":
                        ButtonNameChange.TempWord = "べ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "bo.png":
                        ButtonNameChange.TempWord = "ぼ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "pa.png":
                        ButtonNameChange.TempWord = "ぱ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "pi.png":
                        ButtonNameChange.TempWord = "ぴ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "pu.png":
                        ButtonNameChange.TempWord = "ぷ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "pe.png":
                        ButtonNameChange.TempWord = "ぺ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    case "po.png":
                        ButtonNameChange.TempWord = "ぽ";
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(ButtonNameChange.TempWord);  //現在設定されているパーツに対応する文字を登録
                        break;
                    #endregion

                    //スタンプ位置の判定
                    #region
                    case "0,0":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[0, 0] = true;
                        break;
                    case "1,0":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[1, 0] = true;
                        break;
                    case "2,0":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[2, 0] = true;
                        break;
                    case "3,0":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[3, 0] = true;
                        break;
                    case "4,0":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[4, 0] = true;
                        break;
                    case "0,1":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[0, 1] = true;
                        break;
                    case "1,1":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[1, 1] = true;
                        break;
                    case "2,1":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[2, 1] = true;
                        break;
                    case "3,1":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[3, 1] = true;
                        break;
                    case "4,1":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[4, 1] = true;
                        break;
                    case "0,2":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[0, 2] = true;
                        break;
                    case "1,2":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[1, 2] = true;
                        break;
                    case "2,2":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[2, 2] = true;
                        break;
                    case "3,2":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[3, 2] = true;
                        break;
                    case "4,2":
                        StumpScript.TempStump = stampPartsName;    //変数に設定
                        PushF[4, 2] = true;
                        break;
                        #endregion
                }
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
        //StumpScript.TempStump = parts;
        //StumpImageScript.NowParts.text = parts;
    }



    //設定スクリプト
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
}