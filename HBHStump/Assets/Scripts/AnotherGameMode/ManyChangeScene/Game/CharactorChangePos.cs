using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/*
キャラクターが一定時間で位置を移動するスクリプト
現在画面内にいる全てのキャラクターを管理(3体)
*/

public class CharactorChangePos : MonoBehaviour
{
    public static CharactorChangePos Instance;

    public CharactorChangePos() { Instance = this; }


    //キャラクターのクラス
    //要変更
    //  ゲームオブジェクトが被らないようにする
    [System.Serializable]
    public class CharctorClass {
        [SerializeField] int posX;
        [SerializeField] int posY;
        //該当するゲームオブジェクト
        [SerializeField] GameObject charObj;
        //画面外へと向かうオブジェクト
        GameObject outObj;
        //変化後のオブジェクト
        GameObject newObj;

        //位置を変えるまでの時間
        [SerializeField] float changePosTime;
        [SerializeField] float nowTime;
        //進む方向  T:right F:left
        bool lookDirection;

        //オブジェクトを配置する位置
        float[] posXPosition = { -780.0f, -470.0f, -155.0f, 155.0f, 470.0f, 780.0f };
        float[] posYPosition = { 225.0f, -125.0f, -475.0f };
        

        //コンストラクタ
        public CharctorClass(){

        }

        //各データを設定する
        public void SetData(int posY, GameObject charObj, GameObject parent, int beforePosX = -1)
        {
            this.posY = posY;
            this.charObj = Instantiate(charObj);
            this.charObj.transform.SetParent(parent.transform);
            this.charObj.transform.localScale = Vector2.one;
            
            if(beforePosX == 0)
            {
                lookDirection = false;
                posX = 5;
            }
            else if(beforePosX == 5)
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
            nowTime = 0.0f;
            changePosTime = Random.Range(OperationCharctor.getInstance().minTimeRange, OperationCharctor.getInstance().maxTimeRange);

            //位置設定
            SetPos();
        }

        //オブジェクト位置を設定する
        public void SetPos() {
            //新たに生み出した場合は画面外に初期位置を設定する
            if (lookDirection && posX == 0)
            {
                charObj.transform.localPosition = new Vector2(-1200, posYPosition[posY]);
            }
            else if (!lookDirection && posX == 5)
            {
                charObj.transform.localPosition = new Vector2(1200, posYPosition[posY]);
            }

            charObj.transform.DOLocalMove(new Vector2(posXPosition[posX], posYPosition[posY]), 0.5f);
            charObj.transform.GetChild(0).GetComponent<CharctorScript>().SetPosition(posX, posY);
        }

        public void SetNewObj(GameObject newObj)
        {
            this.newObj = newObj.gameObject;
        }

        //時間を測ってオブジェクトを前へ進める
        public void GoFront() {
            nowTime += Time.deltaTime;
            if (nowTime >= changePosTime)
            {
                if (lookDirection)
                {
                    if (posX < 5)
                    {
                        //前に進める
                        posX++;
                    }
                    else {
                        //ここから
                        //今あるオブジェクトは画面外へ
                        outObj = charObj.gameObject;
                        outObj.transform.DOLocalMove(new Vector2(1200, posYPosition[posY]), 0.5f);
                        Destroy(outObj.gameObject, 1.0f);

                        //もう前に進めないので新しいオブジェクトを生成する
                        OperationCharctor.getInstance().DeathBornObject(posY);
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
                        outObj = charObj.gameObject;
                        outObj.transform.DOLocalMove(new Vector2(-1200, posYPosition[posY]), 0.5f);
                        Destroy(outObj.gameObject, 1.0f);

                        //もう前に進めないので新しいオブジェクトを生成する
                        OperationCharctor.getInstance().DeathBornObject(posY);
                    }
                }
                SetPos();

                nowTime = 0.0f;
            }
        }
        
        //変化後のオブジェクトを破棄する
        public async UniTask DestroyNewObj()
        {
            var obj = newObj;
            //画面外へ移動
            if (newObj.transform.position.x > 0)
            {
                //数秒待機
                await UniTask.Delay(2000);
                await newObj.transform.DOLocalMove(new Vector2(1200, posY), 1.0f);
            }else
            {
                //数秒待機
                await UniTask.Delay(2000);
                await newObj.transform.DOLocalMove(new Vector2(-1200, posY), 1.0f);
            }
            //オブジェクトを削除
            Destroy(obj.gameObject, 0.5f);
        }
        //ゲームモード終了時各オブジェクトを縮小する
        public IEnumerator objShrink()
        {
            charObj.transform.DOScale(Vector2.zero, 0.9f).SetEase(Ease.InBack);
            yield return new WaitForSeconds(0.9f);
            Destroy(charObj.gameObject);
        }
    }

