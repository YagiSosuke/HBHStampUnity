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

    MasterData MasterData => MasterData.Instance;
    SceneControl sceneControl;

    private void Start()
    {
        sceneControl = GameObject.Find("GameControler").GetComponent<SceneControl>();
    }


    // Update is called once per frame
    void Update()
    {
        if(sceneControl.screenMode == SceneControl.ScreenMode.GameSetting)
        {
            if (sceneControl.transitionMode == SceneControl.TransitionMode.afterSwitching)
            {
                remainingTimeText.text = MasterData.remainingTimeTemp.ToString();
                timerCountImage.fillAmount = 1.0f;
                transform.DOLocalMoveY(480, slideTime).SetEase(Ease.OutCubic);
            }
            if (sceneControl.transitionMode == SceneControl.TransitionMode.continuation)
            {

            }
            if (sceneControl.transitionMode == SceneControl.TransitionMode.beforeSwitching) { }
        }
        if (sceneControl.screenMode == SceneControl.ScreenMode.Game)
        {
            if (sceneControl.transitionMode == SceneControl.TransitionMode.afterSwitching)
            {
            }
            if (sceneControl.transitionMode == SceneControl.TransitionMode.continuation)
            {
                remainingTimeText.text = MasterData.remainingTime.ToString("0");
                timerCountImage.fillAmount = (MasterData.remainingTime / MasterData.remainingTimeTemp);
            }
            if(sceneControl.transitionMode == SceneControl.TransitionMode.beforeSwitching)
            {
                remainingTimeText.text = "0";
                timerCountImage.fillAmount = 0.0f;
                transform.DOLocalMoveY(700, slideTime).SetEase(Ease.OutCubic);
            }
        }
    }
}
