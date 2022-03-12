using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class SerealReadDebug : MonoBehaviour
{
    [SerializeField] Serial serial;
    [SerializeField] InputField inputField;

    public void DebugSerialRead()
    {
        try
        {
            serial.DataAnalysis(inputField.text);
            inputField.text = "";
            PushFlugInit().Forget();

            async UniTask PushFlugInit()
            {
                await UniTask.DelayFrame(1);
                serial.PushFlugInit();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("シリアル系のエラーでデバッグできません");
            Debug.LogException(e);
        }
    }


}
