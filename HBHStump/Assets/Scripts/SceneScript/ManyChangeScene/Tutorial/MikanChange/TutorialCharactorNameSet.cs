using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*十中八九エラる*/

public class TutorialCharactorNameSet : MonoBehaviour
{
    string Name;                    //オブジェクトの名前
    [SerializeField]
    GameObject FramePrefab;         //フレーム
    GameObject Frame;               //フレームの実体

    [SerializeField]
    GameObject FrameChildPrefab;    //フレーム1つ
    GameObject FrameChild;          //フレーム1つの実体
    GameObject newFrame;            //追加した文字
    float count = 0;                //カウントする
    float NamePosY = 150;

    [SerializeField] TutorialCharactorScript tutorialCharactorScript;

    void Start()
    {
        Name = this.gameObject.name.Replace("Image_", "").Replace("(Clone)", "");      //名前取得
        
        //枠組みを形成
        Frame = Instantiate(FramePrefab, new Vector3(transform.position.x, transform.position.y + NamePosY, transform.position.z), Quaternion.identity, this.gameObject.transform.parent.gameObject.transform);
        Frame.transform.localPosition = new Vector3(0, NamePosY, transform.position.z);
        Frame.transform.localScale = Vector2.one;
        Frame.GetComponent<RectTransform>().sizeDelta = new Vector2((Name.Length) * 100, 100);        //枠のサイズを決定

        //文字を指定
        for (int i = 0; i < Name.Length; i++)
        {
            FrameChild = Instantiate(FrameChildPrefab, Frame.transform.position, Quaternion.identity, Frame.transform);
            FrameChild.transform.GetChild(1).GetComponent<Text>().text = Name.Substring(i, 1);

            if (i == 0)
            {
                if (gameObject.tag == "Head" || gameObject.tag == "HeadSample")
                {
                    newFrame = FrameChild;
                    FrameChild.GetComponent<Image>().color = Color.green;
                }
            }
            else if (i == (Name.Length) / 2)
            {
                if (gameObject.tag == "Body" || gameObject.tag == "BodySample")
                {
                    newFrame = FrameChild;
                    FrameChild.GetComponent<Image>().color = Color.green;
                }
            }
            else if (i == Name.Length - 1)
            {
                if (gameObject.tag == "Hip" || gameObject.tag == "HipSample")
                {
                    newFrame = FrameChild;
                    FrameChild.GetComponent<Image>().color = Color.green;
                }
            }
        }
    }

    void Update()
    {
        /*
        //スタンプ 打つとき
        if ((gameObject.tag == "Head" || gameObject.tag == "Body" || gameObject.tag == "Hip") && count < 3)
        {
            if (count == 0)
            {
                newFrame.transform.localScale = Vector3.zero;
            }
            else if (count < 1)
            {
                float lerp = (count) * (count) * (count) * (count);
                newFrame.transform.localScale = new Vector3(3 - lerp * 2, 3 - lerp * 2, 3 - lerp * 2);
            }
            else
            {
                newFrame.transform.localScale = Vector2.one;
                Frame.GetComponent<AudioSource>().Play();       //TODO: ここエラると思う
                count = 10;
            }
        }

        //かんを変身させたときに count を増やす
        if (tutorialCharactorScript.isSerch)
        {
            count += Time.deltaTime;
        }
        else
        {
            count = 0;
        }
        */
    }
}