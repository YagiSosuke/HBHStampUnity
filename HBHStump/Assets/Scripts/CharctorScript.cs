using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using Cysharp.Threading.Tasks;

/*キャラクターにスタンプ打った時のスクリプト（辞書）*/


public class CharctorScript : MonoBehaviour, IPointerClickHandler
{
    //辞書クラス
    [System.Serializable]
    class DictionaryClass
    {
        public Parts Parts = Parts.Head;        //パーツ部分
        public string Word;       //言葉
        public GameObject AfterObject;      //変化後のオブジェクト
    };
    [SerializeField] DictionaryClass[] Dic;     //辞書クラス

    GameObject newObj;                   //変化後のオブジェクト    
    GameObject parentObj;                //親となるオブジェクト
    GameObject fogParticle;              //煙のオブジェクト    
    public AudioSource audio = null;     //オーディオソース（スタンプに）
    int changeNum = 0;                   //辞書の何番目に変化させるか
    [SerializeField] int pos_x;          //自分の今いる座標
    [SerializeField] int pos_y;          //自分の今いる座標

    MasterData MasterData => MasterData.Instance;
    

    //位置情報を代入する
    public void SetPosition(int pos_x, int pos_y) {
        this.pos_x = pos_x;
        this.pos_y = pos_y;
    }
    void TryCharacterChange()
    {
        //何に変身させるか
        for (int i = 0; i < Dic.Length; i++)
        {
            //辞書と言葉、部位が同じかどうか検索
            if (Stamp.Instance.Word == Dic[i].Word && Stamp.Instance.Parts == Dic[i].Parts)
            {
                changeNum = i;
                InstantiateNewObj();

                //プレイデータ保存
                if (MasterData.recordPlayData.enabled)
                {
                    var beforeName = gameObject.name.Replace("Image_", "");
                    var afterName = Dic[i].AfterObject.name.Replace("Image_", "");
                    var partsName = Stamp.Instance.Parts.ToString();

                    MasterData.recordPlayData.WriteChangeData(beforeName, afterName, partsName);
                }
            }
        }
    }
    void InstantiateNewObj()
    {
        Debug.Log("Coordinate = " + pos_x + "," + pos_y);

        //煙を出す
        var fog = Instantiate(fogParticle, new Vector3(GameObject.Find(("" + pos_x + "-" + pos_y + "Camera")).transform.position.x, GameObject.Find((string)("" + pos_x + "-" + pos_y + "Camera")).transform.position.y - .8f, GameObject.Find((string)("" + pos_x + "-" + pos_y + "Camera")).transform.position.z + 5), Quaternion.Euler(new Vector3(90, 0, -this.gameObject.transform.eulerAngles.z)));
        Destroy(fog, 10.0f);

        //変化後のオブジェクトを出す
        if (this.gameObject.GetComponent<Image>())
        {
            Debug.Log(Dic[changeNum].AfterObject);
            newObj = Instantiate(Dic[changeNum].AfterObject, this.transform.parent.transform.position, Quaternion.identity, parentObj.transform);

            CharactorChangePos.Instance.operationCharctor.ChangeCharctor(pos_x, pos_y, newObj);

            Destroy(this.gameObject.GetComponent<Image>());
            Destroy(this.gameObject.transform.parent.transform.Find("WordFlame(Clone)").gameObject);
        }

        //旧オブジェクトを消す
        Destroy(this.gameObject.transform.parent.gameObject, 5.0f);
    }

    void Start()
    {
        parentObj = GameObject.Find("Charctors");
        fogParticle = (GameObject)Resources.Load("Prefabs/CFX2_WWExplosion_C_Copy");
    }
    void Update()
    {
        //この位置にスタンプが押されたとき
        if (Serial.PushF[pos_x, pos_y])
        {
            TryCharacterChange();
        }
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        TryCharacterChange();
    }
}