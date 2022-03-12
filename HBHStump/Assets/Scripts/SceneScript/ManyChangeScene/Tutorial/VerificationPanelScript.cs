using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
チュートリアルを見るか確認するパネル
*/

public class VerificationPanelScript : MonoBehaviour
{
    public bool isTakeTutorial;
    public bool isNotTakeTutorial;

    public void SetUp()
    {
        isTakeTutorial = false;
        isNotTakeTutorial = false;
    }
    
    //「はい」の選択時
    public void VerificationYes()
    {
        isTakeTutorial = true;
    }
    //「いいえ」の選択時
    public void VerificationNo()
    {
        isNotTakeTutorial = true;
    }
}
