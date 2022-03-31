using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

/*キャラクターにスタンプ打った時のスクリプト（チュートリアル）*/

public class TutorialCharactorScript : MonoBehaviour, IPointerClickHandler
{
    readonly Vector2[] mikanPosition =
    {
        new Vector2(1, 0),
        new Vector2(1, 1)
    };

    bool isMikanChange;
    [SerializeField] GameObject kanObject;
    [SerializeField] GameObject mikanObject;
    [SerializeField] Serial serial;
    

    //TODO: 管理プログラムから呼ぶようにする
    async UniTask Initialize()
    {
        //TODO: みかんを隠し、かんを表示する処理
        isMikanChange = false;
        kanObject.SetActive(true);
        mikanObject.SetActive(false);
    }

    void CharaChange()
    {
        kanObject.SetActive(false);
        mikanObject.SetActive(true);
    }

    public bool OnTutorialMikanChange()
    {
        if((Stamp.Instance.Word == "み" && Stamp.Instance.Parts == Parts.Head &&
            (Serial.PushF[(int)mikanPosition[0].x, (int)mikanPosition[0].y] ||
             Serial.PushF[(int)mikanPosition[1].x, (int)mikanPosition[1].y])) ||
             isMikanChange)
        {
            CharaChange();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        isMikanChange = true;
    }

    /*
    [System.Serializable]
    class DictionaryClass
    {
        public Parts Parts = Parts.Head;        //パーツ部分
        public string Word;       //言葉
        public GameObject AfterObject;      //変化後のオブジェクト
    };
    [System.Serializable]
    class Coordinate
    {
        public int pos_x;
        public int pos_y;
    }

    [SerializeField] DictionaryClass[] Dic;     //辞書クラス
    [SerializeField] Coordinate[] coordinate;   //座標クラス
    string CharName;                            //キャラクタの名前
    [SerializeField] GameObject ParentObject;   //親となるオブジェクト
    [Header("変化後に表示するもの")]
    [SerializeField] CanvasGroup kanObj;        //かんのオブジェクト群
    [SerializeField] CanvasGroup mikanObj;      //変化後のオブジェクト
    [Header("煙系")]
    GameObject FogParticle;                     //煙のオブジェクト
    GameObject FogUI;                           //煙のオブジェクト
    GameObject FogUIInstance;
    [SerializeField] GameObject tutorialFogCamera;
    
    int ChangeNum = 0;                  //辞書の何番目に変化させるか
    float count = 0;                    //カウント
    public bool isChange;                //キャラクターを変身させるフラグ    
    public bool isSerch;                 //検索結果
    [SerializeField] TutorialMessage tutorialMessage;   //チュートリアルの段階を管理

    
    public void KanSetup()
    {
        if (tutorialMessage.transitionMode == TransitionMode.afterSwitching)
        {
            isSerch = false;
            isChange = false;
            mikanObj.alpha = 0.0f;
            kanObj.alpha = 1.0f;
            count = 0.0f;
        }
    }
    

    
    void Start()
    {
        CharName = this.gameObject.name.Replace("Image_", "");
        FogParticle = (GameObject)Resources.Load("Prefabs/CFX2_WWExplosion_C_Copy");
    }
    //スタンプが押されたときの処理
    public void PushStamp()
    {
        //この位置にスタンプが押されたとき
        for (int i = 0; i < coordinate.Length; i++)
        {
            if (Serial.PushF[coordinate[i].pos_x, coordinate[i].pos_y])
            {
                isChange = true;
                Serial.PushF[coordinate[i].pos_x, coordinate[i].pos_y] = false;
                break;
            }
        }

        //キャラクターを変身させる
        if (isChange && !isSerch)
        {
            //何に変身させるか
            for (int i = 0; i < Dic.Length; i++)
            {
                //辞書と言葉、部位が同じかどうか検索
                if (Stamp.Instance.Word == Dic[i].Word && Stamp.Instance.Parts == Dic[i].Parts)
                {
                    isSerch = true;
                    ChangeNum = i;
                }
                else
                {
                    isChange = false;
                }
            }
        }


        if (isSerch == true)
        {
            if (count == 0)
            {
                //煙を出す
                Instantiate(FogParticle, new Vector3(tutorialFogCamera.transform.position.x, tutorialFogCamera.transform.position.y - .8f, tutorialFogCamera.transform.position.z + 5), Quaternion.Euler(new Vector3(90, 0, -this.gameObject.transform.eulerAngles.z)));
            }
            else if (count > 0.0f && count <= 5.0f)
            {
                //変化後のオブジェクトを出す
                if (this.gameObject.GetComponent<Image>())
                {
                    Debug.Log(Dic[ChangeNum].AfterObject);
                    mikanObj.alpha = 1.0f;
                    kanObj.alpha = 0.0f;
                }
            }
            if (0 <= count && count <= 6.0f)
            {
                count += Time.deltaTime;
            }
        }
    }
   //キャラクターをクリックしたら
    public void OnPointerClick(PointerEventData pointerData)
    {
        isChange = true;
    }
    */
}
