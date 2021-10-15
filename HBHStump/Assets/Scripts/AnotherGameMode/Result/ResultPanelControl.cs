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

    //パネルを表示する
    public void DisplayPanel(float interval)
    {
        DOTween.To(
            () => resultPanel.alpha,
            (n) => resultPanel.alpha = n,
            1.0f,
            interval);
    }
    //スコアを表示する
    public void DisplayScore()
    {
        scoreText.text = masterData.score.ToString();
    }
    //変化させたキャラクター一覧を表示する
    public void DisplayCharacter()
    {
        //キャラクターを表示するオブジェクトを初期化する
        for(int i = 0; i < 50; i++)
        {
            characterImages[i].GetComponent<Image>().sprite = null;
            characterImages[i].SetActive(false);
        }

        
        for(int i=0; i<masterData.newObjects.Count; i++)
        {
            characterImages[i].SetActive(true);
            characterImages[i].transform.localScale = new Vector2(1.5f, 1.5f);
            IEnumerator iEnumerator = DisplayOneCharacter(i);
            StartCoroutine(iEnumerator);
        }
    }
    //キャラクターを一体表示する
    IEnumerator DisplayOneCharacter(int num) {
        float interval = num * 0.1f;
        yield return new WaitForSeconds(interval);
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
        scoreText.text = "0";
        DisplayPanel(num);
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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
