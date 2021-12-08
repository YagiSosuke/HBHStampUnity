using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/*
    タスク完了時Goodを表示する 
*/

public class GoodText : MonoBehaviour
{
    //Goodのテキスト
    [SerializeField] GameObject GoodTextImage;
    AudioSource audio;

    //Goodを表示する
    public async UniTask displayGoodText()
    {
        GoodTextImage.transform.localScale = Vector3.zero;
        GoodTextImage.transform.rotation = Quaternion.Euler(0, 0, 90);
        GoodTextImage.transform.DOScale(Vector3.one, 0.4f);
        GoodTextImage.transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutBack);
        audio.Play();
        await UniTask.Delay(1000);
        GoodTextImage.transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack);
    }


    // Start is called before the first frame update
    void Start()
    {
        GoodTextImage.transform.localScale = Vector3.zero;
        GoodTextImage.transform.rotation = Quaternion.Euler(0, 0, 90);
        audio = GetComponent<AudioSource>();
    }
}
