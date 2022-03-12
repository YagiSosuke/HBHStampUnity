using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour
{
    //キャラクター基本データ
    public int ID { get; private set; }
    public string CharaName { get; private set; }
    public Parts Parts { get; private set; }
    AnimType animType;
    Sprite sprite;

    [SerializeField] Image image;
    [SerializeField] CharacterAnimationScript characterAnimationScript;
    [SerializeField] CharacterNameSet characterNameSet;

    public void Initialize(CharaData _data)
    {
        SetCharaData(_data);
        SetImage();
        SetAnimation();
        characterNameSet.Initialize();
    }
    protected void SetCharaData(CharaData _data)
    {
        ID = _data.ID;
        CharaName = _data.CharaName;
        Parts = _data.Parts;
        animType = _data.AnimType;
        sprite = _data.Sprite;
    }
    public void SetCharaNameType(bool _isGameMode)
    {
        characterNameSet.SetGameMode(_isGameMode);
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