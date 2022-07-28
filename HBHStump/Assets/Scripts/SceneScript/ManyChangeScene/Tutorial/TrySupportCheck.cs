using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/*みかんを変身させるときのサポートのチェックボックス*/

public class TrySupportCheck : MonoBehaviour
{
    [SerializeField] Slider[] slider;
    [SerializeField] TutorialCharactorScript tutorialCharacter;
    Parts Parts => Stamp.Instance.Parts;
    string Word => Stamp.Instance.Word;

    CancellationTokenSource cts;
    SceneController SceneController => SceneController.Instance;

    public void Init()
    {
        Check_Off(0);
        Check_Off(1);
        Check_Off(2);
        Check_Off(3);
        CheckBoxCondition_1().Forget();
        CheckBoxCondition_2().Forget();
        CheckBoxCondition_3().Forget();
        CheckBoxCondition_4().Forget();
        cts = new CancellationTokenSource();
        Cancel().Forget();

        async UniTask Cancel()
        {
            await UniTask.WaitUntil(() => SceneController.screenMode != ScreenMode.Tutorial, cancellationToken: this.GetCancellationTokenOnDestroy());
            cts.Cancel();
        }
    }

    public void Check_On(int num)
    {
        float checkTime = 0.5f;
        DOTween.To(() => slider[num].value, (x) => slider[num].value = x, 1.0f, checkTime).SetEase(Ease.OutQuad);
    }
    public void Check_Off(int num)
    {
        slider[num].value = 0.0f;
    }

    async UniTask CheckBoxCondition_1()
    {
        bool Condition() => Word == "ま" || Word == "み" || Word == "む" || Word == "め" || Word == "も";
        await UniTask.WaitUntil(() => Condition(), cancellationToken: this.GetCancellationTokenOnDestroy());
        Check_On(0);

        await UniTask.WaitUntil(() => !Condition(), cancellationToken: cts.Token);
        Check_Off(0);
        CheckBoxCondition_1().Forget();
    }
    async UniTask CheckBoxCondition_2()
    {
        bool Condition() => Word == "み";
        await UniTask.WaitUntil(() => Condition(), cancellationToken: this.GetCancellationTokenOnDestroy());
        Check_On(1);

        await UniTask.WaitUntil(() => !Condition(), cancellationToken: cts.Token);
        Check_Off(1);
        CheckBoxCondition_2().Forget();
    }
    async UniTask CheckBoxCondition_3()
    {
        bool Condition() => Parts == Parts.Head;
        await UniTask.WaitUntil(() => Condition(), cancellationToken: this.GetCancellationTokenOnDestroy());
        Check_On(2);

        await UniTask.WaitUntil(() => !Condition(), cancellationToken: cts.Token);
        Check_Off(2);
        CheckBoxCondition_3().Forget();
    }
    async UniTask CheckBoxCondition_4()
    {
        await UniTask.WaitUntil(() => tutorialCharacter.IsPushStampToMikan(), cancellationToken: this.GetCancellationTokenOnDestroy());
        Check_On(3);
    }
}
