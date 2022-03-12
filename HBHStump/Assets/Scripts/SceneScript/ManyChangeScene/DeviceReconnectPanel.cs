using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/*
デバイスを再接続する処理 
*/

    //接続完了を知らせる処理書く
public class DeviceReconnectPanel : MonoBehaviour
{
    [SerializeField] Serial serial;
    [SerializeField] ConnectionTextControler connectionTextControler;
    [SerializeField] Text connectionText;
    [SerializeField] Text errorText;

    [SerializeField] GameObject connectingPanel;

    //再接続の処理を書く
    async UniTask TryConnection()
    {
        connectingPanel.SetActive(true);

        await UniTask.DelayFrame(1);

        try
        {
            serial.OpenCheck();
            serial.SerialReadWordAndParts();

            connectionText.text = serial.portName;
            errorText.text = "";
            Debug.Log(serial.serial.PortName);
        }
        catch(System.Exception e)
        {
            connectionText.text = "";
            connectionTextControler.DisplayErrorMessage(e);
        }

        connectingPanel.SetActive(false);
    }

    async UniTask OnPushCom1Button()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1),
                                      cancellationToken:this.GetCancellationTokenOnDestroy());
        if (serial.serial != null) serial.Close();
        serial.portName = "COM1";
        TryConnection().Forget();

        OnPushCom1Button().Forget();
    }
    async UniTask OnPushCom2Button()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2),
                                      cancellationToken: this.GetCancellationTokenOnDestroy());
        if (serial.serial != null) serial.Close();
        serial.portName = "COM2";
        TryConnection().Forget();

        OnPushCom2Button().Forget();
    }
    async UniTask OnPushCom3Button()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3),
                                      cancellationToken: this.GetCancellationTokenOnDestroy());
        if (serial.serial != null) serial.Close();
        serial.portName = "COM3";
        TryConnection().Forget();

        OnPushCom3Button().Forget();
    }
    async UniTask OnPushCom4Button()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4),
                                      cancellationToken: this.GetCancellationTokenOnDestroy());
        if (serial.serial != null) serial.Close();
        serial.portName = "COM4";
        TryConnection().Forget();

        OnPushCom4Button().Forget();
    }
    async UniTask OnPushCom5Button()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5),
                                      cancellationToken: this.GetCancellationTokenOnDestroy());
        if (serial.serial != null) serial.Close();
        serial.portName = "COM5";
        TryConnection().Forget();

        OnPushCom5Button().Forget();
    }
    async UniTask OnPushCom6Button()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6),
                                      cancellationToken: this.GetCancellationTokenOnDestroy());
        if (serial.serial != null) serial.Close();
        serial.portName = "COM6";
        TryConnection().Forget();

        OnPushCom6Button().Forget();
    }
    async UniTask OnPushCom7Button()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7),
                                      cancellationToken: this.GetCancellationTokenOnDestroy());
        if (serial.serial != null) serial.Close();
        serial.portName = "COM7";
        TryConnection().Forget();

        OnPushCom7Button().Forget();
    }
    async UniTask OnPushCom8Button()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8),
                                      cancellationToken: this.GetCancellationTokenOnDestroy());
        if (serial.serial != null) serial.Close();
        serial.portName = "COM8";
        TryConnection().Forget();

        OnPushCom8Button().Forget();
    }
    async UniTask OnPushCom9Button()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9),
                                      cancellationToken: this.GetCancellationTokenOnDestroy());
        if (serial.serial != null) serial.Close();
        serial.portName = "COM9";
        TryConnection().Forget();

        OnPushCom9Button().Forget();
    }

    private void Start()
    {
        if (serial.IsUseDevice)
        {
            if (SerialCheck.instance)
            {
                connectionText.text = SerialCheck.instance.comNumber.ToString();
            }
            OnPushCom1Button().Forget();
            OnPushCom2Button().Forget();
            OnPushCom3Button().Forget();
            OnPushCom4Button().Forget();
            OnPushCom5Button().Forget();
            OnPushCom6Button().Forget();
            OnPushCom7Button().Forget();
            OnPushCom8Button().Forget();
            OnPushCom9Button().Forget();
        }
    }
}
