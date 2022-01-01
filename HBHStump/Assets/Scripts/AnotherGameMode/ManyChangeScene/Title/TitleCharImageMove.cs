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
    [SerializeField] Sprite[] ImageSample;

    //各キャラクターのゲームオブジェクト
    [SerializeField] GameObject[] OneObject;
    Vector2[] OneObjectPosition;
    Image[] OneObjectImage;

    //タイトルパネル
    [SerializeField] CanvasGroup titlePanel;

    //コルーチン
    public Coroutine titleCharacterMove;

    CancellationTokenSource ct;


    private void OnDestroy()
    {
        ct.Cancel();
    }


    //各キャラクターを全てキャッシュ
    void GetAllObject()
    {
        OneObject = new GameObject[transform.childCount];
        OneObjectPosition = new Vector2[transform.childCount];
        OneObjectImage = new Image[transform.childCount];

        for(int i = 0; i < OneObject.Length; i++)
        {
            OneObject[i] = transform.GetChild(i).gameObject;
            OneObjectPosition[i] = OneObject[i].transform.position;
            OneObjectImage[i] = OneObject[i].GetComponent<Image>();
        }
    }
    //最初に全てのキャラクターオブジェクトにSpriteをセットする
    void StartSetSprite()
    {
        for(int i = 0; i < OneObjectImage.Length; i++)
        {
            SetSprite(OneObjectImage[i]);
        }
    }
    //全てのキャラクターを初期位置に設定
    public void StartSetPosition() {
        transform.localPosition = Vector2.zero;
        for(int i=0;i<OneObject.Length; i++)
        {
            OneObject[i].transform.position = OneObjectPosition[i];
        }
    }
    //キャラクターオブジェクトにSpriteをセットする
    void SetSprite(Image charObj) {
        charObj.sprite = ImageSample[Random.Range(0, ImageSample.Length)];
    }

    //キャラクターオブジェクトをまとめた親を動かす
    //キャラクターオブジェク1つずつを動かす
    public async UniTask TitleCharacterMove()
    {
        while (!ct.IsCancellationRequested)
        {
            float moveTime = 3.0f;
            foreach (GameObject obj in OneObject)
            {
                obj.transform.DOLocalMove(
                    new Vector2(obj.transform.localPosition.x - 300,
                                obj.transform.localPosition.y - 300), moveTime).SetEase(Ease.Linear);
            }
            await UniTask.Delay((int)(moveTime * 1000), cancellationToken: ct.Token);
            await UniTask.DelayFrame(1);

            //左下に行ったオブジェクトを右上に移動させる
            foreach(GameObject obj in OneObject) { 
                if (obj.transform.localPosition.y <= -900)
                {
                    Debug.Log("move");
                    obj.transform.localPosition = new Vector2(obj.transform.localPosition.x + 600, 900);
                }
                else if (obj.transform.localPosition.x <= -1200)
                {
                    Debug.Log("move");
                    obj.transform.localPosition = new Vector2(1200, obj.transform.localPosition.y + 300);
                }
            }
        }
    }

    //タイトルパネルをFIする
    public void DisplayTitlePanel(float num)
    {
        DOTween.To(
            () => titlePanel.alpha,         //何に
            (n) => titlePanel.alpha = n,    //何を
            1.0f,    //どこまで
            num     //どれくらいの時間
        );
    }
    //タイトルパネルをFOする
    public IEnumerator NonDisplayTitlePanel(float num) {
        DOTween.To(
            () => titlePanel.alpha,         //何に
            (n) => titlePanel.alpha = n,    //何を
            0.0f,    //どこまで
            num     //どれくらいの時間
        );
        yield return new WaitForSeconds(num);   //TitleCharacterMove()のインターバルに合わせて変更すること
        yield return new WaitForSeconds(3.0f);   //TitleCharacterMove()のインターバルに合わせて変更すること
        StartSetPosition();
    }


    //他スクリプトで呼び出し用の変数
    public void TitleSceneAfter(float num)
    {
        DisplayTitlePanel(num);
        StartSetPosition();
        ct = new CancellationTokenSource();
        TitleCharacterMove().Forget();
    }
    public void TitleSceneContinuation()
    {
        
    }
    public void TitleSceneBefore(float interval)
    {
        StartCoroutine(NonDisplayTitlePanel(interval));
        ct.Cancel();
    }


    // Start is called before the first frame update
    void Start()
    {
        GetAllObject();
        StartSetSprite();
    }
}
