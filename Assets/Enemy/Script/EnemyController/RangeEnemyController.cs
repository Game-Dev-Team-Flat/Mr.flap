using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class RangeEnemyController : EnemyController
    {
        protected override void HandleDetectingTarget()
        {
            LookAtTarget(targetObject.transform.position, navMeshAgent.angularSpeed / 180f);
            StopNavMeshAgentMovement();
            LastDetectedPosition = targetObject.transform.position;
        }
    }
}