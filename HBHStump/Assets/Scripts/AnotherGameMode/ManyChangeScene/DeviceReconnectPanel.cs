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

    public void OnPushCom1Button()
    {
        if (serial.serial != null) serial.Close();
        serial.portName = "COM1";
        TryConnection().Forget();
    }
    public void OnPushCom2Button()
    {
        if (serial.serial != null) serial.Close();
        serial.portName = "COM2";
        TryConnection().Forget();
    }
    public void OnPushCom3Button()
    {
        if (serial.serial != null) serial.Close();
        serial.portName = "COM3";
        TryConnection().Forget();
    }

    private void Start()
    {
        if (SerialCheck.instance)
        {
            connectionText.text = SerialCheck.instance.comNumber.ToString();
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            OnPushCom1Button();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            OnPushCom2Button();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            OnPushCom3Button();
        }
    }
}
