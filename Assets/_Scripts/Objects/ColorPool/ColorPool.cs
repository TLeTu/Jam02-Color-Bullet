using System;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(CircleCollider2D))]
public class ColorPool : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private CircleCollider2D _circleCollider2D;
    [Header("Data")]
    [SerializeField] private PlayerColor _playerColor;
    [SerializeField] private float _collisionRadius;
    [SerializeField] private bool _isUsing;

    private CountdownTimer _existTimer;


    public Action<ColorPool> DespawnAction;
    public PlayerColor PlayerColor => _playerColor;



    private void Reset()
    {
        SetupDefault();
    }

    private void Awake()
    {
        SetupDefault();
        _existTimer = new CountdownTimer(0);
        _existTimer.Stop();
    }

    private void Update()
    {
        _existTimer.Tick(Time.deltaTime);

        if (_existTimer.IsFinished)
        {
            _spriteRenderer.color = Color.white;
            DespawnAction?.Invoke(this);
            _existTimer.Stop();
        }

        if(_existTimer.IsRunning && _existTimer.Progress <= 0.2f)
        {
            _spriteRenderer.color = new Color(1, 1, 1, Mathf.PingPong(Time.time * 10, 1));
        }

    }

    private void SetupDefault()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    public void Initialize()
    {
    }

    public void SetupPool(PlayerColor playerColor, ColorPoolSpawner spawner)
    {
        SetColor(playerColor);
        SetCollisionRadius(spawner.CollisionRadius);
        _animator.runtimeAnimatorController = spawner.GetColorAnimator(playerColor);

        _existTimer.Reset(spawner.ExistTime);
        _existTimer.Start();

        _isUsing = false;
        _animator.CrossFade("Explode", 0f);
    }

    private void SetColor(PlayerColor playerColor)
    {
        _playerColor = playerColor;
        transform.name = "Pool - " + _playerColor.ToString();
    }

    private void SetCollisionRadius(float collisionRadius)
    {
        _collisionRadius = collisionRadius;
        if (_circleCollider2D != null ) _circleCollider2D.radius = collisionRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isUsing) return;

        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.ChangeWeapon(_playerColor);
            _isUsing = true;
        }

        _existTimer.SetToProgress(0.2f);
    }
}
