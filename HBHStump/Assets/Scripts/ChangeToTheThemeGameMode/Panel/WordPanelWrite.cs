using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*WordPanelの描画順設定*/

public class WordPanelWrite : MonoBehaviour
{
    public bool NomalModeF = true;     //現在表示しているのはノーマルか?

    GameObject WordNomal;       //普通のあいうえお表
    Vector3 NomalPos;

    GameObject WordAdd;         //濁点など
    Vector3 AddPos;

    // Start is called before the first frame update
    void Start()
    {
        WordNomal = GameObject.Find("WordPanelNomal");
        NomalPos = WordNomal.transform.localPosition;

        WordAdd = GameObject.Find("WordPanelAdd");
        AddPos = WordAdd.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (NomalModeF)
        {
            WordNomal.transform.localPosition = NomalPos;
            WordNomal.transform.SetSiblingIndex(1);
            WordAdd.transform.localPosition = AddPos;
            WordAdd.transform.SetSiblingIndex(0);
        }
        else
        {
            WordNomal.transform.localPosition = AddPos;
            WordNomal.transform.SetSiblingIndex(0);
            WordAdd.transform.localPosition = NomalPos;
            WordAdd.transform.SetSiblingIndex(1);
        }
    }

    //ボタンをクリックしたら
    public void ButtonClick()
    {
        NomalModeF = !NomalModeF;       //NomalModeFを反転させる
    }
}
