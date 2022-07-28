using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/*
    ゲーム終了テキストを表示する 
*/

public class DisplayFinishText : MonoBehaviour
{
    [SerializeField] GameObject finishImages;
    
    [Header("効果音系")]
    [SerializeField] AudioClip finishAudioClip;

    SceneController SceneController => SceneController.Instance;

    async UniTask DisplayFinishImages()
    {
        finishImages.SetActive(true);
        finishImages.transform.localScale = Vector2.zero;
        finishImages.transform.DOScale(Vector2.one, 1.0f).SetEase(Ease.OutElastic);
        AudioManager.Instance.PlaySE(finishAudioClip);
        await UniTask.Delay(1000);
        finishImages.transform.DOScale(Vector2.zero, 0.5f).SetEase(Ease.InBack);
        await UniTask.Delay(500);
    }

    void Start()
    {
        finishImages.SetActive(false);
    }

    void Update()
    {
        if (SceneController.screenMode == ScreenMode.GameFinish) {
            if (SceneController.transitionMode == TransitionMode.afterSwitching) 
            {
                DisplayFinishImages().Forget();
            }
            if (SceneController.transitionMode == TransitionMode.continuation)
            {
            }
            if (SceneController.transitionMode == TransitionMode.beforeSwitching)
            {
            }
        }
    }
}
