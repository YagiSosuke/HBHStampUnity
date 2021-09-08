using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/*
タイトル画面
キャラクターたちが動き続ける
*/

public class TitleCharImageMove : MonoBehaviour
{
    //各キャラクターのイメージサンプル
    [SerializeField] Sprite[] ImageSample;

    //各キャラクターのゲームオブジェクト
    GameObject[] OneObject;
    Vector2[] OneObjectPosition;
    Image[] OneObjectImage;

    //タイトルパネル
    [SerializeField] CanvasGroup titlePanel;

    //コルーチン
    public Coroutine titleCharacterMove;


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
    public IEnumerator TitleCharacterMove()
    {
        while (true)
        {
            float moveTime = 3.0f;
            transform.DOLocalMove(new Vector2(transform.localPosition.x - 300, transform.localPosition.y - 300), moveTime).SetEase(Ease.Linear);
            yield return new WaitForSeconds(moveTime);

            //左下に言ったオブジェクトを右上に移動させる
            for (int i = 0; i < OneObject.Length; i++)
            {
                if (OneObject[i].transform.position.y == -300)
                {
                    OneObject[i].transform.position = new Vector2(OneObject[i].transform.position.x + 600, 1500);
                }
                else if (OneObject[i].transform.position.x == -240)
                {
                    OneObject[i].transform.position = new Vector2(2160, OneObject[i].transform.position.y + 300);
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
        //タイトル画面の動きを停止、初期位置に戻す
        StopCoroutine(titleCharacterMove);
        yield return new WaitForSeconds(3.0f);   //TitleCharacterMove()のインターバルに合わせて変更すること
        StartSetPosition();
    }


    //他スクリプトで呼び出し用の変数
    public void TitleSceneAfter(float num)
    {
        DisplayTitlePanel(num);
        StartSetPosition();
        titleCharacterMove = StartCoroutine(TitleCharacterMove());
    }
    public void TitleSceneContinuation()
    {
        
    }
    public void TitleSceneBefore(float interval)
    {
        StartCoroutine(NonDisplayTitlePanel(interval));
    }


    //テスト用 - コルーチンが止まるか確認
    void sStopCoroutine()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (titleCharacterMove != null)
            {
                StopCoroutine(titleCharacterMove);
                titleCharacterMove = null;
            }
            else
            {
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        GetAllObject();
        StartSetSprite();
    }
}
