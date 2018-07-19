using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBoss : AbstractEntity
{
    [SerializeField]
    List<AbstractBossBehaviour> Behaviours;

    public readonly EBoss BossType;

    protected int CurrentBehaviourIndex;
    protected int PreviousBehaviourIndex;

    private GameObject _player;
    private Animator _animator;

    private Vector2 _initialPosition;
    private float _initialSpeed;

    private GameManager _gameManager;

    private bool _ready;

    // Shoot timer
    public bool EnableShootTimer = false;
    private float ShootTimer = 0f;
    public float ShootTimerTime = 0f;
    public Action ShootTimerCallback = null;

    public float InitialSpeed
    {
        get { return _initialSpeed; }
    }

    public Vector2 InitialPosition
    {
        get { return _initialPosition; }
    }

    public Animator Animator
    {
        get { return _animator; }
    }

    public GameManager GameManager
    {
        get { return _gameManager; }
    }

    public GameObject Player
    {
        get { return _player; }
    }

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponentInChildren<Animator>();

        if (!_animator)
            throw new Exception("No Animator found on this Boss!");

        _gameManager = GetComponentInParent<GameManager>();

        if (!_gameManager)
            throw new Exception("No GameManager found in this scene!");

        _player = GameObject.FindGameObjectWithTag("Player");

        if (!_player)
            throw new Exception("No player found in this scene!");
    }

    protected override void Start()
    {
        base.Start();

        _initialSpeed = Speed;

        var gameArea = _gameManager.GameArea.GetWorldRect();

        _initialPosition = new Vector2(0, gameArea.yMax - (0.1f * gameArea.yMax) - SpriteSize.y / 2f);

        _randomMovingArea = gameArea;

        // Restrict the default random area to the top part of the screen
        _randomMovingArea.height = gameArea.height * 0.2f;
        _randomMovingArea.y = gameArea.yMax - _randomMovingArea.height;
        _randomMovingArea.width = gameArea.width * 0.9f;
        _randomMovingArea.x = gameArea.xMin + ((gameArea.width - _randomMovingArea.width) / 2f);

        // Area position = bottom left corner
        _randomMovingArea.x += SpriteSize.x / 2f;
        _randomMovingArea.y += SpriteSize.y / 2f;
        // We substract the entire sprite size from the width and height 
        // of the area as we move it according to the half sprite's size
        _randomMovingArea.width -= SpriteSize.x;
        _randomMovingArea.height -= SpriteSize.y;

        var dot = Resources.Load("Debug/Dot");
        Instantiate(dot, new Vector2(_randomMovingArea.x, _randomMovingArea.y), Quaternion.identity);
        Instantiate(dot, new Vector2(_randomMovingArea.x + _randomMovingArea.width, _randomMovingArea.y + _randomMovingArea.height), Quaternion.identity);

        #region Initialize behaviour
        if (Behaviours.Count > 0)
        {
            // Make sure each behaviour have access to the linked boss
            foreach (var behaviour in Behaviours)
                behaviour.Initialize(this);
        }
        #endregion

        Reset();
    }

    private void Reset()
    {
        RestoreDefaultState();

        // Reset behaviours
        PreviousBehaviourIndex = -1;
        CurrentBehaviourIndex = 0;

        foreach (var behaviour in Behaviours)
            behaviour.Reset();

        Invincible = true;
        _ready = false;

        // Entrance "animation"
        transform.position = new Vector2(0, 15);
        MoveToInitialPosition(1, true);
    }

    protected override void RestoreDefaultState()
    {
        base.RestoreDefaultState();
    }

    protected override void Update()
    {
        base.Update();

        if (!_ready)
        {
            if (!(Mathf.Abs(Position.x - InitialPosition.x) < 0.5f &&
                Mathf.Abs(Position.y - InitialPosition.y) < 0.5f))
            {
                return;
            }

            Invincible = false;
            _ready = true;
        }

        UpdateBehaviour();

        UpdateTimers();
    }

    public void MoveToInitialPosition(float? time = null, bool force = false)
    {
        MoveTo(InitialPosition, time, force);
    }

    #region Behaviours

    private void UpdateBehaviour()
    {
        UpdateBehaviourIndex();

        if (CurrentBehaviourIndex != PreviousBehaviourIndex)
        {
            NextBehaviour();
        }

        if (Behaviours.Count > 0)
            Behaviours[CurrentBehaviourIndex].Step();

        PreviousBehaviourIndex = CurrentBehaviourIndex;
    }

    protected void UpdateBehaviourIndex()
    {
        if (Behaviours.Count == 0)
            return;

        if (Behaviours[CurrentBehaviourIndex].IsBehaviourEnded())
        {
            CurrentBehaviourIndex++;

            if (CurrentBehaviourIndex >= Behaviours.Count)
                Destroy(gameObject);
        }
    }

    private void NextBehaviour()
    {
        if (PreviousBehaviourIndex >= 0)
            Behaviours[PreviousBehaviourIndex].StopBehaviour();

        // TODO: Trigger signal to clear all bullets
        // TODO: Make sure we restore the initial boss state for transition
        RestoreDefaultState();

        if (Behaviours.Count > 0)
        {
            Behaviours[CurrentBehaviourIndex].StartBehaviour();
        }
    }

    #endregion

    public override void TakeDamage(float damage)
    {
        if (Invincible)
            return;

        base.TakeDamage(damage);
        
        if (Behaviours.Count > CurrentBehaviourIndex)
            Behaviours[CurrentBehaviourIndex].TakeDamage(damage);
    }

    public float GetLifePercentage()
    {
        if (Behaviours.Count > CurrentBehaviourIndex)
            return Behaviours[CurrentBehaviourIndex].GetLifePercentage();

        return 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: Use an enum for tag
        if (collision.gameObject.tag == "PlayerBullet")
        {
            var bullet = collision.gameObject.GetComponent<AbstractBullet>();

            if (bullet)
                TakeDamage(bullet.Damage);
        }
    }

    public void StartShootTimer(float time, Action callback)
    {
        EnableShootTimer = true;
        ShootTimerTime = time;
        ShootTimerCallback = callback;
    }

    public void StopShootTimer()
    {
        EnableShootTimer = false;
        ShootTimerTime = 0;
        ShootTimerCallback = null;
    }

    private void UpdateTimers()
    {
        // Shoot timer
        if (EnableShootTimer && ShootTimerCallback != null)
        {
            if (ShootTimer > 0)
                ShootTimer -= Time.deltaTime;
            else
            {
                ShootTimer = ShootTimerTime;
                ShootTimerCallback();
            }
        }
    }
}
