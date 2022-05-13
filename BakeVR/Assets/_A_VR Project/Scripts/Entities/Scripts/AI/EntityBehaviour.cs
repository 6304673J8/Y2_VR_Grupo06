using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class GuardEvents
{
    public UnityEvent OnStartPatrolling;
    public UnityEvent OnStartSuspicious;
    public UnityEvent OnStartInvestigate;
    public UnityEvent OnStartChasing;
    public UnityEvent OnNone;
    public UnityEvent OnCatch;
}

public class EntityBehaviour : MonoBehaviour
{
    [Header("Player Visibility Settings")]

    [Tooltip("The point from which the entity detection will start. Set to gameobject transform if none. ")]
    public Transform visibilitySource;
    [Tooltip("The player that the entity want to catch. Set to Main Camera if none.")]
    public Transform target;
    [Tooltip("Element which the entity  cannot see behind ")]
    public LayerMask Obstacles = 1;
    [Tooltip("Angle above which the entity  cannot detect. ")]
    public float viewAngle = 90;
    [Tooltip("Distance above which the entity cannot detect. ")]
    public float viewDistance = 10;

    [Header("Patrol Settings")]

    [Tooltip("Passing points for the entity while patrolling. Will loop these points.")]
    public Transform[] patrolPoints;
    [Tooltip("Walking speed of the entity while patrolling.")]
    public float patrolWalkSpeed = 0.8f;
    public float stoppingDistance = 0.3f;
    [Header("Suspicious Settings")]

    [Tooltip("The time before we start chasing/investigating after the entity sees the player. Increase the value to let a chance for the player to hide.")]
    public float suspiciousDelay = 0.5f;
    [Tooltip("Speed at which the entity turn to the suspicious source.")]
    public float suspiciousTurningSpeed = 3;
    [Tooltip("The distance under which the entity will investigate")]
    public float investigateDistance = 6;
    [Tooltip("The distance under which the entity will investigate")]
    public float chasingDistance = 2;

    [Header("Chase Settings")]

    [Tooltip("Walking speed of the entity while chasing.")]
    public float chaseWalkSpeed = 1.6f;
    [Tooltip("The time the entity wait after loosing its target before going back patrolling.")]
    public float waitAfterLost = 3;

    [Header("Catch Settings")]

    [Tooltip("Distance under which the entity has catched the player.")]
    public float catchDistance = 2;
    public bool stopOnReach = true;

    [Header("Debug Settings")]
    public bool drawDebug = true;

    [Header("Guard Events Settings")]
    public GuardEvents guardEvents;

    //Private References
    #region Private References
    private NavMeshAgent _navMeshAgent;

    private bool playerVisible = false;
    private Vector3 suspiciousSourcePosition;
    private int currentPatrolPointIndex = 0;

    private float suspiciousLevel;
    private float suspiciousTime = 0;
    private float additionalWaitTime;

    private Transform TargetReference { get => target ? target : Camera.main.transform; }
    private GuardStates guardState;
    public GuardStates GuardState { get { return guardState; } }
    #endregion

    public enum GuardStates
    {
        Patrolling,
        Suspicious,
        Investigating,
        Chasing,
        None
    }
    protected void Awake()
    {
        //Get navmesh reference.
        _navMeshAgent = GetComponent<NavMeshAgent>();

        //Set the visibility source to transform if none.
        if (!visibilitySource)
            visibilitySource = transform;

        //Initialize the patrolling state.
        guardState = GuardStates.Patrolling;
        MoveToNextPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (_navMeshAgent.enabled)
        {
            UpdateGuardState();
            UpdateNavMesh();
        }

        if (drawDebug)
            DrawDebug();
    }

