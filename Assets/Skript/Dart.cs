using System;
using System.Collections;
using UnityEngine;

public class Dart : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float destroyDelay = 20f;
    public bool isThrowed = false;
    private Coroutine delayAnim;
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
        if (other.gameObject.GetComponent<Balloon>()) return;
        if (other.gameObject.CompareTag("DartStick")) this.transform.SetParent(other.transform);

        if (other.gameObject.CompareTag("Furina"))
        {
            this.transform.SetParent(other.transform);
            SetAnim(other.gameObject.GetComponentInParent<Animator>(), "isAttacked", true);
            AudioManager.instance.PlaySFX(1);
            SetAnim(other.gameObject.GetComponentInParent<Animator>(), "isAttacked", false, 10);
        }
        
        AudioManager.instance.PlaySFX(7);
        FreezeRB();
    }

    private void SetAnim(Animator anim,string animString,bool _bool,float delay=0)
    {
        if (delay > 0)
        {
            if(delayAnim!=null) StopCoroutine(delayAnim);
            delayAnim = StartCoroutine(SetAnimRoutine(anim, animString, _bool, delay));
        }
        else anim.SetBool(animString, _bool);
    }

    IEnumerator SetAnimRoutine(Animator anim, string animString, bool _bool, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool(animString, _bool);
    }

    public void FreezeRB(float delay = 0f)
    {
        StartCoroutine(FreezeDelay(delay));
    }

    private IEnumerator FreezeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        this.rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void UnfreezeRB()
    {
        this.rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
    }
}
