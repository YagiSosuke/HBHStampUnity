using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*キャラクターにスタンプ打った時のスクリプト（チュートリアル）*/

public class TutorialCharactorScript : MonoBehaviour, IPointerClickHandler
{
    //パーツの種類
    public enum PartsType
    {
        Head,
        Body,
        Hip
    }
    
    //辞書クラス
    [System.Serializable]
    class DictionaryClass
    {
        public PartsType Parts = PartsType.Head;        //パーツ部分
        public string Word;       //言葉
        public GameObject AfterObject;      //変化後のオブジェクト
    };

    //変化後に表示するもの、消すもの
    [Header("変化後に表示するもの")]
    [SerializeField] CanvasGroup kanObj;        //かんのオブジェクト群
    [SerializeField] CanvasGroup mikanObj;          //変化後のオブジェクト

    string CharName;           //キャラクタの名前
    [SerializeField] GameObject ParentObject;   //親となるオブジェクト

    [SerializeField]
    DictionaryClass[] Dic;     //辞書クラス

    string PartsJap;            //部分名を日本語で言ったら?
    public bool ChangeF;               //キャラクターを変身させるフラグ

    GameObject FogParticle;        //煙のオブジェクト
    GameObject FogUI;        //煙のオブジェクト
    GameObject FogUIInstance;

    float count = 0;                  //カウント

    public AudioSource audio = null;     //オーディオソース（スタンプに）

    public bool SerchF;        //検索結果
    int ChangeNum = 0;      //辞書の何番目に変化させるか

    [SerializeField] GameObject tutorialFogCamera;
    

    //座標クラス
    [System.Serializable] class Coordinate
    {
        public int pos_x;
        public int pos_y;
    }
    [SerializeField] Coordinate[] coordinate;
   
    //チュートリアルでのセットアップメソッド
    public void KanSetup()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            SerchF = false;
            ChangeF = false;
            mikanObj.alpha = 0.0f;
            kanObj.alpha = 1.0f;
            count = 0.0f;
        }
    }
    
    //チュートリアルの段階を管理
    [SerializeField] TutorialMessage tutorialMessage;

    
    // Start is called before the first frame update
    void Start()
    {
        CharName = this.gameObject.name.Replace("Image_", "");
        
        PartsJap = "";
        

        FogParticle = (GameObject)Resources.Load("Prefabs/CFX2_WWExplosion_C_Copy");
        /*
        string objName = pos_x + "-" + pos_y + "Effect";
        */
        //FogUI = GameObject.Find(objName);
        //FogUI.transform.SetParent(this.gameObject.transform.parent.transform, true);

        //audio = GameObject.Find("StumpImage").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //PushStamp();
    }



    //スタンプが押されたときの処理
    public void PushStamp()
    {
        //この位置にスタンプが押されたとき
        for (int i = 0; i < coordinate.Length; i++)
        {
            if (Serial.PushF[coordinate[i].pos_x, coordinate[i].pos_y])
            {
                ChangeF = true;
                Serial.PushF[coordinate[i].pos_x, coordinate[i].pos_y] = false;
                break;
            }
        }

        //キャラクターを変身させる
        if (ChangeF && !SerchF)
        {
            //何に変身させるか
            for (int i = 0; i < Dic.Length; i++)
            {
                switch (Dic[i].Parts)
                {
                    case PartsType.Head:
                        PartsJap = "頭";
                        break;
                    case PartsType.Body:
                        PartsJap = "体";
                        break;
                    case PartsType.Hip:
                        PartsJap = "尻";
                        break;
                }
                //辞書と言葉、部位が同じかどうか検索
                if (ButtonNameChange.TempWord == Dic[i].Word && StumpScript.TempStump == PartsJap)
                {
                    SerchF = true;
                    ChangeNum = i;
                }
                else
                {
                    ChangeF = false;
                }
            }
        }


        if (SerchF == true)
        {
            if (count == 0)
            {
                //煙を出す
                Instantiate(FogParticle, new Vector3(tutorialFogCamera.transform.position.x, tutorialFogCamera.transform.position.y - .8f, tutorialFogCamera.transform.position.z + 5), Quaternion.Euler(new Vector3(90, 0, -this.gameObject.transform.eulerAngles.z)));

                //クリック音声再生
                //audio.Play();
            }
            else if (count > 0.0f && count <= 5.0f)
            {
                //変化後のオブジェクトを出す
                if (this.gameObject.GetComponent<Image>())
                {
                    Debug.Log(Dic[ChangeNum].AfterObject);
                    mikanObj.alpha = 1.0f;
                    kanObj.alpha = 0.0f;
                    //Destroy(this.gameObject.GetComponent<Image>());
                    //Destroy(this.gameObject.transform.parent.transform.Find("WordFlame(Clone)").gameObject);
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
        ChangeF = true;
    }
}
