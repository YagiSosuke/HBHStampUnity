using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class BeforeChangeCharacter : CharacterBase
{
    //変化後キャラデータ
    [SerializeField] List<CharaData> afterChangeCharaDatas;

    int posX;
    int posY;
    bool isLookRight;                       //進む方向  T:right F:left  
    GameObject newObj;                      //変化後のオブジェクト
    //位置を変えるまでの時間
    float minTimeRange = 5.0f;
    float maxTimeRange = 10.0f;
    
    //オブジェクトを配置する位置
    float[] posXPosition = { -780.0f, -470.0f, -155.0f, 155.0f, 470.0f, 780.0f };
    float[] posYPosition = { 225.0f, -125.0f, -475.0f };


    //各データを設定する
    public void Initialize(int posY, CharaData _data, int beforePosX = -1)
    {
        base.Initialize(_data);
        InitPosX();
        this.posY = posY;
        
        //位置設定
        SetPos();
        GoFront().Forget();
    }
    protected void SetCharaData(CharaData _data)
    {
        base.SetCharaData(_data);
        afterChangeCharaDatas = new List<CharaData>(CharaCsvLoader.Instance.afterChangeCharaDatas[charaName]);
    }
    void InitPosX()
    {
        if (Random.Range(0, 2) == 0)
        {
            isLookRight = true;
            posX = 0;
        }
        else
        {
            isLookRight = false;
            posX = 5;
        }
        /*
        if (beforePosX == 0)
        {
            lookDirection = false;
            posX = 5;
        }
        else if (beforePosX == 5)
        {
            lookDirection = true;
            posX = 0;

        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                lookDirection = true;
                posX = 0;
            }
            else
            {
                lookDirection = false;
                posX = 5;
            }
        }
        */
    }

    public void SetPos()
    {
        //新たに生み出した場合は画面外に初期位置を設定する
        if (isLookRight && posX == 0)
        {
            transform.localPosition = new Vector2(-1200, posYPosition[posY]);
        }
        else if (!isLookRight && posX == 5)
        {
            transform.localPosition = new Vector2(1200, posYPosition[posY]);
        }

        transform.DOLocalMove(new Vector2(posXPosition[posX], posYPosition[posY]), 0.5f);
        transform.GetChild(0).GetComponent<CharctorScript>().SetPosition(posX, posY);
    }

    public void SetNewObj(GameObject newObj)
    {
        this.newObj = newObj.gameObject;
    }

    //時間を測ってオブジェクトを前へ進める
    public async UniTask GoFront()
    {
        var duration = Random.Range(minTimeRange, maxTimeRange);
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        if (isLookRight)
        {
            if (posX < 5)
            {
                //前に進める
                posX++;
            }
            else
            {
                //今あるオブジェクトは画面外へ
                transform.DOLocalMove(new Vector2(1200, posYPosition[posY]), 0.5f);

                //もう前に進めないので新しいオブジェクトを生成する
                //TODO: 修正 OperationCharctor.getInstance().DeathBornObject(posY);

                Destroy(gameObject, 1.0f);
            }
        }
        else
        {
            if (posX > 0)
            {
                //前に進める
                posX--;
            }
            else
            {
                //今あるオブジェクトは画面外へ
                transform.DOLocalMove(new Vector2(-1200, posYPosition[posY]), 0.5f);

                //もう前に進めないので新しいオブジェクトを生成する
                //TODO: 修正 OperationCharctor.getInstance().DeathBornObject(posY);

                Destroy(gameObject, 1.0f);
            }
        }
        SetPos();
        GoFront().Forget();
    }

    //変化後のオブジェクトを破棄する
    public async UniTask DestroyNewObj()
    {
        var obj = newObj;
        //画面外へ移動
        if (newObj.transform.position.x > 0)
        {
            await UniTask.Delay(2000);
            newObj.transform.DOLocalMove(new Vector2(1200, posY), 1.0f);
        }
        else
        {
            await UniTask.Delay(2000);
            newObj.transform.DOLocalMove(new Vector2(-1200, posY), 1.0f);
        }
        //オブジェクトを削除
        Destroy(obj.gameObject, 1.5f);
    }
    //ゲームモード終了時各オブジェクトを縮小する
    public void ObjShrink()
    {
        transform.DOScale(Vector2.zero, 0.9f).SetEase(Ease.InBack);
        Destroy(gameObject, 0.9f);
    }
}
