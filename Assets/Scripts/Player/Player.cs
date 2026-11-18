using System;
using UnityEngine;
[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerTakeDamage;
    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private float _maxHealth = 50;
    private float _currentHealth = 0;
    private Rigidbody2D rb;

    
    private float _pAttackTime = 0;

    private float _minMovingSpeed = 0.1f;
    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += Gameinput_OnPlayerAttack;
        _currentHealth = _maxHealth;
    }

  

    private void FixedUpdate()
    {
        HandleMovement();
    }
    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
    public bool IsRunning()
    {
        return isRunning;
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (Time.fixedDeltaTime * _movingSpeed));

        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
    private void Gameinput_OnPlayerAttack(object sender, EventArgs e)
    {

        if (Time.time - _pAttackTime < ActiveWeapon.Instance.attackAnimationDuration) { }
        else
        {
            ActiveWeapon.Instance.GetActiveWeapon().Attack();
            _pAttackTime = Time.time;
        }
    }
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnPlayerTakeDamage?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }
    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}
