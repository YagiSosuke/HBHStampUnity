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
    const float fadeTime = 0.5f;        //パネルがフェードする時間
    
    [SerializeField] TutorialMessage tutorialMessage;

    [Header("パネル群")]
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
    
    [SerializeField] Image wordPanelPlaceImage;             //文字カード説明時の照明
    [SerializeField] CanvasGroup wordSupportArrowGroups;


    //チュートリアルを受けるか確認
    public void ShowVerificationPanel()
    {
        VerificationPanel.blocksRaycasts = true;
        VerificationPanel.DOFade(endValue: 1.0f, duration: fadeTime);
    }
    public void HideVerificationPanel()
    {
        VerificationPanel.blocksRaycasts = false;
        VerificationPanel.DOFade(endValue: 0.0f, duration: fadeTime);
    }
    
    //説明パネルの内容を変更するメソッド
    public void ChangeExplainPanel()
    {
        if (tutorialMessage.transitionMode == TransitionMode.afterSwitching)
        {
            switch (tutorialMessage.GetTutorialStep())
            {
                case TutorialStep.BearGreeting:
                    CharAndMessagePanel.DOFade(endValue: 1.0f, duration: fadeTime);
                    break;
                case TutorialStep.BearGreeting_Mikan:
                    explainPanel.DOFade(endValue: 1.0f, duration: fadeTime);
                    kanPanel.DOFade(endValue: 1.0f, duration: fadeTime);
                    mikanPanel.DOFade(endValue: 1.0f, duration: fadeTime);
                    break;
                case TutorialStep.BearGreeting_Kamen:
                    mikanPanel.DOFade(endValue: 0.0f, duration: fadeTime);
                    kamenPanel.DOFade(endValue: 1.0f, duration: fadeTime);
                    break;
                case TutorialStep.BearGreeting_Kannna:
                    kamenPanel.DOFade(endValue: 0.0f, duration: fadeTime);
                    kannaPanel.DOFade(endValue: 1.0f, duration: fadeTime);
                    break;
                case TutorialStep.StampOperation_GrapStamp:
                    kannaPanel.DOFade(endValue: 0.0f, duration: fadeTime);
                    kanPanel.DOFade(endValue: 0.0f, duration: fadeTime);
                    devicePanel.DOFade(endValue: 1.0f, duration: fadeTime);
                    break;
                case TutorialStep.StampOperation_RightButton:
                    buttonCoverObj.GetComponent<CanvasGroup>().DOFade(endValue: 1.0f, duration: fadeTime);
                    buttonCoverObj.GetComponent<Animator>().SetBool("AnimationF", true);
                    buttonCoverObj.transform.localPosition = new Vector2(39.0f, 60.0f);
                    BtnWindowImg.DOFade(endValue: 1.0f, duration: fadeTime);
                    rightBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 1.0f, duration: fadeTime);
                    rightBtnExImg.GetComponent<Animator>().SetBool("AnimationF", true);
                    break;
                case TutorialStep.StampOperation_LeftButton:
                    buttonCoverObj.transform.localPosition = new Vector2(-39.0f, 60.0f);
                    rightBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 0.0f, duration: fadeTime);
                    rightBtnExImg.GetComponent<Animator>().SetBool("AnimationF", false);
                    leftBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 1.0f, duration: fadeTime);
                    leftBtnExImg.GetComponent<Animator>().SetBool("AnimationF", true);
                    break;
                case TutorialStep.StampOperation_MiddleButton:
                    buttonCoverObj.transform.localPosition = new Vector2(0.0f, 60.0f);
                    BtnWindowImg.DOFade(endValue: 0.0f, duration: fadeTime);
                    leftBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 0.0f, duration: fadeTime);
                    leftBtnExImg.GetComponent<Animator>().SetBool("AnimationF", false);
                    middleBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 1.0f, duration: fadeTime);
                    middleBtnExImg.GetComponent<Animator>().SetBool("AnimationF", true);
                    break;
                case TutorialStep.StampOperation_CardRead:
                    devicePanel.DOFade(endValue: 0.0f, duration: fadeTime);
                    middleBtnExImg.GetComponent<CanvasGroup>().DOFade(endValue: 0.0f, duration: fadeTime);
                    middleBtnExImg.GetComponent<Animator>().SetBool("AnimationF", false);
                    wordPanelImg.GetComponent<CanvasGroup>().DOFade(endValue: 1.0f, duration: fadeTime);
                    wordPanelImg.GetComponent<Animator>().SetBool("AnimationF", true);
                    WordPanelAnimation().Forget();
                    break;
                case TutorialStep.StampOperation_CardReadDoes:
                    wordPanelPlaceCt.Cancel();
                    break;
                case TutorialStep.StampOperation_TutorialMikan1:
                    wordPanelImg.GetComponent<CanvasGroup>().DOFade(endValue: 0.0f, duration: fadeTime);
                    wordPanelImg.GetComponent<Animator>().SetBool("AnimationF", false);
                    explainPanel.DOFade(endValue: 0.0f, duration: fadeTime);
                    tryMikanChangeImages.DOFade(endValue: 1.0f, duration: fadeTime);
                    break;
                case TutorialStep.EndStep://TODO; バグがないか確認
                    tryMikanChangeImages.DOFade(endValue: 0.0f, duration: fadeTime);
                    CharAndMessagePanel.DOFade(endValue: 0.0f, duration: fadeTime);
                    break;
            }
        }
    }

    //カード読むときのパネル部分アニメーション
    CancellationTokenSource wordPanelPlaceCt;
    async UniTask WordPanelAnimation()
    {
        wordPanelPlaceCt = new CancellationTokenSource();
        wordSupportArrowGroups.DOFade(endValue: 1.0f, duration: 1.0f);
        while (!wordPanelPlaceCt.IsCancellationRequested)
        {
            wordPanelPlaceImage.DOFade( 0.3f, 1.0f).OnComplete(() => {
                wordPanelPlaceImage.DOFade(endValue: 1.0f, duration: 1.0f);
            });
            await UniTask.Delay(2000);
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
