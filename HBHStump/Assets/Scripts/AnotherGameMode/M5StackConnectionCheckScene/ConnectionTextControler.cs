using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
エラーメッセージを表示するテキスト 
*/

public class ConnectionTextControler : MonoBehaviour
{
    [SerializeField] Text nowConnectionDeviceText;
    [SerializeField] Text errorText;


    public void Clear()
    {
        if(nowConnectionDeviceText)
            nowConnectionDeviceText.text = "デバイスは接続されていません";
        if(errorText)
            errorText.text = "";
    }

    public void DisplayConnectionDevice(COMNumber comNum)
    {
        nowConnectionDeviceText.text = comNum.ToString() + " のデバイスに接続されています";
    }
    public void DesplayErrorMessage(System.Exception e)
    {
        if (e.ToString().IndexOf("IOException: デバイスが接続されていません。")             >= 0 ||
            e.ToString().IndexOf("System.IO.IOException: セマフォがタイムアウトしました。") >= 0)
        {
            errorText.text = "デバイスにエラーがあります\nデバイスを再起動してください";
        }
        else if (e.ToString().IndexOf("System.IO.IOException: アクセスが拒否されました。") >= 0)
        {
            errorText.text = "デバイスのポートが閉じていません\n時間をおいて再度試してください";
        }
        else
        {
            errorText.text = "デバイスが存在しないため接続できません";
        }
    }
}
