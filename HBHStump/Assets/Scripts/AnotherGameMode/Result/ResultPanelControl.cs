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
    [SerializeField] MasterData masterData;
    //リザルト画面時に表示するパネル
    [SerializeField] CanvasGroup resultPanel;


    //得点を表示するパネル
    [SerializeField] Text scoreText;
    //変化させたキャラクターを表示するパネル
    [SerializeField] GameObject[] characterImages = new GameObject[50];
    
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
        SetupCharacter();
        scoreText.text = "0";
        resultPanel.DOFade(endValue: 1.0f, duration: num);
        await UniTask.Delay((int)(num * 1000));
        DisplayCharacter();
        DisplayScore();
    }
    public void ResultSceneContinuation()
    {
    }
    public void ResultSceneBefore(float num)
    {
        NonDisplayPanel(num);
    }
    
}
