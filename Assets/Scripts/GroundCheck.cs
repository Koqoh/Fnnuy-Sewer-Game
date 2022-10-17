using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform sensor;
    [SerializeField] private LayerMask layerMask;

    public bool IsGrounded() {
        RaycastHit2D hit = Physics2D.CircleCast(
            sensor.position,
            sensor.localScale.x/2,
            sensor.forward,
            0,
            layerMask
            );
        return hit.collider != null;
    }
}
