using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaImageData : MonoBehaviour
{
    public static CharaImageData Instance;
    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    [SerializeField] Sprite[] charaSprite;

    public Sprite GetSprite(string name)
    {
        foreach(Sprite sprite in charaSprite)
        {
            if (sprite.name == name) return sprite;
        }
        Debug.Log($"{name}:spriteは存在しません");
        return null;
    }
}
