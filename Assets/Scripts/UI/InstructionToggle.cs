using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionToggle : MonoBehaviour
{
    public GameObject Instructions;
    public bool InstructionsEnabled;

    public void ButtonBool()
    {
        InstructionsEnabled = !InstructionsEnabled;
        Instructions.SetActive(InstructionsEnabled);
    }
}
