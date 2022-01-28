using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*キャラクターのアニメーションのスクリプト*/


public class CharctorAnimationScript : MonoBehaviour
{
    Animator anim;

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
    public AnimType animtype = AnimType.NULL;       //アニメーションのタイプ

    void Start()
    {
        //TODO: 直せる
        anim = GetComponent<Animator>();
        anim.SetInteger("AnimMode", (int)animtype);
    }
}
