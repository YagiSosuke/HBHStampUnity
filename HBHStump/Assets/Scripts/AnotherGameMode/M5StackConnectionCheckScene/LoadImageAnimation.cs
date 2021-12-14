using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;

public class LoadImageAnimation : MonoBehaviour
{
    [SerializeField] GameObject LoadImage;
    UniTaskCompletionSource uniSource;

    public async UniTask RotateImage(SerialCheck serial)
    {
        LoadImage.SetActive(true);
        await UniTask.DelayFrame(1);
        while (serial.serial != null && !serial.serial.IsOpen)
        {
            Debug.Log("OK");
            LoadImage.transform.Rotate(0, 0, 10);
            //LoadImage.transform.DORotate(new Vector3(0, 0, 360), 1, RotateMode.Fast).SetEase(Ease.Linear);
            //await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
        LoadImage.SetActive(false);
    }
}