    //Update the state of the guard according to the current state and if he sees or not the player.
    private void UpdateGuardState()
    {
        //Check player visibility
        playerVisible = IsPlayerVisible();

        //Check if we catched the target.
        if (playerVisible && GetDistanceFromTarget() < catchDistance)
        {
            Debug.Log("Target is reached");
            if (stopOnReach)
                guardState = GuardStates.None;
            guardEvents.OnCatch.Invoke();
            return;
        }

        //Get the current state of the guard.
        GuardStates previousGuardState = GuardState;

        switch (previousGuardState)
        {
            //If Patrolling and player is visible, we're getting suspicious
            case GuardStates.Patrolling:
                if (playerVisible)
                {
                    guardState = GuardStates.Suspicious;
                    suspiciousTime = 0;
                    additionalWaitTime = 0;
                }
                break;

            //If suspicious after a certain delay, according to the suspicious level we can go to Three States, Chase, investigate, Patrol.
            case GuardStates.Suspicious:
                //Delay before chasing / investigating
                if (suspiciousTime < suspiciousDelay)
                {
                    guardState = GuardStates.Suspicious;
                    suspiciousTime += Time.deltaTime;
                }
                else
                {
                    //Update the level of suspicious
                    guardState = ShouldInvestigate();
                }
                break;

            //If end investigate, we go back to suspicious, if during investigation we see something, we chase.
            case GuardStates.Investigating:
                if (playerVisible)
                    guardState = GuardStates.Chasing;
                else if (!playerVisible && HasReachedDestination())
                {
                    suspiciousTime = 0;
                    additionalWaitTime = waitAfterLost;
                    guardState = GuardStates.Suspicious;
                }
                else
                    guardState = GuardStates.Investigating;
                break;

            //If we lost the target, we go back to suspicious state and we wait "WaitAfterLost" seconds.
            case GuardStates.Chasing:
                if (playerVisible)
                    guardState = GuardStates.Chasing;
                else if (!playerVisible && HasReachedDestination())
                {
                    suspiciousTime = 0;
                    additionalWaitTime = waitAfterLost;
                    guardState = GuardStates.Suspicious;
                }
                break;

            default:
                guardState = GuardStates.None;
                break;
        }

        //New State Detected
        if (previousGuardState != GuardState)
        {
            RaiseEvent();
        }
    }

    //Update the navmesh depending on the state of the guard.
    void UpdateNavMesh()
    {
        switch (GuardState)
        {
            case GuardStates.Patrolling:
                StopNavMesh(false);
                _navMeshAgent.speed = patrolWalkSpeed;
                _navMeshAgent.stoppingDistance = stoppingDistance;
                if (HasReachedDestination())
                    MoveToNextPatrolPoint();
                break;

            case GuardStates.Suspicious:
                StopNavMesh(true);
                RotateNavmeshTowards(suspiciousSourcePosition);
                break;

            case GuardStates.Investigating:
                StopNavMesh(false);
                _navMeshAgent.speed = patrolWalkSpeed;
                _navMeshAgent.stoppingDistance = stoppingDistance;
                _navMeshAgent.SetDestination(suspiciousSourcePosition);
                break;

            case GuardStates.Chasing:
                StopNavMesh(false);
                _navMeshAgent.speed = chaseWalkSpeed;
                _navMeshAgent.stoppingDistance = stoppingDistance;
                _navMeshAgent.SetDestination(suspiciousSourcePosition);
                break;

            case GuardStates.None:
                StopNavMesh(true);
                break;

            default:
                break;
        }
    }

    //From the suspicious state, should the guard goes investigate, chase or go back patrolling ?
    private GuardStates ShouldInvestigate()
    {
        if (playerVisible)
        {
            return GuardStates.Chasing;
        }
        else
        {
            if (suspiciousTime < suspiciousDelay + additionalWaitTime)
            {
                suspiciousTime += Time.deltaTime;
                return GuardStates.Suspicious;
            }
            else
            {
                if (HasReachedDestination())
                    return GuardStates.Patrolling;

                float distance = GetDistanceFromSource();
                if (_navMeshAgent.stoppingDistance <= distance && distance < chasingDistance)
                    return GuardStates.Chasing;
                else if (_navMeshAgent.stoppingDistance <= distance && distance < investigateDistance)
                    return GuardStates.Investigating;
                else
                    return GuardStates.Patrolling;
            }
        }
    }

    //Distance from the SOURCE position, we dont take the "Y" value into account here, only the horizontal difference in position
    public float GetDistanceFromSource()
    {
        return Vector3.Distance(Vector3.ProjectOnPlane(visibilitySource.position, Vector3.up), Vector3.ProjectOnPlane(suspiciousSourcePosition, Vector3.up));
    }

    //Distance from the TARGET position, we dont take the "Y" value into account here, only the horizontal difference in position
    public float GetDistanceFromTarget()
    {
        return Vector3.Distance(Vector3.ProjectOnPlane(visibilitySource.position, Vector3.up), Vector3.ProjectOnPlane(TargetReference.position, Vector3.up));
    }

