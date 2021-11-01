using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/*
    ゲーム終了時、ヒントを表示するパネル 
*/

public class HintPanel : MonoBehaviour
{
    GameObject hintParent;
    [System.Serializable]
    class HintData {
        [SerializeField] string name;
        [SerializeField] GameObject sampleObj;
        [SerializeField] GameObject[] headHint;
        [SerializeField] GameObject[] bodyHint;
        [SerializeField] GameObject[] hipHint;
        //表示用のヒント
        GameObject sampleObjView;
        public List<GameObject> headHintView;
        List<GameObject> bodyHintView;
        List<GameObject> hipHintView;

        //ランダムでヒント要素を取得
        List<GameObject> instantiateHint(GameObject[] hintGoup)
        {
            List<GameObject> returnHint = new List<GameObject>();
            if(hintGoup.Length == 0)
            {
                return returnHint;
            }
            else if(hintGoup.Length == 1)
            {
                returnHint.Add(Instantiate(hintGoup[Random.Range(0, hintGoup.Length)]));

                return returnHint;
            }
            else
            {
                int tempNum1 = Random.Range(0, hintGoup.Length);
                int tempNum2;
                do
                {
                    tempNum2 = Random.Range(0, hintGoup.Length);
                } while (tempNum1 == tempNum2);
                returnHint.Add(Instantiate(hintGoup[tempNum1]));
                returnHint.Add(Instantiate(hintGoup[tempNum2]));

                return returnHint;
            }
        }


        
        //ヒント要素をパネルにセット
        public void HintSetup(GameObject parent)
        {
            //変化させるキャラクターを表示
            sampleObjView = Instantiate(sampleObj, Vector3.zero, Quaternion.identity, parent.transform);
            sampleObjView.transform.localPosition = Vector2.zero;

            //表示用のヒント
            headHintView = instantiateHint(headHint);
            bodyHintView = instantiateHint(bodyHint);
            hipHintView = instantiateHint(hipHint);

            //親を設定する、
            foreach(GameObject obj in headHintView)
            {
                obj.transform.SetParent(parent.transform);
            }
            foreach (GameObject obj in bodyHintView)
            {
                obj.transform.SetParent(parent.transform);
            }
            foreach (GameObject obj in hipHintView)
            {
                obj.transform.SetParent(parent.transform);
            }
            //位置を設定する
            if (headHintView.Count >= 1)
            {
                headHintView[0].transform.localPosition = new Vector2(-300, 200);
                if (headHintView.Count == 2)
                {
                    headHintView[1].transform.localPosition = new Vector2(300, 200);
                }
            }
            if (bodyHintView.Count >= 1)
            {
                bodyHintView[0].transform.localPosition = new Vector2(-550, -50);
                if (bodyHintView.Count == 2)
                {
                    bodyHintView[1].transform.localPosition = new Vector2(550, -50);
                }
            }
            if (hipHintView.Count >= 1)
            {
                hipHintView[0].transform.localPosition = new Vector2(300, -350);
                if (hipHintView.Count == 2)
                {
                    hipHintView[1].transform.localPosition = new Vector2(-300, -350);
                }
            }
            //コンポーネント外す
            sampleObjView.transform.GetChild(0).GetComponent<CharctorScript>().enabled = false;
            //タグを変更する
            foreach (GameObject obj in headHintView)
            {
                obj.tag = "HeadSample";
                obj.transform.GetChild(0).tag = "HeadSample";
            }
            foreach (GameObject obj in bodyHintView)
            {
                obj.tag = "BodySample";
                obj.transform.GetChild(0).tag = "BodySample";
            }
            foreach (GameObject obj in hipHintView)
            {
                obj.tag = "HipSample";
                obj.transform.GetChild(0).tag = "HipSample";
            }
        }        
        //ヒント要素を破棄する
        public void HintDestroy()
        {
            Destroy(sampleObjView);
            foreach(GameObject obj in headHintView)
            {
                Destroy(obj);
            }
            foreach (GameObject obj in bodyHintView)
            {
                Destroy(obj);
            }
            foreach (GameObject obj in hipHintView)
            {
                Destroy(obj);
            }
            headHintView.Clear();
            bodyHintView.Clear();
            hipHintView.Clear();
        }
    }
    [SerializeField] HintData[] hintData;
    int viewHintId = 0;

    CanvasGroup hintCanvasGroup;

    //終了時音声
    [SerializeField] AudioSource voiceAudio;
    [SerializeField] AudioSource seAudio;


    public async UniTask GameSceneAfter()
    {
        //パネルとヒントを表示する
        viewHintId = Random.Range(0, hintData.Length);
        hintData[viewHintId].HintSetup(hintParent);

        seAudio.Play();
        await hintCanvasGroup.DOFade(endValue: 1.0f, duration: 0.5f);
        voiceAudio.Play();
    }
    public void GameSceneContinuation()
    {

    }
    public async UniTask GameSceneBefore()
    {
        //パネルを消すときにヒントも消す
        hintData[viewHintId].HintDestroy();
        await hintCanvasGroup.DOFade(endValue: 0.0f, duration: 0.5f);
    }

    private void Start()
    {
        hintParent = GameObject.Find("HintObjParent");
        hintCanvasGroup = GameObject.Find("HintPanel").GetComponent<CanvasGroup>();
    }
}
