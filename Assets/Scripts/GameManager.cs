using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameInput _gameInput;
    [SerializeField] public GameObject _pauseMenu;

    private void Start()
    {
        _gameInput.PauseEvent += HandlePause;
        _gameInput.ResumeEvent += HandleResume;
    }

    private void HandleResume()
    {
        _pauseMenu.SetActive(false);
    }

    private void HandlePause()
    {
        _pauseMenu?.SetActive(true);
    }
}
