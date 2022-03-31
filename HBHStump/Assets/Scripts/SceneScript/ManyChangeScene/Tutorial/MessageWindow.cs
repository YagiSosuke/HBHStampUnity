using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

/*
メッセージウィンドウのクラス
*/

public class MessageWindow : MonoBehaviour
{
    const float duration = 0.05f;           //次の文字が表れるまでの時間
    const string markupTagAction = "<Action>";
    const string markupTagCondition = "<Condition>";
    List<Action> actionGroup;
    List<Func<bool>> conditionGroup;
    Func<bool> currentCondition;
    int actionIndex;
    int conditionIndex;

    List<string> messageGroup;              //メッセージ群
    string messageLine;                     //表示するメッセージ1行
    int    messageLineNum;                  //メッセージはメッセージ群のうち何行目か      
    int    currentMessageLength;            //現在のメッセージの文字数
    int    totalMessageLength;              //最終的に表示するのメッセージの文字数
    [SerializeField] Text nameText;
    [SerializeField] Text messageText;
    [SerializeField] Serial serial;
    [SerializeField] AudioClip talkAudioClip;

    CancellationTokenSource cts = new CancellationTokenSource();


    public bool IsFinishMessageLine() => currentMessageLength >= totalMessageLength;
    public bool IsFinishMessageGroup() => messageLineNum * 2 + 2 >= messageGroup.Count;
    public bool IsAdvanceStep() => Input.GetMouseButtonDown(0) || serial.pushCheck();

    //メッセージ列を読み込む。加えて、初期化もする
    public void LoadMessage(List<string> message, List<Action> _actions = null, List<Func<bool>> _conditions = null)
    {
        messageGroup = message;
        actionGroup = _actions;
        conditionGroup = _conditions;

        actionIndex = 0;
        conditionIndex = 0;
        messageLineNum = 0;
        currentMessageLength = 0;

        messageLine = messageGroup[messageLineNum * 2 + 1];
        currentCondition = IsAdvanceStep;
        ScanMarkupLanguage();
        totalMessageLength = messageLine.Length;
        messageText.text = "";
    }
    //メッセージを1文字ごと表示
    async UniTask PrintText()
    {
        if (!IsFinishMessageLine())
        {
            //表示途中だった場合、強制キャンセルさせる
            await UniTask.Delay((int)(duration * 1000), cancellationToken: cts.Token);
            currentMessageLength++;
            AudioManager.Instance.PlaySE(talkAudioClip);
        }
        else
        {
            await UniTask.DelayFrame(1);
        }
        
        if(nameText != null)
        {
            nameText.text = messageGroup[messageLineNum * 2];
        }
        messageText.text = messageLine.Substring(0, currentMessageLength);
    }
    //クリック or スタンプを押した時に、次のテキストを表示する
    void NextMessage()
    {
        messageLineNum++;
        currentMessageLength = 0;
        messageLine = messageGroup[messageLineNum * 2 + 1];
        currentCondition = IsAdvanceStep;
        ScanMarkupLanguage();
        totalMessageLength = messageLine.Length;
    }
    void ScanMarkupLanguage()
    {
        if (messageLine.IndexOf(markupTagAction) != -1)
        {
            actionGroup[actionIndex].Invoke();
            actionIndex++;
            messageLine = messageLine.Replace(markupTagAction, "");
        }
        if (messageLine.IndexOf(markupTagCondition) != -1)
        {
            currentCondition = conditionGroup[conditionIndex];
            conditionIndex++;
            messageLine = messageLine.Replace(markupTagCondition, "");
        }
    }
    public async UniTask ShowMessage(Action onNextMessage)
    {
        cts.Cancel();   //TODO: バグるかも
        cts = new CancellationTokenSource();

        while (!cts.IsCancellationRequested) {
            await PrintText();

            if (IsFinishMessageLine() && !IsFinishMessageGroup())
            {
                await UniTask.WaitUntil(() => currentCondition.Invoke(), cancellationToken: this.GetCancellationTokenOnDestroy());
                onNextMessage.Invoke();
                NextMessage();
            }
        }
    }
}
