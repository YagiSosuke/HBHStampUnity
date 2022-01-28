using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*スタンプ画像*/

public class StumpImageScript : MonoBehaviour
{
    [SerializeField] GameObject Image;       //スタンプのイメージ
    Vector3 MousePos;       //マウスの位置

    [SerializeField]
    Text NowParts;          //現在の部位
    [SerializeField]
    Text NowWord;           //現在の言葉
    

    //スタンプ画像に付いているパーツテキストをセット
    public void NowPartsTextSet(string parts)
    {
        NowParts.text = parts;
    }

    string PartsConversionJapanese(Parts _parts)
    {
        switch (_parts)
        {
            case Parts.Head:
                return "頭";
            case Parts.Body:
                return "体";
            case Parts.Hip:
                return "尻";
            default:
                return "";
        }
    }

    void Update()
    {
        MousePos = Input.mousePosition;
        MousePos.z = 0;
        Image.transform.position = MousePos;

        //TODO: 改良できる
        NowParts.text = PartsConversionJapanese(Stamp.Instance.Parts);
        NowWord.text = Stamp.Instance.Word;
    }
}