    //キャラクターを操作するクラス
    [System.Serializable]
    public class OperationCharctor
    {
        //キャラクターのクラス
        [SerializeField]
        CharctorClass[] charClass = new CharctorClass[3]
                                    { new CharctorClass(), new CharctorClass(), new CharctorClass() };
        //生成するキャラクターオブジェクトの実体
        [SerializeField] GameObject[] charactorIns;
        //キャラクターが占有されているか管理する
        int[] OccupancyCharctor = { -1, -1, -1 };

        //位置を変えるまでの時間の上限下限
        [Header("位置を変えるまでの時間の上限下限")]
        public float minTimeRange = 3.5f;
        public float maxTimeRange = 5.0f;

        //マスターデータ操作用オブジェクト
        MasterData masterData;

        //コルーチンを実行用のオブジェクト
        MyMonobehaviour myMonobehaviour;


        //Singleton用
        private static OperationCharctor operationCharctor = new OperationCharctor();


        //コンストラクタ
        //オブジェクトをステージに3つ配置
        public OperationCharctor() {  }

        //Singleton用
        //インスタンスを返す
        public static OperationCharctor getInstance() {
            return operationCharctor;
        }
        //インスタンスに代入する
        public static void loadInstance(OperationCharctor opr) {
            operationCharctor = opr;
        }


        //Startで実行
        public void OperatonCharctorStart()
        { 
            int tempCharNum = 0;            
            for (int i = 0; i < 3; i++)
            {
                do
                {
                    tempCharNum = Random.Range(0, charactorIns.Length);
                } while (tempCharNum == OccupancyCharctor[0] || tempCharNum == OccupancyCharctor[1] || tempCharNum == OccupancyCharctor[2]);

                charClass[i].SetData(i, charactorIns[tempCharNum], GameObject.Find("Charctors"));
                OccupancyCharctor[i] = tempCharNum;
            }
            myMonobehaviour = GameObject.Find("CoroutineSystem").GetComponent<MyMonobehaviour>();
            masterData = GameObject.Find("GameControler").GetComponent<MasterData>();
        }
        
        //Updateで実行
        public void OperatonCharctorUpdate() {

            for(int i = 0; i < 3; i++)
            {
                charClass[i].GoFront();
            }

        }

        //オブジェクトを破棄し、新たなオブジェクトを生成する
        public void DeathBornObject(int posY)
        {
            OccupancyCharctor[posY] = -1;
            int tempCharNum = 0;
            do
            {
                tempCharNum = (int)Random.Range(0, charactorIns.Length);
            } while (tempCharNum == OccupancyCharctor[0] || tempCharNum == OccupancyCharctor[1] || tempCharNum == OccupancyCharctor[2]);
            charClass[posY].SetData(posY, charactorIns[tempCharNum], GameObject.Find("Charctors"));
            OccupancyCharctor[posY] = tempCharNum;
        }

        //キャラクター変化があった時
        //新規オブジェクトを作り出す
        //変化後オブジェクトを変数に記憶する
        public void ChangeCharctor(int posX, int posY, GameObject newObj)
        {
            OccupancyCharctor[posY] = -1;
            int tempCharNum = 0;
            do
            {
                tempCharNum = (int)Random.Range(0, charactorIns.Length);
            } while (tempCharNum == OccupancyCharctor[0] || tempCharNum == OccupancyCharctor[1] || tempCharNum == OccupancyCharctor[2]);
            charClass[posY].SetData(posY, charactorIns[tempCharNum], GameObject.Find("Charctors"), posX);
            OccupancyCharctor[posY] = tempCharNum;

            charClass[posY].SetNewObj(newObj);

            //得点をプラスする
            masterData.AddScore(newObj);

            //changeObjを一定時間でFOさせる
            charClass[posY].DestroyNewObj().Forget();           
        }

        //ゲームモード終了時各オブジェクトを縮小する
        public void GameFinish()
        {
            for(int i = 0; i < 3; i++)
            {
                myMonobehaviour.CallStartCoroutine(charClass[i].objShrink());
            }
        }
    }


    //他スクリプトで呼び出し用の変数
    public void GameSceneAfter()
    {
        OperationCharctor.getInstance().OperatonCharctorStart();
    }
    public void GameSceneContinuation()
    {
        OperationCharctor.getInstance().OperatonCharctorUpdate();
    }
    public void GameSceneBefore()
    {
        OperationCharctor.getInstance().GameFinish();
    }


    public OperationCharctor opChar = new OperationCharctor();



    
    void Start()
    {
        OperationCharctor.loadInstance(opChar);
    }
    
    void Update()
    {
        //OperationCharctor.getInstance().OperatonCharctorUpdate();
    }
}
