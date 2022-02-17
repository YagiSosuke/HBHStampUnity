using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class CharaCsvLoader : MonoBehaviour
{
    public static CharaCsvLoader Instance;
    public CharaCsvLoader() { if (!Instance) Instance = this; }

    [SerializeField] TextAsset beforeChangeCharaCsvData;
    [SerializeField] TextAsset[] charaCsvDatas;
    public List<CharaData> beforeChangeCharaDatas = new List<CharaData>();
    public Dictionary<string, List<CharaData>> afterChangeCharaDatas = new Dictionary<string, List<CharaData>>();


    void Initialize()
    {
        CharaDataLoad();
    }
    void CharaDataLoad()
    {
        StringReader reader = new StringReader(beforeChangeCharaCsvData.text);

        //変化前キャラクターをセット
        reader.ReadLine();
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            beforeChangeCharaDatas.Add(new CharaData(line.Split(','), beforeChangeCharaDatas.Count));
        }

        //変化後キャラクターをセット
        foreach (TextAsset charaCsvData in charaCsvDatas)
        {
            List<CharaData> data = new List<CharaData>();
            StringReader reader2 = new StringReader(charaCsvData.text);

            reader2.ReadLine();
            while (reader2.Peek() != -1)
            {
                string line = reader2.ReadLine();
                data.Add(new CharaData(line.Split(','), data.Count));
            }
            afterChangeCharaDatas.Add(charaCsvData.name.Replace("_data", ""), data);
        }
    }

    void Start()
    {
        Initialize();
    }
}

public class CharaData
{
    public int ID { get; private set; }
    public string CharaName { get; private set; }
    public AnimType AnimType { get; private set; }
    public Parts Parts { get; private set; }
    public Sprite Sprite { get; private set; }

    public CharaData(string[] text, int num)
    {
        try
        {
            ID = num;
            CharaName = text[0];
            AnimType = (AnimType)int.Parse(text[1]);
            if (text.Length > 2)
            {
                switch (text[2])
                {
                    case "Head":
                        Parts = Parts.Head;
                        break;
                    case "Body":
                        Parts = Parts.Body;
                        break;
                    case "Hip":
                        Parts = Parts.Hip;
                        break;
                }
            }
            Sprite = CharaImageData.Instance.GetSprite(CharaName);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            Debug.Log($"{text[0]} {text[1]} {text[2]}:csv変換エラーです");
        }
    }
}