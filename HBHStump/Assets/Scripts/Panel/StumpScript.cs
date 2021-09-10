using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*身体のパーツを選択するスクリプト*/

public class StumpScript : MonoBehaviour
{

    //スタンプのパーツごとに言葉を登録する変数
    /*
    public static StampPartsWord stampPartsWord = new StampPartsWord[3]
                                        { new StampPartsWord("頭"),
                                          new StampPartsWord("体"),
                                          new StampPartsWord("尻") };
    */
    public static Dictionary<string, string> stampPartsWord = new Dictionary<string, string>()
    {
        { "頭", "あ" },
        { "体", "あ" },
        { "尻", "あ" }
    };

    public static string TempStump;         //選択したスタンプ名

    [SerializeField] GameObject Frame;      //選択したボタンの枠

    //各パーツごとの言葉を表示するテキスト
    public static Dictionary<string, Text> nowWordText = new Dictionary<string, Text>();

    // Start is called before the first frame update
    void Start()
    {
        TempStump = "頭";
        nowWordText["頭"] = GameObject.Find("AtamaText").GetComponent<Text>();
        nowWordText["体"] = GameObject.Find("KaradaText").GetComponent<Text>();
        nowWordText["尻"] = GameObject.Find("ShiriText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        StumpScript.nowWordText["頭"].text = stampPartsWord["頭"]; //記録されている文字を視覚的に表す
        StumpScript.nowWordText["体"].text = stampPartsWord["体"]; //記録されている文字を視覚的に表す
        StumpScript.nowWordText["尻"].text = stampPartsWord["尻"]; //記録されている文字を視覚的に表す
    }

    //スタンプ名記録
    public void SetStump()
    {
        TempStump = this.gameObject.name;
        Frame.gameObject.transform.position = this.transform.position;
    }
}
