using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/*キャラクタの名前を自動でセットする*/

public class CharctorNameSet : MonoBehaviour
{
    GameObject flamePrefab;        //フレーム表示領域
    GameObject flame;        //フレームの実体

    GameObject flameChildPrefab;   //フレーム1つ
    GameObject flameChild;   //フレーム1つの実体

    GameObject newFlame;        //追加した文字のフレーム
    
    float namePosY = 150;


    //スタンプ打たれたキャラの場合、文字追加アニメーションの再生
    async UniTask AddFlameAnimation()
    {
        if ((gameObject.tag == "Head" || gameObject.tag == "Body" || gameObject.tag == "Hip"))
        {
            newFlame.transform.localScale = Vector3.zero;
            
            newFlame.transform.localScale = Vector2.one * 3;
            newFlame.transform.DOScale(Vector2.one, 1.0f).SetEase(Ease.InCubic);
            await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());
            
            flame.GetComponent<AudioSource>().Play();
        }
    }

    void Start()
    {
        var objName = gameObject.name.Replace("Image_", "").Replace("(Clone)", "");

        flamePrefab = (GameObject)Resources.Load("Prefabs/WordFlame/WordFlame");
        flameChildPrefab = (GameObject)Resources.Load("Prefabs/WordFlame/Flame");

        //枠組みを形成
        flame = Instantiate(flamePrefab, new Vector3(transform.position.x, transform.position.y+ namePosY, transform.position.z), Quaternion.identity, this.gameObject.transform.parent.gameObject.transform);
        flame.transform.localPosition = new Vector3(0, namePosY, transform.position.z);
        flame.GetComponent<RectTransform>().sizeDelta = new Vector2((objName.Length) * 100, 100);        //枠のサイズを決定

        //追加された文字枠についての処理
        for(int i = 0; i < objName.Length; i++)
        {
            flameChild = Instantiate(flameChildPrefab, flame.transform.position, Quaternion.identity, flame.transform);
            flameChild.transform.GetChild(1).GetComponent<Text>().text = objName.Substring(i, 1);

            if ((i == 0 && (gameObject.tag == "Head" || gameObject.tag == "HeadSample")) ||
                (i == 1 && (gameObject.tag == "Body" || gameObject.tag == "BodySample")) ||
                (i == 2 && (gameObject.tag == "Hip" || gameObject.tag == "HipSample")))
            {
                newFlame = flameChild;
                flameChild.GetComponent<Image>().color = Color.green;

                AddFlameAnimation().Forget();
            }
        }
    }
}
