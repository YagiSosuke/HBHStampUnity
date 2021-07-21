using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*シーン遷移時、フェードするスクリプト*/

public class SceneFadeScript : MonoBehaviour
{
    [SerializeField] Fade fade = null;
    float FadeTime = 3;
    [SerializeField] bool OpenSceneF = false;

    [SerializeField] GameObject BlackPanel;

    
    // Start is called before the first frame update
    void Start()
    {
        BlackPanel.SetActive(true);

        if (OpenSceneF)
        {
            fade.FadeIn(0, () =>
            {
                BlackPanel.SetActive(false);
                fade.FadeOut(FadeTime);
            });
        }
        else
        {
            fade.FadeIn(FadeTime);
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void FadeIn()
    {
        fade.FadeIn(FadeTime);
    }


    public void FadeOut()
    {
        fade.FadeOut(FadeTime);
    }
}
