using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
チュートリアルを見るか確認するパネル
*/

public class VerificationPanelScript : MonoBehaviour
{
    public bool yesF;
    public bool noF;

    public void SetUp()
    {
        yesF = false;
        noF = false;
    }

    //以下、デバイスでもできるようにする
    //「はい」の選択時
    public void VerificationYes()
    {
        yesF = true;
    }

    //「いいえ」の選択時
    public void VerificationNo()
    {
        noF = true;
    }
}
