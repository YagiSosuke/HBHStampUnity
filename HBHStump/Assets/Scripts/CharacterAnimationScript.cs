using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*キャラクターのアニメーションのスクリプト*/


public class CharacterAnimationScript : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AnimType animtype = AnimType.NULL;       //アニメーションのタイプ

    public void SetAnimation(AnimType _type)
    {
        animtype = _type;
        anim.SetInteger("AnimMode", (int)animtype);
    }
}

public enum AnimType
{
    NULL = 0,
    InorganicObject = 1,
    AnimalObject = 2,
    BallObject = 3,
    SpringObject = 4,
    FallDownObjec = 5,
    UpUpObject = 6
}
