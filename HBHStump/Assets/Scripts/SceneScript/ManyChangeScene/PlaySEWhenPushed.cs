using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/*
    スタンプ押下時にSEを鳴らす 
*/

public class PlaySEWhenPushed : MonoBehaviour
{

    bool isStampOnPanel = false;         //パネル上にスタンプが載り続けているかどうかのフラグ

    [SerializeField] Serial serial;
    [SerializeField] SceneControl sceneControl;

    [Header("Audio")]
    [SerializeField] AudioClip clip;

    //押下時にSEを鳴らす
    void PlaySE() {
        if (serial.IsUseDevice)
        {
            if (serial.pushCheck() && !isStampOnPanel)
            {
                AudioManager.Instance.PlaySE(clip);
                isStampOnPanel = true;
            }
            else if (!serial.pushCheck())
            {
                isStampOnPanel = false;
            }
        }
    }


    public void Initialize()
    {
        PlaySeWhenClick().Forget();
        async UniTask PlaySeWhenClick()
        {
            while (gameObject.GetCancellationTokenOnDestroy().CanBeCanceled) {
                await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: this.GetCancellationTokenOnDestroy());
                AudioManager.Instance.PlaySE(clip);
            }
        }
    }
    //TODO: あまりよろしくない……
    void Start()
    {
        Initialize();
    }
    void Update()
    {
        PlaySE();
    }
}
