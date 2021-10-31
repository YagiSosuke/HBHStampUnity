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
    MasterData masterdata;
    SceneControl sceneControl;

    //スコア表を展開
    //スコア表を収納
    
    // Start is called before the first frame update
    void Start()
    {
        masterdata = GameObject.Find("GameControler").GetComponent<MasterData>();
        sceneControl = GameObject.Find("GameControler").GetComponent<SceneControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sceneControl.screenMode == SceneControl.ScreenMode.GameSetting)
        {
            if(sceneControl.transitionMode == SceneControl.TransitionMode.afterSwitching)
            {
                scoreText.text = "0";
                transform.DOLocalMoveY(-40, 0.5f).SetEase(Ease.OutCubic);
            }
        }
        if(sceneControl.screenMode == SceneControl.ScreenMode.Game)
        {
            if (sceneControl.transitionMode == SceneControl.TransitionMode.continuation)
            {
                scoreText.text = masterdata.score.ToString();
            }
            if (sceneControl.transitionMode == SceneControl.TransitionMode.beforeSwitching)
            {
                transform.DOLocalMoveY(150, 0.5f).SetEase(Ease.OutCubic);
            }
        }
    }
}
