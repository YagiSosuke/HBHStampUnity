using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

/*
チュートリアルのメッセージを管理する
スクリプト

ゲームの内容を説明する
    かん→みかん
    かん→かめん
    かん→かんな
やってみよう
スタンプ操作方法
    右と左ボタンであいうえおの変更
    真ん中ボタンで部位の変更
    カードを読み込んで子音の変更
    
    かんを変身（自由変身）
    自由にやってみよう！(〇〇秒でたくさん変身させてね！)
*/

    //TODO: タイムアウトの実装
public class TutorialMessage : MonoBehaviour
{
    //シーンを管理する
    [SerializeField] SceneControl sceneControl;

    [SerializeField] TutorialStep tutorialStep = TutorialStep.BearGreeting;
    public TransitionMode transitionMode = TransitionMode.afterSwitching;
    
    [SerializeField] CanvasGroup tutorialPanelCanvasGroup;
    [SerializeField] VerificationPanelScript verificationPanelScript;
    [SerializeField] MessageWindow messageWindow;
    [SerializeField] ExplainPanel explainPanel;
    #region 表示するメッセージ
    string[] message_BearGreeting = { "くま", "こんにちは、ぼくはくま！\nモジプラスタンプの世界にようこそ！",
                                      "くま", "モジプラスタンプのあそびかたを\nせつめいするよ",
                                      "くま", "モジプラスタンプはキャラクターの名前に\n文字をくわえて\n別のキャラクターに変身させるゲームだよ"
                                    };
    string[] message_BearGreeting_Mikan = { "くま", "たとえば、『かん』のあたまに『み』をつけると\n『みかん』"
                                    };
    string[] message_BearGreeting_Kamen = { "くま", "『かん』のまんなかに『め』をつけると\n『かめん』"
                                    };
    string[] message_BearGreeting_Kannna = { "くま", "『かん』のおしりに『な』をつけると\n『かんな』に変身するよ",
                                    };
    string[] message_StampOperation_GrapStamp = { "くま", "次に文字の追加について説明するよ",
                                                  "くま", "文字の追加にはスタンプをつかうよ"
                                    };
    string[] message_StampOperation_RightButton = { "くま", "右のボタンで母音\n(あ, い, う……)を変えられるよ\n早速やってみよう！"
                                    };
    string[] message_StampOperation_RightButtonDoes = { "くま", "いいね！その調子！"
                                    };
    string[] message_StampOperation_LeftButton = { "くま", "左のボタンではんたいに母音\n(お, え, う……)を変えられるよ\n早速やってみよう！"
                                    };
    string[] message_StampOperation_LeftButtonDoes = { "くま", "いいね！その調子！"
                                    };
    string[] message_StampOperation_MiddleButton = { "くま", "真ん中のボタンで文字をくわえる場所\n(頭, 体, お尻)を変えられるよ\n早速やってみよう！"
                                    };
    string[] message_StampOperation_MiddleButtonDoes = { "くま", "いいね！その調子！"
                                    };
    string[] message_StampOperation_CardRead = { "くま", "カードにスタンプを押すとスタンプに文字を\n読み込むことができるよ\n下にあるカードを読んでみよう！"
                                    };
    string[] message_StampOperation_CardReadDoes = { "くま", "いいね！その調子！",
                                                   "くま", "スタンプの説明は以上だよ"
                                    };
    string[] message_StampOperation_TutorialMikan1 = { "くま", "それじゃあ、『かん』を『みかん』\nに変身させてみよう！",
                                    };
    string[] message_StampOperation_TutorialMikan2 = { "くま", "変身させるまでのステップはここに\n表示しておくよ",
                                                       "くま", "文字をセットしたら、みかんにスタンプを\n打ってね！\n早速やってみよう！"
                                    };
    string[] message_StampOperation_TutorialMikanDoes = { "くま", "いいね！その調子！",
                                                        "くま", "本番は90秒でたくさんキャラクターを\n変身させてね！",
                                                        "くま", "本番もがんばってね！"
                                    };
    #endregion
    

