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
    //各キャラクターのイメージサンプル
    [SerializeField] Sprite[] imageSample;

    //各キャラクターのゲームオブジェクト
    [SerializeField] GameObject[] oneObject;
    Vector2[] oneObjectPosition;
    Image[]   oneObjectImage;

    //タイトルパネル
    [SerializeField] CanvasGroup titlePanel;

    CancellationTokenSource ct;


    private void OnDestroy()
    {
        ct.Cancel();
    }


    //各キャラクターを全てキャッシュ
    void GetAllObject()
    {
        oneObject = new GameObject[transform.childCount];
        oneObjectPosition = new Vector2[transform.childCount];
        oneObjectImage = new Image[transform.childCount];

        for(int i = 0; i < oneObject.Length; i++)
        {
            oneObject[i] = transform.GetChild(i).gameObject;
            oneObjectPosition[i] = oneObject[i].transform.position;
            oneObjectImage[i] = oneObject[i].GetComponent<Image>();
        }
    }
    //最初に全てのキャラクターオブジェクトにSpriteをセットする
    void StartSetSprite()
    {
        for(int i = 0; i < oneObjectImage.Length; i++)
        {
            SetSprite(oneObjectImage[i]);
        }
    }
    //全てのキャラクターを初期位置に設定
    public void StartSetPosition()
    {
        transform.localPosition = Vector2.zero;
        for (int i = 0; i < oneObject.Length; i++)
        {
            oneObject[i].transform.position = oneObjectPosition[i];
        }
    }
    //キャラクターオブジェクトにSpriteをセットする
    void SetSprite(Image charObj)
    {
        charObj.sprite = imageSample[Random.Range(0, imageSample.Length)];
    }
    
    //キャラクターオブジェクト1つずつを動かす
    public async UniTask TitleCharacterMove()
    {
        while (!ct.IsCancellationRequested)
        {
            var moveTime = 3.0f;
            foreach (GameObject obj in oneObject)
            {
                var destiantionPos = new Vector2(obj.transform.localPosition.x - 300, obj.transform.localPosition.y - 300);
                obj.transform.DOLocalMove(destiantionPos, moveTime).SetEase(Ease.Linear);
            }
            await UniTask.Delay((int)(moveTime * 1000), cancellationToken: ct.Token);
            await UniTask.DelayFrame(1);

            //左下に行ったオブジェクトを右上に移動させる
            foreach(GameObject obj in oneObject) { 
                if (obj.transform.localPosition.y <= -900)
                {
                    obj.transform.localPosition = new Vector2(obj.transform.localPosition.x + 600, 900);
                }
                else if (obj.transform.localPosition.x <= -1200)
                {
                    obj.transform.localPosition = new Vector2(1200, obj.transform.localPosition.y + 300);
                }
            }
        }
    }

    void FadeInTitlePanel(float num)
    {
        DOTween.To(
            () => titlePanel.alpha,         //何に
            (n) => titlePanel.alpha = n,    //何を
            1.0f,    //どこまで
            num     //どれくらいの時間
        );
    }
    async UniTask FadeOutTitlePanel(float num) {
        DOTween.To(
            () => titlePanel.alpha,         //何に
            (n) => titlePanel.alpha = n,    //何を
            0.0f,    //どこまで
            num     //どれくらいの時間
        );
        await UniTask.Delay((int)(num*1000), cancellationToken:this.GetCancellationTokenOnDestroy());   //TitleCharacterMove()のインターバルに合わせて変更すること
        await UniTask.Delay(3000, cancellationToken: this.GetCancellationTokenOnDestroy());   //TitleCharacterMove()のインターバルに合わせて変更すること
        StartSetPosition();
    }


    //他スクリプトで呼び出し用の変数
    public void TitleSceneAfter(float num)
    {
        FadeInTitlePanel(num);
        StartSetPosition();
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

    void Start()
    {
        GetAllObject();
        StartSetSprite();
    }
}
