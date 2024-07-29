using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using BHSCamp.UI;
using System.Collections.Generic;

namespace BHSCamp
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static event Action OnScoreChanged;
        public static event Action<int> OnDifficultyChanged;

        [SerializeField] private LevelPreviewData[] _levels;
        [SerializeField] public List<string> DifficultyLevels;
        [SerializeField] private int _defaultDifficultyLevel;
        private int _currentLevelIndex;
        

        public int Score
        {
            get { return _score; }
        }
        private int _score;

        public int Difficulty {
            get { return _difficulty; }
        }
        private int _difficulty;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return; 
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            _difficulty = SaveLoadSystem.GetDifficulty(_defaultDifficultyLevel);

            if (_difficulty >= DifficultyLevels.Count)
                SetDifficulty(DifficultyLevels.Count - 1);
            OnDifficultyChanged?.Invoke(_difficulty);

            Debug.Log($"Diff: {_difficulty}");
        }

        public void SetDifficulty(int difficulty)
        {
            if (difficulty < 0)
                throw new ArgumentOutOfRangeException("difficulty level can't be less than 0");
            if (difficulty >= DifficultyLevels.Count)
                throw new ArgumentOutOfRangeException($"difficulty level can't be greater than {DifficultyLevels.Count-1}");
            _difficulty = difficulty;
            SaveLoadSystem.SetDifficulty(_difficulty);
            OnDifficultyChanged?.Invoke(_difficulty);
        }

        public void AddScore(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(
                    $"Amount should be positive!: {gameObject.name}"
                );
            _score += amount;
            OnScoreChanged?.Invoke();
        }

        public void SetScore(int amount) {
            if (amount < 0)
                throw new ArgumentOutOfRangeException($"Amount can't be less than 0");
            _score = amount;
        }

        public void FinishCurrentLevel()
        {
            SceneManager.LoadScene(0); //Back to main menu
            OpenAccessToNextlevel();
        }

        private void OpenAccessToNextlevel()
        {
            if (_currentLevelIndex + 1 == _levels.Length)
                return;

            _levels[_currentLevelIndex + 1].IsAccesible = true;
        }

        public void SetLevelIndex(int newIndex)
        {
            _currentLevelIndex = newIndex;
        }
    }
}