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

    SceneController SceneController => SceneController.Instance;
    

    async UniTask OnGameSetting()
    {
        await UniTask.WaitUntil(() => SceneController.screenMode == ScreenMode.GameSetting, cancellationToken: this.GetCancellationTokenOnDestroy());
        scoreText.text = "0";
        transform.DOLocalMoveY(-40, 0.5f).SetEase(Ease.OutCubic);
        await UniTask.WaitUntil(() => SceneController.screenMode != ScreenMode.GameSetting, cancellationToken: this.GetCancellationTokenOnDestroy());

        OnGameSetting().Forget();
    }
    async UniTask OnGame()
    {
        await UniTask.WaitUntil(() => SceneController.screenMode == ScreenMode.Game, cancellationToken: this.GetCancellationTokenOnDestroy());
        await UniTask.WaitUntil(() => SceneController.transitionMode == TransitionMode.continuation, cancellationToken: this.GetCancellationTokenOnDestroy());
        while (SceneController.transitionMode == TransitionMode.continuation)
        {
            scoreText.text = masterData.Score.ToString();
            await UniTask.DelayFrame(1, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
        transform.DOLocalMoveY(150, 0.5f).SetEase(Ease.OutCubic);
        await UniTask.WaitUntil(() => SceneController.screenMode != ScreenMode.Game, cancellationToken: this.GetCancellationTokenOnDestroy());

        OnGame().Forget();
    }

    void Start()
    {
        OnGameSetting().Forget();
        OnGame().Forget();
    }

    void Update()
    {
        if(SceneController.screenMode == ScreenMode.GameSetting)
        {
            if(SceneController.transitionMode == TransitionMode.afterSwitching)
            {
                scoreText.text = "0";
                transform.DOLocalMoveY(-40, 0.5f).SetEase(Ease.OutCubic);
            }
        }
        if(SceneController.screenMode == ScreenMode.Game)
        {
            if (SceneController.transitionMode == TransitionMode.continuation)
            {
                scoreText.text = masterData.Score.ToString();
            }
            if (SceneController.transitionMode == TransitionMode.beforeSwitching)
            {
                transform.DOLocalMoveY(150, 0.5f).SetEase(Ease.OutCubic);
            }
        }
    }
}
