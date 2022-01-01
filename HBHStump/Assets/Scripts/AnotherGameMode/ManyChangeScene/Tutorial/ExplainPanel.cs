using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

/*
説明パネルをコントロール
*/

public class ExplainPanel : MonoBehaviour
{
    //チュートリアルのメッセージを管理するクラス
    [SerializeField] TutorialMessage tutorialMessage;

    //パネル群
    #region
    //説明するためのパネル]
    [SerializeField] CanvasGroup VerificationPanel;
    [SerializeField] CanvasGroup CharAndMessagePanel;
    //『かん』の変身を説明するオブジェクト群
    [SerializeField] CanvasGroup explainPanel;
    [SerializeField] CanvasGroup kanPanel;
    [SerializeField] CanvasGroup mikanPanel;
    [SerializeField] CanvasGroup kamenPanel;
    [SerializeField] CanvasGroup kannaPanel;
    //デバイスの操作を説明するオブジェクト群
    [SerializeField] CanvasGroup devicePanel;
    [SerializeField] GameObject buttonCoverObj;
    [SerializeField] CanvasGroup BtnWindowImg;
    [SerializeField] GameObject rightBtnExImg;
    [SerializeField] GameObject leftBtnExImg;
    [SerializeField] GameObject middleBtnExImg;
    [SerializeField] GameObject wordPanelImg;
    //「みかん」への変身を説明するオブジェクト群
    [SerializeField] CanvasGroup tryMikanChangeImages;

    //カード部分の照明
    [SerializeField] Image wordPanelPlaceImage;
    [SerializeField] CanvasGroup wordSupportArrowGroups;
    #endregion

    //パネルがフェードする時間
    float fadeTime = 0.5f;

