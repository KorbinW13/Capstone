using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemController : MonoBehaviour
{
    public void Scroll()
    {
        Debug.Log(Mouse.current.scroll.ReadValue().normalized);
    }

}
