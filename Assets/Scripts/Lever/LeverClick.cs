using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverClick : MonoBehaviour
{
    [SerializeField] private Animator AnimationControl;

    private void OnMouseDown()
    {
        AnimationControl.SetTrigger("LeverClick");
    }
}
