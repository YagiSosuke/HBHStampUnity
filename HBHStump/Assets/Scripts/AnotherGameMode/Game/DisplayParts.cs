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
    MasterData masterdata;
    SceneControl sceneControl;
    
    //現在のパーツを表示する
    void DisplayPartsUpdate()
    {
        switch (StumpScript.TempStump) {
            case "頭":
                partsImage.sprite = partsSample[0];
                break;
            case "体":
                partsImage.sprite = partsSample[1];
                break;
            case "尻":
                partsImage.sprite = partsSample[2];
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        masterdata = GameObject.Find("GameControler").GetComponent<MasterData>();
        sceneControl = GameObject.Find("GameControler").GetComponent<SceneControl>();
    }

    // Update is called once per frame
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
