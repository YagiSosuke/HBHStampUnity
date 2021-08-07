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
    //画面遷移のパターン
    public enum ScreenMode
    {
        Title,
        Tutorial,
        Game,
        Result
    }
    [SerializeField] ScreenMode serializeScreenMode = ScreenMode.Title;
    public static ScreenMode screenMode;

    //ゲームのステータス
    [SerializeField] float serializeRemainingTime = 60*3;
    public static float remainingTime = 60*3;    //残り時間
    public static float score;            //得点

    //時間を表示するテキスト
    [SerializeField] Text remainingTimeText;
    [SerializeField] Text ScoreText;

    //スコアを加算する
    public void AddScore()
    {
        score++;
    }

    // Start is called before the first frame update
    void Start()
    {
        screenMode = serializeScreenMode;
        remainingTime = serializeRemainingTime;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (screenMode == ScreenMode.Game)
        {
            //制限時間を減らしていく
            remainingTime -= Time.deltaTime;
            remainingTimeText.text = remainingTime.ToString(".00");
            //得点を表示
            ScoreText.text = score.ToString() + "point";

            //キャラクター変化全般を操作する
            CharactorChangePos.OperationCharctor.getInstance().OperatonCharctorUpdate();


            //時間が無くなったら結果発表
            if(remainingTime <= 0)
            {
                remainingTimeText.text = "0";
                CharactorChangePos.OperationCharctor.getInstance().GameFinish();
                screenMode = ScreenMode.Result;
            }
        }
        else if(screenMode == ScreenMode.Result)
        {

        }
    }
}
