using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
マスタークラスを管理するスクリプト
csv書込みも行う予定
*/

public class MasterData : MonoBehaviour
{
    //ゲームのステータス
    public float remainingTime = 60*3;    //此方の変数を減らしていく
    public float remainingTimeTemp;       //制限時間は何秒か記憶用
    public float score;            //得点

    //変化させたオブジェクト
    public List<Sprite> newObjects = new List<Sprite>();

    //スタンプに読み込む文字群
    public static readonly List<string> wordSample = new List<string>()
                                    { "あ", "い", "う", "え", "お",
                                      "か", "き", "く", "け", "こ", "が", "ぎ", "ぐ", "げ", "ご",
                                      "さ", "し", "す", "せ", "そ", "ざ", "じ", "ず", "ぜ", "ぞ",
                                      "た", "ち", "つ", "て", "と", "だ", "ぢ", "づ", "で", "ど",
                                      "な", "に", "ぬ", "ね", "の",
                                      "は", "ひ", "ふ", "へ", "ほ", "ば", "び", "ぶ", "べ", "ぼ", "ぱ", "ぴ", "ぷ", "ぺ", "ぽ",
                                      "ま", "み", "む", "め", "も",
                                      "や", "ゆ", "よ",
                                      "ら", "り", "る", "れ", "ろ",
                                      "わ", "を", "ん"
    };

    //スコアを加算する
    //変化させたオブジェクトを記憶
    public void AddScore(GameObject newObj)
    {
        score++;

        Image img = newObj.transform.GetChild(0).GetComponent<Image>();
        newObjects.Add(img.sprite);
    }
    
    //ステータスを初期化する
    public void StatusInitialization()
    {
        newObjects = new List<Sprite>();
        remainingTime = remainingTimeTemp;
        score = 0;
    }

    //ボタンを押した時に変化する文字を取得する
    public static string getNextWord(string str, string direction)
    {
        if (direction == "right")
        {
            switch (str)
            {
                case "お":
                    return "あ";
                case "ご":
                    return "か";
                case "ぞ":
                    return "さ";
                case "ど":
                    return "た";
                case "の":
                    return "な";
                case "ぽ":
                    return "は";
                case "も":
                    return "ま";
                case "よ":
                    return "や";
                case "ろ":
                    return "ら";
                case "ん":
                    return "わ";
                default:
                    return wordSample[wordSample.IndexOf(str) + 1];
            }
        }
        else if(direction == "left")
        {
            switch (str)
            {
                case "あ":
                    return "お";
                case "か":
                    return "ご";
                case "さ":
                    return "ぞ";
                case "た":
                    return "ど";
                case "な":
                    return "の";
                case "は":
                    return "ぽ";
                case "ま":
                    return "も";
                case "や":
                    return "よ";
                case "ら":
                    return "ろ";
                case "わ":
                    return "ん";
                default:
                    return wordSample[wordSample.IndexOf(str) - 1];
            }
        }
        else
        {
            return "error";
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        remainingTimeTemp = remainingTime;
        score = 0;
    }
}
