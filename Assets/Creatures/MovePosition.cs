using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePosition : MonoBehaviour {
    private Vector3 movePosition;
    private void Awake() {
        movePosition = transform.position;
    }

    public void SetMovePosition(Vector3 position) {
        this.movePosition = position;
    }


    void Update()
    {
        Vector3 moveDir = (movePosition - transform.position).normalized;
        if (Vector3.Distance(movePosition, transform.position) < 0.1f) {
            moveDir = Vector3.zero; // Stop moving when near
            transform.position = movePosition;
        }

        GetComponent<BasicCreature>().SetVelocity(moveDir);
    }
}
