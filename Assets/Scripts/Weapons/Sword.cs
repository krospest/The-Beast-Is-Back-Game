using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]

public class Sword : MonoBehaviour
{
    [SerializeField] private int _hitDamage = 5;
    public event EventHandler OnSwordSwing;
    private PolygonCollider2D _collider;
    private float _attackStartTime = 0.0f;
    private float _range = 3.7f;
    private void Awake()
    {
        _collider = GetComponent<PolygonCollider2D>();
    }
    private void Start()
    {
        TurnSwordColliderOff();
    }

    private void Update()
    {
        if (Time.time - _attackStartTime >= ActiveWeapon.Instance.attackAnimationDuration)
        {
            TurnSwordColliderOff();
        }
    }
    public void Attack()
    {
        TurnSwordColliderOn();
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
        _attackStartTime = Time.time;
    }

    public void TurnSwordColliderOff()
    {
        _collider.enabled = false;
    }
    private void TurnSwordColliderOn()
    {
        _collider.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            Debug.Log("hit");
            enemyEntity.TakeDamage(_hitDamage);
        }
    }



}
