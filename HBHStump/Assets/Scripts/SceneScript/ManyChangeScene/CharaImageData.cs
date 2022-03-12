using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaImageData : MonoBehaviour
{
    public static CharaImageData Instance;
    public CharaImageData() { if (!Instance) Instance = this; }

    [SerializeField] Sprite[] charaSprite;
    public Sprite[] CharaSprite { get { return charaSprite; } private set { charaSprite = value; } }

    public Sprite GetSpriteByName(string name)
    {
        foreach(Sprite sprite in charaSprite)
        {
            if (sprite.name == name) return sprite;
        }
        Debug.Log($"{name}:spriteは存在しません");
        return null;
    }
}
