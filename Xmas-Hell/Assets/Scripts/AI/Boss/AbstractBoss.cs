using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBoss : AbstractEntity
{
    [SerializeField]
    List<AbstractBossBehaviour> Behaviours;

    [SerializeField]
    private RuntimeAnimatorController BaseAnimatorController;

    // Life Bar 
    [SerializeField]
    private GameObject BossLifeBarPrefab;
    private BossLifeBar _bossLifeBar;

    public readonly EBoss BossType;

    protected int CurrentBehaviourIndex;
    protected int PreviousBehaviourIndex;

    private GameObject _player;
    private Animator _animator;

    private Vector2 _initialPosition;
    private float _initialSpeed;

    private bool _ready;

    // Shoot timer
    private bool _enableShootTimer = false;
    private float _shootTimer = 0f;
    private float _shootTimerTime = 0f;
    private Action _shootTimerCallback = null;

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

        _player = GameObject.FindGameObjectWithTag("Player");

        if (!_player)
            throw new Exception("No player found in this scene!");

        var bossLifeBarHolder = GameObject.FindGameObjectWithTag("BossLifeBarHolder");
        var lifeBar = Instantiate(BossLifeBarPrefab, bossLifeBarHolder.transform);
        _bossLifeBar = lifeBar.GetComponent<BossLifeBar>();
        _bossLifeBar.Initialize(this);
    }

    protected override void Start()
    {
        base.Start();

        _initialSpeed = Speed;

        var gameArea = base._gameManager.GameArea.GetWorldRect();

        _initialPosition = new Vector2(0, gameArea.yMax - (0.1f * gameArea.yMax) - SpriteSize.y / 2f);

        // Restrict the default random area to the top part of the screen
        var randomMovingArea = new Rect(
            gameArea.xMin + ((gameArea.width - (gameArea.width * 0.9f)) / 2f),
            gameArea.yMax - (gameArea.height * 0.2f),
            gameArea.width * 0.9f,
            gameArea.height * 0.2f
        );

        UpdateRandomMovingArea(new Vector4(0f, 0.95f, 1f, 0.98f));

        //var dot = Resources.Load("Debug/Dot");
        //Instantiate(dot, new Vector2(_randomMovingArea.x, _randomMovingArea.y), Quaternion.identity);
        //Instantiate(dot, new Vector2(_randomMovingArea.x + _randomMovingArea.width, _randomMovingArea.y + _randomMovingArea.height), Quaternion.identity);

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

        _animator.runtimeAnimatorController = BaseAnimatorController;

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

        _bossLifeBar.GetComponent<BossLifeBar>().Reset();
    }

    protected override void Update()
    {
        base.Update();

        if (!_ready)
        {
            if (TargetingPosition)
                return;

            Invincible = false;
            _ready = true;
        }

        UpdateTimers();
        UpdateBehaviour();
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
            if (CurrentBehaviourIndex >= Behaviours.Count)
            {
                Reset();
                return;
            }
            else
            {
                NextBehaviour();
            }
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
            CurrentBehaviourIndex++;
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
        _enableShootTimer = true;
        _shootTimerTime = time;
        _shootTimerCallback = callback;
    }

    public void StopShootTimer()
    {
        _enableShootTimer = false;
        _shootTimerTime = 0;
        _shootTimerCallback = null;
    }

    private void UpdateTimers()
    {
        // Shoot timer
        if (_enableShootTimer && _shootTimerCallback != null)
        {
            if (_shootTimer > 0)
                _shootTimer -= Time.deltaTime;
            else
            {
                _shootTimer = _shootTimerTime;
                _shootTimerCallback();
            }
        }
    }

    #region Player

    public Vector2 GetPlayerPosition()
    {
        return _player.transform.position;
    }

    public Vector2 GetPlayerDirection()
    {
        Vector2 playerPosition = GetPlayerPosition();
        Vector2 currentPosition = transform.position;
        var heading = currentPosition - playerPosition;
        var direction = heading / heading.magnitude;

        return direction;
    }

    public float GetPlayerDirectionAngle()
    {
        return MathHelper.DirectionToAngle(GetPlayerDirection());
    }

    #endregion
}
