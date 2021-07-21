using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*キャラクタの名前を自動でセットする*/

public class CharctorNameSet : MonoBehaviour
{
    string Name;        //オブジェクトの名前
    [SerializeField] GameObject FramePrefab;        //フレーム
    GameObject Frame;        //フレームの実体

    [SerializeField] GameObject FrameChildPrefab;   //フレーム1つ
    GameObject FrameChild;   //フレーム1つの実体

    GameObject AddFrame;        //追加した文字

    float count = 0;            //カウントする

    float NamePosY = 150;

    // Start is called before the first frame update
    void Start()
    {
        Name = this.gameObject.name.Replace("Image_", "").Replace("(Clone)", "");      //名前取得
        //Name += " ";
        Debug.Log(Name);

        //枠組みを形成
        Frame = Instantiate(FramePrefab, new Vector3(transform.position.x, transform.position.y+NamePosY, transform.position.z), Quaternion.identity, this.gameObject.transform.parent.gameObject.transform);
        Frame.GetComponent<RectTransform>().sizeDelta = new Vector2((Name.Length) * 100, 100);        //枠のサイズを決定

        //文字を指定
        for(int i = 0; i < Name.Length; i++)
        {
            FrameChild = Instantiate(FrameChildPrefab, Frame.transform.position, Quaternion.identity, Frame.transform);
            FrameChild.transform.GetChild(0).GetComponent<Text>().text = Name.Substring(i, 1);

            if (i == 0)
            {
                if (gameObject.tag == "Head")
                {
                    AddFrame = FrameChild;
                    FrameChild.GetComponent<Image>().color = Color.green;
                    //Frame.GetComponent<RectTransform>().sizeDelta = new Vector2((Name.Length - 1) * 100, 100);
                }
            }
            else if (i == (Name.Length) / 2)
            {
                if (gameObject.tag == "Body")
                {
                    AddFrame = FrameChild;
                    FrameChild.GetComponent<Image>().color = Color.green;
                    //Frame.GetComponent<RectTransform>().sizeDelta = new Vector2((Name.Length - 1) * 100, 100);
                }
            }
            else if (i == Name.Length - 1)
            {
                if (gameObject.tag == "Hip")
                {
                    AddFrame = FrameChild;
                    FrameChild.GetComponent<Image>().color = Color.green;
                    //Frame.GetComponent<RectTransform>().sizeDelta = new Vector2((Name.Length - 1) * 100, 100);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //スタンプ 打つとき
        if ((gameObject.tag == "Head" || gameObject.tag == "Body" || gameObject.tag == "Hip") && count < 3)
        {
            if(count == 0)
            {
                //Frame.GetComponent<RectTransform>().sizeDelta = new Vector2((Name.Length-1) * 100, 100);        //枠のサイズを決定
                AddFrame.transform.localScale = Vector3.zero;
            }
            else if(count < 1 )
            {
                float lerp = (count) * (count) * (count) * (count);
                //Frame.GetComponent<RectTransform>().sizeDelta = new Vector2((Name.Length - 1) * 100 + (lerp * 100), 100);        //枠のサイズを決定
                AddFrame.transform.localScale = new Vector3(3 - lerp*2,3 - lerp*2,3 - lerp*2);
            }
            else
            {
                //Frame.GetComponent<RectTransform>().sizeDelta = new Vector2((Name.Length) * 100, 100);        //枠のサイズを決定
                AddFrame.transform.localScale = Vector3.one;
                Frame.GetComponent<AudioSource>().Play();
                count = 10;
            }

            count += Time.deltaTime;
        }
    }
}
