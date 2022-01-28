using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*言葉を選択するスクリプト*/

public class WordPanel : MonoBehaviour
{
    [SerializeField] Text ButtonText;    //ボタンのテキスト
    [SerializeField] GameObject Frame;      //選択したボタンの枠


    void Start()
    {
        ButtonText.fontSize = 40;
        ButtonText.text = this.gameObject.name;
    }
       
    //言葉をセットする
    public void SetWord()
    {
        Stamp.Instance.SetWord(ButtonText.text);
    }
}
