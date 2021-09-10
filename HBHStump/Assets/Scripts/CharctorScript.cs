using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;

/*キャラクターにスタンプ打った時のスクリプト（辞書）*/


public class CharctorScript : MonoBehaviour, IPointerClickHandler
{
    public enum PartsType
    {
        Head,
        Body,
        Hip
    }
    public enum ClareFlag
    {
        Clare,
        Failed
    }

    //辞書クラス
    [System.Serializable]
    class DictionaryClass
    {
        public PartsType Parts = PartsType.Head;        //パーツ部分
        public string Word;       //言葉
        public GameObject AfterObject;      //変化後のオブジェクト
        public ClareFlag Flag = ClareFlag.Failed;       //このオブジェクトがクリア条件であるか
    };


    GameObject NewObj;          //変化後のオブジェクト

    string CharName;           //キャラクタの名前
    GameObject ParentObject;   //親となるオブジェクト

    [SerializeField] DictionaryClass[] Dic;     //辞書クラス

    string PartsJap;            //部分名を日本語で言ったら?
    bool ChangeF;               //キャラクターを変身させるフラグ

    GameObject FogParticle;        //煙のオブジェクト
    GameObject FogUI;        //煙のオブジェクト
    GameObject FogUIInstance;

    float count = 0;                  //カウント

    public AudioSource audio = null;     //オーディオソース（スタンプに）

    bool SerchF;        //検索結果
    int ChangeNum = 0;      //辞書の何番目に変化させるか

    [SerializeField] int pos_x;     //自分の今いる座標
    [SerializeField] int pos_y;     //自分の今いる座標

    ClareConditions clare;          //クリア条件を組み込んでいるスクリプト
    

    // Start is called before the first frame update
    void Start()
    {
        CharName = this.gameObject.name.Replace("Image_", "");

        ParentObject = GameObject.Find("Charctors");
        PartsJap = "";

        ChangeF = false;

        FogParticle = (GameObject)Resources.Load("Prefabs/CFX2_WWExplosion_C_Copy");
        string objName = pos_x + "-" + pos_y + "Effect";
        //FogUI = GameObject.Find(objName);
        //FogUI.transform.SetParent(this.gameObject.transform.parent.transform, true);
        
        //audio = GameObject.Find("StumpImage").GetComponent<AudioSource>();

        SerchF = false;

        clare = GameObject.Find("Main Camera").GetComponent<ClareConditions>();
    }

    // Update is called once per frame
    void Update()
    {
        //この位置にスタンプが押されたとき
        if (Serial.PushF[pos_x, pos_y])
        {
            ChangeF = true;
            Serial.PushF[pos_x, pos_y] = false;
        }

        //キャラクターを変身させる
        if (ChangeF && !SerchF)
        {
            //何に変身させるか
            for(int i = 0; i < Dic.Length; i++)
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
                if(ButtonNameChange.TempWord == Dic[i].Word && StumpScript.TempStump == PartsJap)
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


        if(SerchF == true)
        {
            if (count == 0)
            {
                Debug.Log("Coordinate = " + pos_x + "," + pos_y);
                //煙を出す
                Instantiate(FogParticle, new Vector3(GameObject.Find(("" + pos_x + "-" + pos_y + "Camera")).transform.position.x, GameObject.Find((string)("" + pos_x + "-" + pos_y + "Camera")).transform.position.y-.8f, GameObject.Find((string)("" + pos_x + "-" + pos_y + "Camera")).transform.position.z + 5), Quaternion.Euler(new Vector3(90, 0, -this.gameObject.transform.eulerAngles.z)));

                //クリック音声再生
                //audio.Play();
            }
            else if (count > 0.0f && count <= 5.0f)
            {
                //変化後のオブジェクトを出す
                if (this.gameObject.GetComponent<Image>())
                {
                    Debug.Log(Dic[ChangeNum].AfterObject);
                    NewObj = Instantiate(Dic[ChangeNum].AfterObject, this.transform.parent.transform.position, Quaternion.identity, ParentObject.transform);


                    //新ゲームモード用の処理
                    if (SceneManager.GetActiveScene().name == "ManyChangeScene")
                    {
                        CharactorChangePos ccp = GameObject.Find("Charctors").GetComponent<CharactorChangePos>();
                        ccp.opChar.ChangeCharctor(pos_y, NewObj);
                    }
                    
                    //FogUI.transform.SetParent(NewObj.transform, true);
                    //FogUIInstance.transform.SetParent(NewObj.transform.GetChild(0).transform, true);
                    Destroy(this.gameObject.GetComponent<Image>());
                    Destroy(this.gameObject.transform.parent.transform.Find("WordFlame(Clone)").gameObject);
                    
                    ClareConditions.ChangeCharctor++;     //変化したキャラ数をプラスする
                    if (Dic[ChangeNum].Flag == ClareFlag.Clare)
                    {
                        ClareConditions.ClareCharctor++;
                    }
                }
            }
            else if(count > 5.0f)
            {
                Destroy(this.gameObject.transform.parent.gameObject);
            }
            count += Time.deltaTime;
        }
    }


    //キャラクターをクリックしたら
    public void OnPointerClick(PointerEventData pointerData)
    {
        ChangeF = true;
    }

    //位置情報を代入する
    public void SetPosition(int pos_x, int pos_y) {
        this.pos_x = pos_x;
        this.pos_y = pos_y;
    }
}