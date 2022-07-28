using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/*十中八九エラる*/

public class TutorialCharactorNameSet : MonoBehaviour
{
    [SerializeField] string charaName;
    [SerializeField] Image flamePartsPrefab;
    [SerializeField] GameObject flamePrefab;
    [SerializeField] AudioClip clip;

    GameObject addFrame;
    const float namePosY = 150;


    public void Initialize()
    {
        //枠組みを形成
        var flame = Instantiate(flamePrefab, transform.parent);
        flame.transform.localPosition = new Vector3(0, namePosY, transform.position.z);
        flame.GetComponent<RectTransform>().sizeDelta = new Vector2(charaName.Length * 100, 100);

        //文字を表示
        for (int i = 0; i < charaName.Length; i++)
        {
            var flameParts = Instantiate(flamePartsPrefab, flame.transform.position, Quaternion.identity, flame.transform);
            flameParts.transform.GetChild(1).GetComponent<Text>().text = charaName.Substring(i, 1);

            if (i == 0 && (charaName == "みかん"))
            {
                addFrame = flameParts.gameObject;
                flameParts.color = Color.green;                
            }
        }
    }

    //追加された文字の表示
    public async UniTask AddFlameAnimation()
    {
        addFrame.transform.localScale = Vector2.one * 3;
        addFrame.transform.DOScale(Vector2.one, 1.0f).SetEase(Ease.InCubic);
        await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());

        AudioManager.Instance.PlaySE(clip);
    }
}