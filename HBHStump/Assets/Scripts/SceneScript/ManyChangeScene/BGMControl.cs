using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

//TODO: AudioManagerに処理を移す

public class BGMControl : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    [SerializeField] SceneControl sceneControl;

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
        if(sceneControl.screenMode == ScreenMode.GameSetting ||
           sceneControl.screenMode == ScreenMode.GameFinish)
        {
            if (nowMode != sceneControl.screenMode)
            {
                AudioStop();
                nowMode = sceneControl.screenMode;
            }
        }
        else if(sceneControl.screenMode == ScreenMode.Game ||
                sceneControl.screenMode == ScreenMode.Title)
        {
            if (nowMode != sceneControl.screenMode)
            {
                AudioPlay();
                nowMode = sceneControl.screenMode;
            }
        }
    }
}
