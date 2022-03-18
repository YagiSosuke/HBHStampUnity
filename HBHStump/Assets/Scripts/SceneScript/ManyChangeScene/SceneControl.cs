﻿using System.Collections;
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
    public ScreenMode screenMode = ScreenMode.Title;
    public TransitionMode transitionMode = TransitionMode.afterSwitching;
    //1度のみ実行するときのフラグ
    bool onceDoF = false;
    
    //画面遷移するまでのインターバル
    float stateChangeInterval = -1.0f;
    
    //別スクリプトからメソッド呼び出し用
    MasterData MasterData => MasterData.Instance;
    [Header("別スクリプトからメソッド呼び出し用")]
    [SerializeField] TitleCharImageMove titleCharImageMove;
    CharacterController CharacterController => CharacterController.Instance;
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
        Debug.Log($"pushCheck = {serialScript.pushCheck()}");

        if (screenMode == ScreenMode.Title)
        {
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
                else if(serialScript.IsUseDevice == true)
                {
                    if (serialScript.pushCheck())
                    {
                        StateChange();
                    }
                }
            }
            else if (transitionMode == TransitionMode.beforeSwitching)
            {
                if (stateChangeInterval == -1)
                {
                    stateChangeInterval = 1.0f;
                    titleCharImageMove.TitleSceneBefore(stateChangeInterval);
                    Debug.Log("statechange");
                    StateChange(stateChangeInterval).Forget();
                }
            }
        }
        else if(screenMode == ScreenMode.Tutorial)
        {
        }
        else if(screenMode == ScreenMode.GameSetting)
        {
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
        }
        else if (screenMode == ScreenMode.Game)
        {
            if (transitionMode == TransitionMode.afterSwitching)
            {
                MasterData.Initialize();
                CharacterController.GameSceneAfter();
                
                StateChange();
            }
            else if (transitionMode == TransitionMode.continuation)
            {
                //制限時間を減らしていく
                MasterData.CurrentTime -= Time.deltaTime;

                //キャラクター変化全般を操作する
                CharacterController.GameSceneContinuation();

                //時間が無くなったら状態遷移
                if (MasterData.CurrentTime <= 0)
                {
                    StateChange();
                }
            }
            else if (transitionMode == TransitionMode.beforeSwitching)
            {
                //TODO: 変更
                CharacterController.GameSceneBefore();

                StateChange();
            }
        }
        else if (screenMode == ScreenMode.GameFinish)
        {
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
        }
        else if (screenMode == ScreenMode.Result)
        {
            if (transitionMode == TransitionMode.afterSwitching)
            {
                if (stateChangeInterval == -1)
                {
                    stateChangeInterval = 1.0f;

                    resultPanelControl.ResultSceneAfter(stateChangeInterval).Forget();
                    StateChange(MasterData.Score * 0.1f + 1.0f).Forget();
                }
            }
            else if (transitionMode == TransitionMode.continuation)
            {
                resultPanelControl.ResultSceneContinuation();

                //ランキングパネルが出ていないときに実行
                if (resultPanelControl.RankNum == -1)
                {
                    if (Input.GetMouseButtonDown(0) || (serialScript.IsUseDevice == true && serialScript.pushCheck()))
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
        }
        else if (screenMode == ScreenMode.Hint)
        {
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
                        await UniTask.Delay(6000);
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
        }
    }
}

public enum ScreenMode
{
    Title = 0,
    Tutorial,
    GameSetting,
    Game,
    GameFinish,
    Result,
    Hint
}
public enum TransitionMode
{
    afterSwitching = 0,
    continuation,
    beforeSwitching
}