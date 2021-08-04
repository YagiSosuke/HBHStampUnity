using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
マスタークラスを管理するスクリプト
csv書込みも行う予定
*/

public class MasterData : MonoBehaviour
{
    public static float remainingTime = 60*3;    //残り時間
    public static float score;            //得点

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //制限時間を減らしていく
        remainingTime -= Time.deltaTime;

        //時間が無くなったら結果発表
    }
}
