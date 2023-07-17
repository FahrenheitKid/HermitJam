using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace HermitJam
{


    public class StageManager : MonoBehaviour
    {
        [Inject(Id = "FloorPool")] [SerializeField]
        private PlatformPool _floorPool;

        [Inject(Id = "CeilingPool")] [SerializeField]
        private PlatformPool _ceilingPool;

        [SerializeField] private float _timeScaleTickRate = 5f;
        [SerializeField] private float _timeScaleIncrease = 0.025f;
        [SerializeField] private int _score = 0;
        [SerializeField] private int _timeElapsed = 0;
        
        private Player _player;
        private UIManager _UIManager;
        ZenjectSceneLoader _sceneLoader;

        private PlatformReleaser m_platformReleaser;
        private Difficulty _mainDifficulty;
        private Difficulty m_DifficultyProgress = Difficulty.Easy;
        private Timer scoreTimer;
        private Timer timeTimer;

        [Inject]
        public void Construct(PlatformReleaser platformReleaser, Difficulty mainDifficulty, Player player, UIManager uiManager, ZenjectSceneLoader sceneLoader)
        {
            m_platformReleaser = platformReleaser;
            _mainDifficulty = mainDifficulty;
            _player = player;
            _UIManager = uiManager;
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            if (_player) _player.OnDeath += OnPlayerDeath;
        }

        // Start is called before the first frame update
        void Start()
        {
            SetupTimers();
        }

        void SetupTimers()
        {
            Timer.Register(_timeScaleTickRate, (() =>
            {
                Time.timeScale += _timeScaleIncrease;
            }), null, true, true);

            scoreTimer = Timer.Register(1f, () =>
            {
                _score += (int)(1 * Time.timeScale);
                _UIManager.UpdateScore(_score);
            }, null, true, true);
            
            timeTimer = Timer.Register(1f, () =>
            {
                _UIManager.UpdateTime(++_timeElapsed);
            }, null, true, true,true);
        }


        void OnPlayerDeath()
        {
            scoreTimer?.Pause();
            timeTimer?.Pause();
            
            _UIManager.ShowEndMenu();
        }

        private void OnDestroy()
        {
            if (_player) _player.OnDeath -= OnPlayerDeath;
        }

        public void RestartStage()
        {
            _sceneLoader.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
        }
        
        public void ReturnToMenu()
        {
            int countActive = SceneManager.sceneCount;
            int countBuild = SceneManager.sceneCountInBuildSettings;
            _sceneLoader.LoadScene(0,LoadSceneMode.Single);
        }
    }
}
