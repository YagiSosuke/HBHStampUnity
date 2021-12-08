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
    [SerializeField] SceneControl sceneControl;

    //効果音系
    [Header("効果音系")]
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip finishAudioClip;

    async UniTask DisplayFinishImages()
    {
        finishImages.SetActive(true);
        finishImages.transform.localScale = Vector2.zero;
        finishImages.transform.DOScale(Vector2.one, 1.0f).SetEase(Ease.OutElastic);
        audio.PlayOneShot(finishAudioClip);
        await UniTask.Delay(1000);
        finishImages.transform.DOScale(Vector2.zero, 0.5f).SetEase(Ease.InBack);
        await UniTask.Delay(500);
    }

    // Start is called before the first frame update
    void Start()
    {
        finishImages.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneControl.screenMode == SceneControl.ScreenMode.GameFinish) {
            if (sceneControl.transitionMode == SceneControl.TransitionMode.afterSwitching) 
            {
                DisplayFinishImages().Forget();
            }
            if (sceneControl.transitionMode == SceneControl.TransitionMode.continuation)
            {
            }
            if (sceneControl.transitionMode == SceneControl.TransitionMode.beforeSwitching)
            {
            }
        }
    }
}
