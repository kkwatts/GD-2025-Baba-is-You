using UnityEngine;

public class ConnectorBehavior : MonoBehaviour {
    public bool hasObject;
    public bool hasAttribute;

    public GameObject objectBlock;
    public GameObject attributeBlock;

    private GameObject gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        objectBlock = null;
        attributeBlock = null;

        gameManager = GameObject.FindGameObjectWithTag("GameController");
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
    // Will probably need list of some sort for objects that will have multiple tags

    private GameObject CheckForObject() {
        GameObject block = null;

        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit upHit, 0.9f)) {
            if (upHit.transform.gameObject.CompareTag("Object Block")) {
                block = upHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit downHit, 0.9f) && block == null) { 
            if (downHit.transform.gameObject.CompareTag("Object Block")) {
                block = downHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit leftHit, 0.9f) && block == null) {
            if (leftHit.transform.gameObject.CompareTag("Object Block")) {
                block = leftHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit rightHit, 0.9f) && block == null) {
            if (rightHit.transform.gameObject.CompareTag("Object Block")) {
                block = rightHit.transform.gameObject;
            }
        }

        if (block != objectBlock && objectBlock != null) {
            objectBlock.GetComponent<BlockBehavior>().isConnected = false;
            GetComponent<BlockBehavior>().isConnected = false;
            if (attributeBlock != null) {
                gameManager.GetComponent<GameManager>().RemoveTags(objectBlock.name, attributeBlock.tag);
            }
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

        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit upHit, 0.9f)) {
            if (upHit.transform.gameObject.CompareTag("Attribute Block")) {
                block = upHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit downHit, 0.9f) && block == null) {
            if (downHit.transform.gameObject.CompareTag("Attribute Block")) {
                block = downHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit leftHit, 0.9f) && block == null) {
            if (leftHit.transform.gameObject.CompareTag("Attribute Block")) {
                block = leftHit.transform.gameObject;
            }
        }
        if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit rightHit, 0.9f) && block == null) {
            if (rightHit.transform.gameObject.CompareTag("Attribute Block")) {
                block = rightHit.transform.gameObject;
            }
        }

        if (block != attributeBlock && attributeBlock != null) {
            attributeBlock.GetComponent<BlockBehavior>().isConnected = false;
            GetComponent<BlockBehavior>().isConnected = false;
            if (objectBlock != null) {
                gameManager.GetComponent<GameManager>().RemoveTags(objectBlock.name, attributeBlock.tag);
            }
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
