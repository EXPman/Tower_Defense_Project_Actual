using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject Quitting;
    [SerializeField] public GameObject LevelPanel;

    private void Start()
    {
        LevelPanel.SetActive(false);

    }

    public void loadLevelSelection()
    {
        StartButton.SetActive(false);
        Quitting.SetActive(false);
        LevelPanel.SetActive(true);
    }

    public void ReturnToPreviousUI()
    {
        StartButton.SetActive(true);
        Quitting.SetActive(true);
        LevelPanel.SetActive(false);
    }

}