    public void EnableGuardBehavior(bool yes)
    {
        _navMeshAgent.enabled = yes;
        this.enabled = yes;
    }

    public void ForceInvestigation(Vector3 sourcePoint)
    {
        suspiciousSourcePosition = sourcePoint;
        additionalWaitTime = 0;
        guardState = ShouldInvestigate();
        RaiseEvent();
    }

    public void ForceChasing(Vector3 sourcePoint)
    {
        suspiciousSourcePosition = sourcePoint;
        additionalWaitTime = 0;
        guardState = GuardStates.Chasing;
        RaiseEvent();
    }

    public void RaiseEvent()
    {
        switch (GuardState)
        {
            case GuardStates.Patrolling:
                guardEvents.OnStartPatrolling.Invoke();
                break;
            case GuardStates.Suspicious:
                guardEvents.OnStartSuspicious.Invoke();
                break;
            case GuardStates.Investigating:
                guardEvents.OnStartInvestigate.Invoke();
                break;
            case GuardStates.Chasing:
                guardEvents.OnStartChasing.Invoke();
                break;
            case GuardStates.None:
                guardEvents.OnNone.Invoke();
                break;
            default:
                break;
        }
    }

    public void StopNavMesh(bool yes)
    {
        _navMeshAgent.isStopped = yes;
    }

    void RotateNavmeshTowards(Vector3 destination)
    {
        Vector3 newDirection = Vector3.ProjectOnPlane(destination - transform.position, Vector3.up).normalized;
        transform.forward = Vector3.RotateTowards(transform.forward, newDirection, suspiciousTurningSpeed * Time.deltaTime, 0);
    }

    //Check if the navmesh has reached its destination
    public bool HasReachedDestination()
    {
        return !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;
    }

    //Check if player is on sight
    bool IsPlayerVisible()
    {
        if (GetDistanceFromTarget() < viewDistance)
        {
            Vector3 directionToPlayer = (TargetReference.position - visibilitySource.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(visibilitySource.forward, directionToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(visibilitySource.position, TargetReference.position, Obstacles))
                {
                    suspiciousSourcePosition = TargetReference.position;
                    return true;
                }
            }
        }

        return false;
    }

    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length > 0)
        {
            _navMeshAgent.destination = patrolPoints[currentPatrolPointIndex].position;
            currentPatrolPointIndex++;
            currentPatrolPointIndex %= patrolPoints.Length;
        }
    }

    //DEBUG
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (drawDebug && suspiciousSourcePosition != null && !suspiciousSourcePosition.Equals(Vector3.zero))
        {
            Vector3 middle = (suspiciousSourcePosition + transform.position) / 2;
            Handles.Label(middle, GetDistanceFromSource().ToString("#.##"));
        }
    }
#endif
    private void DrawDebug()
    {
        Color debugColor = Color.red;
        Vector3 point1 = visibilitySource.position + Quaternion.Euler(0, viewAngle / 2, 0) * visibilitySource.forward * viewDistance;
        Vector3 point2 = visibilitySource.position + Quaternion.Euler(0, -viewAngle / 2, 0) * visibilitySource.forward * viewDistance;
        Vector3 point3 = visibilitySource.position + Quaternion.Euler(0, -viewAngle / 4, 0) * visibilitySource.forward * viewDistance;
        Vector3 point4 = visibilitySource.position + Quaternion.Euler(0, viewAngle / 4, 0) * visibilitySource.forward * viewDistance;
        Vector3 pointmiddle = visibilitySource.position + visibilitySource.forward * viewDistance;

        Debug.DrawLine(visibilitySource.position, point1, debugColor);
        Debug.DrawLine(visibilitySource.position, point2, debugColor);
        Debug.DrawLine(point3, pointmiddle, debugColor);
        Debug.DrawLine(point4, pointmiddle, debugColor);
        Debug.DrawLine(point1, point4, debugColor);
        Debug.DrawLine(point2, point3, debugColor);

        if (suspiciousSourcePosition != null && !suspiciousSourcePosition.Equals(Vector3.zero))
        {
            Debug.DrawLine(transform.position, suspiciousSourcePosition, Color.black);
        }
    }
}
