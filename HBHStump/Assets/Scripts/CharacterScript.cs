using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/*キャラクターにスタンプ打った時のスクリプト（辞書）*/


public class CharacterScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] BeforeChangeCharacter beforeChangeCharacter;
    List<CharaData> changeCharaDatas;
      
    [SerializeField] GameObject fogPrefab;

    MasterData MasterData => MasterData.Instance;
    CharacterController CharacterController => CharacterController.Instance;


    void Initialize()
    {
        changeCharaDatas = CharaCsvLoader.Instance.afterChangeCharaDatas[beforeChangeCharacter.CharaName];
    }
    
    void OnCharacterClick()
    {
        foreach (CharaData changeData in changeCharaDatas)
        {
            if (Stamp.Instance.Word == changeData.GetAddedWord() && Stamp.Instance.Parts == changeData.Parts)
            {
                ChangeCharacter(changeData);

                //プレイデータ保存
                if (MasterData.recordPlayData.enabled)
                {
                    var beforeName = beforeChangeCharacter.CharaName;
                    var afterName = changeData.CharaName;
                    var partsName = Stamp.Instance.Parts.ToString();

                    MasterData.recordPlayData.WriteChangeData(beforeName, afterName, partsName);
                }
            }
        }
    }
    void ChangeCharacter(CharaData changeData)
    {
        Debug.Log("Coordinate = " + beforeChangeCharacter.PosX + "," + beforeChangeCharacter.PosY);

        //煙を出す
        EffectManager.Instance.InstantiateFogEffect(beforeChangeCharacter.PosX, beforeChangeCharacter.PosY);

        beforeChangeCharacter.ObjChange(changeData).Forget();

        MasterData.AddScore(changeData.Sprite);
    }
    
    void Start()
    {
        Initialize();
    }
    void Update()
    {
        //この位置にスタンプが押されたとき
        if (Serial.PushF[beforeChangeCharacter.PosX, beforeChangeCharacter.PosY])
        {
            OnCharacterClick();
        }
    }
    public void OnPointerClick(PointerEventData pointerData)

    {
        OnCharacterClick();
    }
}