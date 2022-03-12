using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2Controller : MonoBehaviour
{
    [SerializeField] CharacterController characterController;

    void Initialize()
    {
        CharaCsvLoader.Instance.Initialize();
        characterController.Initialize();
    }

    void Start()
    {
        Initialize();
    }
}
