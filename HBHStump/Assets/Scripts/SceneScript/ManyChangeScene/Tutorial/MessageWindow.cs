using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/*
メッセージウィンドウのクラス
*/

public class MessageWindow : MonoBehaviour
{
    const float duration = 0.05f;       //次の文字が表れるまでの時間

    //メッセージ列
    public List<string> messageGroup;
    
    string messageLine;             //表示するメッセージ
    public int messageLineNum;      //メッセージ番号(何行目か)      
    
    public int currentMessageLength;    //現在のメッセージの文字数
    public int totalMessageLength;      //最終的に表示するのメッセージの文字数
        
    float elapsedTime;              //TODO: 消せそう、経過時間
    public bool messageFinish;      //TODO: 消せそう、メッセージが終了したかのフラグ

    //文字列を表示するテキスト
    [SerializeField] Text nameText;
    [SerializeField] Text messageText;
    [SerializeField] Serial serial;
    [SerializeField] AudioClip talkAudioClip;

    public bool IsFinishMessageLine() => currentMessageLength >= totalMessageLength;
    public bool IsFinishMessageGroup() => messageLineNum * 2 + 2 >= messageGroup.Count;

    //メッセージ列を読み込む。加えて、初期化もする
    public void LoadMessage(List<string> message)
    {
        messageGroup = message;

        messageLineNum = 0;
        currentMessageLength = 0;
        totalMessageLength = messageGroup[messageLineNum * 2 + 1].Length;
        messageLine = messageGroup[messageLineNum * 2 + 1];
        elapsedTime = 0.0f;
        messageFinish = false;
    }

    //メッセージを1文字ごとに表示する
    public void PrintText()
    {
        //時間が経過するにつれ、文字が表れていく
        if (currentMessageLength < totalMessageLength) {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= duration)
            {
                elapsedTime -= duration;
                currentMessageLength++;
                AudioManager.Instance.PlaySE(talkAudioClip);
            }
        }
        
        if(nameText != null)
        {
            nameText.text = messageGroup[messageLineNum * 2];
        }
        messageText.text = messageGroup[messageLineNum * 2 + 1].Substring(0, currentMessageLength);
    }
    
    //クリック or スタンプを押した時に、次のテキストを表示する
    public void NextMessage()
    {
        if (!IsFinishMessageGroup())
        {
            messageLineNum++;
            currentMessageLength = 0;
            totalMessageLength = messageGroup[messageLineNum * 2 + 1].Length;
            messageLine = messageGroup[messageLineNum * 2 + 1];
            elapsedTime = 0.0f;
        }else
        {
            //メッセージが終了
            messageFinish = true;
        }
    }
   
    //メッセージウィンドウのUpdate
    public void MessageWindowUpdate()
    {
        PrintText();

        if ((Input.GetMouseButtonDown(0) || serial.pushCheck()) && currentMessageLength >= totalMessageLength)
        {
            NextMessage();
        }
    }
}
