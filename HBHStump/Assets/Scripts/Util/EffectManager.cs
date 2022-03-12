using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    public EffectManager() { if (!Instance) Instance = this; }

    [SerializeField] ParticleSystem fogEffect;
    [SerializeField] Transform effectParent;
    Vector2[] effectPositions =
    {
        new Vector2(-780,350),
        new Vector2(-470,350),
        new Vector2(-155,350),
        new Vector2(155,350),
        new Vector2(470,350),
        new Vector2(780,350),
        new Vector2(-780,0),
        new Vector2(-470,0),
        new Vector2(-155,0),
        new Vector2(155,0),
        new Vector2(470,0),
        new Vector2(780,0),
        new Vector2(-780,-350),
        new Vector2(-470,-350),
        new Vector2(-155,-350),
        new Vector2(155,-350),
        new Vector2(470,-350),
        new Vector2(780,-350)
    };
    float positionDifferenceY = -100;

    public void InstantiateFogEffect(int _posx, int _posy)
    {
        var fog = Instantiate(fogEffect, effectParent);
        fog.transform.localPosition = effectPositions[6 * _posy + _posx];
        fog.transform.localPosition = new Vector2(fog.transform.localPosition.x, fog.transform.localPosition.y + positionDifferenceY);
        Destroy(fog, 10.0f);
    }
}
