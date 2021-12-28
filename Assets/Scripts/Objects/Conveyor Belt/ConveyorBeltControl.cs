using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltControl : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;

    private Rigidbody2D rb;
    Vector2 beginningPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        beginningPos = rb.position;
        rb.position = beginningPos + Vector2.left * speed * Time.fixedDeltaTime;
        rb.MovePosition(beginningPos);
    }
}
