using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Linq;

/*
    ゲーム終了時、ヒントを表示するパネル 
*/

public class HintPanel : MonoBehaviour
{
    int viewHintId = 0;
    List<GameObject> hintObjects = new List<GameObject>();

    [Header("各ヒントオブジェクト")]
    [SerializeField] BeforeChangeCharacter beforeCharacter;
    [SerializeField] CharacterBase afterCharacters;
    [Header("各ヒントの親オブジェクト")]
    [SerializeField] Transform   beforeChangeParent;
    [SerializeField] Transform[] hintParents;
    [Header("SE")]
    [SerializeField] AudioClip voiceClip;
    [SerializeField] AudioClip seClip;
    [Header("オブジェクト群")]
    [SerializeField] GameObject pleaseWaitPanel;
    [SerializeField] CanvasGroup hintCanvasGroup;

    CharaCsvLoader CharaCsvLoader => CharaCsvLoader.Instance;


    void Initialize()
    {
        var beforeChangeCharaDaras = CharaCsvLoader.beforeChangeCharaDatas;
        CharaData beforeData = beforeChangeCharaDaras[Random.Range(0, beforeChangeCharaDaras.Count)];
    }

    void ShowHint()
    {
        viewHintId = Random.Range(0, CharaCsvLoader.beforeChangeCharaDatas.Count);
        var beforeData = CharaCsvLoader.beforeChangeCharaDatas[viewHintId];
        var afterDatas = CharaCsvLoader.afterChangeCharaDatas[CharaCsvLoader.beforeChangeCharaDatas[viewHintId].CharaName];

        //ヒントの設定
        List<int> afterDataCounts = new List<int>();
        for (int i = 0; i < afterDatas.Count; i++)
        {
            afterDataCounts.Add(i);
        }
        afterDataCounts.OrderBy(a => System.Guid.NewGuid()).ToList();

        //ヒントの生成
        var before = Instantiate(beforeCharacter, beforeChangeParent);
        before.Initialize(beforeData);
        before.enabled = false;
        hintObjects.Add(before.gameObject);
        var InstanceCount = (afterDataCounts.Count > hintParents.Length) ? 6 : afterDataCounts.Count;
        for (int i = 0; i < InstanceCount; i++)
        {
            var after = Instantiate(afterCharacters, hintParents[i]);
            after.SetCharaNameType(false);
            after.Initialize(afterDatas[afterDataCounts[i]]);
            hintObjects.Add(after.gameObject);
        }
    }
    void DeleteHint()
    {
        foreach(GameObject obj in hintObjects)
        {
            Destroy(obj);
        }
        hintObjects.Clear();
    }
    async UniTask PlayFinishSE()
    {
        AudioManager.Instance.PlaySE(seClip);
        await hintCanvasGroup.DOFade(endValue: 1.0f, duration: 0.5f);
        AudioManager.Instance.PlaySE(voiceClip);
    }
    
    public async UniTask GameSceneAfter()
    {
        pleaseWaitPanel.SetActive(false);

        ShowHint();
        await PlayFinishSE();
        await UniTask.Delay(2000);

        pleaseWaitPanel.SetActive(true);
    }
    public void GameSceneContinuation()
    {

    }
    public async UniTask GameSceneBefore()
    {
        await hintCanvasGroup.DOFade(endValue: 0.0f, duration: 0.5f);
        DeleteHint();
    }
}
