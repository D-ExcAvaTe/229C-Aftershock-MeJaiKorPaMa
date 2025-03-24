using System;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Dart _dart = other.gameObject.GetComponent<Dart>();
        if (_dart == null || !_dart.isThrowed) return;
        
        Destroy(this.gameObject);
    }
}
