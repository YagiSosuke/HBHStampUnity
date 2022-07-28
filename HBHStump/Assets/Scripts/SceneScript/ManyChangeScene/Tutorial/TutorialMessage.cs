using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

/*
チュートリアルのメッセージを管理する - 以下、チュートリアル順序

ゲームの内容を説明する
    かん→みかん
    かん→かめん
    かん→かんな
スタンプ操作方法
    右と左ボタンで母音の変更
    真ん中ボタンで部位の変更
    カードを読み込んで子音の変更
かんをみかんに変身
*/

//TODO: タイムアウトの実装
public class TutorialMessage : MonoBehaviour
{
    string[] message = { "くま", "<Action>こんにちは、ぼくはくま！\nモジプラスタンプの世界にようこそ！",
                                      "くま", "モジプラスタンプのあそびかたを\nせつめいするよ",
                                      "くま", "モジプラスタンプはキャラクターの名前に\n文字をくわえて\n別のキャラクターに変身させるゲームだよ",
                                      "くま", "<Action>たとえば、『かん』のあたまに『み』をつけると\n『みかん』",
                                      "くま", "<Action>『かん』のまんなかに『め』をつけると\n『かめん』",
                                      "くま", "<Action>『かん』のおしりに『な』をつけると\n『かんな』に変身するよ",
                                      "くま", "<Action>次に文字の追加について説明するよ",
                                      "くま", "文字の追加にはスタンプをつかうよ",
                                      "くま", "<Action><Condition>右のボタンで母音\n(あ, い, う……)を変えられるよ\n早速やってみよう！",
                                      "くま", "いいね！その調子！",
                                      "くま", "<Action><Condition>左のボタンではんたいに母音\n(お, え, う……)を変えられるよ\n早速やってみよう！",
                                      "くま", "いいね！その調子！",
                                      "くま", "<Action><Condition>真ん中のボタンで文字をくわえる場所\n(頭, 体, お尻)を変えられるよ\n早速やってみよう！",
                                      "くま", "いいね！その調子！",
                                      "くま", "<Action><Condition>カードにスタンプを押すとスタンプに文字を\n読み込むことができるよ\n下にあるカードを読んでみよう！",
                                      "くま", "<Action>いいね！その調子！",
                                      "くま", "スタンプの説明は以上だよ",
                                      "くま", "<Action>それじゃあ、『かん』を『みかん』\nに変身させてみよう！",
                                      "くま", "変身させるまでのステップはここに\n表示しておくよ",
                                      "くま", "<Condition>文字をセットしたら、みかんにスタンプを\n打ってね！\n早速やってみよう！",
                                      "くま", "いいね！その調子！",
                                      "くま", "本番は90秒でたくさんキャラクターを\n変身させてね！",
                                      "くま", "本番もがんばってね！"
                                    };
    [Header("StepControllers")]
    [SerializeField] TutorialStep tutorialStep = TutorialStep.BearGreeting;
    [SerializeField] TransitionMode transitionMode = TransitionMode.afterSwitching;
    [Header("Panels")]
    [SerializeField] CanvasGroup tutorialPanelCanvasGroup;
    [SerializeField] VerificationPanelScript verificationPanelScript;
    [SerializeField] MessageWindow messageWindow;
    [SerializeField] ExplainPanel explainPanel;
    [Header("TutorialMaterial")]
    [SerializeField] TutorialCharactorScript tutorialCharactorScript;
    [Header("Serial")]
    [SerializeField] Serial serial;

    SceneController SceneController => SceneController.Instance;
        
    void TransitionChange()
    {
        if (tutorialStep < TutorialStep.EndStep && transitionMode < TransitionMode.beforeSwitching)
        {
            transitionMode++;
        }
        else
        {
            transitionMode = 0;
            tutorialStep++;
        }
    }
    
