using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Balloon : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private float minPopSpeed;
    [SerializeField] private float[] randomMinPopSpeed;
    [SerializeField] private MinigameBalloon minigame;
    public void Init(Material _material, MinigameBalloon _minigame)
    {
        minigame = _minigame;
        mesh.material = _material;
        minPopSpeed = Random.Range(randomMinPopSpeed[0], randomMinPopSpeed[1]);
    }
    private void OnCollisionEnter(Collision other)
    {
        Dart _dart = other.gameObject.GetComponent<Dart>();
        if (_dart == null || !_dart.isThrowed) return;
        
        float collisionSpeed = other.relativeVelocity.magnitude;
        Debug.Log($"Balloon hit by speed {collisionSpeed:F2}");
        
        if (collisionSpeed < minPopSpeed) return;
        
        _dart.isThrowed = true;
        _dart.FreezeRB(0.5f);

        minigame.ballonPopped++;
        AudioManager.instance.PlaySFX(0);
        
        Destroy(this.gameObject);
    }
}
