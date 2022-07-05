using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private Transform path;
        [Space(20)]
        [SerializeField]
        TextMeshPro state;
        [SerializeField]
        private DetectTarget detectTarget;
        [SerializeField]
        private float normalSpeed;
        [SerializeField]
        private float runningSpeed;
        private float m_detectedTime = 0f;
        private float detectedTime // 0 이상 identifyingTime 이하로 제한
        {
            get => m_detectedTime;
            set
            {
                if (value < 0)
                {
                    m_detectedTime = 0;
                }
                else if (value > identifyingTime)
                {
                    m_detectedTime = identifyingTime;
                }
                else
                {
                    m_detectedTime = value;
                }
            }
        }
        [SerializeField]
        private float identifyingTime;
        private NavMeshAgent navMeshAgent;
        private int m_currentNode = 0;
        private int currentNode // 0부터 path.childCount까지 순환
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
        private EnemyState m_enemyState = EnemyState.DetectingNothing;
        public EnemyState enemyState => m_enemyState;
        private GameObject m_targetObject;
        public GameObject targetObject
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
        private Transform LastDetectedTransform;

        public enum EnemyState
        {
            DetectingNothing,
            DetectingSomthing,
            DetectingTarget,
            MissingTargetSideways,
            TargetOverDetectingDistance
        }

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            state.text = enemyState.ToString();

            m_enemyState = DetermineState();
            detectTarget.SearchTarget(detectTarget.targetLayerMask);
            Movement();
        }

        private EnemyState DetermineState()
        {
            if (detectTarget.detectedObject != null) // 발견되면
            {
                if (m_enemyState != EnemyState.DetectingTarget && detectedTime < identifyingTime)
                {
                    detectedTime += detectTarget.detectionDistance / Vector3.Distance(transform.position, detectTarget.detectedObject.transform.position) * Time.deltaTime;
                    return EnemyState.DetectingSomthing;
                }
                else
                {
                    return EnemyState.DetectingTarget;
                }
            }
            else // 발견이 안되면
            {
                if (m_enemyState == EnemyState.DetectingSomthing && detectedTime > 0f) // 자꾸 DetectTarget이 Null값이 되는 버그와 lastdetecttransform이 실시간 반영되는 버그가 있음
                {
                    detectedTime -= Time.deltaTime / 2f;
                    return EnemyState.DetectingSomthing;
                }
                else if (detectedTime > 0f &&
                    (m_enemyState == EnemyState.DetectingTarget || m_enemyState == EnemyState.MissingTargetSideways || m_enemyState == EnemyState.TargetOverDetectingDistance))
                {
                    if (Vector3.Distance(transform.position, targetObject.transform.position) < detectTarget.detectionDistance)
                    {
                        return EnemyState.MissingTargetSideways;
                    }
                    else
                    {
                        return EnemyState.TargetOverDetectingDistance;
                    }
                }
                else
                {
                    detectedTime -= Time.deltaTime;
                    return EnemyState.DetectingNothing;
                }
            }
        }

        private void Movement()
        {
            targetObject = detectTarget.detectedObject;
            switch (m_enemyState)
            {
                case EnemyState.DetectingNothing:
                    Patrol();
                    break;
                case EnemyState.DetectingTarget:
                    LookAtTarget(targetObject.transform);
                    StopNavMeshAgentMovement();
                    LastDetectedTransform = targetObject.transform;
                    break;
                case EnemyState.DetectingSomthing:
                    StopNavMeshAgentMovement();
                    LookAtTarget(targetObject.transform);
                    break;
                case EnemyState.MissingTargetSideways:
                    StopNavMeshAgentMovement();
                    LookAtTarget(LastDetectedTransform);
                    break;
                case EnemyState.TargetOverDetectingDistance:
                    ChaseTarget(LastDetectedTransform.position);
                    break;
                default:
                    StopNavMeshAgentMovement();
                    break;
            }
        }

        private void Patrol()
        {
            navMeshAgent.speed = normalSpeed;
            if (Vector3.Distance(transform.position, path.GetChild(currentNode).position) > 0.3f)
            {
                navMeshAgent.destination = path.GetChild(currentNode).position;
            }
            else
            {
                currentNode++;
            }
        }

        private void ChaseTarget(Vector3 targetPosition)
        {
            navMeshAgent.speed = runningSpeed;
            navMeshAgent.destination = targetPosition;
            if (Vector3.Distance(transform.position - Vector3.up * transform.position.y, targetPosition - Vector3.up * targetPosition.y) < 1f)
            {
                detectedTime -= Time.deltaTime;
            }
        }

        private void StopNavMeshAgentMovement()
        {
            navMeshAgent.destination = transform.position;
        }

        private void LookAtTarget(Transform target)
        {
            navMeshAgent.speed = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * navMeshAgent.angularSpeed / 180f);
        }
    }
}