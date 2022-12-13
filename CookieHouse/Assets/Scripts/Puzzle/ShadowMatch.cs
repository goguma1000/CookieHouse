using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMatch : MonoBehaviour
{
    public GameObject matchTarget;
    public GameObject visual;
    [SerializeField] private GameObject eventItem;
    private bool isMatch = false;
    // Update is called once per frame
    void Update()
    {
        if (!isMatch)
        {
            Debug.Log($"match y: {matchTarget.transform.up}, obj y: {transform.up}, Dot: {Vector3.Dot(matchTarget.transform.up, transform.up)}");
            Debug.Log($"match x: {matchTarget.transform.right}, obj x: {transform.right}, Dot: {Vector3.Dot(matchTarget.transform.right, transform.right)}");
            if (Vector3.Dot(matchTarget.transform.up, transform.up) > 0.84 && Vector3.Dot(matchTarget.transform.right, transform.right) > 0.84)
            {
                GetComponent<XrOffsetGrabInteractable>().enabled = false;
                matchTarget.SetActive(false);
                visual.SetActive(false);
                isMatch = true;
            }
        }

        if (isMatch)
        {
            if (eventItem.transform.localRotation.eulerAngles.y >= 30)
            {
                isMatch = false;
                this.gameObject.SetActive(false);
                return;
            }
            eventItem.transform.Rotate(Vector3.up, 30 * Time.deltaTime);
        }
    }
}
