using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TextMeshProUGUI _completeText;
    [SerializeField] private GameObject _completePanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        _completePanel.SetActive(true);
        _playButton.onClick.AddListener(OnPlayButtonPressed);
        _exitButton.onClick.AddListener(OnExitButtonPressed);
        Time.timeScale = 0;
    }

    private void OnPlayButtonPressed()
    {
        GameManager.Instance.CurrentState = GameState.Playing;
        PlayerController.instance.StartGame();
        _completePanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void UpdateCompletePanel(string message, Color color)
    {
        GameManager.Instance.CurrentState = GameState.GameOver;
        _completePanel.SetActive(true);
        _completeText.text = message;
        _completeText.color = color;
    }

    public void OnWin()
    {
        UpdateCompletePanel("Win", Color.green);
    }

    public void OnLose()
    {
        UpdateCompletePanel("Lose", Color.red);
    }

    private void OnExitButtonPressed()
    {
        Application.Quit();
    }
}