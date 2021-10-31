using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

/*
シーン遷移や各シーンごとの処理をまとめて操作する
*/

public class SceneControl : MonoBehaviour
{
    //画面遷移のパターン
    public enum ScreenMode
    {
        Title,
        Tutorial,
        GameSetting,
        Game,
        GameFinish,
        Result,
        Hint
    }
    public ScreenMode screenMode = ScreenMode.Title;
    public enum TransitionMode
    {
        afterSwitching,
        continuation,
        beforeSwitching
    }
    public TransitionMode transitionMode = TransitionMode.afterSwitching;
    //1度のみ実行するときのフラグ
    bool onceDoF = false;
    
    //画面遷移するまでのインターバル
    float stateChangeInterval = -1.0f;
    
    //別スクリプトからメソッド呼び出し用
    [Header("別スクリプトからメソッド呼び出し用")]
    [SerializeField] MasterData masterData;
    [SerializeField] TitleCharImageMove titleCharImageMove;
    [SerializeField] CharactorChangePos charactorChangePos;
    [SerializeField] ResultPanelControl resultPanelControl;
    [SerializeField] HintPanel hintPanel;


    //数秒後に状態を遷移する
    public void StateChange()
    {
        switch (transitionMode)
        {
            case TransitionMode.afterSwitching:
                transitionMode++;
                break;
            case TransitionMode.continuation:
                transitionMode++;
                break;
            case TransitionMode.beforeSwitching:
                transitionMode = 0;
                switch (screenMode)
                {
                    case ScreenMode.Hint:
                        screenMode = 0;
                        break;
                    default:
                        screenMode++;
                        break;
                }
                break;
        }
    }
    public async UniTask StateChange(float num)
    {
        await UniTask.Delay((int)(num * 1000));

        switch (transitionMode)
        {
            case TransitionMode.afterSwitching:
                transitionMode++;
                break;
            case TransitionMode.continuation:
                transitionMode++;
                break;
            case TransitionMode.beforeSwitching:
                transitionMode = 0;
                switch (screenMode)
                {
                    case ScreenMode.Hint:
                        screenMode = 0;
                        break;
                    default:
                        screenMode++;
                        break;
                }
                break;
        }
        stateChangeInterval = -1;
    }

    //デバイスとの通信をするスクリプト
    [SerializeField] Serial serialScript;

    //セームをセットアップするときのカウント
    async UniTask GameSetupCount()
    {
        await UniTask.Delay(3000);
        StateChange();
    }

    //セームを終了するときのカウント
    async UniTask GameFinishCount()
    {
        await UniTask.Delay(1500);
        StateChange();
    }
    
    void Update()
    {
        if (screenMode == ScreenMode.Title)
        {
            #region
            if (transitionMode == TransitionMode.afterSwitching)
            {
                if (stateChangeInterval == -1)
                {
                    stateChangeInterval = 1.0f;
                    titleCharImageMove.TitleSceneAfter(stateChangeInterval);

                    StateChange(stateChangeInterval).Forget();
                }
            }
            else if (transitionMode == TransitionMode.continuation)
            {
                if (Input.GetMouseButton(0))
                {
                    StateChange();
                }
                else if(serialScript.enabled == true)
                {
                    for(int i=0; i < 15; i++)
                    {
                        if(Serial.PushF[i%5, i / 5])
                        {
                            StateChange();
                            break;
                        }
                    }
                }
            }
            else if (transitionMode == TransitionMode.beforeSwitching)
            {
                if (stateChangeInterval == -1)
                {
                    stateChangeInterval = 1.0f;
                    titleCharImageMove.TitleSceneBefore(stateChangeInterval);
                    StateChange(stateChangeInterval).Forget();
                }
            }
            #endregion
        }
        else if(screenMode == ScreenMode.Tutorial)
        {
            #region

            #endregion
        }
        else if(screenMode == ScreenMode.GameSetting)
        {
            #region
            if (transitionMode == TransitionMode.afterSwitching)
            {
                GameSetupCount().Forget();
                StateChange();
            }
            else if (transitionMode == TransitionMode.continuation)
            {
            }
            else if (transitionMode == TransitionMode.beforeSwitching)
            {
                StateChange();
            }
            #endregion
        }
        else if (screenMode == ScreenMode.Game)
        {
            #region
            if (transitionMode == TransitionMode.afterSwitching)
            {
                masterData.StatusInitialization();
                charactorChangePos.GameSceneAfter();
                
                StateChange();
            }
            else if (transitionMode == TransitionMode.continuation)
            {
                //制限時間を減らしていく
                masterData.remainingTime -= Time.deltaTime;

                //キャラクター変化全般を操作する
                charactorChangePos.GameSceneContinuation();

                //時間が無くなったら状態遷移
                if (masterData.remainingTime <= 0)
                {
                    StateChange();
                }
            }
            else if (transitionMode == TransitionMode.beforeSwitching)
            {
                charactorChangePos.GameSceneBefore();
                
                StateChange();
            }
            #endregion
        }
        else if (screenMode == ScreenMode.GameFinish)
        {
            #region
            if (transitionMode == TransitionMode.afterSwitching)
            {
                GameFinishCount().Forget();
                StateChange();
            }
            else if (transitionMode == TransitionMode.continuation)
            {
            }
            else if (transitionMode == TransitionMode.beforeSwitching)
            {
                StateChange();
            }

            #endregion
        }
        else if (screenMode == ScreenMode.Result)
        {
            #region
            if (transitionMode == TransitionMode.afterSwitching)
            {
                if (stateChangeInterval == -1)
                {
                    stateChangeInterval = 1.0f;

                    resultPanelControl.ResultSceneAfter(stateChangeInterval).Forget();
                    StateChange(masterData.score * 0.1f + 1.0f).Forget();
                }
            }
            else if (transitionMode == TransitionMode.continuation)
            {
                resultPanelControl.ResultSceneContinuation();

                //ランキングパネルが出ていないときに実行
                if (resultPanelControl.rankNum == -1)
                {
                    if (Input.GetMouseButtonDown(0) || (serialScript.enabled == true && serialScript.pushCheck()))
                    {
                        StateChange();
                    }
                }
            }
            else if (transitionMode == TransitionMode.beforeSwitching)
            {
                resultPanelControl.ResultSceneBefore(1.0f);
                StateChange();
            }
            #endregion
        }
        else if (screenMode == ScreenMode.Hint)
        {
            #region
            if (transitionMode == TransitionMode.afterSwitching)
            {
                if (!onceDoF)
                {
                    //パネル表示するまでのインターバル
                    UniTask.Void(async () =>
                    {
                        onceDoF = true;
                        await hintPanel.GameSceneAfter();
                        onceDoF = false;
                        StateChange();
                    });
                }
            }
            else if (transitionMode == TransitionMode.continuation)
            {
                hintPanel.GameSceneContinuation();

                //時間差でステップ変わる
                if (!onceDoF)
                {
                    UniTask.Void(async () =>
                    {
                        onceDoF = true;
                        await UniTask.Delay(10000);
                        onceDoF = false;
                        StateChange();
                    });
                }
            }
            else if (transitionMode == TransitionMode.beforeSwitching)
            {
                //パネル消えるまでのインターバル
                if (!onceDoF)
                {
                    UniTask.Void(async () =>
                    {
                        onceDoF = true;
                        await hintPanel.GameSceneBefore();
                        onceDoF = false;
                        StateChange();
                    });
                }
            }
            #endregion
        }
    }
}
