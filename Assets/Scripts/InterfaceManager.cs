using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private Text progress;
    
    private static InterfaceManager instance;
    
    public static InterfaceManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<InterfaceManager>();
            return instance;
        }
    }
    
    public void OnRestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }

    public void SetProgress(string progress)
    {
        this.progress.text = progress;
    }
}
