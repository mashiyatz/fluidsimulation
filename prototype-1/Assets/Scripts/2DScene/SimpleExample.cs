using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleExample : MonoBehaviour
{
    [SerializeField]
    private GameObject sphereObject;

    public InputAction spawnSphere;

    void Awake()
    {
        spawnSphere.performed += context => { OnSpawnSphere(context); };
    }


    void Update()
    {
        
    }

    public void OnSpawnSphere(InputAction.CallbackContext context)
    {
        GameObject go = Instantiate(sphereObject);
        go.transform.position = new Vector3(0, 0, 0);
    }

    public void OnEnable()
    {
        spawnSphere.Enable();
    }

    public void OnDisable()
    {
        spawnSphere.Disable();
    }
}
