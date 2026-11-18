using UnityEngine;
using UnityEngine.AI;
using Adventure_kk.Utils;
using UnityEngine.InputSystem.XR.Haptics;
using System;
public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance { get; private set; }
    [SerializeField] private EnemyEntity _enemyEntity;
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 2f;
    [SerializeField] private float _roamingDistanceMin = 1f;
    [SerializeField] private float _roamingTimeMax = 2f;

    [SerializeField] private float _chasingDistance = 7f;
    [SerializeField] private float _attackingDistance = 4f;
    [SerializeField] private float _attackingRate = 2f;
    private float _nextAttackingTime = 0;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roamingTime;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;

    private float _nextCheckDirectionTime = 0f;
    private float _checkDiractionDuration = 0.1f;
    private Vector3 _previousPosition;

    public event EventHandler OnEnemyAttack;
    public event EventHandler OnEnemyDeath;



    private enum State //машина состояний
    {
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    private void Start()
    {
        _startingPosition = transform.position;
    }
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;
    }

    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();
    }

    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        Debug.Log("dead");
        _currentState = State.Death;
    }

    private void StateHandler()
    {
        switch (_currentState)
        {
            default:
            case State.Roaming:
                _roamingTime -= Time.deltaTime;
                if (_roamingTime < 0)
                {
                    Roaming();
                    _roamingTime = _roamingTimeMax;
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
        }
    }

    private void CheckCurrentState()  //логика смены состояний врага
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;

        if (distanceToPlayer <= _chasingDistance)
            {
                    newState = State.Chasing;
            } 
        
        if (distanceToPlayer <= _attackingDistance && _nextAttackingTime<=Time.time)
            {
                    newState = State.Attacking;
            }

        if(newState!= _currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
            }
            else if (newState == State.Roaming)
            {
                _roamingTime = 0f;
            }
            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath();
            }

            _currentState = newState;
        }


    }


    private void Roaming()
    {
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }

    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    private void AttackingTarget()
    {
        if (_enemyEntity.isAttacking)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty); //событие
            _nextAttackingTime = Time.time + _attackingRate;
            Debug.Log("Wolf attacks");
        }
    }


    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (_currentState == State.Attacking)
            {
                FlipNPC(transform.position, Player.Instance.transform.position);
            }
            else if (_currentState == State.Roaming)
            {
                FlipNPC(transform.position, _roamPosition);
            }
            else if (_currentState == State.Chasing)
            {
                FlipNPC(transform.position, Player.Instance.transform.position);
            }
            _previousPosition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDiractionDuration;
        }
    }
    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void FlipNPC(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
