using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

//TODO: AudioManagerに処理を移す

public class BGMControl : MonoBehaviour
{
    [SerializeField] AudioSource audio;

    SceneController SceneController => SceneController.Instance;
    ScreenMode nowMode = ScreenMode.Title;

    void AudioStop()
    {
        audio.Stop();
    }
    void AudioStop(float time)
    {
        float value = 0.4f;
        DOTween.To(
                () => value,
                (x) => value = x,
                0.0f,
                time
                );
        
        UniTask.Void(async () =>
        {
            do
            {
                audio.volume = value;
            } while (value > 0.0f);
            await UniTask.Delay(0);
        });
        
        audio.Stop();
    }
    void AudioPlay()
    {
        audio.volume = 0.4f;
        audio.Play();
    }
    void AudioPlay(float time)
    {
        float value = 0; 

        DOTween.To(
                () => value,
                (x) => value = x,
                1.0f,
                time
                );

        UniTask.Void(async () =>
        {
            do
            {
                audio.volume = value;
            } while (value <= 0.4f);
            await UniTask.Delay(0);
        });
        audio.Play();
    }

    void Update()
    {
        if(SceneController.screenMode == ScreenMode.GameSetting ||
           SceneController.screenMode == ScreenMode.GameFinish)
        {
            if (nowMode != SceneController.screenMode)
            {
                AudioStop();
                nowMode = SceneController.screenMode;
            }
        }
        else if(SceneController.screenMode == ScreenMode.Game ||
                SceneController.screenMode == ScreenMode.Title)
        {
            if (nowMode != SceneController.screenMode)
            {
                AudioPlay();
                nowMode = SceneController.screenMode;
            }
        }
    }
}
