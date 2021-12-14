using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*スタンプ画像*/

public class StumpImageScript : MonoBehaviour
{
    GameObject Image;       //スタンプのイメージ
    Vector3 MousePos;       //マウスの位置

    [SerializeField]
    Text NowParts;          //現在の部位
    [SerializeField]
    Text NowWord;           //現在の言葉
    //public static Text NowParts;          //現在の部位
    //public static Text NowWord;           //現在の言葉


    // Start is called before the first frame update
    void Start()
    {
        Image = GameObject.Find("StumpImage");
    }

    // Update is called once per frame
    void Update()
    {
        MousePos = Input.mousePosition;
        MousePos.z = 0;
        Image.transform.position = MousePos;

        NowParts.text = StumpScript.TempStump;
        NowWord.text = StumpScript.stampPartsWord[StumpScript.TempStump];
    }

    //スタンプ画像に付いているパーツテキストをセット
    public void NowPartsTextSet(string parts)
    {
        NowParts.text = parts;
    }
}
