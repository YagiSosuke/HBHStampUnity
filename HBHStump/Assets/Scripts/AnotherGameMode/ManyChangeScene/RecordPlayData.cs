using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class RecordPlayData : MonoBehaviour
{
    string recordTimeName;
    string directoryPass;
    string filePass;

    float startTime;

    [SerializeField] SceneControl sceneControl;
    SceneControl.ScreenMode nowScene = SceneControl.ScreenMode.Title;
    MasterData MasterData => MasterData.Instance;
    
    

    //記録データのセットアップ　
    void Init()
    {
        recordTimeName = $"{System.DateTime.Now.Year.ToString("0000")}{System.DateTime.Now.Month.ToString("00")}{System.DateTime.Now.Day.ToString("00")}_{System.DateTime.Now.Hour.ToString("00")}{System.DateTime.Now.Minute.ToString("00")}{System.DateTime.Now.Second.ToString("00")}";

#if UNITY_EDITOR
        directoryPass = Path.Combine(Application.dataPath, @"File\");
#elif UNITY_STANDALONE
        directoryPass = Path.Combine(Application.persistentDataPath, @"File");
#endif
        filePass = Path.Combine(directoryPass, $@"{recordTimeName}.csv");
        var f = File.CreateText(filePass);
        f.Dispose();

        startTime = Time.time;
    }

    //情報をファイルに追加 上は別クラスで呼び出し
    public void WriteChangeData(string beforeName, string afterName, string partsName)
    {
        var changeTime = Time.time;
        var text = $"{beforeName},{afterName},{partsName},{changeTime - startTime}\n";
        
        File.AppendAllText(filePass, text);
    }
    void WriteResultData()
    {
        File.AppendAllText(filePass, $"{MasterData.score}");
    }

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
                WriteResultData();
                nowScene = sceneControl.screenMode;
            }
        }
    }
}
