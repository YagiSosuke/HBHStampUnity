using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    スタンプ押下時にSEを鳴らす 
*/

public class PlaySEWhenPushed : MonoBehaviour
{
    bool stampOnPanelF = false;         //パネル上にスタンプが載り続けているかどうかのフラグ

    [SerializeField] Serial serial;
    [SerializeField] SceneControl sceneControl;

    [Header("Audio")]
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip clip;

    //押下時にSEを鳴らす
    void PlaySE() {
        if (serial.enabled)
        {
            if (serial.pushCheck() && !stampOnPanelF)
            {
                audio.PlayOneShot(clip);
                stampOnPanelF = true;
            }
            else if (!serial.pushCheck())
            {
                stampOnPanelF = false;
            }
        }
    }
    
    void Update()
    {
        PlaySE();
    }
}
