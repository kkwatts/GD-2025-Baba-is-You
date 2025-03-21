using UnityEngine;

public class ConnectorBehavior : MonoBehaviour {
    public bool hasObject;
    public bool hasAttribute;

    public GameObject objectBlock;
    public GameObject attributeBlock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        objectBlock = null;
        attributeBlock = null;
    }

    // Update is called once per frame
    void Update() {
        objectBlock = CheckForObject();
        attributeBlock = CheckForAttribute();

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

    // Object and attribute blocks may need to be arrays if putting more than two blocks into one statement is allowed

    private GameObject CheckForObject() {
        GameObject block = null;
        RaycastHit upHit, downHit, leftHit, rightHit;

        if (Physics.Raycast(transform.position, Vector3.up, out upHit, 0.9f)) {
            if (upHit.transform.gameObject.tag == "Object Block") {
                block = upHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.down, out downHit, 0.9f) && block == null) { 
            if (downHit.transform.gameObject.tag == "Object Block") {
                block = downHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.left, out leftHit, 0.9f) && block == null) {
            if (leftHit.transform.gameObject.tag == "Object Block") {
                block = leftHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.right, out rightHit, 0.9f) && block == null) {
            if (rightHit.transform.gameObject.tag == "Object Block") {
                block = rightHit.transform.gameObject;
            }
        }

        if (block != objectBlock && objectBlock != null) {
            objectBlock.GetComponent<BlockBehavior>().isConnected = false;
            GetComponent<BlockBehavior>().isConnected = false;
        }
        if (block != null && attributeBlock != null) {
            block.GetComponent<BlockBehavior>().isConnected = true;
            GetComponent<BlockBehavior>().isConnected = true;
        }
        else if (block == null && attributeBlock != null) {
            attributeBlock.GetComponent<BlockBehavior>().isConnected = false;
            GetComponent<BlockBehavior>().isConnected = false;
        }
        else if (block != null && attributeBlock == null) {
            block.GetComponent<BlockBehavior>().isConnected = false;
            GetComponent<BlockBehavior>().isConnected = false;
        }

        return block;
    }

    private GameObject CheckForAttribute() {
        GameObject block = null;
        RaycastHit upHit, downHit, leftHit, rightHit;

        if (Physics.Raycast(transform.position, Vector3.up, out upHit, 0.9f)) {
            if (upHit.transform.gameObject.tag == "Attribute Block") {
                block = upHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.down, out downHit, 0.9f) && block == null) {
            if (downHit.transform.gameObject.tag == "Attribute Block") {
                block = downHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.left, out leftHit, 0.9f) && block == null) {
            if (leftHit.transform.gameObject.tag == "Attribute Block") {
                block = leftHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.right, out rightHit, 0.9f) && block == null) {
            if (rightHit.transform.gameObject.tag == "Attribute Block") {
                block = rightHit.transform.gameObject;
            }
        }

        if (block != attributeBlock && attributeBlock != null) {
            attributeBlock.GetComponent<BlockBehavior>().isConnected = false;
            GetComponent<BlockBehavior>().isConnected = false;
        }
        if (block != null && objectBlock != null) {
            block.GetComponent<BlockBehavior>().isConnected = true;
            GetComponent<BlockBehavior>().isConnected = true;
        }
        else if (block == null && objectBlock != null) {
            objectBlock.GetComponent<BlockBehavior>().isConnected = false;
            GetComponent<BlockBehavior>().isConnected = false;
        }
        else if (block != null && objectBlock == null) {
            block.GetComponent<BlockBehavior>().isConnected = false;
            GetComponent<BlockBehavior>().isConnected = false;
        }

        return block;
    }
}
