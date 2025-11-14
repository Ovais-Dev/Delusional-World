using UnityEngine;

public class CircleScaler : MonoBehaviour
{
    [Header("Speed of scaling")]
    public float scaleSpeed = 2f;

    private float targetRadius = 1f;   // final size you want
    private void Start()
    {
        SetRadius(0f);
    }
    void Update()
    {
        float currentRadius = transform.localScale.x;

        // Smoothly move toward target radius
        float newRadius = Mathf.Lerp(currentRadius, targetRadius, Time.deltaTime * scaleSpeed);

        // Apply scale (uniform)
        transform.localScale = new Vector3(newRadius, newRadius, 1f);
    }

    // Call this from another script
    public void EnableMask(float radius, float time)
    {
        SetRadius(radius); 
        Invoke(nameof(ResetMask), time);
    }
    public void SetRadius(float radius)
    {
        targetRadius = radius;
    }
    void ResetMask() => SetRadius(0f);
}
