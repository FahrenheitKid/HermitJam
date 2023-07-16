using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        private PlatformReleaser m_platformReleaser;
        private Difficulty _mainDifficulty;
        private Difficulty m_DifficultyProgress = Difficulty.Easy;

        [Inject]
        public void Construct(PlatformReleaser platformReleaser, Difficulty mainDifficulty)
        {
            m_platformReleaser = platformReleaser;
            _mainDifficulty = mainDifficulty;
        }

        // Start is called before the first frame update
        void Start()
        {
            Timer.Register(_timeScaleTickRate, (() =>
            {
                Time.timeScale += _timeScaleIncrease;
            }), null, true, true);

            m_platformReleaser.OnPlatformRelease += OnPlatformReleased;
            Debug.Log(_mainDifficulty);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnPlatformReleased(Platform platform)
        {

        }

        private void OnDisable()
        {
            m_platformReleaser.OnPlatformRelease -= OnPlatformReleased;
        }
    }
}
