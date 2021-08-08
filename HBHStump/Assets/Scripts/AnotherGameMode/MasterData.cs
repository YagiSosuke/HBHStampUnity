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
    public ScreenMode screenMode = ScreenMode.Title;

    //ゲームのステータス
    public float remainingTime = 60*3;
    public float score;            //得点

    //変化させたオブジェクト
    public List<Sprite> newObjects = new List<Sprite>();

    //時間を表示するテキスト
    [SerializeField] Text remainingTimeText;
    [SerializeField] Text ScoreText;



    //別スクリプトからメソッド呼び出し用
    [Header("別スクリプトからメソッド呼び出し用")]
    [SerializeField] ResultPanelControl resultPanelControl;




    //スコアを加算する
    //変化させたオブジェクトを記憶
    public void AddScore(GameObject newObj)
    {
        score++;

        Image img = newObj.transform.GetChild(0).GetComponent<Image>();
        newObjects.Add(img.sprite);
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (screenMode == ScreenMode.Title) {
            //モード変更時に呼び出す
            //CharactorChangePos.OperationCharctor.getInstance().OperatonCharctorStart();
        }
        else if (screenMode == ScreenMode.Game)
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

                resultPanelControl.DisplayPanel();
                resultPanelControl.DisplayCharacter();
                resultPanelControl.DisplayScore();
            }
        }
        else if(screenMode == ScreenMode.Result)
        {

        }
    }
}
