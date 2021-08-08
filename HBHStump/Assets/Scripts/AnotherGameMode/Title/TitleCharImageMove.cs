using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/*
タイトル画面
キャラクターたちが動き続ける
*/

public class TitleCharImageMove : MonoBehaviour
{
    //各キャラクターのイメージサンプル
    [SerializeField] Sprite[] ImageSample;

    //各キャラクターのゲームオブジェクト
    GameObject[] OneObject;
    Image[] OneObjectImage;

    //各キャラクターを全てキャッシュ
    void GetAllObject()
    {
        OneObject = new GameObject[transform.childCount];
        OneObjectImage = new Image[transform.childCount];

        for(int i = 0; i < OneObject.Length; i++)
        {
            OneObject[i] = transform.GetChild(i).gameObject;
            Debug.Log(i + " " + OneObject[i].GetComponent<Image>());
            OneObjectImage[i] = OneObject[i].GetComponent<Image>();
        }
    }
    //最初に全てのキャラクターオブジェクトにSpriteをセットする
    void StartSetSprite()
    {
        for(int i = 0; i < OneObjectImage.Length; i++)
        {
            SetSprite(OneObjectImage[i]);
        }
    }

    //キャラクターオブジェクトにSpriteをセットする
    void SetSprite(Image charObj) {
        charObj.sprite = ImageSample[Random.Range(0, ImageSample.Length)];
    }


    //キャラクターオブジェクトをまとめた親を動かす
    public IEnumerator TitleCharacterMove()
    {
        while (true)
        {
            float moveTime = 3.0f;
            transform.DOLocalMove(new Vector2(transform.localPosition.x - 300, transform.localPosition.y - 300), moveTime).SetEase(Ease.Linear);
            yield return new WaitForSeconds(moveTime);

            //左下に言ったオブジェクトを右上に移動させる
            for (int i = 0; i < OneObject.Length; i++)
            {
                if (OneObject[i].transform.position.y == -300)
                {
                    OneObject[i].transform.position = new Vector2(OneObject[i].transform.position.x + 600, 1500);
                }
                else if (OneObject[i].transform.position.x == -240)
                {
                    OneObject[i].transform.position = new Vector2(2160, OneObject[i].transform.position.y + 300);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetAllObject();
        StartSetSprite();
        StartCoroutine(TitleCharacterMove());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
