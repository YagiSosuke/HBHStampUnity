using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
メッセージウィンドウのクラス
*/

public class MessageWindow : MonoBehaviour
{
    //文字列を表示するテキスト
    [SerializeField] Text nameText;
    [SerializeField] Text messageText;

    //メッセージ列
    public List<string> message;

    //表示するメッセージ
    string messageLine;
    string messageView;

    //メッセージ番号(何行目か)
    public int messageNum;

    //メッセージの文字数
    public int messageLength;
    public int messageCount;

    //経過時間
    float elapsedTime;
    //次の文字が表れるまでの時間
    [SerializeField] float intervalTime = 0.5f;

    //メッセージが終了したかのフラグ
    public bool messageFinish;

    //画面にスタンプが押されたかどうかのフラグ
    public bool pushF = false;


    //文字送り時の効果音鳴らすものたち
    #region
    [Header("文字送り時の効果音鳴らすものたち")]
    [SerializeField] AudioSource pushAudio;
    [SerializeField] AudioSource talkAudio;
    [SerializeField] AudioClip pushAudioClip;
    [SerializeField] AudioClip talkAudioClip;
    #endregion

    //メッセージ列を読み込む
    //加えて、初期化もする
    public void LoadMessage(List<string> message)
    {
        this.message = message;

        messageNum = 0;
        messageCount = 0;
        messageLength = this.message[messageNum*2 + 1].Length;
        messageLine = this.message[messageNum*2 + 1];
        elapsedTime = 0.0f;
        messageFinish = false;
    }

    //メッセージを視覚的に表示する
    //1文字ごとに表示されていく
    public void PrintText()
    {
        //時間が経過するにつれ、文字が表れていく
        if (messageCount < messageLength) {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= intervalTime)
            {
                elapsedTime -= intervalTime;
                messageCount++;
                talkAudio.PlayOneShot(talkAudioClip);
            }
        }
        
        if(nameText != null)
        {
            nameText.text = this.message[messageNum * 2];
        }
        messageText.text = this.message[messageNum*2 + 1].Substring(0, messageCount);
    }
    
    //クリック or スタンプを押した時に、次のテキストを表示する
    public void NextMessage()
    {
        if (message.Count > messageNum * 2 + 2)
        {
            messageNum++;
            messageCount = 0;
            messageLength = this.message[messageNum * 2 + 1].Length;
            messageLine = this.message[messageNum * 2 + 1];
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

        for(int i= 0; i< 15; i++)
        {
            if(Serial.PushF[i%5, i / 5])
            {
                pushF = true;
                break;
            }
        }

        if ((Input.GetMouseButtonDown(0) || pushF) && messageCount >= messageLength)
        {
            pushAudio.PlayOneShot(pushAudioClip);
            NextMessage();
        }
        pushF = false;
    }
}
