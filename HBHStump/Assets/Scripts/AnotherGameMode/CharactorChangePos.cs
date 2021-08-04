using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
キャラクターが一定時間で位置を移動するスクリプト
現在画面内にいる全てのキャラクターを管理(3体)
*/

public class CharactorChangePos : MonoBehaviour
{


    //キャラクターのクラス
    //要変更
    //  ゲームオブジェクトが被らないようにする
    [System.Serializable]
    public class CharctorClass {
        [SerializeField] int posX;
        [SerializeField] int posY;
        //該当するゲームオブジェクト
        [SerializeField] GameObject charObj;
        
        //位置を変えるまでの時間
        [SerializeField] float changePosTime;
        [SerializeField] float minTimeRange = 3.5f;
        [SerializeField] float maxTimeRange = 5.0f;
        [SerializeField] float nowTime;
        //進む方向  T:right F:left
        bool lookDirection;

        //オブジェクトを配置する位置
        float[] posXPosition = { -750.0f, -350.0f, 0.0f, 350.0f, 750.0f };
        float[] posYPosition = { 225.0f, -175.0f, -475.0f };


        //コンストラクタ
        public CharctorClass(){

        }

        //各データを設定する
        public void SetData(int posY, GameObject charObj, GameObject parent) {
            if (Random.Range(0, 2) == 0)
            {
                lookDirection = true;
                posX = 0;
            }else
            {
                lookDirection = false;
                posX = 4;
            }
            this.posY = posY;
            this.charObj = Instantiate(charObj);
            this.charObj.transform.parent = parent.transform;
            nowTime = 0.0f;
            changePosTime = Random.Range(minTimeRange, maxTimeRange);

            Debug.Log(charObj);

            SetPos();
        }

        //オブジェクト位置を設定する
        public void SetPos() {
            charObj.transform.localPosition = new Vector2(posXPosition[posX], posYPosition[posY]);
        }

        //時間を測ってオブジェクトを前へ進める
        //  進めないときの処理を加える
        public void GoFront() {
            nowTime += Time.deltaTime;
            if (nowTime >= changePosTime)
            {
                if (lookDirection)
                {
                    if (posX < 4)
                    {
                        posX++;
                    }
                    else {

                    }
                }
                else
                {
                    if (posX > 0)
                    {
                        posX--;
                    }
                }
                SetPos();

                nowTime = 0.0f;
            }
        }

        //オブジェクトを破棄する
        //OperationCharctorでしか呼び出さない
        public void DestroyObject()
        {
            charObj.gameObject.transform.localPosition = new Vector2(1200, 0);
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


        //コンストラクタ
        //オブジェクトをステージに3つ配置
        public OperationCharctor() {  }

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
        }


        //Updateで実行
        public void OperatonCharctorUpdate() {

            for(int i = 0; i < 3; i++)
            {
                charClass[i].GoFront();
            }

        }

        //オブジェクトを破棄し、新たなオブジェクトを生成する
        public void DestroyObject(int posY)
        {
            charClass[posY].DestroyObject();
            charClass[posY].SetData(posY, charactorIns[Random.Range(0, charactorIns.Length)], GameObject.Find("Charctors"));

            OccupancyCharctor[posY] = -1;
            int tempCharNum = 0;
            do
            {
                tempCharNum = (int)Random.Range(0, charactorIns.Length);
            } while (tempCharNum == OccupancyCharctor[0] || tempCharNum == OccupancyCharctor[1] || tempCharNum == OccupancyCharctor[2]);
            charClass[posY].SetData(posY, charactorIns[tempCharNum], GameObject.Find("Charctors"));
            OccupancyCharctor[posY] = tempCharNum;
        }
        
    }


    [SerializeField]OperationCharctor operationCharctor = new OperationCharctor();


    // Start is called before the first frame update
    void Start()
    {
        operationCharctor.OperatonCharctorStart();
    }

    // Update is called once per frame
    void Update()
    {
        operationCharctor.OperatonCharctorUpdate();
    }
}
