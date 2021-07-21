using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndTitle : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                  UnityEngine.Application.Quit();
#endif
        }

    }
}