    //チュートリアルの段階でパネルを開く、閉じるメソッド
    #region
    //チュートリアルを受けるか確認
    public void TutorialVerification()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            VerificationPanel.blocksRaycasts = true;
            VerificationPanel.DOFade(endValue: 1.0f, duration: fadeTime);
        }
        else if(tutorialMessage.transitionMode == TutorialMessage.TransitionMode.beforeSwitching)
        {
            VerificationPanelFO();
        }
    }
    //確認パネルをFOさせる
    public void VerificationPanelFO()
    {
        VerificationPanel.blocksRaycasts = false;
        VerificationPanel.DOFade(endValue: 0.0f, duration: fadeTime);
    }
    //説明するクマを表示する
   　public void ExplainBearOn()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            CharAndMessagePanel.DOFade(endValue: 1.0f, duration: fadeTime);
        }
    }

    //みかんの説明
    public void ExplainMikan()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            explainPanel.DOFade(endValue: 1.0f, duration: fadeTime);
            kanPanel.DOFade(endValue: 1.0f, duration: fadeTime);
            mikanPanel.DOFade(endValue: 1.0f, duration: fadeTime);
        }
    }
    //かめんの説明
    public void ExplainKamen()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            mikanPanel.DOFade(endValue: 0.0f, duration: fadeTime);
            kamenPanel.DOFade(endValue: 1.0f, duration: fadeTime);
        }
    }
    //かんなの説明
    public void ExplainKanna()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            kamenPanel.DOFade(endValue: 0.0f, duration: fadeTime);
            kannaPanel.DOFade(endValue: 1.0f, duration: fadeTime);
        }
    }

    //デバイスを掴むことの説明
    public void ExplainGrapDevice()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            kannaPanel.DOFade(endValue: 0.0f, duration: fadeTime);
            kanPanel.DOFade(endValue: 0.0f, duration: fadeTime);
            devicePanel.DOFade(endValue: 1.0f, duration: fadeTime);
        }
    }
    //右ボタンの説明
    public void ExplainRightButton()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            buttonCoverObj.GetComponent<CanvasGroup>().DOFade(endValue: 1.0f, duration: fadeTime);
            buttonCoverObj.GetComponent<Animator>().SetBool("AnimationF", true);
            buttonCoverObj.transform.localPosition = new Vector2(39.0f, 60.0f);
            BtnWindowImg.DOFade(endValue: 1.0f, duration: fadeTime);
            rightBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 1.0f, duration: fadeTime);
            rightBtnExImg.GetComponent<Animator>().SetBool("AnimationF", true);
        }
    }
    //左ボタンの説明
    public void ExplainLeftButton()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            buttonCoverObj.transform.localPosition = new Vector2(-39.0f, 60.0f);
            rightBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 0.0f, duration: fadeTime);
            rightBtnExImg.GetComponent<Animator>().SetBool("AnimationF", false);
            leftBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 1.0f, duration: fadeTime);
            leftBtnExImg.GetComponent<Animator>().SetBool("AnimationF", true);
        }
    }
    //真ん中ボタンの説明
    public void ExplainMiddleButton()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            buttonCoverObj.transform.localPosition = new Vector2(0.0f, 60.0f);
            BtnWindowImg.DOFade(endValue: 0.0f, duration: fadeTime);
            leftBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 0.0f, duration: fadeTime);
            leftBtnExImg.GetComponent<Animator>().SetBool("AnimationF", false);

            middleBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 1.0f, duration: fadeTime);
            middleBtnExImg.GetComponent<Animator>().SetBool("AnimationF", true);
        }
    }
    //言葉パネルの説明
    public void ExplainCardRead()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            devicePanel.DOFade(endValue: 0.0f, duration: fadeTime);
            middleBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 0.0f, duration: fadeTime);
            middleBtnExImg.GetComponent<Animator>().SetBool("AnimationF", false);

            wordPanelImg.GetComponent<CanvasGroup>().DOFade(endValue: 1.0f, duration: fadeTime);
            wordPanelImg.GetComponent<Animator>().SetBool("AnimationF", true);

            WordPanelAnimation().Forget();
        }
        else if(tutorialMessage.transitionMode == TutorialMessage.TransitionMode.beforeSwitching)
        {
            wordPanelPlaceCt.Cancel();
        }
    }
    //みかん説明補助パネルの説明
    public void ExplainTryMikanPanel()
    {
        if (tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            wordPanelImg.GetComponent<CanvasGroup>().DOFade(endValue: 0.0f, duration: fadeTime);
            wordPanelImg.GetComponent<Animator>().SetBool("AnimationF", false);
            explainPanel.DOFade(endValue: 0.0f, duration: fadeTime);

            tryMikanChangeImages.DOFade(endValue: 1.0f, duration: fadeTime);
        }
    }
    //チュートリアル終了時、パネルを不可視にする
    public void ExplainFinish()
    {
        tryMikanChangeImages.DOFade(endValue: 0.0f, duration: fadeTime);
        CharAndMessagePanel.DOFade(endValue: 0.0f, duration: fadeTime);
    }
    #endregion

    //カード読むときのパネル部分アニメーション
    CancellationTokenSource wordPanelPlaceCt;
    async UniTask WordPanelAnimation()
    {
        wordPanelPlaceCt = new CancellationTokenSource();
        wordSupportArrowGroups.DOFade(endValue: 1.0f, duration: 1.0f);
        while (!wordPanelPlaceCt.IsCancellationRequested)
        {
            wordPanelPlaceImage.DOFade(endValue: 0.3f, duration: 1.0f);
            await UniTask.Delay(1000);
            wordPanelPlaceImage.DOFade(endValue: 1.0f, duration: 1.0f);
            await UniTask.Delay(1000);
        }
        wordSupportArrowGroups.DOFade(endValue: 0.0f, duration: 1.0f);
    }

    public void PanelsInit()
    {
        //説明するためのパネル
        VerificationPanel.alpha = 0;
        CharAndMessagePanel.alpha = 0;
        //『かん』の変身を説明するオブジェクト群
        explainPanel.alpha = 0;
        kanPanel.alpha = 0;
        mikanPanel.alpha = 0;
        kamenPanel.alpha = 0;
        kannaPanel.alpha = 0;
        //デバイスの操作を説明するオブジェクト群
        devicePanel.alpha = 0;
        buttonCoverObj.GetComponent<CanvasGroup>().alpha = 0;
        buttonCoverObj.GetComponent<Animator>().SetBool("AnimationF", false);
        BtnWindowImg.alpha = 0;
        rightBtnExImg.GetComponent<CanvasGroup>().alpha = 0;
        rightBtnExImg.GetComponent<Animator>().SetBool("AnimationF", false);
        leftBtnExImg.GetComponent<CanvasGroup>().alpha = 0;
        leftBtnExImg.GetComponent<Animator>().SetBool("AnimationF", false);
        middleBtnExImg.GetComponent<CanvasGroup>().alpha = 0;
        middleBtnExImg.GetComponent<Animator>().SetBool("AnimationF", false);
        wordPanelImg.GetComponent<CanvasGroup>().alpha = 0;
        wordPanelImg.GetComponent<Animator>().SetBool("AnimationF", false);
        tryMikanChangeImages.alpha = 0;
        wordSupportArrowGroups.alpha = 0;
    }
    
    void Awake()
    {
        PanelsInit();
    }
}
