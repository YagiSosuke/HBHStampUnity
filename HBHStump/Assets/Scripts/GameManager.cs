using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CharacterController characterController;

    //仮
    [SerializeField] TitleCharImageMove title;


    void Initialize()
    {
        CharaCsvLoader.Instance.Initialize();

        //仮
        title.Initialize();
    }

    void Start()
    {
        Initialize();
    }
}
