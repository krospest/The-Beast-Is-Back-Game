using Unity.VisualScripting;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Player _player;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";
    private const string TAKE_DAMAGE = "TakeDamage";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        _animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        FlipCharacter();
    }
    private void Start()
    {
        _player.OnPlayerDeath += _player_OnDeath;
        _player.OnPlayerTakeDamage += _player_OnTakeDamage; ;
    }

    private void _player_OnTakeDamage(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TAKE_DAMAGE);
    }

    private void _player_OnDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_DEAD, true);
    }

    private void FlipCharacter()
    {
        if (GameInput.Instance.GetMovementVector().x == 0 && GameInput.Instance.GetMovementVector().y == 0)
        {
            return;
        }
        else if (GameInput.Instance.GetMovementVector().x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;   
        }
    }
}
