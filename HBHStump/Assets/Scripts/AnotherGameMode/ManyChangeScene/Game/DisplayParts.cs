using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/*
    ゲーム開始時、設定されているパーツを表示する 
*/


public class DisplayParts : MonoBehaviour
{
    [SerializeField] Image partsImage;
    [SerializeField] List<Sprite> partsSample = new List<Sprite>();
    SceneControl sceneControl;
    
    //現在のパーツを表示する
    void DisplayPartsUpdate()
    {
        switch (Stamp.Instance.Parts) {
            case Parts.Head:
                partsImage.sprite = partsSample[0];
                break;
            case Parts.Body:
                partsImage.sprite = partsSample[1];
                break;
            case Parts.Hip:
                partsImage.sprite = partsSample[2];
                break;
        }
    }


    void Start()
    {
        sceneControl = GameObject.Find("GameControler").GetComponent<SceneControl>();
    }
    
    void Update()
    {
        if (sceneControl.screenMode == SceneControl.ScreenMode.GameSetting)
        {
            DisplayPartsUpdate();

            if (sceneControl.transitionMode == SceneControl.TransitionMode.afterSwitching)
            {
                transform.DOLocalMoveY(-40, 0.5f).SetEase(Ease.OutCubic);
            }
        }
        if (sceneControl.screenMode == SceneControl.ScreenMode.Game)
        {
            DisplayPartsUpdate();
            
            if (sceneControl.transitionMode == SceneControl.TransitionMode.beforeSwitching)
            {
                transform.DOLocalMoveY(150, 0.5f).SetEase(Ease.OutCubic);
            }
        }
    }
}
