using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*身体のパーツを選択するスクリプト*/

public class StumpScript : MonoBehaviour
{
    //スタンプのパーツごとに記憶している言葉を登録するクラス
    public class StampPartsWord {
        private string parts;
        private string word;

        public StampPartsWord(string s) {
            parts = s;
            word = "あ";
        }
        public void setWord(string s) {
            word = s;
        }
        public string getParts() {
            return parts;
        }
        public string getWord()
        {
            return word;
        }
    }
    //指定したパーツのクラスを返す
    public static StampPartsWord GetPartsWord(StampPartsWord[] spw, string parts) {
        StampPartsWord tempSPW = new StampPartsWord("");
        for(int i = 0; i < spw.Length; i++)
        {
            if(spw[i].getParts() == parts)
            {
                tempSPW = spw[i];
                break;
            }
        }
        return tempSPW;
    }

    //スタンプのパーツごとに言葉を登録するクラス
    public static StampPartsWord[] stampPartsWord = new StampPartsWord[3]
                                        { new StampPartsWord("頭"),
                                          new StampPartsWord("体"),
                                          new StampPartsWord("尻") };

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
        StumpScript.nowWordText["頭"].text = StumpScript.GetPartsWord(stampPartsWord, "頭").getWord(); //記録されている文字を視覚的に表す
        StumpScript.nowWordText["体"].text = StumpScript.GetPartsWord(stampPartsWord, "体").getWord(); //記録されている文字を視覚的に表す
        StumpScript.nowWordText["尻"].text = StumpScript.GetPartsWord(stampPartsWord, "尻").getWord(); //記録されている文字を視覚的に表す
    }

    //スタンプ名記録
    public void SetStump()
    {
        TempStump = this.gameObject.name;
        Frame.gameObject.transform.position = this.transform.position;
    }
}
