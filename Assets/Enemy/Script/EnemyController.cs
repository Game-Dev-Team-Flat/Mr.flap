using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public abstract class EnemyController : MonoBehaviour
    {
        private float m_detectedTime = 0f;
        private int m_currentNode = 0;
        private EnemyState m_enemyState = EnemyState.DetectingNothing;
        private GameObject m_targetObject;
        private EnemyInfo m_enemyInfo;
        private NavMeshAgent m_navMeshAgent;

        [SerializeField]
        protected Transform path;
        [Space(20)]

        [SerializeField]
        protected DetectTarget detectTarget;
        [SerializeField]
        protected float identifyingTime;
        protected NavMeshAgent navMeshAgent
        {
            get
            {
                if (m_navMeshAgent == null)
                {
                    m_navMeshAgent = GetComponent<NavMeshAgent>();
                }
                return m_navMeshAgent;
            }
        }
        protected Vector3 LastDetectedPosition;
        [Header("-Move Setting")]
        [SerializeField]
        protected float normalSpeed;
        [SerializeField]
        protected float runningSpeed;
        protected float detectedTime    // 0 이상 identifyingTime 이하로 제한
        {
            get => m_detectedTime;
            set
            {
                Mathf.Clamp(value, 0f, identifyingTime);
                m_detectedTime = value;
            }
        }
        protected int currentNode       // 0부터 path.childCount까지 순환
        {
            get => m_currentNode;
            set
            {
                if (value < 0)
                {
                    m_currentNode = path.childCount - 1;
                }
                else if (value >= path.childCount)
                {
                    m_currentNode = 0;
                }
                else
                {
                    m_currentNode = value;
                }
            }
        }
        public EnemyState enemyState => m_enemyState;
        public GameObject targetObject  // null은 담지 않음
        {
            get => m_targetObject;
            set
            {
                if (value != null)
                {
                    m_targetObject = value;
                }
            }
        }
        public EntityInfo enemyInfo
        {
            get
            {
                if (m_enemyInfo == null)
                {
                    m_enemyInfo = GetComponent<EnemyInfo>();
                }
                return m_enemyInfo;
            }
        }

        public enum EnemyState
        {
            DetectingNothing,
            DetectingSomething,
            DetectingTarget,
            MissingTargetSideways,
            TargetOverDetectingDistance,
            TargetHindInObject
        }

        protected virtual void Update()
        {
            if (!IsGroggy())
            {
                navMeshAgent.enabled = true;
                m_enemyState = DetermineState();
                Movement();
            }
            else
            {
                navMeshAgent.enabled = false;
            }
        }

        DetectTarget.DetectingState detectingState;
        protected EnemyState DetermineState()
        {
            detectingState = detectTarget.SearchTarget();

            if (detectingState == DetectTarget.DetectingState.DetectingTarget) // 발견되면
            {
                if (m_enemyState != EnemyState.DetectingTarget && detectedTime < identifyingTime)
                {
                    detectedTime += detectTarget.detectionDistance * Time.deltaTime;// / Vector3.Distance(transform.position, detectTarget.detectedObject.transform.position) * Time.deltaTime;
                    return EnemyState.DetectingSomething;
                }

                return EnemyState.DetectingTarget;
            }
            else // 발견이 안되면
            {
                if (m_enemyState == EnemyState.DetectingSomething && detectedTime > 0f)
                {
                    ReduceDetectedTime(0.5f);
                    return EnemyState.DetectingSomething;
                }
                else if (m_enemyState == EnemyState.DetectingTarget)
                {
                    if (Vector3.Distance(transform.position, targetObject.transform.position) < detectTarget.detectionDistance)
                    {
                        if (detectingState == DetectTarget.DetectingState.TargetInDetectingArea)
                        {
                            return EnemyState.TargetHindInObject;
                        }
                        else
                        {
                            return EnemyState.MissingTargetSideways;
                        }
                    }
                    else
                    {
                        return EnemyState.TargetOverDetectingDistance;
                    }
                }
                else if (m_enemyState == EnemyState.MissingTargetSideways)
                {
                    if (Vector3.Distance(transform.position, targetObject.transform.position) > detectTarget.detectionDistance) // 멀어지면 따라 갈 수 있게
                    {
                        return EnemyState.TargetOverDetectingDistance;
                    }

                    return m_enemyState;
                }
                else if ((m_enemyState == EnemyState.TargetHindInObject || m_enemyState == EnemyState.TargetOverDetectingDistance) && detectedTime > 0f)
                {
                    return m_enemyState;
                }
                else
                {
                    return EnemyState.DetectingNothing;
                }
            }
        }

        protected void Movement()
        {
            targetObject = detectTarget.detectedObject;
            switch (enemyState)
            {
                case EnemyState.DetectingNothing:
                    HandleDetectingNothing();
                    break;
                case EnemyState.DetectingSomething:
                    HandleDetectingSomething();
                    break;
                case EnemyState.DetectingTarget:
                    HandleDetectingTarget();
                    break;
                case EnemyState.MissingTargetSideways:
                    HandleMissingTargetSideways();
                    break;
                case EnemyState.TargetOverDetectingDistance:
                    HandleTargetOverDetectingDistance();
                    break;
                case EnemyState.TargetHindInObject:
                    HandleTargetHindInObject();
                    break;
                default:
                    StopNavMeshAgentMovement();
                    break;
            }
        }

        protected virtual void HandleDetectingNothing()
        {
            Patrol();
        }

        protected virtual void HandleDetectingSomething()
        {
            StopNavMeshAgentMovement();
            LookAtTarget(targetObject.transform.position, navMeshAgent.angularSpeed / 180f);
        }

        protected abstract void HandleDetectingTarget();
        
        protected virtual void HandleMissingTargetSideways()
        {
            StopNavMeshAgentMovement();
            LookAtTarget(targetObject.transform.position, navMeshAgent.angularSpeed / 180f);
        }

        protected virtual void HandleTargetOverDetectingDistance()
        {
            ChaseTarget(targetObject.transform.position, 1f);
        }

        protected virtual void HandleTargetHindInObject()
        {
            ChaseTarget(targetObject.transform.position, 1f);
        }

        /// <summary>
        /// path를 따라 움직임
        /// </summary>
        protected void Patrol()
        {
            navMeshAgent.speed = normalSpeed;

            if (path == null)
            {
                return;
            }

            if (Vector3.Distance(transform.position, path.GetChild(currentNode).position) > 0.3f)
            {
                navMeshAgent.destination = path.GetChild(currentNode).position;
            }
            else
            {
                currentNode++;
            }
        }

        /// <summary>
        /// targetPosition과의 거리가 stoppingDistance보다 짧아질 때까지 추격
        /// </summary>
        protected void ChaseTarget(Vector3 targetPosition, float stoppingDistance)
        {
            navMeshAgent.speed = runningSpeed;
            if (Vector3.Distance(transform.position - Vector3.up * transform.position.y, targetPosition - Vector3.up * targetPosition.y) > stoppingDistance)
            {
                navMeshAgent.destination = targetPosition;
            }
            else
            {
                navMeshAgent.destination = transform.position;
            }
        }

        protected void StopNavMeshAgentMovement()
        {
            navMeshAgent.destination = transform.position;
        }

        /// <summary>
        /// target 방향을 바라옴 (y축으로만 주시)
        /// </summary>
        protected void LookAtTarget(Vector3 target, float angularSpeed)
        {
            navMeshAgent.speed = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position - Vector3.up * target.y + Vector3.up * transform.position.y);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * angularSpeed);
        }

        protected void ReduceDetectedTime(float declineSpeed)
        {
            detectedTime -= Time.deltaTime * declineSpeed;
        }

        protected bool IsGroggy()
        {
            enemyInfo.effect.Stun -= Time.deltaTime;

            return enemyInfo.effect.Stun > 0;
        }
    }
}