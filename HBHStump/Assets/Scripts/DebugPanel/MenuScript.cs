using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/*メニューを押した時のスクリプト*/

public class MenuScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject partsAndStampPanel;          //言葉とスタンプのパネル
    bool isMenuOpen = true;      //メニューが展開されているか
    

    async UniTask MenuOpen()
    {
        var duration = 0.5f;

        partsAndStampPanel.transform.DOLocalMoveY(0, duration);
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
    }
    async UniTask MenuClose()
    {
        var duration = 0.5f;

        partsAndStampPanel.transform.DOLocalMoveY(-900, duration);
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
    }
        
    //メニュー展開パネルをクリックしたら
    public void OnPointerClick(PointerEventData pointerData)
    {
        isMenuOpen = !isMenuOpen;
        
        if (isMenuOpen) MenuClose().Forget();
        else MenuOpen().Forget();
    }
}
