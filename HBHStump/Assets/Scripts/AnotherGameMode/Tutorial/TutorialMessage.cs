using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

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
//ステップごとに実行 enumを使おうか

public class TutorialMessage : MonoBehaviour
{
    //シーンを管理する
    [SerializeField] SceneControl sceneControl;

    //チュートリアルの段階
    public enum TutorialStep
    {
        TutorialVerification,
        BearGreeting,
        BearGreeting_Mikan,
        BearGreeting_Kamen,
        BearGreeting_Kannna,
        StampOperation_GrapStamp,
        StampOperation_RightButton,
        StampOperation_RightButtonDoing,
        StampOperation_RightButtonDoes,
        StampOperation_LeftButton,
        StampOperation_LeftButtonDoing,
        StampOperation_LeftButtonDoes,
        StampOperation_MiddleButton,
        StampOperation_MiddleButtonDoing,
        StampOperation_MiddleButtonDoes,
        StampOperation_CardRead,
        StampOperation_CardReadDoing,
        StampOperation_CardReadDoes,
        StampOperation_TutorialMikan1,
        StampOperation_TutorialMikan2,
        StampOperation_TutorialMikanDoing,
        StampOperation_TutorialMikanDoes,
        EndStep
    }
    //状態遷移の段階
    //段階が変化した後 - 変化後 - 変化する前
    public enum TransitionMode
    {
        afterSwitching,
        continuation,
        beforeSwitching
    }
    [SerializeField] TutorialStep tutorialStep = TutorialStep.BearGreeting;
    public TransitionMode transitionMode = TransitionMode.afterSwitching;
    
    //チュートリアルパネルのRaycast
    [SerializeField] CanvasGroup tutorialPanelRay;
    //チュートリアル確認クラス
    [SerializeField] VerificationPanelScript verificationPanelScript;
    //メッセージウィンドウ操作クラス
    [SerializeField] MessageWindow messageWindow;
    //概要説明時のパネル操作クラス
    [SerializeField] ExplainPanel explainPanel;

    //以下、表示するメッセージ
    #region
    string[] message_BearGreeting = { "くま", "こんにちは、ぼくはくま！\nモジプラスタンプの世界にようこそ！",
                                      "くま", "モジプラスタンプのあそびかたを\nせつめいするよ",
                                      "くま", "モジプラスタンプはキャラクターの名前に文字をくわえて\n別のキャラクターに変身させるゲームだよ"
                                    };
    string[] message_BearGreeting_Mikan = { "くま", "たとえば、『かん』のあたまに『み』をつけると『みかん』"
                                    };
    string[] message_BearGreeting_Kamen = { "くま", "『かん』のまんなかに『め』をつけると『かめん』"
                                    };
    string[] message_BearGreeting_Kannna = { "くま", "『かん』のおしりに『な』をつけると『かんな』に変身するよ",
                                    };
    string[] message_StampOperation_GrapStamp = { "くま", "次に文字の追加方法を説明するよ",
                                                  "くま", "文字の追加にはスタンプを使用するよ"
                                    };
    string[] message_StampOperation_RightButton = { "くま", "右のボタンで母音(あ, い, う……)を変えられるよ",
                                                    "くま", "早速やってみよう！"
                                    };
    string[] message_StampOperation_RightButtonDoes = { "くま", "いいね！その調子！"
                                    };
    string[] message_StampOperation_LeftButton = { "くま", "左のボタンで反対向きに母音(お, え, う……)を変えられるよ",
                                                   "くま", "早速やってみよう！"
                                    };
    string[] message_StampOperation_LeftButtonDoes = { "くま", "いいね！その調子！"
                                    };
    string[] message_StampOperation_MiddleButton = { "くま", "真ん中のボタンで文字を加える場所(頭, 体, お尻)を変えられるよ",
                                                     "くま", "早速やってみよう！",
                                    };
    string[] message_StampOperation_MiddleButtonDoes = { "くま", "いいね！その調子！"
                                    };
    string[] message_StampOperation_CardRead = { "くま", "カードにスタンプを押すとスタンプに文字を読み込むことができるよ",
                                                 "くま", "早速やってみよう！",
                                    };
    string[] message_StampOperation_CardReadDoes = { "くま", "いいね！その調子！",
                                                   "くま", "スタンプの操作説明は以上だよ"
                                    };
    string[] message_StampOperation_TutorialMikan1 = { "くま", "それじゃあ、試しに『かん』を『みかん』に変身させてみよう！",
                                    };
    string[] message_StampOperation_TutorialMikan2 = { "くま", "変身させるまでのステップはここに表示しておくよ",
                                                       "くま", "文字をセットしたら、みかんにスタンプを打ってね！",
                                                       "くま", "早速やってみよう！"
                                    };
    string[] message_StampOperation_TutorialMikanDoes = { "くま", "いいね！その調子！",
                                                        "くま", "本番では制限時間のうちに\nたくさんキャラクターを変身させてね！",
                                                        "くま", "本番もがんばってね！"
                                    };
    #endregion

