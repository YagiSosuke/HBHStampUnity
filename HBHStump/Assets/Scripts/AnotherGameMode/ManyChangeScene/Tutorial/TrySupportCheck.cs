using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

/*みかんを変身させるときのサポートのスクリプト*/

public class TrySupportCheck : MonoBehaviour
{
    abstract class CheckControler
    {
        Slider slider;
        bool checkF = false;
        float value = 0.0f;
        float checkTime = 0.5f;

        public CheckControler(Slider slider)
        {
            this.slider = slider;
            slider.value = 0.0f;
        }
        public void Setup()
        {
            slider.value = 0.0f;
            checkF = false;
            value = 0.0f;
        }
        public void Check_On()
        {
            if (!checkF)
            {
                DOTween.To(() => value, (x) => value = x, 1.0f, checkTime).SetEase(Ease.OutQuad);
            }
            checkF = true;
        }
        public void Check_Off()
        {
            checkF = false;
            value = 0.0f;
        }
        public void ValueSet()
        {
            slider.value = value;
        }

        //チェックする条件
        public abstract void CheckBoxConditions();
    }
    //「ま」行を読み込んだかチェック
    class Check1 : CheckControler
    {
        public Check1(Slider slider) : base(slider) { }
        public override void CheckBoxConditions()
        {
            if(Stamp.Instance.Word == "ま" ||
               Stamp.Instance.Word == "み" ||
               Stamp.Instance.Word == "む" ||
               Stamp.Instance.Word == "め" ||
               Stamp.Instance.Word == "も")
            {
                Check_On();
            }
            else
            {
                Check_Off();
            }

            ValueSet();
        }
    }
    //「み」をセットしたかチェック
    class Check2 : CheckControler
    {
        public Check2(Slider slider) : base(slider) { }
        public override void CheckBoxConditions()
        {
            if (Stamp.Instance.Word == "み")
            {
                Check_On();
            }
            else
            {
                Check_Off();
            }

            ValueSet();
        }
    }
    //「あたま」を選択したかチェック
    class Check3 : CheckControler
    {
        public Check3(Slider slider) : base(slider) { }
        public override void CheckBoxConditions()
        {
            if (Stamp.Instance.Parts == Parts.Head)
            {
                Check_On();
            }
            else
            {
                Check_Off();
            }

            ValueSet();
        }
    }
    //みかんにスタンプを打ったかチェック
    class Check4 : CheckControler
    {
        TutorialCharactorScript tutorialCharacterScript;

        public Check4(Slider slider, TutorialCharactorScript tutorialCharacterScript) : base(slider)
        {
            this.tutorialCharacterScript = tutorialCharacterScript;
        }
        public override void CheckBoxConditions()
        {
            if (this.tutorialCharacterScript.SerchF)
            {
                Check_On();
            }
            else
            {
                Check_Off();
            }

            ValueSet();
        }
    }

    CheckControler check1;
    CheckControler check2;
    CheckControler check3;
    CheckControler check4;

    [SerializeField] Slider[] slider = new Slider[4];

    //みかんに変身させたかチェックするスクリプト
    [SerializeField] TutorialCharactorScript tutorialCharactorScript;


    //チェックボックスの条件が達成されているかチェックする
    public void CheckBoxCondition()
    {
        check1.CheckBoxConditions();
        check2.CheckBoxConditions();
        check3.CheckBoxConditions();
        check4.CheckBoxConditions();
    }

    //チュートリアルの段階をコントロールするクラス
    [SerializeField] TutorialMessage tutorialMessage;

    //チェックボックスのセットアップ(チェックを消す)
    public void CheckBoxSetup()
    {
        if(tutorialMessage.transitionMode == TutorialMessage.TransitionMode.afterSwitching)
        {
            check1.Setup();
            check2.Setup();
            check3.Setup();
            check4.Setup();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        check1 = new Check1(slider[0]);
        check2 = new Check2(slider[1]);
        check3 = new Check3(slider[2]);
        check4 = new Check4(slider[3], tutorialCharactorScript);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
