using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Utilities;

public class EnemySpawner : PoolerBase<EnemyController>
{
    [Header("Spawner Settings")]
    [SerializeField] private Transform topleftLimit;
    [SerializeField] private Transform bottomdownLimit;

    [SerializeField] private float _waveCooldown = 1f;
    [SerializeField][UnityEngine.Range(5,100)] private int _waveSize = 5;
    [SerializeField] private int _waveCount = 0;
    [Header("Close Range Enemy Settings")]
    [SerializeField] private float _closeRangeSpeed = 2f;
    [SerializeField] private float _closeRangeDamage = 2f;
    [SerializeField] private float _closeRangeAtkRange = 1f;
    [SerializeField] private float _closeRangeAtkDuration = 1f;
    [SerializeField] private float _closeRangeAtkCooldown = 1f;
    [SerializeField] private float _closeRangeHealth = 1f;
    [Header("Ranged Enemy Settings")]
    [SerializeField] private float _longRangeSpeed = 2f;
    [SerializeField] private float _longRangeDamage = 2f;
    [SerializeField] private float _longRangeAtkRange = 5f;
    [SerializeField] private float _longRangeAtkDuration = 2f;
    [SerializeField] private float _longRangeAtkCooldown = 2f;
    [SerializeField] private float _longRangeHealth = 1f;
    [Header("Visual Enemy")]
    [SerializeField] private List<AnimatorController> _closeRangeAnimator;
    [SerializeField] private List<AnimatorController> _longRangeAnimator;

    #region Properties
    public float CloseRangeSpeed => _closeRangeSpeed;
    public float CloseRangeDamage => _closeRangeDamage;
    public float CloseRangeAtkRange => _closeRangeAtkRange;
    public float CloseRangeAtkCooldown => _closeRangeAtkCooldown;
    public float CloseRangeAtkDuration => _closeRangeAtkDuration;
    public float CloseRangeHealth => _closeRangeHealth;
    public float LongRangeSpeed => _longRangeSpeed;
    public float LongRangeDamage => _longRangeDamage;
    public float LongRangeAtkRange => _longRangeAtkRange;
    public float LongRangeAtkCooldown => _longRangeAtkCooldown;
    public float LongRangeAtkDuration => _longRangeAtkDuration;
    public float LongRangeHealth => _longRangeHealth;

    public AnimatorController RandomCloseRangeAnimator => _closeRangeAnimator[UnityEngine.Random.Range(0, _closeRangeAnimator.Count)];
    public AnimatorController RandomLongRangeAnimator => _longRangeAnimator[UnityEngine.Random.Range(0, _longRangeAnimator.Count)];
    #endregion

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

            // Randomize the position of the enemy within the spawn range

            Vector2 spawnPos = new Vector2(
                UnityEngine.Random.Range(topleftLimit.position.x, bottomdownLimit.position.x), 
                UnityEngine.Random.Range(topleftLimit.position.y, bottomdownLimit.position.y));


            enemy.transform.position = spawnPos;

            enemy.Initialize(EnemyType.LongRange, this);

            enemy.gameObject.SetActive(true);
        }
    }
}
