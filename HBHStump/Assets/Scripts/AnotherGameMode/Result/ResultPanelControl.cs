using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

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

    //ステップが進んだ時のプッシュ音
    AudioSource pushAudio;

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

    void Start() {
        masterData = GameObject.Find("GameControler").GetComponent<MasterData>();
        rankCtrl = GameObject.Find("RankPanel").GetComponent<RankingControl>();
        resultPanel = GameObject.Find("ResultPanel").GetComponent<CanvasGroup>();

        pushAudio = GameObject.Find("PushSpeaker").GetComponent<AudioSource>();

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

    //他スクリプトで呼び出し用の変数
    public async UniTask ResultSceneAfter(float num)
    {
        //ランキング更新
        rankNum = rankCtrl.rankUpdate(masterData.score);

        //パネル更新
        SetupCharacter();
        scoreText.text = "0";
        resultPanel.DOFade(endValue: 1.0f, duration: num);
        await UniTask.Delay((int)(num * 1000));
        DisplayCharacter();
        DisplayScore();
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
                    //パネル - 自分の順位だけ強調する
                    rankCtrl.rankBackFlash(rankNum).Forget();
                    //パネルを出す
                    await UniTask.Delay(1000);
                    rankPanel.transform.DOLocalMoveY(0, 1.0f);
                    await UniTask.Delay(1000);
                    //「ランクイン」の音、テキストを表示
                    UniTask.Void(async () =>
                    {
                        await UniTask.Delay(700);
                        rankInAudio.Play();
                    });
                    UniTask.Void(async () =>
                    {
                        rankInAnim.SetBool("AnimationF", true);
                        await UniTask.Delay(4000);
                        rankInAnim.SetBool("AnimationF", false);
                    });
                    TransitionUpdate();
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
                    //プッシュ音を鳴らす
                    pushAudio.Play();
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
        pushAudio.Play();
        NonDisplayPanel(num);
    }
}
