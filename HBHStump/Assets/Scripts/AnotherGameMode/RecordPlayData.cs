using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecordPlayData : MonoBehaviour
{
    string recordTimeName;
    string directoryPass;
    string filePass;

    float startTime;

    [SerializeField] SceneControl sceneControl;
    SceneControl.ScreenMode nowScene;
    [SerializeField] MasterData masterData;
    
    // Start is called before the first frame update
    void Start()
    {
        //Init();

        nowScene = SceneControl.ScreenMode.Title;
    }

    //記録データのセットアップ　
    void Init()
    {
        recordTimeName = $"{System.DateTime.Now.Year}{System.DateTime.Now.Month}{System.DateTime.Now.Day}_{System.DateTime.Now.Hour}{System.DateTime.Now.Minute}{System.DateTime.Now.Second}";

#if UNITY_EDITOR
        directoryPass = Path.Combine(Application.dataPath, @"File\");
#elif UNITY_STANDALONE
        directoryPass = Path.Combine(Application.persistentDataPath, @"File");
#endif
        filePass = Path.Combine(directoryPass, $@"{recordTimeName}.csv");
        var f = File.CreateText(filePass);
        f.Dispose();
        Debug.Log(recordTimeName);

        startTime = Time.time;
    }

    //情報をファイルに追加 上は別クラスで呼び出し
    public void writeChangeData(string beforeName, string afterName, string partsName)
    {
        var changeTime = Time.time;
        var text = $"{beforeName},{afterName},{partsName},{changeTime - startTime}\n";

        File.AppendAllText(filePass, text);
    }
    void writeResultData()
    {
        File.AppendAllText(filePass, $"{masterData.score}");
    }


    // Update is called once per frame
    void Update()
    {
        if(sceneControl.screenMode == SceneControl.ScreenMode.Game)
        {
            if (nowScene != sceneControl.screenMode) {
                Init();
                nowScene = sceneControl.screenMode;
            }
        }
        else if(sceneControl.screenMode == SceneControl.ScreenMode.GameFinish)
        {
            if (nowScene != sceneControl.screenMode)
            {
                writeResultData();
                nowScene = sceneControl.screenMode;
            }
        }
    }
}
