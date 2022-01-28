using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*五十音表パネルの描画順設定*/

public class WordPanelWrite : MonoBehaviour
{
    bool nomalModeF = true;     //現在表示しているのは通常の五十音表か?

    [SerializeField] GameObject wordNomal;       //普通のあいうえお表
    Vector3 nomalPos;
    [SerializeField] GameObject wordAdd;         //濁点など
    Vector3 addPos;


    void Start()
    {
        nomalPos = wordNomal.transform.localPosition;
        addPos = wordAdd.transform.localPosition;
    }

    //ボタンをクリックしたら
    public void ButtonClick()
    {
        nomalModeF = !nomalModeF;       //五十音表、濁点表の描画順を変える
        
        if (nomalModeF)
        {
            wordNomal.transform.localPosition = nomalPos;
            wordNomal.transform.SetSiblingIndex(1);
            wordAdd.transform.localPosition = addPos;
            wordAdd.transform.SetSiblingIndex(0);
        }
        else
        {
            wordNomal.transform.localPosition = addPos;
            wordNomal.transform.SetSiblingIndex(0);
            wordAdd.transform.localPosition = nomalPos;
            wordAdd.transform.SetSiblingIndex(1);
        }
    }
}
