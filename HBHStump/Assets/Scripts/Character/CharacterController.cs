using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController Instance;
    public CharacterController() { if (!Instance) Instance = this; }

    List<BeforeChangeCharacter> beforeChangeCharacters = new List<BeforeChangeCharacter>();
    [SerializeField]Transform parent;
    
    [Header("Prefabs")]
    [SerializeField] BeforeChangeCharacter beforeCharaPrefab;

    CharaCsvLoader CharaCsvLoader => CharaCsvLoader.Instance;


    public void Initialize()
    {
        beforeChangeCharacters.Clear();
        int[] charaIDs = new int[3] { -1, -1, -1 };

        for (int i = 0; i < 3; i++)
        {
            do
            {
                charaIDs[i] = Random.Range(0, CharaCsvLoader.beforeChangeCharaDatas.Count);
            } while ((i != 0 && charaIDs[i] == charaIDs[0]) ||
                     (i != 1 && charaIDs[i] == charaIDs[1]) ||
                     (i != 2 && charaIDs[i] == charaIDs[2]));


            beforeChangeCharacters.Add(Instantiate(beforeCharaPrefab, parent));
            beforeChangeCharacters[i].Initialize(i, CharaCsvLoader.beforeChangeCharaDatas[charaIDs[i]]);
        }
    }
    //TODO: 要改良
    public void CharaGenerate(int _generatePosY)
    {
        int _id;
        do
        {
            _id = Random.Range(0, CharaCsvLoader.beforeChangeCharaDatas.Count);
        } while (_id == beforeChangeCharacters[0].ID ||
                 _id == beforeChangeCharacters[1].ID ||
                 _id == beforeChangeCharacters[2].ID);

        beforeChangeCharacters[_generatePosY] = Instantiate(beforeCharaPrefab, parent);
        beforeChangeCharacters[_generatePosY].Initialize(_generatePosY, CharaCsvLoader.beforeChangeCharaDatas[_id]);
    }


    //他スクリプトで呼び出し用の変数
    public void GameSceneAfter()
    {
        Initialize();
    }
    public void GameSceneContinuation() { }
    public void GameSceneBefore()
    {
        foreach(var chara in beforeChangeCharacters)
        {
            chara.ObjShrink();
        }
    }
}
