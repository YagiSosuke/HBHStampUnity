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
    public static MasterData Instance;
    MasterData() { if (!Instance) Instance = this; }

    public RecordPlayData recordPlayData;

    //ゲームのステータス
    [SerializeField] float currentTime;
    public float TimeLimit { get; private set; } = 60 * 3;
    public float CurrentTime { get { return currentTime; } set { currentTime = value; } }//TODO: アクセス領域を要変更
    public int   Score { get; private set; } = 0;

    //変化させたオブジェクト
    public List<Sprite> changeObjs = new List<Sprite>();
    //スタンプに読み込む文字群 TODO: 何に使っている。濁点の位置は変えたほうがいいかも
    public readonly List<string> wordSample = new List<string>()
                                    { "あ", "い", "う", "え", "お",
                                      "か", "き", "く", "け", "こ",
                                      "さ", "し", "す", "せ", "そ",
                                      "た", "ち", "つ", "て", "と",
                                      "な", "に", "ぬ", "ね", "の",
                                      "は", "ひ", "ふ", "へ", "ほ",
                                      "ま", "み", "む", "め", "も",
                                      "や", "ゆ", "よ",
                                      "ら", "り", "る", "れ", "ろ",
                                      "わ", "を", "ん",
                                      "が", "ぎ", "ぐ", "げ", "ご",
                                      "ざ", "じ", "ず", "ぜ", "ぞ",
                                      "だ", "ぢ", "づ", "で", "ど",
                                      "ば", "び", "ぶ", "べ", "ぼ",
                                      "ぱ", "ぴ", "ぷ", "ぺ", "ぽ",
    };

    public void Initialize()
    {
        changeObjs = new List<Sprite>();
        CurrentTime = TimeLimit;
        Score = 0;
    }
    public void AddScore(Sprite newCharaSprite)
    {
        Score++;
        changeObjs.Add(newCharaSprite);
    }

    //ボタンを押した時に変化する文字を取得する
    public string getNextWord(string str, string direction)
    {
        if (direction == "right")
        {
            switch (str)
            {
                case "お": return "あ";
                case "こ": return "か";
                case "そ": return "さ";
                case "と": return "た";
                case "の": return "な";
                case "ほ": return "は";
                case "も": return "ま";
                case "よ": return "や";
                case "ろ": return "ら";
                case "ん": return "わ";
                case "ご": return "が";
                case "ぞ": return "ざ";
                case "ど": return "だ";
                case "ぼ": return "ば";
                case "ぽ": return "ぱ";
                default:   return wordSample[wordSample.IndexOf(str) + 1];
            }
        }
        else if(direction == "left")
        {
            switch (str)
            {
                case "あ": return "お";
                case "か": return "こ";
                case "さ": return "そ";
                case "た": return "と";
                case "な": return "の";
                case "は": return "ほ";
                case "ま": return "も";
                case "や": return "よ";
                case "ら": return "ろ";
                case "わ": return "ん";
                case "が": return "ご";
                case "ざ": return "ぞ";
                case "だ": return "ど";
                case "ば": return "ぼ";
                case "ぱ": return "ぽ";
                default:   return wordSample[wordSample.IndexOf(str) - 1];
            }
        }
        else
        {
            return "error";
        }
    }
}
