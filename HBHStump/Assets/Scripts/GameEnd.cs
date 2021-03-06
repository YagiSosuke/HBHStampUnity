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

    // Start is called before the first frame update
    void Start()
    {
        if (Canvas2) Canvas2.SetActive(CanvasF);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)){
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                  UnityEngine.Application.Quit();
#endif
        }
        else if (Input.GetKey(KeyCode.Backspace))
        {
            SceneManager.LoadScene("Menu");
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            CanvasF = !CanvasF;
            if(Canvas2) Canvas2.SetActive(CanvasF);
        }
    }
}