    async UniTask TutorialVerification()
    {
        await UniTask.WaitUntil(() => verificationPanelScript.IsTakeTutorial || verificationPanelScript.IsNotTakeTutorial);
        if (verificationPanelScript.IsTakeTutorial)
        {
            TransitionChange();
        }
        else if (verificationPanelScript.IsNotTakeTutorial)
        {
            ct.Cancel();
            SceneController.screenMode = (ScreenMode)(SceneController.screenMode + 1);
            transitionMode = TransitionMode.afterSwitching;
            explainPanel.HideVerificationPanel();
        }
    }
    void ShowMessage(string[] message, float time_sec)
    {
        switch (transitionMode)
        {
            case TransitionMode.afterSwitching:
                var actionInMessage = new List<Action>(){
                    () => explainPanel.OnBearGreeting(),
                    () => explainPanel.OnBearGreeting_Mikan(),
                    () => explainPanel.OnBearGreeting_Kamen(),
                    () => explainPanel.OnBearGreeting_Kannna(),
                    () => explainPanel.OnStampOperation_GrapStamp(),
                    () => explainPanel.OnStampOperation_RightButton(),
                    () => explainPanel.OnStampOperation_LeftButton(),
                    () => explainPanel.OnStampOperation_MiddleButton(),
                    () => explainPanel.OnStampOperation_CardRead(),
                    () => explainPanel.OnStampOperation_CardReadDoes(),
                    () => explainPanel.OnStampOperation_TutorialMikan1()
                };
                var conditionInMessage = new List<Func<bool>>(){
                    () => OnRightButtonPush(),
                    () => OnLeftButtonPush(),
                    () => OnMiddleButtonPush(),
                    () => OnCardRead(),
                    () => tutorialCharactorScript.OnTutorialMikanChange()
                };
                messageWindow.LoadMessage(new List<string>(message), actionInMessage, conditionInMessage);
                WaitForTimeout(time_sec).Forget();
                messageWindow.ShowMessage().Forget();
                TransitionChange();
                break;
            case TransitionMode.continuation:
                if ((Input.GetMouseButtonDown(0) || serial.pushCheck()) && 
                     messageWindow.IsFinishMessageGroup() && messageWindow.IsFinishMessageLine())
                {
                    TransitionChange();
                }
                break;
            case TransitionMode.beforeSwitching:
                ct.Cancel();
                TransitionChange();
                break;
        }
    }
    async UniTask PlayTutorial()
    {
        await UniTask.WaitUntil(() => SceneController.screenMode == ScreenMode.Tutorial, cancellationToken: this.GetCancellationTokenOnDestroy());
        while (SceneController.screenMode == ScreenMode.Tutorial)
        {
            tutorialPanelCanvasGroup.blocksRaycasts = true;

            switch (tutorialStep)
            {
                case TutorialStep.TutorialVerification:
                    explainPanel.ShowVerificationPanel();
                    ShowTutorialPanel();
                    WaitForTimeout(verificationTimeout_sec).Forget();
                    verificationPanelScript.Init();
                    tutorialCharactorScript.Init();
                    TransitionChange();

                    TutorialVerification();
                    await UniTask.WaitUntil(() => transitionMode != TransitionMode.continuation);

                    explainPanel.HideVerificationPanel();
                    ct.Cancel();
                    TransitionChange();
                    break;
                case TutorialStep.BearGreeting:
                    ShowMessage(message, tutorialTimeout_sec);
                    break;
                case TutorialStep.EndStep:
                    explainPanel.OnEndStep();
                    transitionMode = TransitionMode.afterSwitching;
                    tutorialStep = TutorialStep.TutorialVerification;
                    SceneController.screenMode = (ScreenMode)(SceneController.screenMode + 1);
                    break;
            }
            await UniTask.DelayFrame(1, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
        tutorialPanelCanvasGroup.blocksRaycasts = false;
        PlayTutorial().Forget();
    }

    #region 条件を変化させる際に必要な処理
    string beforeWord = "";
    Parts beforeParts = Parts.NULL;
    bool OnRightButtonPush()
    {
        string nextWord = "";
        var wordSanple = MasterData.Instance.wordSample;
        switch (beforeWord)
        {
            case "": break;
            case "お":
                nextWord = "あ"; break;
            case "こ":
                nextWord = "か"; break;
            case "そ":
                nextWord = "さ"; break;
            case "と":
                nextWord = "た"; break;
            case "の":
                nextWord = "な"; break;
            case "ほ":
                nextWord = "は"; break;
            case "も":
                nextWord = "ま"; break;
            case "よ":
                nextWord = "や"; break;
            case "ろ":
                nextWord = "ら"; break;
            case "ん":
                nextWord = "わ"; break;
            case "ご":
                nextWord = "が"; break;
            case "ぞ":
                nextWord = "ざ"; break;
            case "ど":
                nextWord = "だ"; break;
            case "ぼ":
                nextWord = "ば"; break;
            case "ぽ":
                nextWord = "ぱ"; break;
            default:
                nextWord = wordSanple[wordSanple.IndexOf(beforeWord) + 1]; break;
        }

        if (Stamp.Instance.Word == nextWord)
        {
            beforeWord = "";
            return true;
        }
        else
        {
            if (beforeWord != Stamp.Instance.Word) beforeWord = Stamp.Instance.Word;
            return false;
        }
    }
    bool OnLeftButtonPush()
    {
        string nextWord = "";
        var wordSanple = MasterData.Instance.wordSample;
        switch (beforeWord)
        {
            case "": break;
            case "あ":
                nextWord = "お"; break;
            case "か":
                nextWord = "こ"; break;
            case "さ":
                nextWord = "そ"; break;
            case "た":
                nextWord = "と"; break;
            case "な":
                nextWord = "の"; break;
            case "は":
                nextWord = "ほ"; break;
            case "ま":
                nextWord = "も"; break;
            case "や":
                nextWord = "よ"; break;
            case "ら":
                nextWord = "ろ"; break;
            case "わ":
                nextWord = "ん"; break;
            case "が":
                nextWord = "ご"; break;
            case "ざ":
                nextWord = "ぞ"; break;
            case "だ":
                nextWord = "ど"; break;
            case "ば":
                nextWord = "ぼ"; break;
            case "ぱ":
                nextWord = "ぽ"; break;
            default:
                nextWord = wordSanple[wordSanple.IndexOf(beforeWord) - 1]; break;
        }

        if (Stamp.Instance.Word == nextWord)
        {
            beforeWord = "";
            return true;
        }
        else
        {
            if (beforeWord != Stamp.Instance.Word) beforeWord = Stamp.Instance.Word;
            return false;
        }
    }
    bool OnMiddleButtonPush()
    {
        Parts nextParts = Parts.NULL;
        switch (beforeParts)
        {
            case Parts.NULL: break;
            case Parts.Head:
                nextParts = Parts.Body; break;
            case Parts.Body:
                nextParts = Parts.Hip; break;
            case Parts.Hip:
                nextParts = Parts.Head; break;
        }


        Debug.Log($"beforeParts = {beforeParts}:nextParts = {nextParts}");
        if (Stamp.Instance.Parts == nextParts)
        {
            return true;
        }
        else
        {
            if (beforeParts != Stamp.Instance.Parts) beforeParts = Stamp.Instance.Parts;
            return false;
        }
    }
    bool OnCardRead()
    {
        if (serial.IsUseDevice)
        {
            return Serial.isCardRead;
        }
        else
        {
            //本当はこの処理ではいけないが、デバッグ用に新たに処理を追加すると冗長になるので……
            return (Stamp.Instance.Word == "あ" ||
                    Stamp.Instance.Word == "か" ||
                    Stamp.Instance.Word == "さ" ||
                    Stamp.Instance.Word == "た" ||
                    Stamp.Instance.Word == "な" ||
                    Stamp.Instance.Word == "は" ||
                    Stamp.Instance.Word == "ま" ||
                    Stamp.Instance.Word == "や" ||
                    Stamp.Instance.Word == "ら" ||
                    Stamp.Instance.Word == "わ" ||
                    Stamp.Instance.Word == "が" ||
                    Stamp.Instance.Word == "ざ" ||
                    Stamp.Instance.Word == "だ" ||
                    Stamp.Instance.Word == "ば" ||
                    Stamp.Instance.Word == "ぱ");
        }
    }
    #endregion
    #region タイムアウト関連
    //TODO: デバッグ時邪魔だったので一時1000倍した、後に元に戻す
    CancellationTokenSource ct = new CancellationTokenSource();
    float verificationTimeout_sec = 30000.0f;
    float tutorialTimeout_sec = 60000.0f;
    float taskTimeout_sec = 120000.0f;
    
    async UniTask WaitForTimeout(float duration)
    {
        TutorialStep _step = tutorialStep;

        ct = new CancellationTokenSource();

        await UniTask.Delay((int)(duration * 1000), cancellationToken: ct.Token);

        if (_step == tutorialStep)
        {
            //全てのパネルを非表示
            HideTutorialPanel();
            tutorialStep = TutorialStep.TutorialVerification;
            transitionMode = TransitionMode.afterSwitching;

            //タイトルへ
            SceneController.screenMode = ScreenMode.Title;
            SceneController.transitionMode = TransitionMode.afterSwitching;
        }
    }
    #endregion

    void ShowTutorialPanel()
    {
        tutorialPanelCanvasGroup.DOFade(endValue: 1.0f, duration: 1.0f);
    }
    void HideTutorialPanel()
    {
        tutorialPanelCanvasGroup.DOFade(endValue: 0.0f, duration: 1.0f).OnComplete(() =>
        {
            explainPanel.PanelsInit();
        });
    }

    void Start()
    {
        ct = new CancellationTokenSource();
        PlayTutorial().Forget();
    }
}

//チュートリアルの段階
public enum TutorialStep
{
    TutorialVerification = 0,
    BearGreeting,
    EndStep
};