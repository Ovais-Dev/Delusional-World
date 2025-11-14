using System;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class Player : MonoBehaviour
{

    [Header("Player Properties")]
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private float rotationSpeed = 30f;

    [Header("Other Properties")]
    [SerializeField] private float scaleRadius = 50f;
    
    public static event Action<bool> GameOverEvent;

    Rigidbody2D rb;
    Vector2 pointerInput;
    Vector2 mousePos;
    Vector2 lookDir;

    bool scaled = false;
   // private Vector2 endPosition;
    private CircleScaler circleScaler;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleScaler = FindFirstObjectByType<CircleScaler>(FindObjectsInactive.Include);
    }

    /*public void OnPointer(InputAction.CallbackContext context)
    {
        pointerInput = context.ReadValue<Vector2>();
    }*/

    public void HandleInteract(InputAction.CallbackContext context)
    {

        //scaled = !scaled;
        //circleScaler.SetRadius((scaled ? scaleRadius : 0f));
        //Debug.Log("Working");
    }
    private void Update()
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;
        PlayerLook();
        //CheckForEndPosition();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;
        PlayerMove();
    }

    void PlayerLook()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        lookDir = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * rotationSpeed);
    }
    void PlayerMove()
    {
        if (Vector2.Distance(transform.position, mousePos) < 0.5f) return;
       rb.MovePosition(rb.position + lookDir.normalized * movespeed * Time.fixedDeltaTime);
    }
    //void CheckForEndPosition()
    //{
        
    //    if(Vector2.Distance(transform.position, endPosition) < 1f)
    //    {
    //        Debug.Log("Reached End Position");
    //        ReachedEndPosition();
    //    }
    //}
    //public void SetEndPoint(Vector2 point)
    //{
    //    endPosition = point;
    //}
    public void ReachedEndPosition()
    {
               GameOverEvent?.Invoke(true);
        
        GameManager.Instance.currentState = GameState.GameOver;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroy"))
        {
            GameOverEvent?.Invoke(false);
            Debug.Log("Player Destroyed");
        }
        if (collision.CompareTag("EndPoint"))
        {
            Invoke(nameof(ReachedEndPosition), 0.5f);
        }
        if (collision.CompareTag("Portal"))
        {
            circleScaler.EnableMask(scaleRadius, 5f);
            Destroy(collision.gameObject);

        }
        if (collision.CompareTag("Enemy"))
        {
            GameOverEvent?.Invoke(false);

        }
        if (collision.CompareTag("NonPortal"))
        {
            Destroy(collision.gameObject);
        }
    }
}
