using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DeisplayTimer : MonoBehaviour
{
    //時間を表示するテキスト
    [SerializeField] Text remainingTimeText;
    //時間経過を示唆するイメージ
    [SerializeField] Image timerCountImage;

    //タイマーがスライドインするまでの時間
    float slideTime = 0.5f;

    MasterData masterData;
    SceneControl sceneControl;

    private void Start()
    {
        masterData = GameObject.Find("GameControler").GetComponent<MasterData>();
        sceneControl = GameObject.Find("GameControler").GetComponent<SceneControl>();
    }


    // Update is called once per frame
    void Update()
    {
        if(sceneControl.screenMode == SceneControl.ScreenMode.GameSetting)
        {
            #region
            if (sceneControl.transitionMode == SceneControl.TransitionMode.afterSwitching)
            {
                remainingTimeText.text = masterData.remainingTimeTemp.ToString();
                timerCountImage.fillAmount = 1.0f;
                transform.DOLocalMoveY(480, slideTime).SetEase(Ease.OutCubic);
            }
            if (sceneControl.transitionMode == SceneControl.TransitionMode.continuation)
            {

            }
            if (sceneControl.transitionMode == SceneControl.TransitionMode.beforeSwitching) { }
            #endregion
        }
        if (sceneControl.screenMode == SceneControl.ScreenMode.Game)
        {
            #region
            if (sceneControl.transitionMode == SceneControl.TransitionMode.afterSwitching)
            {
            }
            if (sceneControl.transitionMode == SceneControl.TransitionMode.continuation)
            {
                remainingTimeText.text = masterData.remainingTime.ToString("0");
                timerCountImage.fillAmount = (masterData.remainingTime / masterData.remainingTimeTemp);
            }
            if(sceneControl.transitionMode == SceneControl.TransitionMode.beforeSwitching)
            {
                remainingTimeText.text = "0";
                timerCountImage.fillAmount = 0.0f;
                transform.DOLocalMoveY(700, slideTime).SetEase(Ease.OutCubic);
            }
            #endregion
        }
    }
}
