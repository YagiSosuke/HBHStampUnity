using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class BGMControl : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    SceneControl sceneControl;

    SceneControl.ScreenMode nowMode = SceneControl.ScreenMode.Title;

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

    private void Start()
    {
        sceneControl = GameObject.Find("GameControler").GetComponent<SceneControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sceneControl.screenMode == SceneControl.ScreenMode.GameSetting ||
           sceneControl.screenMode == SceneControl.ScreenMode.GameFinish)
        {
            if (nowMode != sceneControl.screenMode)
            {
                AudioStop();
                nowMode = sceneControl.screenMode;
            }
        }
        else if(sceneControl.screenMode == SceneControl.ScreenMode.Game ||
                sceneControl.screenMode == SceneControl.ScreenMode.Title)
        {
            if (nowMode != sceneControl.screenMode)
            {
                AudioPlay();
                nowMode = sceneControl.screenMode;
            }
        }
    }
}
