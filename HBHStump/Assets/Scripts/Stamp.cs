using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*パーツ、部位を記憶するクラス*/

public class Stamp : MonoBehaviour
{
    public static Stamp Instance;
    Stamp() { if (!Instance) Instance = this; }

    public string Word { get; private set; } = "あ";
    public Parts Parts { get; private set; } = Parts.NULL;
    
    //各パーツごとの言葉を表示するテキスト
    [SerializeField] Text[] nowWordTexts;

    
    public void SetWord(string _word)
    {
        Word = _word;
        DisplayWord();
    }
    public void SetParts(Parts _parts)
    {
        Parts = _parts;
    }
    void DisplayWord()
    {
        foreach(Text text in nowWordTexts)
        {
            text.text = Word;
        }
    }
}
public enum Parts
{
    NULL = 0,
    Head = 1,
    Body = 2,
    Hip = 3
    
}