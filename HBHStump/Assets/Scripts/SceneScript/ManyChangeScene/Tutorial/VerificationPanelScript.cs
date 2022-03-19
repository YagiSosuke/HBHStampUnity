using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

/*
チュートリアルを見るか確認するパネル
*/

public class VerificationPanelScript : MonoBehaviour
{
    readonly Vector2 verificationYesPosition = new Vector2(2, 1);
    readonly Vector2 verificationNoPosition = new Vector2(3, 1);
    public bool IsTakeTutorial { get; private set; }
    public bool IsNotTakeTutorial { get; private set; }
    [SerializeField] Serial serial;
    CancellationTokenSource cts = new CancellationTokenSource();


    void OnDestroy()
    {
        cts.Cancel();
    }

    //「はい」「いいえ」のボタン押下時
    public void VerificationYes()
    {
        IsTakeTutorial = true;
        cts.Cancel();
    }
    public void VerificationNo()
    {
        IsNotTakeTutorial = true;
        cts.Cancel();
    }

    async UniTask WaitForTakeTutorial()
    {
        await UniTask.WaitUntil(() => serial.IsUseDevice == false &&
            Serial.PushF[(int)verificationYesPosition.x, (int)verificationYesPosition.y],
            cancellationToken: cts.Token);
        IsTakeTutorial = true;
        cts.Cancel();
    }
    async UniTask WaitForNotTakeTutorial()
    {
        await UniTask.WaitUntil(() => serial.IsUseDevice == false &&
            Serial.PushF[(int)verificationNoPosition.x, (int)verificationNoPosition.y],
            cancellationToken: cts.Token);
        IsNotTakeTutorial = true;
        cts.Cancel();
    }

    public void SetUp()
    {
        cts = new CancellationTokenSource();
        IsTakeTutorial = false;
        IsNotTakeTutorial = false;
        WaitForTakeTutorial().Forget();
        WaitForNotTakeTutorial().Forget();
    }    
}
