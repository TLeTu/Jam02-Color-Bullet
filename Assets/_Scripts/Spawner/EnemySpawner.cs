using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Utilities;

public class EnemySpawner : PoolerBase<EnemyController>
{
    [Header("SpawnPool")]
    [SerializeField] private ColorPoolSpawner _colorPoolSpawner;

    [Header("Spawner Settings")]
    [SerializeField] private Transform topleftMaxLimit;
    [SerializeField] private Transform topleftMinLimit;
    [SerializeField] private Transform bottomrightMaxLimit;
    [SerializeField] private Transform bottomrightMinLimit;

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
        _colorPoolSpawner.SpawnRadomPool(obj.transform.position);
        Return(obj);
    }

    public void SpawnWave()
    {
        if (_waveIndex >= _waveCount)
        {
            return;
        }

        _waveIndex++;
        //first 3 wave will only spawn close range enemy
        //next 5 wave will spawn both close range and long range enemy, but more close range enemy and increase the amount of long range enemy each wave
        // the rest will spawn both close range and long range enemy with equal amount
        int closeRangeCount = 0;
        int longRangeCount = 0;

        if (_waveIndex <= 3)
        {
            closeRangeCount = _waveSize;
        }
        else if (_waveIndex <= 8)
        {
            closeRangeCount = _waveSize - (_waveIndex - 3);
            longRangeCount = _waveIndex - 3;
        }
        else
        {
            closeRangeCount = _waveSize / 2;
            longRangeCount = _waveSize / 2;
        }

        for (int i = 0; i < closeRangeCount; i++)
        {
            var enemy = Get();
            enemy.transform.position = GetRandomPosition();
            enemy.Initialize(EnemyType.CloseRange, this);
            enemy.gameObject.SetActive(true);
        }

        for (int i = 0; i < longRangeCount; i++)
        {
            var enemy = Get();
            enemy.transform.position = GetRandomPosition();
            enemy.Initialize(EnemyType.LongRange, this);
            enemy.gameObject.SetActive(true);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 randomPos;

        while (true)
        {
            float x = UnityEngine.Random.Range(topleftMaxLimit.position.x, bottomrightMaxLimit.position.x);
            float y = UnityEngine.Random.Range(topleftMaxLimit.position.y, bottomrightMaxLimit.position.y);
            randomPos = new Vector2(x, y);

            if (randomPos.x < topleftMinLimit.position.x || randomPos.x > bottomrightMinLimit.position.x ||
                randomPos.y > topleftMinLimit.position.y || randomPos.y < bottomrightMinLimit.position.y)
            {
                break; 
            }
        }

        return randomPos;
    }
}
