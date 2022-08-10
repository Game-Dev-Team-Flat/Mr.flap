using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MeleeEnemyController : EnemyController
    {
        protected override void HandleDetectingTarget()
        {
            LookAtTarget(targetObject.transform.position, navMeshAgent.angularSpeed / 180f);
            ChaseTarget(targetObject.transform.position, 3f);
            LastDetectedPosition = targetObject.transform.position;
        }
    }
}