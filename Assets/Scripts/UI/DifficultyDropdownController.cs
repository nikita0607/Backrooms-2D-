using System.Collections;
using System.Collections.Generic;
using BHSCamp;
using TMPro;
using UnityEngine;

public class DifficultyDropdownController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _difficultyDropdown;

    void Start() {
        InitializeDropdown();
        GameManager.OnDifficultyChanged += OnDifficultyChanged;
    }

    private void OnDestroy() {
        GameManager.OnDifficultyChanged -= OnDifficultyChanged;
    }

    private void InitializeDropdown() {
        _difficultyDropdown.ClearOptions();
        _difficultyDropdown.AddOptions(GameManager.Instance.DifficultyLevels);
        _difficultyDropdown.RefreshShownValue();

        _difficultyDropdown.value = GameManager.Instance.Difficulty;
    }

    private void OnDifficultyChanged(int difficultyLevel) {
        Debug.Log($"Difficulty changed to {difficultyLevel}");
        _difficultyDropdown.value = difficultyLevel;
    }

    public void OnDropdownChanged() {
        Debug.Log(_difficultyDropdown.value);
        GameManager.Instance.SetDifficulty(_difficultyDropdown.value);
    }
}
