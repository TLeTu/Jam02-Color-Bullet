using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EnemySpawner : PoolerBase<EnemyController>
{
    [Header("Spawner Settings")]
    [SerializeField] private float _spawnRange = 5f;
    [SerializeField] private float _waveCooldown = 1f;
    [SerializeField][UnityEngine.Range(5,100)] private int _waveSize = 5;
    [SerializeField] private int _waveCount = 0;
    [Header("Close Range Enemy Settings")]
    [SerializeField] private float _closeRangeSpeed = 2f;
    [SerializeField] private float _closeRangeAtkRange = 1f;
    [SerializeField] private float _closeRangeAtkCooldown = 1f;
    [SerializeField] private float _closeRangeHealth = 1f;
    [Header("Ranged Enemy Settings")]
    [SerializeField] private float _longRangeSpeed = 2f;
    [SerializeField] private float _longRangeAtkRange = 5f;
    [SerializeField] private float _longRangeAtkCooldown = 2f;
    [SerializeField] private float _longRangeHealth = 1f;
    [Header("Visual Enemy")]
    [SerializeField] private List<AnimatorControllerParameter> _closeRangeAnimator;
    [SerializeField] private List<AnimatorControllerParameter> _longRangeAnimator;

    CountdownTimer _waveTimer;

    int _waveIndex = 0;

    private void Start()
    {
        _waveTimer = new CountdownTimer(_waveCooldown);
        _waveTimer.Stop();
    }

    private void Update()
    {
        _waveTimer.Tick(Time.deltaTime);

        if (_waveTimer.IsFinished || !_waveTimer.IsRunning)
        {
            _waveTimer.Reset();
            _waveTimer.Start();

            SpawnWave();
        }
    }

    protected override void GetSetup(EnemyController obj)
    {
        obj.DespawnAction += Despawn;
    }

    protected override void Initialize(EnemyController obj)
    {
        obj.Initialize(obj.Type, this);
    }

    private void Despawn(EnemyController obj)
    {
        Return(obj);
    }

    public void SpawnWave()
    {
        if (_waveIndex >= _waveCount)
        {
            return;
        }

        _waveIndex++;

        for (int i = 0; i < _waveSize; i++)
        {
            var enemy = Get();
            enemy.transform.position = transform.position + Random.insideUnitSphere * _spawnRange;
            enemy.gameObject.SetActive(true);
        }
    }

}
