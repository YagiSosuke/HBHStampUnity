using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

/*キー入力*/

public class GameEnd : MonoBehaviour
{
    bool CanvasF = false;        //キャンバス2が見えているかのフラグ
    [SerializeField] GameObject Canvas2;
    

    void Start()
    {
        if (Canvas2) Canvas2.SetActive(CanvasF);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)){
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                  UnityEngine.Application.Quit();
#endif
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            CanvasF = !CanvasF;
            if(Canvas2) Canvas2.SetActive(CanvasF);
        }
    }
}
