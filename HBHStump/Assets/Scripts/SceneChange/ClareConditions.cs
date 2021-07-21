using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*クリア条件のスクリプト*/
/*全部が食べ物になったか判定*/

public class ClareConditions : MonoBehaviour
{
    public static int ChangeCharctor = 0;         //変化したキャラクター数
    [SerializeField] private int ChangeLimit;                //変化の最大値
    public static int ClareCharctor = 0;          //クリアのために変化したキャラクター
    public float count = 0;
    [SerializeField] SceneFadeScript FadeScript;

    [SerializeField] GameObject ClarePanel;     //クリア時パネル
    [SerializeField] Animation ClareAnim;                        //クリア時アニメーション

    [SerializeField] GameObject FailedPanel;     //クリア時パネル
    [SerializeField] Animation FailedAnim;                        //クリア時アニメーション

    // Start is called before the first frame update
    void Start()
    {
        ChangeCharctor = 0;
        ClareCharctor = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //全部クリアオブジェクトなら
        if (ClareCharctor == ChangeLimit)
        {
            ClarePanel.SetActive(true);
            ClareAnim.Play();

            if(count > 2.75 && count < 4)
            {
                ClarePanel.GetComponent<AudioSource>().Play();
                count = 4;
            }
            else if(count > 5 && count < 6)
            {
                FadeScript.FadeIn();
                count=6;
            }
            else if(count >= 10)
            {
                SceneManager.LoadScene("Menu");
            }


            count += Time.deltaTime;
        }
        //全部変換されたら
        else if(ChangeCharctor == ChangeLimit)
        {
            FailedPanel.SetActive(true);
            FailedAnim.Play();

            if (count > 2.75 && count < 4)
            {
                FailedPanel.GetComponent<AudioSource>().Play();
                count = 4;
            }
            else if (count > 5 && count < 6)
            {
                FadeScript.FadeIn();
                count = 6;
            }
            else if (count >= 10)
            {
                SceneManager.LoadScene("Menu");
            }


            count += Time.deltaTime;
        }
    }
}
