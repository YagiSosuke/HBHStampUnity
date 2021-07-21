﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*言葉を選択するスクリプト*/

public class ButtonNameChange : MonoBehaviour
{
    Text ButtonText;    //ボタンのテキスト
    public static string TempWord;         //選択した言葉
    [SerializeField] GameObject Frame;      //選択したボタンの枠

    // Start is called before the first frame update
    void Start()
    {
        ButtonText = this.transform.GetChild(0).gameObject.GetComponent<Text>();

        ButtonText.fontSize = 40;
        ButtonText.text = this.gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //言葉をセットする
    public void SetWord()
    {
        TempWord = ButtonText.text;
        StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).setWord(TempWord);  //現在設定されているパーツに対応する文字を登録
        StumpScript.nowWordText[StumpScript.TempStump].text = StumpScript.GetPartsWord(StumpScript.stampPartsWord, StumpScript.TempStump).getWord(); //記録されている文字を視覚的に表す
        //Frame.gameObject.transform.position = this.transform.position;
    }
}
