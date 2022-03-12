using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

/*
タイトル画面
キャラクターたちが動き続ける
*/

public class TitleCharImageMove : MonoBehaviour
{
    [SerializeField] Image[] charaImage;
    [SerializeField] CanvasGroup titlePanel;
    List<Vector2> defaultPos = new List<Vector2>();

    CancellationTokenSource ct;
    CharaImageData CharaImageData => CharaImageData.Instance;


    private void OnDestroy()
    {
        ct.Cancel();
    }

    
    void SetDefaultPos()
    {
        transform.localPosition = Vector2.zero;
        for (int i = 0; i < charaImage.Length; i++)
        {
            charaImage[i].transform.position = defaultPos[i];
        }
    }
    void SetSprite(Image charObj)
    {
        charObj.sprite = CharaImageData.CharaSprite[Random.Range(0, CharaImageData.CharaSprite.Length)];
    }
    
    //キャラクターオブジェクト1つずつを動かす
    async UniTask TitleCharacterMove()
    {
        while (!ct.IsCancellationRequested)
        {
            var moveTime = 3.0f;
            foreach (Image image in charaImage)
            {
                var destiantionPos = new Vector2(image.transform.localPosition.x - 300, image.transform.localPosition.y - 300);
                image.transform.DOLocalMove(destiantionPos, moveTime).SetEase(Ease.Linear);
            }
            await UniTask.Delay((int)(moveTime * 1000), cancellationToken: ct.Token);
            await UniTask.DelayFrame(1);

            //左下に行ったオブジェクトを右上に移動させる
            foreach(Image image in charaImage) { 
                if (image.transform.localPosition.y <= -900)
                {
                    image.transform.localPosition = new Vector2(image.transform.localPosition.x + 600, 900);
                }
                else if (image.transform.localPosition.x <= -1200)
                {
                    image.transform.localPosition = new Vector2(1200, image.transform.localPosition.y + 300);
                }
            }
        }
    }

    void FadeInTitlePanel(float num)
    {
        titlePanel.DOFade(endValue: 1.0f, duration: num);
    }
    async UniTask FadeOutTitlePanel(float num)
    {
        titlePanel.DOFade(endValue: 0.0f, duration: num);
        await UniTask.Delay((int)(num*1000), cancellationToken:this.GetCancellationTokenOnDestroy());
        await UniTask.Delay(3000, cancellationToken: this.GetCancellationTokenOnDestroy());
        SetDefaultPos();
    }
    
    //他スクリプトで呼び出し用の変数
    public void TitleSceneAfter(float num)
    {
        FadeInTitlePanel(num);
        SetDefaultPos();
        ct = new CancellationTokenSource();
        TitleCharacterMove().Forget();
    }
    public void TitleSceneContinuation()
    {
        
    }
    public void TitleSceneBefore(float interval)
    {
        FadeOutTitlePanel(interval).Forget();
        ct.Cancel();
    }

    public void Initialize()
    {
        foreach (Image image in charaImage)
        {
            SetSprite(image);
            defaultPos.Add(image.transform.position);
        }
    }
}
