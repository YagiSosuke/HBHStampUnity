using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*メニューを押した時のスクリプト*/

public class MenuScript : MonoBehaviour, IPointerClickHandler
{
    Image MenuImage;        //メニューボタン（？）のイメージ
    GameObject WordStumpPanel;          //言葉とスタンプのパネル

    bool MenuF;             //メニューが展開されているか

    float MoveCount;        //画面が移動するカウント
    Vector3 TempPos;        //現在位置を格納用変数

    // Start is called before the first frame update
    void Start()
    {
        MenuImage = this.GetComponent<Image>();     //イメージ格納
        WordStumpPanel = GameObject.Find("Stump und Word");         //パネル格納
        MenuF = true;          //メニューは展開されている

        MoveCount = 1;          //カウンターを0に設定（最大1）
    }

    // Update is called once per frame
    void Update()
    {
        //メニューが展開されているなら
        if (MenuF)
        {
            if(MoveCount < 1)
            {
                MoveCount += 0.05f;
            }

            WordStumpPanel.transform.localPosition = Vector3.Lerp(TempPos, new Vector3(0, -900, 0), 1 - (1 - MoveCount) * (1 - MoveCount));
        }
        //メニューが展開されていないなら
        else
        {
            if (MoveCount < 1)
            {
                MoveCount += 0.05f;
            }

            WordStumpPanel.transform.localPosition = Vector3.Lerp(TempPos, new Vector3(0, 0, 0), 1 - (1-MoveCount)*(1-MoveCount));
        }
    }

    //メニュー展開パネルをクリックしたら
    public void OnPointerClick(PointerEventData pointerData)
    {
        MenuF = !MenuF;
        MoveCount = 0;
        TempPos = WordStumpPanel.transform.localPosition;
    }
}
