using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RessourceFalling : MonoBehaviour {
    [SerializeField] private LayerMask groundLayer = new LayerMask();
    [SerializeField] private float groundCheckDistance = 0.25f;
    private Rigidbody rb = null;

    private void Start() => rb = GetComponent<Rigidbody>();

    void Update() {
        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer)) {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, Vector3.down * groundCheckDistance);
    }
#endif
}
