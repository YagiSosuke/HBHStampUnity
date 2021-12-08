using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*タイトル画面*/

public class Title : MonoBehaviour
{
    SceneFadeScript fade;
    bool fadeF = false;

    float count = 0;

    void Start() {
        fade = GameObject.Find("FadeCanvas").GetComponent<SceneFadeScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            fade.FadeIn();
            fadeF = true;
        }
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                if(Serial.PushF[j, i])
                {
                    fade.FadeIn();
                    fadeF = true;
                    Serial.PushF[j, i] = false;
                }
            }
        }
        if(fadeF)
        {
            count += Time.deltaTime;
            if(count >= 4)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