    //Transitionmodeを変化させる
    public void TransitionChange()
    {
        if (tutorialStep < TutorialStep.EndStep)
        {
            if (transitionMode < TransitionMode.beforeSwitching)
            {
                transitionMode++;
            }
            else
            {
                transitionMode = 0;
                tutorialStep++;
            }
        }
    }

    //チュートリアルを受けるか確認する
    void TutorialVerification()
    {
        if ((serialScipt.enabled == false && verificationPanelScript.yesF) || 
            (serialScipt.enabled == true && Serial.PushF[1, 1]))
        {
            TransitionChange();
        }
        else if ((serialScipt.enabled == false && verificationPanelScript.noF) ||
                 (serialScipt.enabled == true && Serial.PushF[3, 1]))
        {
            sceneControl.screenMode = (SceneControl.ScreenMode)((int)sceneControl.screenMode + 1);
            transitionMode = TransitionMode.afterSwitching;
            explainPanel.VerificationPanelFO();
        }
    }

    //各ステップで実行する内容
    //主に文字送りをするメソッド
    //transitionMode変更もまとめている
    //文字送り以外の処理を呼び出す場合は、この関数は最後に描く(transitionMode の Update も兼ねているため)
    #region
    void ViewingMessage(string[] message)
    {
        if (transitionMode == TransitionMode.afterSwitching)
        {
            messageWindow.LoadMessage(new List<string>(message));
            TransitionChange();
        }
        else if (transitionMode == TransitionMode.continuation)
        {
            messageWindow.MessageWindowUpdate();
            DisplayTouchInstruction();
            if (messageWindow.messageFinish)
            {
                TransitionChange();
            }
        }
        else if (transitionMode == TransitionMode.beforeSwitching)
        {
            UndisplayTouchInstruction();
            TransitionChange();
        }
    }
    //文字送りの条件がメッセージの完全表示でない場合
    //stepChangeConditions にて別途条件を設定
    void ViewingMessage(string[] message, StepChangeConditions stepChangeConditions)
    {
        if (transitionMode == TransitionMode.afterSwitching)
        {
            messageWindow.LoadMessage(new List<string>(message));
            stepChangeConditions.setNowData();
            TransitionChange();
        }
        else if (transitionMode == TransitionMode.continuation)
        {
            messageWindow.MessageWindowUpdate();

            if (messageWindow.messageNum * 2 + 2 < message.Length && messageWindow.messageCount >= messageWindow.messageLength) 
            {
                DisplayTouchInstruction();
            }
            else
            {
                UndisplayTouchInstruction();
            }
            if (stepChangeConditions.StepChangeF())
            {
                TransitionChange();
            }
        }
        else if (transitionMode == TransitionMode.beforeSwitching)
        {
            UndisplayTouchInstruction();
            TransitionChange();
        }
    }
    //メッセージを最後まで表示したら自動的に次の transition へ移行するメソッド
    void ViewingMessageSkip(string[] message)
    {
        if (transitionMode == TransitionMode.afterSwitching)
        {
            messageWindow.LoadMessage(new List<string>(message));
            TransitionChange();
        }
        else if (transitionMode == TransitionMode.continuation)
        {
            messageWindow.MessageWindowUpdate();

            if (messageWindow.messageNum * 2 + 2 < message.Length && messageWindow.messageCount >= messageWindow.messageLength)
            {
                DisplayTouchInstruction();
            }
            else
            {
                UndisplayTouchInstruction();
            }
            if (messageWindow.messageNum*2+2 >= message.Length && messageWindow.messageCount >= messageWindow.messageLength)
            {
                TransitionChange();
            }
        }
        else if (transitionMode == TransitionMode.beforeSwitching)
        {
            UndisplayTouchInstruction();
            TransitionChange();
        }
    }
    //タスクの実行で transition を変更するメソッド
    void ExececutionTask(StepChangeConditions stepChangeConditions)
    {
        if (transitionMode == TransitionMode.afterSwitching)
        {
            stepChangeConditions.setNowData();
            TransitionChange();
        }
        else if (transitionMode == TransitionMode.continuation)
        {
            if (stepChangeConditions.StepChangeF())
            {
                TransitionChange();
                goodText.displayGoodText().Forget();
                Debug.Log("タスクをじっこうした");
            }
        }
        else if (transitionMode == TransitionMode.beforeSwitching)
        {
            TransitionChange();
        }
    }

