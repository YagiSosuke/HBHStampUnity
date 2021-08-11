using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
シーン遷移や各シーンごとの処理をまとめて操作する
*/

public class SceneControl : MonoBehaviour
{
    //画面遷移のパターン
    public enum ScreenMode
    {
        Title,
        Game,
        Result
    }
    public ScreenMode screenMode = ScreenMode.Title;
    public enum TransitionMode
    {
        afterSwitching,
        continuation,
        beforeSwitching
    }
    public TransitionMode transitionMode = TransitionMode.afterSwitching;
    
    //画面遷移するまでのインターバル
    float stateChangeInterval = -1.0f;

    //時間を表示するテキスト
    [SerializeField] Text remainingTimeText;
    [SerializeField] Text ScoreText;

    //別スクリプトからメソッド呼び出し用
    [Header("別スクリプトからメソッド呼び出し用")]
    [SerializeField] MasterData masterData;
    [SerializeField] TitleCharImageMove titleCharImageMove;
    [SerializeField] CharactorChangePos charactorChangePos;
    [SerializeField] ResultPanelControl resultPanelControl;
    

    //数秒後に状態を遷移する
    void StateChange()
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
                    case ScreenMode.Title:
                        screenMode++;
                        break;
                    case ScreenMode.Game:
                        screenMode++;
                        break;
                    case ScreenMode.Result:
                        screenMode = 0;
                        break;
                }
                break;
        }
    }
    IEnumerator StateChange(float num)
    {
        yield return new WaitForSeconds(num);

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
                    case ScreenMode.Title:
                        screenMode++;
                        break;
                    case ScreenMode.Game:
                        screenMode++;
                        break;
                    case ScreenMode.Result:
                        screenMode = 0;
                        break;
                }
                break;
        }
        stateChangeInterval = -1;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

                    StartCoroutine(StateChange(stateChangeInterval));
                }
            }
            else if (transitionMode == TransitionMode.continuation)
            {
                if (Input.GetMouseButton(0))
                {
                    StateChange();
                }
            }
            else if (transitionMode == TransitionMode.beforeSwitching)
            {
                if (stateChangeInterval == -1)
                {
                    stateChangeInterval = 1.0f;
                    titleCharImageMove.TitleSceneBefore(stateChangeInterval);
                    StartCoroutine(StateChange(stateChangeInterval));
                }
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
                remainingTimeText.text = masterData.remainingTime.ToString("0");
                //得点を表示
                ScoreText.text = masterData.score.ToString() + "point";

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
                remainingTimeText.text = "0";
                charactorChangePos.GameSceneBefore();
                
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

                    StartCoroutine(resultPanelControl.ResultSceneAfter(stateChangeInterval));
                    StartCoroutine(StateChange(stateChangeInterval));
                }
            }
            else if (transitionMode == TransitionMode.continuation)
            {
                //連続で呼び出し続ける
                if (Input.GetMouseButtonDown(0))
                {
                    StateChange();
                }
            }
            else if (transitionMode == TransitionMode.beforeSwitching)
            {
                resultPanelControl.ResultSceneBefore(1.0f);
                StateChange();
            }
            #endregion
        }
    }
}
