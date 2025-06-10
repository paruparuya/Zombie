using UnityEngine;

public class ZonbeiController : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
