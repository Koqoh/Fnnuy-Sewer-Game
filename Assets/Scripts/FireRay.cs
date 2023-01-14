using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    

public class FireRay : MonoBehaviour
{
    public GameObject violin;
    public Material material;
    SoundBeam beam;

    // Update is called once per frame
    void Update()
    {
        Destroy(GameObject.Find("Aim Beam"));
        beam = new SoundBeam(gameObject.transform.position, gameObject.transform.right, material);
    }

    public void FireViolinRay(InputAction.CallbackContext context)
    {
        Debug.Log("Ray cast");
        Physics2D.Raycast(violin.transform.position, Mouse.current.position.ReadValue());
    }
}
