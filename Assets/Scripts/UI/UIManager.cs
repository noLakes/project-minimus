using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //EventManager.AddListener("PauseGame", OnPauseGame);
        //EventManager.AddListener("ResumeGame", OnResumeGame);
    }

    private void OnDisable()
    {
        //EventManager.RemoveListener("PauseGame", OnPauseGame);
        //EventManager.RemoveListener("ResumeGame", OnResumeGame);
    }
}
