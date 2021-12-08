using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/*
    ランキングを更新する 
*/

public class RankingControl : MonoBehaviour
{
    string directoryName;
    string fileName;

    [SerializeField] List<int> rankData = new List<int>(10);
    [SerializeField] List<Text> rankText = new List<Text>(10);
    [SerializeField] List<Image> rankBack = new List<Image>(10);

    SceneControl sceneControl;

    //現在のスコアによってランキングを更新
    public int rankUpdate(int score)
    {
        int rank = -1;
        int temp = -1;

        for(int i = 0; i < rankData.Count; i++)
        {
            if(temp != -1)
            {
                //値を入れ替える
                (temp, rankData[i]) = (rankData[i], temp);
            }
            else
            {
                if(score >= rankData[i])
                {
                    temp = rankData[i];
                    rankData[i] = score;

                    rank = i + 1;
                }
            }
        }

        //以下、csvの更新
        File.WriteAllText(fileName, "");
        for(int i = 0; i < rankData.Count; i++)
        {
            File.AppendAllText(fileName, rankData[i].ToString());
            if (i + 1 < rankData.Count)
            {
                File.AppendAllText(fileName, ",");
            }
        }

        //以下、ランキングパネルの更新
        for(int i = 0; i < rankData.Count; i++)
        {
            rankText[i].text = rankData[i].ToString();
        }

        return rank;
    }
    
    //ランク更新時、自分のランクが分かるようにbackを点滅させる
    public async UniTask rankBackFlash(int rankNum)
    {
        var duration = 1.0f;

        do
        {
            rankBack[rankNum - 1].DOColor(new Color(1.0f, 1.0f, (150.0f / 255.0f)), duration);
            await UniTask.Delay(((int)duration * 1000));
            rankBack[rankNum - 1].DOColor(Color.white, duration);
            await UniTask.Delay(((int)duration * 1000));
        } while (sceneControl.screenMode == SceneControl.ScreenMode.Result);

    }
    void Start()
    {
        sceneControl = GameObject.Find("GameControler").GetComponent<SceneControl>();

#if UNITY_EDITOR
        directoryName = Path.Combine(Application.dataPath, @"File\");
#elif UNITY_STANDALONE
        directoryName = Path.Combine(Application.persistentDataPath, @"File");
#endif
        fileName = Path.Combine(directoryName, @"RankingData.csv");

        //ディレクトリがない場合作成
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
            for (int i = 0; i < rankData.Count; i++)
            {
                rankData[i] = 0;
                File.AppendAllText(fileName, "0");
                if(i+1 < rankData.Count)
                {
                    File.AppendAllText(fileName, ",");
                }

            }
        }
        //ある場合、値をrankDataに保存
        else
        {
            string[] text = File.ReadAllText(fileName).Split(',');
            for(int i = 0; i < rankData.Count; i++)
            {
                rankData[i] = int.Parse(text[i]);
            }
        }
    }
}
