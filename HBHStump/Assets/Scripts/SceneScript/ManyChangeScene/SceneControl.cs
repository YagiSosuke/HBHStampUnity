using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

/*シーン遷移や各シーンごとの処理をまとめて操作する*/

public class SceneControl : MonoBehaviour
{
    public static SceneControl Instance;
    SceneControl() { if (!Instance) Instance = this; }

    public ScreenMode screenMode = ScreenMode.Title;
    public TransitionMode transitionMode = TransitionMode.afterSwitching;
    //1度のみ実行するときのフラグ
    bool onceDoF = false;
    
    //画面遷移するまでのインターバル
    float stateChangeInterval = -1.0f;
    
    //別スクリプトからメソッド呼び出し用
    [Header("別スクリプトからメソッド呼び出し用")]
    [SerializeField] TitleCharImageMove titleCharImageMove;
    [SerializeField] ResultPanelControl resultPanelControl;
    [SerializeField] HintPanel hintPanel;

    [SerializeField] Serial serialScript;

    MasterData MasterData => MasterData.Instance;
    CharacterController CharacterController => CharacterController.Instance;

    //数秒後に状態を遷移する
    public async UniTask StateChange(float duration = 0.0f)
    {
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
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

    void Update()
    {
        Debug.Log($"pushCheck = {serialScript.pushCheck()}");

        switch (screenMode)
        {
            case ScreenMode.Title:
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
                        StateChange().Forget();
                    }
                    else if (serialScript.IsUseDevice == true)
                    {
                        if (serialScript.pushCheck())
                        {
                            StateChange().Forget();
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
                break;
            case ScreenMode.Tutorial:
                break;
            case ScreenMode.GameSetting:
                if (transitionMode == TransitionMode.afterSwitching)
                {
                    StateChange(3.0f).Forget();
                    StateChange().Forget();
                }
                else if (transitionMode == TransitionMode.continuation)
                {
                }
                else if (transitionMode == TransitionMode.beforeSwitching)
                {
                    StateChange().Forget();
                }
                break;
            case ScreenMode.Game:
                if (transitionMode == TransitionMode.afterSwitching)
                {
                    MasterData.Initialize();
                    CharacterController.GameSceneAfter();

                    StateChange().Forget();
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
                        StateChange().Forget();
                    }
                }
                else if (transitionMode == TransitionMode.beforeSwitching)
                {
                    //TODO: 変更
                    CharacterController.GameSceneBefore();

                    StateChange().Forget();
                }
                break;
            case ScreenMode.GameFinish:
                if (transitionMode == TransitionMode.afterSwitching)
                {
                    StateChange(1.5f).Forget();
                    StateChange().Forget();
                }
                else if (transitionMode == TransitionMode.continuation)
                {
                }
                else if (transitionMode == TransitionMode.beforeSwitching)
                {
                    StateChange().Forget();
                }
                break;
            case ScreenMode.Result:
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
                            StateChange().Forget();
                        }
                    }
                }
                else if (transitionMode == TransitionMode.beforeSwitching)
                {
                    resultPanelControl.ResultSceneBefore(1.0f);
                    StateChange().Forget();
                }
                break;
            case ScreenMode.Hint:
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
                            StateChange().Forget();
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
                            StateChange().Forget();
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
                            StateChange().Forget();
                        });
                    }
                }
                break;
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