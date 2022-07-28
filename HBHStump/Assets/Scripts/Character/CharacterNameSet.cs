using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/*キャラクタの名前を自動でセットする*/

public class CharacterNameSet : MonoBehaviour
{
    [SerializeField] Image flamePartsPrefab;
    [SerializeField] GameObject flamePrefab;
    [SerializeField] AudioClip clip;

    [SerializeField] CharacterBase characterBase;

    const float namePosY = 150;
    public bool IsGameMode { get; private set; } = true;


    public void Initialize()
    {
        var name = characterBase.CharaName;
        var parts = characterBase.Parts;

        //枠組みを形成
        var flame = Instantiate(flamePrefab, transform.parent);
        flame.transform.localPosition = new Vector3(0, namePosY, transform.position.z);
        flame.GetComponent<RectTransform>().sizeDelta = new Vector2(name.Length * 100, 100);

        //文字を表示
        for (int i = 0; i < name.Length; i++)
        {
            var flameParts = Instantiate(flamePartsPrefab, flame.transform.position, Quaternion.identity, flame.transform);
            flameParts.transform.GetChild(1).GetComponent<Text>().text = name.Substring(i, 1);

            if (i == 0 && (parts == Parts.Head) ||
                i == 1 && (parts == Parts.Body) ||
                i == 2 && (parts == Parts.Hip))
            {
                AddFlameAnimation(flameParts).Forget();
            }
        }
    }
    //追加された文字の表示
    async UniTask AddFlameAnimation(Image flameParts)
    {
        flameParts.color = Color.green;
        if (IsGameMode)
        {
            flameParts.transform.localScale = Vector2.one * 3;
            flameParts.transform.DOScale(Vector2.one, 1.0f).SetEase(Ease.InCubic);
            await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());

            AudioManager.Instance.PlaySE(clip);
        }
    }
    public void SetGameMode(bool _isGameMode)
    {
        IsGameMode = _isGameMode;
    }
}
