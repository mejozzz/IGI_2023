using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {
    public float ViewRadius;

    [Range(0f, 360f)]
    public float ViewAngle;

    public LayerMask TargetMask;
    public LayerMask ObstacleMask;

    [HideInInspector]
    public List<Transform> VisibleTargets = new List<Transform>();

    private void Start() {
        StartCoroutine(FindTargetWithDelay(0.2f));
    }

    IEnumerator FindTargetWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    } 

    void FindVisibleTargets() {
        VisibleTargets.Clear();
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, TargetMask);
        
        for (int i = 0; i < targetInViewRadius.Length; i++) {
            Transform target = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) <  ViewAngle / 2) {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, ObstacleMask)) {
                    VisibleTargets.Add(target);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool isAngleGlobal) {
        if (!isAngleGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}
