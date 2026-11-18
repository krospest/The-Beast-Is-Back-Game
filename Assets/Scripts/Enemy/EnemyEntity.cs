using System;
using UnityEngine;
[RequireComponent (typeof(PolygonCollider2D))]
[SelectionBase]
public class EnemyEntity : MonoBehaviour
{
    public static EnemyEntity Instance { get; private set; }
    private EnemyAI _enemyAI;
    [SerializeField] private int _maxHealth = 10;
    private int _currentHealth;
    private PolygonCollider2D _polygonCollider;
    private BoxCollider2D _boxCollider;
    [SerializeField] private int _hitDamage = 2;
    public bool isAttacking = false;
    [SerializeField] private Player _player;
    public bool isDead = false;
    public event EventHandler OnEnemyTakeDamage;
    public event EventHandler OnEnemyDeath;

    private void Awake()
    {
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }
    private void Start()
    {
        _currentHealth = _maxHealth;
        _enemyAI.OnEnemyAttack += _enemyAI_OnAttack; ;
    }

    private void _enemyAI_OnAttack(object sender, EventArgs e)
    {
        _player.TakeDamage(_hitDamage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player)&& collision is BoxCollider2D)
        {
            isAttacking = true;
        }
    }
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnEnemyTakeDamage?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }
    public void TurnColliderOff()
    {
        _polygonCollider.enabled = false;
    }
    public void TurnColliderOn()
    {
        _polygonCollider.enabled = true;
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _boxCollider.enabled = false;
            _polygonCollider.enabled = false;
            _enemyAI.SetDeathState();
            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
        }
    }

}
