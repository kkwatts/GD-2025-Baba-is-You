using UnityEngine;

public class ConnectorBehavior : MonoBehaviour {
    public bool hasObject;
    public bool hasAttribute;

    private GameObject objectBlock;
    private GameObject attributeBlock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        objectBlock = null;
        attributeBlock = null;
    }

    // Update is called once per frame
    void Update() {
        objectBlock = CheckForBlock(1);
        attributeBlock = CheckForBlock(2);

        if (objectBlock != null) {
            hasObject = true;
        }
        else {
            hasObject = false;
        }

        if (attributeBlock != null) {
            hasAttribute = true;
        }
        else {
            hasAttribute = false;
        }
    }

    private GameObject CheckForBlock(int type) {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down * 0.9f, Color.red);

        if (Physics.Raycast(transform.position, Vector3.up, out hit, 0.9f) ||
            Physics.Raycast(transform.position, Vector3.down, out hit, 0.9f) ||
            Physics.Raycast(transform.position, Vector3.left, out hit, 0.9f) ||
            Physics.Raycast(transform.position, Vector3.right, out hit, 0.9f)) {

            if (type == 1 && hit.transform.gameObject.tag == "Object Block") {
                return hit.transform.gameObject;
            }
            else if (type == 2 && hit.transform.gameObject.tag == "Attribute Block") {
                return hit.transform.gameObject;
            }
            else {
                return null;
            }
        }
        else {
            return null;
        }
    }
}
