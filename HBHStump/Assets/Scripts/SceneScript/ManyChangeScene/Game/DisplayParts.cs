using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/*
    ゲーム開始時、設定されているパーツを表示する 
*/


public class DisplayParts : MonoBehaviour
{
    [SerializeField] Image partsImage;
    [SerializeField] List<Sprite> partsSample = new List<Sprite>();

    SceneController SceneController => SceneController.Instance;
    

    //現在のパーツを表示する
    async UniTask DisplayPartsUpdate()
    {
        var parts = Stamp.Instance.Parts;
        await UniTask.WaitUntil(() => parts != Stamp.Instance.Parts, cancellationToken: this.GetCancellationTokenOnDestroy());
        switch (Stamp.Instance.Parts) {
            case Parts.Head:
                partsImage.sprite = partsSample[0];
                break;
            case Parts.Body:
                partsImage.sprite = partsSample[1];
                break;
            case Parts.Hip:
                partsImage.sprite = partsSample[2];
                break;
        }

        DisplayPartsUpdate().Forget();
    }

    async UniTask OnGameSetting()
    {
        await UniTask.WaitUntil(() => SceneController.screenMode == ScreenMode.GameSetting, cancellationToken: this.GetCancellationTokenOnDestroy());
        transform.DOLocalMoveY(-40, 0.5f).SetEase(Ease.OutCubic);
        await UniTask.WaitUntil(() => SceneController.screenMode != ScreenMode.GameSetting, cancellationToken: this.GetCancellationTokenOnDestroy());

        OnGameSetting().Forget();
    }
    async UniTask OnGameFinish()
    {
        await UniTask.WaitUntil(() => SceneController.screenMode == ScreenMode.GameFinish, cancellationToken: this.GetCancellationTokenOnDestroy());
        transform.DOLocalMoveY(150, 0.5f).SetEase(Ease.OutCubic);
        await UniTask.WaitUntil(() => SceneController.screenMode != ScreenMode.GameFinish, cancellationToken: this.GetCancellationTokenOnDestroy());

        OnGameFinish().Forget();
    }

    void Start()
    {
        DisplayPartsUpdate().Forget();
        OnGameSetting().Forget();
        OnGameFinish().Forget();
    }
}
