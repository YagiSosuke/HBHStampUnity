using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/*
    スコアを表示する
*/

public class DisplayScore : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] MasterData masterData;
    [SerializeField] SceneControl sceneControl;
    

    void Update()
    {
        if(sceneControl.screenMode == ScreenMode.GameSetting)
        {
            if(sceneControl.transitionMode == TransitionMode.afterSwitching)
            {
                scoreText.text = "0";
                transform.DOLocalMoveY(-40, 0.5f).SetEase(Ease.OutCubic);
            }
        }
        if(sceneControl.screenMode == ScreenMode.Game)
        {
            if (sceneControl.transitionMode == TransitionMode.continuation)
            {
                scoreText.text = masterData.Score.ToString();
            }
            if (sceneControl.transitionMode == TransitionMode.beforeSwitching)
            {
                transform.DOLocalMoveY(150, 0.5f).SetEase(Ease.OutCubic);
            }
        }
    }
}
