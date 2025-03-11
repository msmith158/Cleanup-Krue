using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")] [SerializeField]
    private Vector2 cameraOffset;
    [SerializeField] private float cameraYOffset;
    
    [Header("Object References")] [SerializeField]
    private Camera playerCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
