using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private GameObject draggedObject;
    private Vector3 mouseOffset;

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

                // Start dragging the object
                draggedObject = hit.collider.gameObject;
                mouseOffset = draggedObject.transform.position - Camera.main.ScreenPointToRay(Input.mousePosition).origin;
            }
        }

        if (Input.GetMouseButton(0) && draggedObject != null)
        {
            // Drag the object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            draggedObject.transform.position = ray.origin + mouseOffset;
        }

        if (Input.GetMouseButtonUp(0) && draggedObject != null)
        {
            // Stop dragging the object
            draggedObject = null;
        }
    }
}