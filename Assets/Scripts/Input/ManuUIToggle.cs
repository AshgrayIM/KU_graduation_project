using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManuUIToggle : MonoBehaviour
{
    public InputActionReference toggleReference = null;

   private void Awake()
   {
       toggleReference.action.started += Toggle;
   }

   private void OnDestory()
   {
       toggleReference.action.started -= Toggle;
   }

   private void Toggle(InputAction.CallbackContext context)
   {
       bool isActive = !gameObject.activeSelf;
       gameObject.SetActive(isActive);
   }
}
