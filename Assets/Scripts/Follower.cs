using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour {
    NavMeshAgent _agent;

    public Transform Player;

    private void Start() {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        _agent.SetDestination(Player.position);
    }
}
