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
    public int RankNum { get; private set; }
    bool isRankUpdate = false;
    RankPanelTransition rankPanelTransition = RankPanelTransition.beforeView;
    //タイムアウト関連
    CancellationTokenSource ct = new CancellationTokenSource();
    float resultTimeout_sec = 30.0f;

    [Header("Objects")]
    [SerializeField] Text        scoreText;
    [SerializeField] GameObject  pleaseTouchText;
    [SerializeField] GameObject  pleaseTouchText_ranking;
    [SerializeField] Image[]     characterImages;
    [SerializeField] CanvasGroup resultPanel;
    [SerializeField] GameObject  rankPanel;
    [SerializeField] Animator    rankInAnimator;
    [Header("PaperParticle")]
    [SerializeField] ParticleSystem redPaper;
    [SerializeField] ParticleSystem yellowPaper;
    [Header("SE")]
    [SerializeField] AudioClip drumRoll;
    [SerializeField] AudioClip drumFinish;
    [SerializeField] AudioClip rankInSE;
    [Header("Controllers")]
    [SerializeField] RankingControl rankControll;
    [SerializeField] Serial         serialScript;
    MasterData MasterData =>        MasterData.Instance;


    void TransitionUpdate()
    {
        rankPanelTransition++;
        isRankUpdate = false;
    }

    //キャラクター一覧を設定する
    void SetupCharacter()
    {
        //キャラクターを表示するオブジェクトを初期化する
        foreach (Image charaImage in characterImages)
        {
            charaImage.sprite = null;
            charaImage.gameObject.SetActive(false);
        }
    }
    void ShowScore()
    {
        scoreText.text = MasterData.Score.ToString();
    }
    void ShowCharacters()
    {        
        for(int i=0; i< MasterData.changeObjs.Count; i++)
        {
            ShowOneChara(i).Forget();
        }

        async UniTask ShowOneChara(int num)
        {
            float interval = num * 0.1f;
            await UniTask.Delay((int)(interval * 1000));
            characterImages[num].gameObject.SetActive(true);
            characterImages[num].sprite = MasterData.changeObjs[num];
            characterImages[num].transform.localScale = new Vector2(1.5f, 1.5f);
            characterImages[num].transform.DOScale(Vector2.one, 0.5f);
        }
    }
    
    void HidePanel(float interval)
    {
        resultPanel.DOFade(endValue: 0.0f, duration: interval);
    }
    
    public async UniTask WaitForTimeout(float time_sec)
    {
        ct = new CancellationTokenSource();

        await UniTask.Delay((int)(time_sec * 1000), cancellationToken: ct.Token);
        
        //TODO: 一定時間経過でヒントへ自動移動する処理を書く
        //ランキングパネル等が適切に遷移するか
        //音も心配
    }

    //他スクリプトで呼び出し用の変数
    public async UniTask ResultSceneAfter(float num)
    {
        //ランキング更新
        RankNum = rankControll.rankUpdate(MasterData.Score);
        if (RankNum != -1)
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
        ShowCharacters();
        ShowScore();

        //紙吹雪表示
        UniTask.Void(async () =>
        {
            await UniTask.Delay(MasterData.Score * 100);
            redPaper.Play();
            yellowPaper.Play();
        });
        //効果音再生
        UniTask.Void(async () =>
        {
            AudioManager.Instance.PlaySE(drumRoll);
            await UniTask.Delay(MasterData.Score * 100);
            AudioManager.Instance.Stop();
            AudioManager.Instance.PlaySE(drumFinish);
        });
    }
    public void ResultSceneContinuation()
    {
        //ランキング更新時パネルを表示
        if (RankNum != -1)
        {
            if (rankPanelTransition == RankPanelTransition.beforeView && !isRankUpdate)
            {
                UniTask.Void(async () =>
                {
                    isRankUpdate = true;

                    pleaseTouchText_ranking.SetActive(false);
                    //パネル - 自分の順位だけ強調する
                    rankControll.rankBackFlash(RankNum).Forget();
                    //パネルを出す
                    await UniTask.Delay(1000);
                    await rankPanel.transform.DOLocalMoveY(0, 1.0f);
                    //「ランクイン」の音、テキストを表示
                    UniTask.Void(async () =>
                    {
                        await UniTask.Delay(700);
                        AudioManager.Instance.PlaySE(rankInSE);
                    });
                    UniTask.Void(async () =>
                    {
                        rankInAnimator.SetBool("AnimationF", true);
                        await UniTask.Delay(3000);
                        rankInAnimator.SetBool("AnimationF", false);
                        pleaseTouchText_ranking.SetActive(true);
                        TransitionUpdate();
                    });
                });
            }
            else if (rankPanelTransition == RankPanelTransition.nowView && !isRankUpdate)
            {
                if(Input.GetMouseButtonDown(0) || (serialScript.IsUseDevice == true && serialScript.pushCheck()))
                {
                    TransitionUpdate();
                }
            }
            else if (rankPanelTransition == RankPanelTransition.afterView && !isRankUpdate)
            {
                UniTask.Void(async () =>
                {
                    isRankUpdate = true;

                    pleaseTouchText.SetActive(true);
                    //パネルを消す
                    rankPanel.transform.DOLocalMoveY(1100, 1.0f);
                    await UniTask.Delay(1500);

                    rankPanelTransition = RankPanelTransition.beforeView;
                    isRankUpdate = false;
                    
                    RankNum = -1;
                });
            }
        }
    }
    public void ResultSceneBefore(float num)
    {
        HidePanel(num);
    }


    enum RankPanelTransition
    {
        beforeView,
        nowView,
        afterView,
    };
}
