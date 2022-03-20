using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class DeisplayTimer : MonoBehaviour
{
    //タイマーがスライドインするまでの時間
    const float slideTime = 0.5f;

    [SerializeField] Text currentTimeText;
    [SerializeField] Image timerImage;

    [SerializeField] SceneControl sceneControl;
    MasterData MasterData => MasterData.Instance;


    async UniTask OnGameSetting()
    {
        await UniTask.WaitUntil(() => sceneControl.screenMode == ScreenMode.GameSetting, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("タイマー表示");
        currentTimeText.text = MasterData.TimeLimit.ToString();
        timerImage.fillAmount = 1.0f;
        transform.DOLocalMoveY(-70, slideTime).SetEase(Ease.OutCubic);
        await UniTask.WaitUntil(() => sceneControl.screenMode != ScreenMode.GameSetting, cancellationToken: this.GetCancellationTokenOnDestroy());

        OnGameSetting().Forget();
    }
    async UniTask OnGame()
    {
        await UniTask.WaitUntil(() => sceneControl.screenMode == ScreenMode.Game, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("タイマーカウント");
        await UniTask.WaitUntil(() => sceneControl.transitionMode == TransitionMode.continuation, cancellationToken: this.GetCancellationTokenOnDestroy());
        while (sceneControl.transitionMode == TransitionMode.continuation)
        {
            currentTimeText.text = MasterData.CurrentTime.ToString("0");
            timerImage.fillAmount = (MasterData.CurrentTime / MasterData.TimeLimit);
            await UniTask.DelayFrame(1, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
        currentTimeText.text = "0";
        timerImage.fillAmount = 0.0f;
        transform.DOLocalMoveY(150, slideTime).SetEase(Ease.OutCubic);
        await UniTask.WaitUntil(() => sceneControl.screenMode != ScreenMode.Game, cancellationToken: this.GetCancellationTokenOnDestroy());

        OnGame().Forget();
    }

    void Start()
    {
        OnGameSetting().Forget();
        OnGame().Forget();
    }
}
