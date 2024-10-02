using UnityEngine;
using UnityEngine.UI;

public class CraneController : MonoBehaviour
{
    public Transform crane; // Reference to the crane's transform
    public Transform trolley; // Reference to the trolley's transform
    public Transform hook; // Reference to the hook's transform
    public Transform pointA; // Start point of the trolley movement
    public Transform pointB; // End point of the trolley movement
    public LineRenderer cableLineRenderer; // Reference to the LineRenderer component for the cable
    public Slider cableSlider; // UI Slider for cable length
    public Slider trolleySlider; // UI Slider for trolley position
    public Transform concreteAttachmentPoint; // Point where hook attaches to concrete

    private float maxCableLength = 10f; // Max length of the cable
    private float minCableLength = 2f; // Min length of the cable

    void Start()
    {
        // Set slider ranges
        cableSlider.minValue = 0f;
        cableSlider.maxValue = 1f;
        trolleySlider.minValue = 0f;
        trolleySlider.maxValue = 1f;
    }

    void Update()
    {
        HandleCraneRotation();
        UpdateTrolleyPosition();
        UpdateHookPosition();
        UpdateCableVisual();
    }

    void HandleCraneRotation()
    {
        // Rotate crane based on user input
        if (Input.GetKey(KeyCode.LeftArrow))
            crane.Rotate(Vector3.up, -30 * Time.deltaTime); // Rotate clockwise
        if (Input.GetKey(KeyCode.RightArrow))
            crane.Rotate(Vector3.up, 30 * Time.deltaTime); // Rotate counter-clockwise
    }

    void UpdateTrolleyPosition()
    {
        // Update the trolley's position based on the slider value
        float normalizedPosition = trolleySlider.value;
        trolley.position = Vector3.Lerp(pointA.position, pointB.position, normalizedPosition);
    }

    void UpdateHookPosition()
    {
        // Calculate the cable length from the slider value
        float normalizedCableLength = cableSlider.value;
        float cableLength = Mathf.Lerp(minCableLength, maxCableLength, normalizedCableLength);

        // Calculate the hook's position
        Vector3 hookPosition = trolley.position + Vector3.down * cableLength;

        // Ensure the hook doesn't go above the trolley's height
        hook.position = new Vector3(hookPosition.x, Mathf.Max(hookPosition.y, trolley.position.y), hookPosition.z);

        // Check for collision with concrete
        CheckForCollisionWithConcrete();
    }

    void CheckForCollisionWithConcrete()
    {
        if (concreteAttachmentPoint == null)
        {
            Debug.LogWarning("Concrete attachment point is not assigned!");
            return;
        }

        float collisionThreshold = 0.5f; // Define how close the hook must be to consider it a collision

        if (Vector3.Distance(hook.position, concreteAttachmentPoint.position) < collisionThreshold)
        {
            Debug.Log("Hook is attached to concrete!");
            // Handle attachment logic here (e.g., disable further movement or trigger an event)
        }
    }

    void UpdateCableVisual()
    {
        // Update the LineRenderer to visually represent the cable
        if (cableLineRenderer != null)
        {
            cableLineRenderer.positionCount = 2; // Cable has two points: trolley and hook
            cableLineRenderer.SetPosition(0, trolley.position); // Start point (trolley)
            cableLineRenderer.SetPosition(1, hook.position); // End point (hook)
        }
    }
}
