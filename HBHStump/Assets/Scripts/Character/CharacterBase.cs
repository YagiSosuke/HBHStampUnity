using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour
{
    //キャラクター基本データ
    public int ID { get; private set; }
    protected string charaName;
    AnimType animType;
    Sprite sprite;

    [SerializeField] Image image;
    [SerializeField] CharacterAnimationScript characterAnimationScript;

    public void Initialize(CharaData _data)
    {
        SetCharaData(_data);
        SetImage();
        SetAnimation();
    }
    protected void SetCharaData(CharaData _data)
    {
        ID = _data.ID;
        charaName = _data.CharaName;
        animType = _data.AnimType;
        sprite = _data.Sprite;
    }
    void SetImage()
    {
        image.sprite = sprite;
    }
    void SetAnimation()
    {
        characterAnimationScript.SetAnimation(animType);
    }
}