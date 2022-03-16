using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/*キー入力*/

public class GameEnd : MonoBehaviour
{
    bool isVisibleDebugCanvas = false;
    [SerializeField] GameObject debugCamvas;

    async UniTask OnEscape()
    {
        await UniTask.WaitUntil(() => Input.GetKey(KeyCode.Escape), cancellationToken: this.GetCancellationTokenOnDestroy());
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }
    async UniTask OnOpenDebugWindow()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.C), cancellationToken: this.GetCancellationTokenOnDestroy());
        isVisibleDebugCanvas = !isVisibleDebugCanvas;
        if (debugCamvas) debugCamvas.SetActive(isVisibleDebugCanvas);

        OnOpenDebugWindow().Forget();
    }

    void Start()
    {
        if (debugCamvas) debugCamvas.SetActive(isVisibleDebugCanvas);

        OnEscape().Forget();
        OnOpenDebugWindow().Forget();
    }
}
