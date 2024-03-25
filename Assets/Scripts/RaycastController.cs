using UnityEngine;

public class RaycastController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Prepare a layer mask to ignore both the "UI" layer and the "Ignore Raycast" layer
            int uiLayerMask = 1 << LayerMask.NameToLayer("UI");
            int ignoreRaycastLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
            int combinedMask = uiLayerMask | ignoreRaycastLayerMask;
            combinedMask = ~combinedMask; // Invert the mask to ignore specified layers

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, combinedMask))
            {
                // Interact with the hit game object
                Debug.Log("Hit: " + hit.collider.gameObject.name);
            }
        }
    }
}
