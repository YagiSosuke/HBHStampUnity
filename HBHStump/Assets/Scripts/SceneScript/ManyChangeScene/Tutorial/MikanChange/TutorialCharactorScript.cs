using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

/*キャラクターにスタンプ打った時のスクリプト（チュートリアル）*/

public class TutorialCharactorScript : MonoBehaviour, IPointerClickHandler
{
    readonly Vector2[] mikanPosition =
    {
        new Vector2(1, 0),
        new Vector2(1, 1)
    };

    public bool IsMikanChange { get; private set; }
    [SerializeField] GameObject kanObject;
    [SerializeField] GameObject mikanObject;
    [SerializeField] CharacterAnimationScript kanAnimation;
    [SerializeField] CharacterAnimationScript mikanAnimation;
    [SerializeField] TutorialCharactorNameSet kanName;
    [SerializeField] TutorialCharactorNameSet mikanName;
    [SerializeField] AudioClip changeSe;
    [SerializeField] Serial serial;

    public bool IsPushStampToMikan() =>
            (Stamp.Instance.Word == "み" && Stamp.Instance.Parts == Parts.Head &&
            (Serial.PushF[(int)mikanPosition[0].x, (int)mikanPosition[0].y] ||
             Serial.PushF[(int)mikanPosition[1].x, (int)mikanPosition[1].y])) ||
             IsMikanChange;

    public void Init()
    {
        IsMikanChange = false;
        kanObject.SetActive(true);
        mikanObject.SetActive(false);
        kanAnimation.SetAnimation(AnimType.FallDownObjec);
        kanName.Initialize();
        mikanName.Initialize();
    }

    void CharaChange()
    {
        kanObject.SetActive(false);
        mikanObject.SetActive(true);
        EffectManager.Instance.InstantiateTutorialFog();
        AudioManager.Instance.PlaySE(changeSe);
        mikanName.AddFlameAnimation().Forget();
        mikanAnimation.SetAnimation(AnimType.BallObject);
    }

    public bool OnTutorialMikanChange()
    {
        if(IsPushStampToMikan())
        {
            CharaChange();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        IsMikanChange = true;
    }
}
