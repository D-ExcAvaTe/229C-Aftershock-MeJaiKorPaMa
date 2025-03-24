using System;
using UnityEngine;

public class Dart : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float destroyDelay = 20f;
    public bool isThrowed = false, isLanded = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FreezeRB();
    }

    public void ThrowDart(Vector3 facing,float throwForce)
    {
        isThrowed = true;
        
        UnfreezeRB();
        this.transform.parent = null;
        
        rb.AddForce(facing * throwForce);

        Destroy(this.gameObject, destroyDelay);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Dart>()) return;
        
        isLanded = true;
        FreezeRB();
    }

    private void FreezeRB()
    {
        this.rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void UnfreezeRB()
    {
        this.rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
    }
}