    //Transitionmodeを変化させる
    public void TransitionChange()
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

    //チュートリアルを受けるか確認する
    void TutorialVerification()
    {
        if (verificationPanelScript.isTakeTutorial || 
            (serialScipt.enabled == true && Serial.PushF[2, 1]))
        {
            TransitionChange();
        }
        else if (verificationPanelScript.isNotTakeTutorial ||
                 (serialScipt.enabled == true && Serial.PushF[3, 1]))
        {
            ct.Cancel();
            sceneControl.screenMode = (ScreenMode)((int)sceneControl.screenMode + 1);
            transitionMode = TransitionMode.afterSwitching;
            explainPanel.VerificationPanelFO();
        }
    }

    #region 各ステップで実行する内容
    //主に文字送りをするメソッド
    //transitionMode変更もまとめている
    //文字送り以外の処理を呼び出す場合は、この関数は最後に描く(transitionMode の Update も兼ねているため)
    void ViewingMessage(string[] message, float time_sec)
    {
        if (transitionMode == TransitionMode.afterSwitching)
        {
            messageWindow.LoadMessage(new List<string>(message));
            WaitForTimeout(time_sec).Forget();
            TransitionChange();
        }
        else if (transitionMode == TransitionMode.continuation)
        {
            messageWindow.MessageWindowUpdate();
            ShowTouchInstruction();
            if (messageWindow.messageFinish)
            {
                TransitionChange();
            }
        }
        else if (transitionMode == TransitionMode.beforeSwitching)
        {
            HideTouchInstruction();
            ct.Cancel();
            TransitionChange();
        }
    }
    //文字送りのためにタスクを達成する必要がある場合
    //stepChangeConditions にて別途条件を設定
    void ViewingMessage(string[] message, StepChangeConditions stepChangeConditions, float time_sec)
    {
        if (transitionMode == TransitionMode.afterSwitching)
        {
            messageWindow.LoadMessage(new List<string>(message));
            stepChangeConditions.setNowData();
            WaitForTimeout(time_sec).Forget();
            TransitionChange();
        }
        else if (transitionMode == TransitionMode.continuation)
        {
            messageWindow.MessageWindowUpdate();

            if (messageWindow.messageNum * 2 + 2 < message.Length && messageWindow.messageCount >= messageWindow.messageLength) 
            {
                ShowTouchInstruction();
            }
            else
            {
                HideTouchInstruction();
            }
            if (stepChangeConditions.StepChangeF())
            {
                TransitionChange();
            }
        }
        else if (transitionMode == TransitionMode.beforeSwitching)
        {
            HideTouchInstruction();
            ct.Cancel();
            TransitionChange();
        }
    }
    //タスクの実行で transition を変更するメソッド
    void ExececutionTask(string[] message, StepChangeConditions stepChangeConditions, float time_sec)
    {
        if (transitionMode == TransitionMode.afterSwitching)
        {
            messageWindow.LoadMessage(new List<string>(message));
            stepChangeConditions.setNowData();
            WaitForTimeout(time_sec).Forget();
            TransitionChange();
        }
        else if (transitionMode == TransitionMode.continuation)
        {
            Debug.Log($"messageWindow.messageCount = {messageWindow.messageCount}\nmessageWindow.messageLength = {messageWindow.messageLength}\nmessageWindow.message.Count = {messageWindow.message.Count}\nmessageWindow.messageNum = {messageWindow.messageNum * 2}");
           //クリックでメッセージを表示する
            if (messageWindow.messageCount < messageWindow.messageLength ||
                messageWindow.message.Count > messageWindow.messageNum * 2 + 2)
            {
                messageWindow.MessageWindowUpdate();
            }
            if (messageWindow.messageNum * 2 + 1 < message.Length-1 && 
                messageWindow.messageCount >= messageWindow.messageLength)
            {
                ShowTouchInstruction();
            }
            else
            {
                HideTouchInstruction();
            }

            //ステップを更新するフラグが立った場合
            if (stepChangeConditions.StepChangeF())
            {
                Debug.Log("実行された");
                TransitionChange();
                goodText.displayGoodText().Forget();
            }
        }
        else if (transitionMode == TransitionMode.beforeSwitching)
        {
            ct.Cancel();
            TransitionChange();
        }
    }

