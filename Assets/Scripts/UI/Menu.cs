using System.Collections;
using System.Collections.Generic;
using BHSCamp;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private bool _enabled;
    private Canvas _canvas;


    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _respawnButton;
    [SerializeField] private Health _playerHealth;
    public bool IsLastLevel;

    [Header("Button texts")]
    [SerializeField] private TMP_Text _nextButtonText;
    [SerializeField] private string _nextButtonNextText;
    [SerializeField] private string _nextButtonExitText;
    private Dictionary<string, GameObject> _menuLists;

    private bool _toNext;

    private void Awake() {
        _canvas = GetComponentInChildren<Canvas>();
    }
    
    void Start()
    {
        if (IsLastLevel)
            _nextButtonText.text = _nextButtonExitText;
        else
            _nextButtonText.text = _nextButtonNextText;
        
        _playerHealth.OnDeath += OnDeath;
        _toNext = false;
        Reset();
    }

    private void OnDisable() {
        _playerHealth.OnDeath -= OnDeath;
    }

    private void OnDeath() {
        _enabled = true;
        _toNext = true;
        _respawnButton.SetActive(true);
        _menu.SetActive(false);
        UpdateMenuStatus();
        Time.timeScale = 1;
    }

    private void Reset() {
        _enabled = false;
        _settingsMenu.SetActive(false);
        _menu.SetActive(true);
        _volumeSlider.value = AudioListener.volume;
        UpdateMenuStatus();
    }

    void Update()
    {
        if (_toNext) return;

        if (Input.GetButtonDown("Cancel")) {
            _enabled = !_enabled;
            if (!_enabled) Reset();
            UpdateMenuStatus();
        }
        
        AudioListener.volume = _volumeSlider.value;
    }

    new public void SendMessage(string msg) {
        if (msg == "respawn") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (_toNext) return;

        if (msg == "resume") {
            _enabled = false;
            Reset();
            UpdateMenuStatus();
        }

        if (msg == "settings") {
            _settingsMenu.SetActive(true);
            _menu.SetActive(false);
        }

        if (msg == "menu") {
            _settingsMenu.SetActive(false);
            _menu.SetActive(true);
        }

        if (msg == "exit") {
            SceneManager.LoadScene(0);
        }

        if (msg == "next") {
            _enabled = true;
            _toNext = true;
            _nextButton.SetActive(true);
            _menu.SetActive(false);
            UpdateMenuStatus();
        }

    }
    
    private void UpdateMenuStatus() {
        _canvas.enabled = _enabled;
        if (_enabled) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
    }

    public void NextLevel() {
        if (!IsLastLevel)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(0);
    }
}
