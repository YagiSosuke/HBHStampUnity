using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    GameObject Stamp;
    string sceneName = null;

    SceneFadeScript fade;
    float count = 0;

    void Start()
    {
        fade = GameObject.Find("FadeCanvas").GetComponent<SceneFadeScript>();
        Stamp = GameObject.Find("Stamp");
    }

    void Update()
    {
        if (Serial.PushF[1, 0] || Serial.PushF[2, 0])
        {
            sceneName = "Festival";
            fade.FadeIn();
        }
        else if (Serial.PushF[0, 1] || Serial.PushF[1, 1])
        {
            sceneName = "Toy";
            fade.FadeIn();
        }
        else if (Serial.PushF[2, 1] || Serial.PushF[3, 1])
        {
            sceneName = "DiningRoom";
            fade.FadeIn();
        }
        else if (Serial.PushF[3, 0] || Serial.PushF[4, 0])
        {
            sceneName = "Zoo";
            fade.FadeIn();
        }

        if (sceneName != null)
        {
            count += Time.deltaTime;
            if(count >= 4)
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Serial.PushF[i, j] = false;
            }
        }
    }

    //マウスがステージに入った
    public void EnterStage()
    {
        Stamp.transform.localScale = new Vector2(1.5f, 1.5f);
    }

    //マウスがステージから出た
    public void ExitStage()
    {
        Stamp.transform.localScale = new Vector2(1f, 1f);
    }

    public void GoFes()
    {
        sceneName = "Festival";
        fade.FadeIn();
    }

    public void GoToy()
    {
        sceneName = "Toy";
        fade.FadeIn();
    }

    public void GoFood()
    {
        sceneName = "DiningRoom";
        fade.FadeIn();
    }

    public void GoZoo()
    {
        sceneName = "Zoo";
        fade.FadeIn();
    }
}
