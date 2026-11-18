using UnityEngine;
[RequireComponent(typeof(Animator))]
public class WolfVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    private Animator _animator;

    private const string IS_DEAD = "IsDead"; 
    private const string ATTACK = "Attack";
    private const string TAKE_DAMAGE = "TakeDamage";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnAttack;
        _enemyEntity.OnEnemyTakeDamage += _enemyEntity_OnTakeDamage;
        _enemyEntity.OnEnemyDeath += _enemyEntity_OnDeath;
    }


    public void TurnOffAttackAnimation()
    {
        _enemyEntity.TurnColliderOff();
    }
    public void TurnOnAttackAnimation()
    {   
        _enemyEntity.TurnColliderOn();
    }
    private void _enemyEntity_OnTakeDamage(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TAKE_DAMAGE);
    }
    private void _enemyAI_OnAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);

    }
    private void _enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnAttack;
        _animator.SetBool(IS_DEAD, true);

    }

}
