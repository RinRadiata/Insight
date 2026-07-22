using UnityEngine;
using UnityEngine.AI;

namespace VHS
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAI : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private Transform player;

        [Header("Components")]
        [SerializeField] private PatrolRoute patrolRoute;
        [SerializeField] private VisionSensor visionSensor;
        [SerializeField] private HearingSensor hearingSensor;

        [Header("Movement")]
        [SerializeField] private float patrolSpeed = 1.8f;
        [SerializeField] private float chaseSpeed = 3.5f;
        [SerializeField] private float stoppingDistance = 1.4f;
        [SerializeField] private float patrolPointReachDistance = 0.6f;

        [Header("State Timing")]
        [SerializeField] private float idleTime = 1f;
        [SerializeField] private float investigateWaitTime = 2f;
        [SerializeField] private float searchTime = 4f;

        [Header("Debug")]
        [SerializeField] private EnemyState currentState = EnemyState.Idle;

        private NavMeshAgent agent;
        private int patrolIndex;
        private float stateTimer;
        private Vector3 investigatePosition;
        private Vector3 lastKnownPosition;

        public EnemyState CurrentState => currentState;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            if (visionSensor == null)
                visionSensor = GetComponent<VisionSensor>();

            if (hearingSensor == null)
                hearingSensor = GetComponent<HearingSensor>();
        }

        private void Start()
        {
            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);

                if (playerObject != null)
                    player = playerObject.transform;
            }

            if (patrolRoute != null && patrolRoute.Count > 0)
                SetState(EnemyState.Patrol);
            else
                SetState(EnemyState.Idle);
        }

        private void Update()
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    UpdateIdle();
                    break;

                case EnemyState.Patrol:
                    UpdatePatrol();
                    break;

                case EnemyState.Investigate:
                    UpdateInvestigate();
                    break;

                case EnemyState.Search:
                    UpdateSearch();
                    break;

                case EnemyState.Chase:
                    UpdateChase();
                    break;

                case EnemyState.Attack:
                    UpdateAttack();
                    break;
            }
        }

        private void UpdateIdle()
        {
            if (TrySeePlayer())
            {
                SetState(EnemyState.Chase);
                return;
            }

            if (TryHearNoise(out Vector3 heardPosition))
            {
                investigatePosition = heardPosition;
                SetState(EnemyState.Investigate);
                return;
            }

            stateTimer += Time.deltaTime;

            if (stateTimer >= idleTime && patrolRoute != null && patrolRoute.Count > 0)
                SetState(EnemyState.Patrol);
        }

        private void UpdatePatrol()
        {
            if (TrySeePlayer())
            {
                SetState(EnemyState.Chase);
                return;
            }

            if (TryHearNoise(out Vector3 heardPosition))
            {
                investigatePosition = heardPosition;
                SetState(EnemyState.Investigate);
                return;
            }

            if (patrolRoute == null || patrolRoute.Count == 0)
                return;

            if (!agent.pathPending && agent.remainingDistance <= patrolPointReachDistance)
            {
                patrolIndex = (patrolIndex + 1) % patrolRoute.Count;
                GoToCurrentPatrolPoint();
            }
        }

        private void UpdateInvestigate()
        {
            if (TrySeePlayer())
            {
                SetState(EnemyState.Chase);
                return;
            }

            if (!agent.pathPending && agent.remainingDistance <= patrolPointReachDistance)
            {
                stateTimer += Time.deltaTime;

                if (stateTimer >= investigateWaitTime)
                    SetState(EnemyState.Search);
            }
        }

        private void UpdateSearch()
        {
            if (TrySeePlayer())
            {
                SetState(EnemyState.Chase);
                return;
            }

            if (TryHearNoise(out Vector3 heardPosition))
            {
                investigatePosition = heardPosition;
                SetState(EnemyState.Investigate);
                return;
            }

            stateTimer += Time.deltaTime;
            transform.Rotate(Vector3.up, 80f * Time.deltaTime);

            if (stateTimer >= searchTime)
                SetState(patrolRoute != null && patrolRoute.Count > 0 ? EnemyState.Patrol : EnemyState.Idle);
        }

        private void UpdateChase()
        {
            if (TrySeePlayer())
            {
                agent.SetDestination(lastKnownPosition);

                if (player != null && Vector3.Distance(transform.position, player.position) <= stoppingDistance)
                {
                    SetState(EnemyState.Attack);
                    return;
                }
            }
            else
            {
                agent.SetDestination(lastKnownPosition);

                if (!agent.pathPending && agent.remainingDistance <= patrolPointReachDistance)
                    SetState(EnemyState.Search);
            }
        }

        private void UpdateAttack()
        {
            agent.isStopped = true;
            Debug.Log("Player caught. Game Over");
        }

        private void SetState(EnemyState newState)
        {
            currentState = newState;
            stateTimer = 0f;

            switch (newState)
            {
                case EnemyState.Idle:
                    agent.isStopped = true;
                    break;

                case EnemyState.Patrol:
                    agent.isStopped = false;
                    agent.speed = patrolSpeed;
                    GoToCurrentPatrolPoint();
                    break;

                case EnemyState.Investigate:
                    agent.isStopped = false;
                    agent.speed = patrolSpeed;
                    agent.SetDestination(investigatePosition);
                    break;

                case EnemyState.Search:
                    agent.isStopped = true;
                    break;

                case EnemyState.Chase:
                    agent.isStopped = false;
                    agent.speed = chaseSpeed;

                    if (player != null)
                    {
                        lastKnownPosition = player.position;
                        agent.SetDestination(lastKnownPosition);
                    }

                    break;

                case EnemyState.Attack:
                    agent.isStopped = true;
                    break;
            }
        }

        private void GoToCurrentPatrolPoint()
        {
            if (patrolRoute == null || patrolRoute.Count == 0)
                return;

            Transform point = patrolRoute.GetPoint(patrolIndex);

            if (point != null)
                agent.SetDestination(point.position);
        }

        private bool TrySeePlayer()
        {
            if (player == null || visionSensor == null)
                return false;

            bool canSee = visionSensor.CanSeeTarget(player);

            if (canSee)
                lastKnownPosition = player.position;

            return canSee;
        }

        private bool TryHearNoise(out Vector3 heardPosition)
        {
            heardPosition = Vector3.zero;

            if (hearingSensor == null)
                return false;

            return hearingSensor.ConsumeHeardNoise(out heardPosition);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, stoppingDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastKnownPosition, 0.25f);
        }
    }
}