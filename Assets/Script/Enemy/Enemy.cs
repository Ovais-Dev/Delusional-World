using System.Net.Sockets;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    [SerializeField] private Transform player;
    [SerializeField] private float speed = 1f;
    private bool isTriggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTriggered) return;
        Follow();
    }

    void Follow()
    {
               Vector3 direction = (player.position - transform.position).normalized;
        transform.up = direction.normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
    public void TriggerActivation()
    {
               isTriggered = true;
    }
}
