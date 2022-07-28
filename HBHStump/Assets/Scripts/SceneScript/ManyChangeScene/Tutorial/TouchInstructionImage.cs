using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/*メッセージを表示しきった時にタッチを促す画像*/

public class TouchInstructionImage : MonoBehaviour
{
    [SerializeField] CanvasGroup touchInstructionImage;
    [SerializeField] Animator touchInstructionAnimator;

    public void Init()
    {
        touchInstructionImage.alpha = 0;
    }
    public void ShowTouchInstruction()
    {
        Debug.Log("Show");
        if (touchInstructionImage.alpha != 1.0f)
        {
            touchInstructionAnimator.SetBool("AnimationF", true);
            touchInstructionImage.DOFade(endValue: 1.0f, duration: 0.2f);
        }
    }
    public void HideTouchInstruction()
    {
        Debug.Log("Hide");
        if (touchInstructionImage.alpha != 0.0f)
        {
            touchInstructionAnimator.SetBool("AnimationF", false);
            touchInstructionImage.DOFade(endValue: 0.0f, duration: 0.2f);
        }
    }
}
