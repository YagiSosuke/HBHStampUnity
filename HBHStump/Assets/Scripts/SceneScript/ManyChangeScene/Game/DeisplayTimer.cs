using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DeisplayTimer : MonoBehaviour
{
    //タイマーがスライドインするまでの時間
    const float slideTime = 0.5f;

    [SerializeField] Text currentTimeText;
    [SerializeField] Image timerImage;

    [SerializeField] SceneControl sceneControl;
    MasterData MasterData => MasterData.Instance;

    
    void Update()
    {
        if(sceneControl.screenMode == ScreenMode.GameSetting)
        {
            if (sceneControl.transitionMode == TransitionMode.afterSwitching)
            {
                currentTimeText.text = MasterData.TimeLimit.ToString();
                timerImage.fillAmount = 1.0f;
                transform.DOLocalMoveY(480, slideTime).SetEase(Ease.OutCubic);
            }
            if (sceneControl.transitionMode == TransitionMode.continuation)
            {

            }
            if (sceneControl.transitionMode == TransitionMode.beforeSwitching) { }
        }
        if (sceneControl.screenMode == ScreenMode.Game)
        {
            if (sceneControl.transitionMode == TransitionMode.afterSwitching)
            {
            }
            if (sceneControl.transitionMode == TransitionMode.continuation)
            {
                currentTimeText.text = MasterData.CurrentTime.ToString("0");
                timerImage.fillAmount = (MasterData.CurrentTime / MasterData.TimeLimit);
            }
            if(sceneControl.transitionMode == TransitionMode.beforeSwitching)
            {
                currentTimeText.text = "0";
                timerImage.fillAmount = 0.0f;
                transform.DOLocalMoveY(700, slideTime).SetEase(Ease.OutCubic);
            }
        }
    }
}
