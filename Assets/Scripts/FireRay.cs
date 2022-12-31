using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    

public class FireRay : MonoBehaviour
{
     public GameObject violin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireViolinRay(InputAction.CallbackContext context)
    {
        Debug.Log("Ray cast");
        Physics2D.Raycast(violin.transform.position, Mouse.current.position.ReadValue());
    }
}
