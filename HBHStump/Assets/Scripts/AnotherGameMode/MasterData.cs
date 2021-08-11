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
    public float remainingTime = 60*3;
    float remainingTimeTemp;       //制限時間は何秒か記憶用
    public float score;            //得点

    //変化させたオブジェクト
    public List<Sprite> newObjects = new List<Sprite>();
        
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

    // Start is called before the first frame update
    void Start()
    {
        remainingTimeTemp = remainingTime;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