    //文字送りメソッドで Step を変更させる為の条件を示すクラス群
    public interface StepChangeConditions
    {
        //現在のデータをセット
        void setNowData();
        //状態を変化させるフラグ
        bool StepChangeF();
    }
    public class LRChangeConditions : StepChangeConditions
    {   
        //現在記憶されてる文字、目的の変化形
        public string nowWord;
        public string nextWord;
        public string direction;

        //コンストラクタ directionをセット
        public LRChangeConditions(string dir)
        {
            direction = dir;
        }

        private void setNowWord()
        {
            nowWord = StumpScript.stampPartsWord[StumpScript.TempStump];
        }
        private void setNextWord()
        {
            nextWord = MasterData.getNextWord(nowWord, direction);
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
            if (nowWord != StumpScript.stampPartsWord[StumpScript.TempStump])
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
    public class MiddleChangeConditions : StepChangeConditions
    {
        //現在記憶されてるパーツ、目的の変化形
        public string nowParts;
        public string nextParts;
            
        private void setNowParts()
        {
            nowParts = StumpScript.TempStump;
        }
        private void setNextParts()
        {
            switch (nowParts)
            {
                case "頭":
                    nextParts = "体";
                    break;
                case "体":
                    nextParts = "尻";
                    break;
                case "尻":
                    nextParts = "頭";
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
            if (nowParts != StumpScript.TempStump)
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
    public class CardReadConditions : StepChangeConditions
    {
        Serial serialScript;
        string nowWord;

        public CardReadConditions(Serial serialScript)
        {
            this.serialScript = serialScript;
        }

        //現在のデータをセット
        public void setNowData() {
            nowWord = StumpScript.stampPartsWord[StumpScript.TempStump];
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
                if(nowWord != StumpScript.stampPartsWord[StumpScript.TempStump])
                {
                    if(StumpScript.stampPartsWord[StumpScript.TempStump] == "あ" ||
                        StumpScript.stampPartsWord[StumpScript.TempStump] == "か" ||
                        StumpScript.stampPartsWord[StumpScript.TempStump] == "さ" ||
                        StumpScript.stampPartsWord[StumpScript.TempStump] == "た" ||
                        StumpScript.stampPartsWord[StumpScript.TempStump] == "な" ||
                        StumpScript.stampPartsWord[StumpScript.TempStump] == "は" ||
                        StumpScript.stampPartsWord[StumpScript.TempStump] == "ま" ||
                        StumpScript.stampPartsWord[StumpScript.TempStump] == "や" ||
                        StumpScript.stampPartsWord[StumpScript.TempStump] == "ら" ||
                        StumpScript.stampPartsWord[StumpScript.TempStump] == "わ")
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
    public class MikanChangeConditions : StepChangeConditions
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
            if (tutorialCharactorScript.SerchF)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public StepChangeConditions rightChangeConditions;
    public StepChangeConditions leftChangeConditions;
    public StepChangeConditions middleChangeConditions;
    public StepChangeConditions cardReadConditions;
    public StepChangeConditions mikanChangeConditions;
    #endregion

    //シリアル通信のスクリプト
    [SerializeField] Serial serialScipt;

    //みかんの変身補助パネルのチェックのアニメ
    [SerializeField] TrySupportCheck trySupportCheck;
    //チュートリアルにて、みかんに変身できたかどうかを判断する
    [SerializeField] TutorialCharactorScript tutorialCharactorScript;

    //メッセージを表示しきった時にタッチを促す画像を表示
    #region
    [Header("メッセージを表示しきった時にタッチを促す画像")]
    [SerializeField] CanvasGroup touchInstructionImage;
    [SerializeField] Animator touchInstructionAnimator;
    void DisplayTouchInstruction()
    {
        if(messageWindow.messageCount >= messageWindow.messageLength)
        {
            if (touchInstructionImage.alpha != 1.0f)
            {
                touchInstructionAnimator.SetBool("AnimationF", true);
                touchInstructionImage.DOFade(endValue: 1.0f, duration: 0.2f);
            }
        }
        else
        {
            UndisplayTouchInstruction();
        }
    }
    void UndisplayTouchInstruction()
    {
        if (touchInstructionImage.alpha != 0.0f)
        {
            touchInstructionAnimator.SetBool("AnimationF", true);
            touchInstructionImage.DOFade(endValue: 0.0f, duration: 0.2f);
        }
    }
    #endregion

    //タスク後にGoodと表示する画像
    [Header("タスク後にGoodと表示する画像")]
    [SerializeField] GoodText goodText;


    // Start is called before the first frame update
    void Start()
    {
        rightChangeConditions = new LRChangeConditions("right");
        leftChangeConditions = new LRChangeConditions("left");
        middleChangeConditions = new MiddleChangeConditions();
        cardReadConditions = new CardReadConditions(serialScipt);
        mikanChangeConditions = new MikanChangeConditions(tutorialCharactorScript);
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneControl.screenMode == SceneControl.ScreenMode.Tutorial)
        {
            tutorialPanelRay.blocksRaycasts = true;

            if (tutorialStep == TutorialStep.TutorialVerification)
            {
                explainPanel.TutorialVerification();
                if (transitionMode == TransitionMode.afterSwitching)
                {
                    touchInstructionImage.alpha = 0;
                    verificationPanelScript.SetUp();
                    TransitionChange();
                }
                else if (transitionMode == TransitionMode.continuation)
                {
                    TutorialVerification();
                }
                else if (transitionMode == TransitionMode.beforeSwitching)
                {
                    TransitionChange();
                }
            }
            if (tutorialStep == TutorialStep.BearGreeting)
            {
                explainPanel.ExplainBearOn();
                ViewingMessage(message_BearGreeting);
            }
            else if (tutorialStep == TutorialStep.BearGreeting_Mikan)
            {
                explainPanel.ExplainMikan();
                ViewingMessage(message_BearGreeting_Mikan);
            }
            else if (tutorialStep == TutorialStep.BearGreeting_Kamen)
            {
                explainPanel.ExplainKamen();
                ViewingMessage(message_BearGreeting_Kamen);
            }
            else if (tutorialStep == TutorialStep.BearGreeting_Kannna)
            {
                explainPanel.ExplainKanna();
                ViewingMessage(message_BearGreeting_Kannna);
            }
            else if (tutorialStep == TutorialStep.StampOperation_GrapStamp)
            {
                explainPanel.ExplainGrapDevice();
                ViewingMessage(message_StampOperation_GrapStamp);
            }
            else if (tutorialStep == TutorialStep.StampOperation_RightButton)
            {
                explainPanel.ExplainRightButton();
                //要変更
                ViewingMessageSkip(message_StampOperation_RightButton);
            }
            else if (tutorialStep == TutorialStep.StampOperation_RightButtonDoing)
            {
                ExececutionTask(rightChangeConditions);
            }
            else if (tutorialStep == TutorialStep.StampOperation_RightButtonDoes)
            {
                ViewingMessage(message_StampOperation_RightButtonDoes);
            }
            else if (tutorialStep == TutorialStep.StampOperation_LeftButton)
            {
                explainPanel.ExplainLeftButton();
                ViewingMessageSkip(message_StampOperation_LeftButton);
            }
            else if (tutorialStep == TutorialStep.StampOperation_LeftButtonDoing)
            {
                ExececutionTask(leftChangeConditions);
            }
            else if (tutorialStep == TutorialStep.StampOperation_LeftButtonDoes)
            {
                ViewingMessage(message_StampOperation_LeftButtonDoes);
            }
            else if (tutorialStep == TutorialStep.StampOperation_MiddleButton)
            {
                explainPanel.ExplainMiddleButton();
                ViewingMessageSkip(message_StampOperation_MiddleButton);
            }
            else if (tutorialStep == TutorialStep.StampOperation_MiddleButtonDoing)
            {
                ExececutionTask(middleChangeConditions);
            }
            else if (tutorialStep == TutorialStep.StampOperation_MiddleButtonDoes)
            {
                ViewingMessage(message_StampOperation_MiddleButtonDoes);
            }
            else if (tutorialStep == TutorialStep.StampOperation_CardRead)
            {
                explainPanel.ExplainCardRead();
                ViewingMessageSkip(message_StampOperation_CardRead);
            }
            else if (tutorialStep == TutorialStep.StampOperation_CardReadDoing)
            {
                ExececutionTask(cardReadConditions);
            }
            else if (tutorialStep == TutorialStep.StampOperation_CardReadDoes)
            {
                ViewingMessage(message_StampOperation_CardReadDoes);
            }
            else if (tutorialStep == TutorialStep.StampOperation_TutorialMikan1)
            {
                tutorialCharactorScript.KanSetup();
                trySupportCheck.CheckBoxSetup();
                explainPanel.ExplainTryMikanPanel();
                ViewingMessage(message_StampOperation_TutorialMikan1);
            }
            else if (tutorialStep == TutorialStep.StampOperation_TutorialMikan2)
            {
                ViewingMessageSkip(message_StampOperation_TutorialMikan2);
                tutorialCharactorScript.ChangeF = false;
            }
            else if (tutorialStep == TutorialStep.StampOperation_TutorialMikanDoing)
            {
                trySupportCheck.CheckBoxCondition();
                tutorialCharactorScript.PushStamp();
                ExececutionTask(mikanChangeConditions);
            }
            else if (tutorialStep == TutorialStep.StampOperation_TutorialMikanDoes)
            {
                trySupportCheck.CheckBoxCondition();
                tutorialCharactorScript.PushStamp();
                ViewingMessage(message_StampOperation_TutorialMikanDoes);
            }
            else if(tutorialStep == TutorialStep.EndStep)
            {
                explainPanel.ExplainFinish();

                transitionMode = TransitionMode.afterSwitching;
                tutorialStep = TutorialStep.TutorialVerification;
                sceneControl.screenMode = (SceneControl.ScreenMode)((int)sceneControl.screenMode + 1);
            }
        }
        else
        {
            tutorialPanelRay.blocksRaycasts = false;
        }
    }
}
