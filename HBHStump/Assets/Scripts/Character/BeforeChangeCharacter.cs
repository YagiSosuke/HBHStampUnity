using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class BeforeChangeCharacter : CharacterBase
{
    [field: SerializeField] public int PosX { get; private set; }
    [field: SerializeField] public int PosY { get; private set; }
    bool isLookRight;       //進む方向  T:right F:left  
    
    //オブジェクトを配置する位置
    float[] posXCoordinates = { -780.0f, -470.0f, -155.0f, 155.0f, 470.0f, 780.0f };
    float[] posYCoordinates = { 225.0f, -125.0f, -475.0f };

    //移動する時間の上限、下限
    float minTimeRange = 2.0f;
    float maxTimeRange = 2.0f;

    [SerializeField] CharacterBase afterChangeCharaPrefab;

    CharaCsvLoader CharaCsvLoader => CharaCsvLoader.Instance;
    CharacterController CharacterController => CharacterController.Instance;


    // 各データを設定する
    public void Initialize(CharaData _data)
    {
        base.Initialize(_data);
    }
    public void Initialize(int _posY, CharaData _data, int beforePosX = -1)
    {
        base.Initialize(_data);

        //位置設定
        InitPosX();
        PosY = _posY;
        transform.localPosition = new Vector2(transform.localPosition.x, posYCoordinates[PosY]);
        SetPos();

        GoFront().Forget();
    }
    protected void SetCharaData(CharaData _data)
    {
        base.SetCharaData(_data);
    }
    void InitPosX()
    {
        if (Random.Range(0, 2) == 0)
        {
            isLookRight = true;
            PosX = 0;
            transform.localPosition = new Vector2(-1200, posYCoordinates[PosY]);
        }
        else
        {
            isLookRight = false;
            PosX = 5;
            transform.localPosition = new Vector2(1200, posYCoordinates[PosY]);
        }
        /*
        if (beforePosX == 0)
        {
            lookDirection = false;
            posX = 5;
            transform.localPosition = new Vector2(1200, posYCoordinates[posY]);
        }
        else if (beforePosX == 5)
        {
            lookDirection = true;
            posX = 0;
            transform.localPosition = new Vector2(-1200, posYCoordinates[posY]);

        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                lookDirection = true;
                posX = 0;
                transform.localPosition = new Vector2(-1200, posYCoordinates[posY]);
            }
            else
            {
                lookDirection = false;
                posX = 5;
                transform.localPosition = new Vector2(1200, posYCoordinates[posY]);
            }
        }
        */
    }
    public void SetPos()
    {
        transform.DOLocalMoveX(posXCoordinates[PosX], 0.5f);
    }

    //時間を測ってオブジェクトを前へ進める
    public async UniTask GoFront()
    {
        var duration = Random.Range(minTimeRange, maxTimeRange);

        int moveDistance = isLookRight ? 1 : -1;
        bool isMovable = isLookRight ? PosX < 5 : PosX > 0;

        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        if (isMovable)
        {
            //前に進める
            PosX += moveDistance;
            SetPos();
            GoFront().Forget();
        }
        else
        {
            //今のオブジェクトは画面外へ送り、新オブジェクトを生成
            transform.DOLocalMoveX(1200 * moveDistance, 0.5f);
            CharacterController.CharaGenerate(PosY);

            Destroy(gameObject, 1.0f);
        }
    }
    //オブジェクト変身
    public async UniTask ObjChange(CharaData _afterChangeChara)
    {
        InstantiateChangeChara().Forget();
        Destroy(gameObject);
        CharacterController.CharaGenerate(PosY);

        async UniTask InstantiateChangeChara()
        {
            var afterChangeCharaObj = Instantiate(afterChangeCharaPrefab, transform.parent);
            afterChangeCharaObj.Initialize(_afterChangeChara);
            afterChangeCharaObj.transform.localPosition = new Vector2(posXCoordinates[PosX], posYCoordinates[PosY]);

            var movingCoodinate = afterChangeCharaObj.transform.localPosition.x > 0 ? 1200 : -1200;

            //画面外へ移動
            await UniTask.Delay(2000);      //キャンセル処理は入れない

            var moveDuration = 1.0f;
            afterChangeCharaObj.transform.DOLocalMoveX(movingCoodinate, moveDuration);
            Destroy(afterChangeCharaObj.gameObject, moveDuration);
        }
    }
    //ゲームモード終了時オブジェクトを縮小する
    public void ObjShrink()
    {
        transform.DOScale(Vector2.zero, 0.9f).SetEase(Ease.InBack);
        Destroy(gameObject, 0.9f);
    }
}
