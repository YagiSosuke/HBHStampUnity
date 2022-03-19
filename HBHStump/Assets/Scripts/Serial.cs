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

    public SerialPort serial;    //変更
    bool isLoop = true;

    Text[] IDGroup = new Text[10];                     //IDを表示するテキストたち

    public static bool[,] PushF = new bool[6, 3];
    public static bool isCardRead = false;

    [SerializeField] StumpImageScript stumpImageScript;     //現在のパーツを視覚的に表示するスクリプト
    [SerializeField] GameObject disconnectPanel;            //切断されたことを示すパネル

    [field: SerializeField] public bool IsUseDevice { get; private set; }     //デバイスを使用する場合trueにする

    void Start()
    {
        PushFlugInit();

        //シリアルを開く
        if (IsUseDevice)
        {
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
            ConnectCheck().Forget();
        }
    }

    void Update()
    {
        if (isCardRead)
        {
            CardReadFlagDown().Forget();
        }
    }

    //シリアル通信でデータを受け取った時
    public void ReadData()
    {
        while (this.isLoop)
        {
            string message = serial.ReadLine();
            Debug.Log("message:" + message);

            DataAnalysis(message);
        }
    }
    public void DataAnalysis(string _message)
    {
        switch (_message)
        {
            //接続済みか確認
            case "connect":
                break;

            //カードの読み込み
            case "CardRead":
                isCardRead = true;
                break;

            #region カードやパネルが読み込まれていないとき
            case "notNewPanelRead":
                PushFlugInit();
                break;
            #endregion

            #region パーツの設定
            case "Head":
                Stamp.Instance.SetParts(Parts.Head);
                break;
            case "Body":
                Stamp.Instance.SetParts(Parts.Body);
                break;
            case "Hip":
                Stamp.Instance.SetParts(Parts.Hip);
                break;
            #endregion

            #region 文字の判定
            case "a.png":
                Stamp.Instance.SetWord("あ");
                break;
            case "i.png":
                Stamp.Instance.SetWord("い");
                break;
            case "u.png":
                Stamp.Instance.SetWord("う");
                break;
            case "e.png":
                Stamp.Instance.SetWord("え");
                break;
            case "o.png":
                Stamp.Instance.SetWord("お");
                break;
            case "ka.png":
                Stamp.Instance.SetWord("か");
                break;
            case "ki.png":
                Stamp.Instance.SetWord("き");
                break;
            case "ku.png":
                Stamp.Instance.SetWord("く");
                break;
            case "ke.png":
                Stamp.Instance.SetWord("け");
                break;
            case "ko.png":
                Stamp.Instance.SetWord("こ");
                break;
            case "sa.png":
                Stamp.Instance.SetWord("さ");
                break;
            case "si.png":
                Stamp.Instance.SetWord("し");
                break;
            case "su.png":
                Stamp.Instance.SetWord("す");
                break;
            case "se.png":
                Stamp.Instance.SetWord("せ");
                break;
            case "so.png":
                Stamp.Instance.SetWord("そ");
                break;
            case "ta.png":
                Stamp.Instance.SetWord("た");
                break;
            case "ti.png":
                Stamp.Instance.SetWord("ち");
                break;
            case "tu.png":
                Stamp.Instance.SetWord("つ");
                break;
            case "te.png":
                Stamp.Instance.SetWord("て");
                break;
            case "to.png":
                Stamp.Instance.SetWord("と");
                break;
            case "na.png":
                Stamp.Instance.SetWord("な");
                break;
            case "ni.png":
                Stamp.Instance.SetWord("に");
                break;
            case "nu.png":
                Stamp.Instance.SetWord("ぬ");
                break;
            case "ne.png":
                Stamp.Instance.SetWord("ね");
                break;
            case "no.png":
                Stamp.Instance.SetWord("の");
                break;
            case "ha.png":
                Stamp.Instance.SetWord("は");
                break;
            case "hi.png":
                Stamp.Instance.SetWord("ひ");
                break;
            case "hu.png":
                Stamp.Instance.SetWord("ふ");
                break;
            case "he.png":
                Stamp.Instance.SetWord("へ");
                break;
            case "ho.png":
                Stamp.Instance.SetWord("ほ");
                break;
            case "ma.png":
                Stamp.Instance.SetWord("ま");
                break;
            case "mi.png":
                Stamp.Instance.SetWord("み");
                break;
            case "mu.png":
                Stamp.Instance.SetWord("む");
                break;
            case "me.png":
                Stamp.Instance.SetWord("め");
                break;
            case "mo.png":
                Stamp.Instance.SetWord("も");
                break;
            case "ya.png":
                Stamp.Instance.SetWord("や");
                break;
            case "yu.png":
                Stamp.Instance.SetWord("ゆ");
                break;
            case "yo.png":
                Stamp.Instance.SetWord("よ");
                break;
            case "ra.png":
                Stamp.Instance.SetWord("ら");
                break;
            case "ri.png":
                Stamp.Instance.SetWord("り");
                break;
            case "ru.png":
                Stamp.Instance.SetWord("る");
                break;
            case "re.png":
                Stamp.Instance.SetWord("れ");
                break;
            case "ro.png":
                Stamp.Instance.SetWord("ろ");
                break;
            case "wa.png":
                Stamp.Instance.SetWord("わ");
                break;
            case "wo.png":
                Stamp.Instance.SetWord("を");
                break;
            case "nn.png":
                Stamp.Instance.SetWord("ん");
                break;
            case "ga.png":
                Stamp.Instance.SetWord("が");
                break;
            case "gi.png":
                Stamp.Instance.SetWord("ぎ");
                break;
            case "gu.png":
                Stamp.Instance.SetWord("ぐ");
                break;
            case "ge.png":
                Stamp.Instance.SetWord("げ");
                break;
            case "go.png":
                Stamp.Instance.SetWord("ご");
                break;
            case "za.png":
                Stamp.Instance.SetWord("ざ");
                break;
            case "zi.png":
                Stamp.Instance.SetWord("じ");
                break;
            case "zu.png":
                Stamp.Instance.SetWord("ず");
                break;
            case "ze.png":
                Stamp.Instance.SetWord("ぜ");
                break;
            case "zo.png":
                Stamp.Instance.SetWord("ぞ");
                break;
            case "da.png":
                Stamp.Instance.SetWord("だ");
                break;
            case "di.png":
                Stamp.Instance.SetWord("ぢ");
                break;
            case "du.png":
                Stamp.Instance.SetWord("づ");
                break;
            case "de.png":
                Stamp.Instance.SetWord("で");
                break;
            case "do.png":
                Stamp.Instance.SetWord("ど");
                break;
            case "ba.png":
                Stamp.Instance.SetWord("ば");
                break;
            case "bi.png":
                Stamp.Instance.SetWord("び");
                break;
            case "bu.png":
                Stamp.Instance.SetWord("ぶ");
                break;
            case "be.png":
                Stamp.Instance.SetWord("べ");
                break;
            case "bo.png":
                Stamp.Instance.SetWord("ぼ");
                break;
            case "pa.png":
                Stamp.Instance.SetWord("ぱ");
                break;
            case "pi.png":
                Stamp.Instance.SetWord("ぴ");
                break;
            case "pu.png":
                Stamp.Instance.SetWord("ぷ");
                break;
            case "pe.png":
                Stamp.Instance.SetWord("ぺ");
                break;
            case "po.png":
                Stamp.Instance.SetWord("ぽ");
                break;
            #endregion

            #region スタンプ位置の判定
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
            case "5,0":
                PushF[5, 0] = true;
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
            case "5,1":
                PushF[5, 1] = true;
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
            case "5,2":
                PushF[5, 2] = true;
                break;
                #endregion
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
        for (int i = 0; i < 10; i++)
        {
            IDGroup[i].text = M[i];
        }
    }

    //カードで文字を読み込んだフラグを下げたいとき
    async UniTask CardReadFlagDown()
    {
        await UniTask.DelayFrame(1);
        isCardRead = false;
    }

    public bool pushCheck()
    {
        for (int i = 0; i < 18; i++)
        {
            if (PushF[i % 6, i / 6])
            {
                return true;
            }
        }
        return false;
    }
    public void PushFlugInit()
    { 
        for (int i = 0; i< 18; i++)
        {
            PushF[i % 6, i / 6] = false;
        }            
    }
}