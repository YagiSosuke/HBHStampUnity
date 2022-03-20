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
    

    async UniTask OnGameSetting()
    {
        await UniTask.WaitUntil(() => sceneControl.screenMode == ScreenMode.GameSetting, cancellationToken: this.GetCancellationTokenOnDestroy());
        scoreText.text = "0";
        transform.DOLocalMoveY(-40, 0.5f).SetEase(Ease.OutCubic);
        await UniTask.WaitUntil(() => sceneControl.screenMode != ScreenMode.GameSetting, cancellationToken: this.GetCancellationTokenOnDestroy());

        OnGameSetting().Forget();
    }
    async UniTask OnGame()
    {
        await UniTask.WaitUntil(() => sceneControl.screenMode == ScreenMode.Game, cancellationToken: this.GetCancellationTokenOnDestroy());
        await UniTask.WaitUntil(() => sceneControl.transitionMode == TransitionMode.continuation, cancellationToken: this.GetCancellationTokenOnDestroy());
        while (sceneControl.transitionMode == TransitionMode.continuation)
        {
            scoreText.text = masterData.Score.ToString();
            await UniTask.DelayFrame(1, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
        transform.DOLocalMoveY(150, 0.5f).SetEase(Ease.OutCubic);
        await UniTask.WaitUntil(() => sceneControl.screenMode != ScreenMode.Game, cancellationToken: this.GetCancellationTokenOnDestroy());

        OnGame().Forget();
    }

    void Start()
    {
        OnGameSetting().Forget();
        OnGame().Forget();
    }

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
