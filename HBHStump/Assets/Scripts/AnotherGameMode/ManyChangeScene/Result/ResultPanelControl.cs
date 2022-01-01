using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

/*
リザルトパネルを操作する
*/

public class ResultPanelControl : MonoBehaviour
{
    MasterData masterData;
    RankingControl rankCtrl;
    CanvasGroup resultPanel;
    [SerializeField] Serial serialScript;
    
    //得点を表示するパネル
    [SerializeField] Text scoreText;
    //変化させたキャラクターを表示するパネル
    [SerializeField] GameObject[] characterImages = new GameObject[50];
    GameObject rankPanel;

    //終了時の紙吹雪
    [SerializeField] ParticleSystem redPaper;
    [SerializeField] ParticleSystem yellowPaper;

    //ドラムロール
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip drumRoll;
    [SerializeField] AudioClip drumFinish;

    //ランキング関連
    #region
    //ランキングが更新されるかのフラグ
    public int rankNum;
    //ランキング更新時、状態を変化させて管理
    bool executionF = false;
    enum RankPanelTransition {
        beforeView,
        nowView,
        afterView,
    };
    RankPanelTransition rankPanelTransition = RankPanelTransition.beforeView;
    void TransitionUpdate()
    {
        rankPanelTransition++;
        executionF = false;
    }
    //「ランクイン」テキストのアニメーション
    Animator rankInAnim;
    AudioSource rankInAudio;
    #endregion

    [SerializeField] GameObject pleaseTouchText;
    [SerializeField] GameObject pleaseTouchText_ranking;

    void Start() {
        masterData = GameObject.Find("GameControler").GetComponent<MasterData>();
        rankCtrl = GameObject.Find("RankPanel").GetComponent<RankingControl>();
        resultPanel = GameObject.Find("ResultPanel").GetComponent<CanvasGroup>();
        
        rankPanel = GameObject.Find("RankPanel");
        rankInAnim = GameObject.Find("RankInText").GetComponent<Animator>();
        rankInAudio = GameObject.Find("RankPanel").GetComponent<AudioSource>();
    }

    //キャラクター一覧を設定する
    public void SetupCharacter()
    {
        //キャラクターを表示するオブジェクトを初期化する
        for (int i = 0; i < 50; i++)
        {
            characterImages[i].GetComponent<Image>().sprite = null;
            characterImages[i].SetActive(false);
        }
    }
    //スコアを表示する
    public void DisplayScore()
    {
        scoreText.text = masterData.score.ToString();
    }
    //変化させたキャラクター一覧を表示する
    public void DisplayCharacter()
    {        
        for(int i=0; i<masterData.newObjects.Count; i++)
        {
            DisplayOneCharacter(i).Forget();
        }
    }
    //キャラクターを一体表示する
    async UniTask DisplayOneCharacter(int num) {
        float interval = num * 0.1f;
        await UniTask.Delay((int)(interval*1000));
        characterImages[num].SetActive(true);
        characterImages[num].transform.localScale = new Vector2(1.5f, 1.5f);
        characterImages[num].GetComponent<Image>().sprite = masterData.newObjects[num];
        characterImages[num].transform.DOScale(Vector2.one, 0.5f);
    }

    //パネルを非表示する
    public void NonDisplayPanel(float interval)
    {
        DOTween.To(
            () => resultPanel.alpha,
            (n) => resultPanel.alpha = n,
            0.0f,
            interval);
    }


    //タイムアウト関連
    CancellationTokenSource ct = new CancellationTokenSource();
    float resultTimeout_sec = 30.0f;

    public async UniTask WaitForTimeout(float time_sec)
    {

        ct = new CancellationTokenSource();

        await UniTask.Delay((int)(time_sec * 1000), cancellationToken: ct.Token);
        
        //TODO: ヒントへ
        //ランキングパネル等が適切に遷移するか
        //音も心配

    }
    /*FI FO 類
    void TutorialPanelFI()
    {
        tutorialPanelRay.DOFade(endValue: 1.0f, duration: 1.0f);
    }
    async UniTask TutorialPanelFO()
    {
        tutorialPanelRay.DOFade(endValue: 0.0f, duration: 1.0f);
        await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());
        explainPanel.PanelsInit();
    }
    */

    //他スクリプトで呼び出し用の変数
    public async UniTask ResultSceneAfter(float num)
    {
        //ランキング更新
        rankNum = rankCtrl.rankUpdate(masterData.score);
        if (rankNum != -1)
        {
            pleaseTouchText.SetActive(false);
        }
        else
        {
            pleaseTouchText.SetActive(true);
        }

        //パネル更新
        SetupCharacter();
        scoreText.text = "0";
        await resultPanel.DOFade(endValue: 1.0f, duration: num);
        DisplayCharacter();
        DisplayScore();

        //紙吹雪表示
        UniTask.Void(async () =>
        {
            await UniTask.Delay(masterData.score * 100);
            redPaper.Play();
            yellowPaper.Play();
        });

        //効果音再生
        UniTask.Void(async () =>
        {
            audio.PlayOneShot(drumRoll);
            await UniTask.Delay(masterData.score * 100);
            audio.Stop();
            audio.PlayOneShot(drumFinish);
        });
    }
    public void ResultSceneContinuation()
    {
        //ランキング更新時パネルを表示
        if (rankNum != -1)
        {
            if (rankPanelTransition == RankPanelTransition.beforeView && !executionF)
            {
                UniTask.Void(async () =>
                {
                    executionF = true;

                    pleaseTouchText_ranking.SetActive(false);
                    //パネル - 自分の順位だけ強調する
                    rankCtrl.rankBackFlash(rankNum).Forget();
                    //パネルを出す
                    await UniTask.Delay(1000);
                    await rankPanel.transform.DOLocalMoveY(0, 1.0f);
                    //「ランクイン」の音、テキストを表示
                    UniTask.Void(async () =>
                    {
                        await UniTask.Delay(700);
                        rankInAudio.Play();
                    });
                    UniTask.Void(async () =>
                    {
                        rankInAnim.SetBool("AnimationF", true);
                        await UniTask.Delay(3000);
                        rankInAnim.SetBool("AnimationF", false);
                        pleaseTouchText_ranking.SetActive(true);
                        TransitionUpdate();
                    });
                });
            }
            else if (rankPanelTransition == RankPanelTransition.nowView && !executionF)
            {
                if(Input.GetMouseButtonDown(0) || (serialScript.enabled == true && serialScript.pushCheck()))
                {
                    TransitionUpdate();
                }
            }
            else if (rankPanelTransition == RankPanelTransition.afterView && !executionF)
            {
                UniTask.Void(async () =>
                {
                    executionF = true;

                    pleaseTouchText.SetActive(true);
                    //パネルを消す
                    rankPanel.transform.DOLocalMoveY(1100, 1.0f);
                    await UniTask.Delay(1500);

                    rankPanelTransition = RankPanelTransition.beforeView;
                    executionF = false;
                    
                    rankNum = -1;
                });
            }
        }
    }
    public void ResultSceneBefore(float num)
    {
        NonDisplayPanel(num);
    }
}
