using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*キャラクターのアニメーションのスクリプト*/


public class CharctorAnimationScript : MonoBehaviour
{
    Animator anim;

    public enum AnimType
    {
        NULL,
        InorganicObject,
        AnimalObject,
        BallObject,
        SpringObject,
        FallDownObject,
        UpUpObject
    }
    public AnimType animtype = AnimType.NULL;       //アニメーションのタイプ

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("AnimMode", (int)animtype);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
