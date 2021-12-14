using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TimeCountSetting : MonoBehaviour
{
    [SerializeField] GameObject countImage3;
    [SerializeField] GameObject countImage2;
    [SerializeField] GameObject countImage1;

    [SerializeField] GameObject GOImage;

    SceneControl sceneControl;

    //カウント時の効果音を鳴らすコンポーネントたち
    #region
    [Header("カウント時の効果音を鳴らすコンポーネントたち")]
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip countAudioClip;
    [SerializeField] AudioClip GOAudioClip;
    #endregion

    //ゲームスタート前にカウントダウンを表示する
    async UniTask DisplayCountDown()
    {
        countImage3.SetActive(true);
        countImage3.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        countImage3.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutElastic);
        audio.PlayOneShot(countAudioClip);
        await UniTask.Delay(1000);
        countImage3.SetActive(false);

        countImage2.SetActive(true);
        countImage2.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        countImage2.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutElastic);
        audio.PlayOneShot(countAudioClip);
        await UniTask.Delay(1000);
        countImage2.SetActive(false);

        countImage1.SetActive(true);
        countImage1.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        countImage1.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutElastic);
        audio.PlayOneShot(countAudioClip);
        await UniTask.Delay(1000);
        countImage1.SetActive(false);

        GOImage.SetActive(true);
        GOImage.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        GOImage.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutElastic);
        audio.PlayOneShot(GOAudioClip);
        await UniTask.Delay(1000);
        GOImage.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        await UniTask.Delay(200);
        GOImage.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        countImage1.transform.localScale =
        countImage2.transform.localScale =
        countImage3.transform.localScale = 
        GOImage.transform.localScale = Vector3.one;
        countImage1.SetActive(false);
        countImage2.SetActive(false);
        countImage3.SetActive(false);
        GOImage.SetActive(false);

        sceneControl = GameObject.Find("GameControler").GetComponent<SceneControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneControl.screenMode == SceneControl.ScreenMode.GameSetting) 
        {
            if (sceneControl.transitionMode == SceneControl.TransitionMode.afterSwitching) 
            {
                DisplayCountDown().Forget();
            }
        }
    }
}