    //文字送りメソッドで Step を変更させる為の条件を示すクラス群
    interface StepChangeConditions
    {
        void setNowData();
        //状態を変化させるフラグ
        bool StepChangeF();
    }
    class LRChangeConditions : StepChangeConditions
    {   
        //現在記憶されてる文字、目的の変化形
        string nowWord;
        string nextWord;
        string direction;

        //コンストラクタ directionをセット
        public LRChangeConditions(string dir)
        {
            direction = dir;
        }

        void setNowWord()
        {
            nowWord = Stamp.Instance.Word;
        }
        void setNextWord()
        {
            nextWord = MasterData.Instance.getNextWord(nowWord, direction);
        }

        //現在のデータをセット
        public void setNowData()
        {
            setNowWord();
            setNextWord();
        }

        //状態を変化させるフラグ
        public bool StepChangeF()
        {
            if (nowWord != Stamp.Instance.Word)
            {
                setNowWord();
                if (nowWord != nextWord)
                {
                    setNextWord();
                }
            }

            if (nowWord == nextWord)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    class MiddleChangeConditions : StepChangeConditions
    {
        //現在記憶されてるパーツ、目的の変化形
        Parts nowParts;
        Parts nextParts;
            
        private void setNowParts()
        {
            nowParts = Stamp.Instance.Parts;
        }
        private void setNextParts()
        {
            switch (nowParts)
            {
                case Parts.Head:
                    nextParts = Parts.Body;
                    break;
                case Parts.Body:
                    nextParts = Parts.Hip;
                    break;
                case Parts.Hip:
                    nextParts = Parts.Head;
                    break;
            }
        }

        //現在のデータをセット
        public void setNowData()
        {
            setNowParts();
            setNextParts();
        }

        //状態を変化させるフラグ
        public bool StepChangeF()
        {
            if (nowParts != Stamp.Instance.Parts)
            {
                setNowParts();
            }

            if (nowParts == nextParts)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    class CardReadConditions : StepChangeConditions
    {
        Serial serialScript;
        string nowWord;

        public CardReadConditions(Serial serialScript)
        {
            this.serialScript = serialScript;
        }

        //現在のデータをセット
        public void setNowData() {
            nowWord = Stamp.Instance.Word;
        }

        //状態を変化させるフラグ
        public bool StepChangeF()
        {
            if (serialScript.enabled)
            {
                if (Serial.cardReadF)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if(nowWord != Stamp.Instance.Word)
                {
                    if( Stamp.Instance.Word == "あ" ||
                        Stamp.Instance.Word == "か" ||
                        Stamp.Instance.Word == "さ" ||
                        Stamp.Instance.Word == "た" ||
                        Stamp.Instance.Word == "な" ||
                        Stamp.Instance.Word == "は" ||
                        Stamp.Instance.Word == "ま" ||
                        Stamp.Instance.Word == "や" ||
                        Stamp.Instance.Word == "ら" ||
                        Stamp.Instance.Word == "わ")
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
    class MikanChangeConditions : StepChangeConditions
    {
        TutorialCharactorScript tutorialCharactorScript;
        string nowWord;

        public MikanChangeConditions(TutorialCharactorScript tutorialCharactorScript)
        {
            this.tutorialCharactorScript = tutorialCharactorScript;
        }

        //現在のデータをセット
        public void setNowData()
        {
        }

        //状態を変化させるフラグ
        public bool StepChangeF()
        {
            if (tutorialCharactorScript.isSerch)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    StepChangeConditions rightChangeConditions;
    StepChangeConditions leftChangeConditions;
    StepChangeConditions middleChangeConditions;
    StepChangeConditions cardReadConditions;
    StepChangeConditions mikanChangeConditions;
    #endregion
    
    [SerializeField] Serial serialScipt;

    //みかんの変身補助パネルのチェックのアニメ
    [SerializeField] TrySupportCheck trySupportCheck;
    //チュートリアルにて、みかんに変身できたかどうかを判断する
    [SerializeField] TutorialCharactorScript tutorialCharactorScript;
    
    [Header("メッセージを表示しきった時にタッチを促す画像")]
    [SerializeField] CanvasGroup touchInstructionImage;
    [SerializeField] Animator touchInstructionAnimator;
    void ShowTouchInstruction()
    {
        if (messageWindow.messageCount >= messageWindow.messageLength && touchInstructionImage.alpha != 1.0f)
        {
            touchInstructionAnimator.SetBool("AnimationF", true);
            touchInstructionImage.DOFade(endValue: 1.0f, duration: 0.2f);
        }
        else
        {
            HideTouchInstruction();
        }
    }
    void HideTouchInstruction()
    {
        if (touchInstructionImage.alpha != 0.0f)
        {
            touchInstructionAnimator.SetBool("AnimationF", true);
            touchInstructionImage.DOFade(endValue: 0.0f, duration: 0.2f);
        }
    }
    
    [SerializeField] GoodText goodText;

    //タイムアウト関連
    CancellationTokenSource ct = new CancellationTokenSource();
    float verificationTimeout_sec = 30.0f;
    float tutorialTimeout_sec = 60.0f;
    float taskTimeout_sec = 120.0f;
    
    public async UniTask WaitForTimeout(float time_sec)
    {
        TutorialStep _step = tutorialStep;

        ct = new CancellationTokenSource();

        await UniTask.Delay((int)(time_sec * 1000), cancellationToken: ct.Token);

        if (_step == tutorialStep)
        {
            //全てのパネルを非表示
            TutorialPanelFO().Forget();
            tutorialStep = TutorialStep.TutorialVerification;
            transitionMode = TransitionMode.afterSwitching;

            //タイトルへ
            sceneControl.screenMode = ScreenMode.Title;
            sceneControl.transitionMode = TransitionMode.afterSwitching;
        }
    }
    void TutorialPanelFI()
    {
        tutorialPanelCanvasGroup.DOFade(endValue: 1.0f, duration: 1.0f);
    }
    async UniTask TutorialPanelFO()
    {
        tutorialPanelCanvasGroup.DOFade(endValue: 0.0f, duration: 1.0f);
        await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());
        explainPanel.PanelsInit();
    }
    

    void Start()
    {
        rightChangeConditions = new LRChangeConditions("right");
        leftChangeConditions = new LRChangeConditions("left");
        middleChangeConditions = new MiddleChangeConditions();
        cardReadConditions = new CardReadConditions(serialScipt);
        mikanChangeConditions = new MikanChangeConditions(tutorialCharactorScript);
        ct = new CancellationTokenSource();
    }

    void Update()
    {
        if (sceneControl.screenMode == ScreenMode.Tutorial)
        {
            tutorialPanelCanvasGroup.blocksRaycasts = true;

            if (tutorialStep == TutorialStep.TutorialVerification)
            {
                explainPanel.TutorialVerification();
                if (transitionMode == TransitionMode.afterSwitching)
                {
                    TutorialPanelFI();
                    touchInstructionImage.alpha = 0;
                    verificationPanelScript.SetUp();
                    WaitForTimeout(verificationTimeout_sec).Forget();
                    TransitionChange();
                }
                else if (transitionMode == TransitionMode.continuation)
                {
                    TutorialVerification();
                }
                else if (transitionMode == TransitionMode.beforeSwitching)
                {
                    ct.Cancel();
                    Debug.Log($"changeTiming1: {ct.IsCancellationRequested}");
                    TransitionChange();
                }
            }
            else if (tutorialStep == TutorialStep.BearGreeting)
            {
                explainPanel.ExplainBearOn();
                ViewingMessage(message_BearGreeting, tutorialTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.BearGreeting_Mikan)
            {
                explainPanel.ExplainMikan();
                ViewingMessage(message_BearGreeting_Mikan, tutorialTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.BearGreeting_Kamen)
            {
                explainPanel.ExplainKamen();
                ViewingMessage(message_BearGreeting_Kamen, tutorialTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.BearGreeting_Kannna)
            {
                explainPanel.ExplainKanna();
                ViewingMessage(message_BearGreeting_Kannna, tutorialTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.StampOperation_GrapStamp)
            {
                explainPanel.ExplainGrapDevice();
                ViewingMessage(message_StampOperation_GrapStamp, tutorialTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.StampOperation_RightButton)
            {
                explainPanel.ExplainRightButton();
                ExececutionTask(message_StampOperation_RightButton, rightChangeConditions, taskTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.StampOperation_RightButtonDoes)
            {
                ViewingMessage(message_StampOperation_RightButtonDoes, tutorialTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.StampOperation_LeftButton)
            {
                explainPanel.ExplainLeftButton();
                ExececutionTask(message_StampOperation_LeftButton, leftChangeConditions, taskTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.StampOperation_LeftButtonDoes)
            {
                ViewingMessage(message_StampOperation_LeftButtonDoes, tutorialTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.StampOperation_MiddleButton)
            {
                explainPanel.ExplainMiddleButton();
                ExececutionTask(message_StampOperation_MiddleButton, middleChangeConditions, taskTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.StampOperation_MiddleButtonDoes)
            {
                ViewingMessage(message_StampOperation_MiddleButtonDoes, tutorialTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.StampOperation_CardRead)
            {
                explainPanel.ExplainCardRead();
                ExececutionTask(message_StampOperation_CardRead, cardReadConditions, taskTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.StampOperation_CardReadDoes)
            {
                ViewingMessage(message_StampOperation_CardReadDoes, tutorialTimeout_sec);
            }
            else if (tutorialStep == TutorialStep.StampOperation_TutorialMikan1)
            {
                tutorialCharactorScript.KanSetup();
                trySupportCheck.CheckBoxSetup();
                explainPanel.ExplainTryMikanPanel();
                ViewingMessage(message_StampOperation_TutorialMikan1, tutorialTimeout_sec);
                tutorialCharactorScript.isChange = false;
            }
            else if (tutorialStep == TutorialStep.StampOperation_TutorialMikan2)
            {
                ExececutionTask(message_StampOperation_TutorialMikan2, mikanChangeConditions, taskTimeout_sec);
                trySupportCheck.CheckBoxCondition();
                tutorialCharactorScript.PushStamp();
            }
            else if (tutorialStep == TutorialStep.StampOperation_TutorialMikanDoes)
            {
                trySupportCheck.CheckBoxCondition();
                tutorialCharactorScript.PushStamp();
                ViewingMessage(message_StampOperation_TutorialMikanDoes, tutorialTimeout_sec);
            }
            else if(tutorialStep == TutorialStep.EndStep)
            {
                explainPanel.ExplainFinish();

                transitionMode = TransitionMode.afterSwitching;
                tutorialStep = TutorialStep.TutorialVerification;
                sceneControl.screenMode = (ScreenMode)((int)sceneControl.screenMode + 1);
            }
        }
        else
        {
            tutorialPanelCanvasGroup.blocksRaycasts = false;
        }
    }
}

//チュートリアルの段階
public enum TutorialStep
{
    TutorialVerification = 0,
    BearGreeting,
    BearGreeting_Mikan,
    BearGreeting_Kamen,
    BearGreeting_Kannna,
    StampOperation_GrapStamp,
    StampOperation_RightButton,
    StampOperation_RightButtonDoes,
    StampOperation_LeftButton,
    StampOperation_LeftButtonDoes,
    StampOperation_MiddleButton,
    StampOperation_MiddleButtonDoes,
    StampOperation_CardRead,
    StampOperation_CardReadDoes,
    StampOperation_TutorialMikan1,
    StampOperation_TutorialMikan2,
    StampOperation_TutorialMikanDoes,
    EndStep
};