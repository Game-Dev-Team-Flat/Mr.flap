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
        private float m_detectingTime = 0f;
        private float detectingTime // 0 이상 identifyingTime 이하로 제한
        {
            get => m_detectingTime;
            set
            {
                if (value < 0)
                {
                    m_detectingTime = 0;
                }
                else if (value > identifyingTime)
                {
                    m_detectingTime = identifyingTime;
                }
                else
                {
                    m_detectingTime = value;
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
        private EnemyState m_enemyState = EnemyState.DetectedNothing;
        public EnemyState enemyState => m_enemyState;
        private GameObject m_targetObject;
        private GameObject targetObject
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

        public enum EnemyState
        {
            DetectedNothing,
            DetectedSomthing,
            DetectedTarget,
            MissingTarget
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
            if (detectTarget.detectedObject != null)
            {
                if (m_enemyState != EnemyState.DetectedTarget && detectingTime < identifyingTime)
                {
                    detectingTime += detectTarget.detectionDistance / Vector3.Distance(transform.position, detectTarget.detectedObject.transform.position) * Time.deltaTime;
                    return EnemyState.DetectedSomthing;
                }
                else
                {
                    return EnemyState.DetectedTarget;
                }
            }
            else
            {
                if (detectingTime > 0f)
                {
                    detectingTime -= Time.deltaTime;
                    return EnemyState.MissingTarget;
                }
                else
                {
                    return EnemyState.DetectedNothing;
                }
            }
        }

        private void Movement()
        {
            targetObject = detectTarget.detectedObject;
            switch (m_enemyState)
            {
                case EnemyState.DetectedNothing:
                    Patrol();
                    break;
                case EnemyState.DetectedTarget:
                    ChaseTarget();
                    break;
                case EnemyState.DetectedSomthing:
                    StopNavMeshAgentMovement();
                    LookAtTarget(targetObject.transform);
                    break;
                case EnemyState.MissingTarget:
                    LookAtTarget(targetObject.transform);
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

        private void ChaseTarget()
        {
            navMeshAgent.speed = runningSpeed;
            navMeshAgent.destination = (Vector3.Distance(transform.position, targetObject.transform.position) > 4f)
                                        ? targetObject.transform.position : transform.position;
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