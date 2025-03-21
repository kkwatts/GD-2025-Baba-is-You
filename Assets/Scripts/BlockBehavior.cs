using UnityEngine;

public class BlockBehavior : MonoBehaviour {
    private KeyCode left;
    private KeyCode right;
    private KeyCode up;
    private KeyCode down;

    private BoxCollider col;
    private GameObject manager;
    private LayerMask pushLayer;         // Can objects like the flag be pushed by blocks?

    private float minX, maxX, minY, maxY;
    private float originalZ, youZ;
    private Vector3 targetPosition;
    private bool canMove;

    private float goalParticleTimer;
    private float goalParticleThreshold;
    private float goalParticleMin, goalParticleMax;

    public int direction;
    public bool isConnected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        left = KeyCode.LeftArrow;
        right = KeyCode.RightArrow;
        up = KeyCode.UpArrow;
        down = KeyCode.DownArrow;

        col = GetComponent<BoxCollider>();
        manager = GameObject.FindGameObjectWithTag("GameController");
        pushLayer = LayerMask.GetMask("Pushable");

        minX = -10.8f;
        maxX = 10.8f;
        minY = -5.4f;
        maxY = 10.8f;

        originalZ = transform.position.z;
        youZ = -4.5f;
        canMove = true;

        goalParticleTimer = 0f;
        goalParticleMin = 0.3f;
        goalParticleMax = 1f;
        goalParticleThreshold = 0f;

        targetPosition = transform.position;

        isConnected = false;
        if (!gameObject.CompareTag("Connector Block") && !gameObject.CompareTag("Object Block") && !gameObject.CompareTag("Attribute Block")) {
            gameObject.tag = "Untagged";
        }
    }

    // Update is called once per frame
    void Update() {
        // If the Object is "You"
        if (gameObject.CompareTag("You")) {
            IsYou();
            col.isTrigger = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, youZ);
        }
        else {
            col.isTrigger = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, originalZ);
        }

        // If the Object is "Win"
        if (gameObject.CompareTag("Win")) {
            goalParticleTimer += Time.deltaTime;
            IsWin();
        }
        else {
            goalParticleTimer = 0f;
            goalParticleThreshold = 0f;
        }

        // For All Objects
        col.size = new Vector3(col.size.x, col.size.y, 20f);

        if (gameObject.CompareTag("Connector Block") || gameObject.CompareTag("Object Block")) {
            if (isConnected) {
                GetComponent<TextAnimation>().Activate();
            }
            else {
                GetComponent<TextAnimation>().Deactivate();
            }
        }
        else if (gameObject.CompareTag("Attribute Block")) {
            if (isConnected) {
                GetComponent<BlockAnimation>().Activate();
            }
            else {
                GetComponent<BlockAnimation>().Deactivate();
            }
        }
    }

    // If the Object is "You"
    private void IsYou() {
        Debug.DrawRay(targetPosition, Vector3.left * 0.9f, Color.red);
        RaycastHit hit;
        Vector3 movement = Vector3.zero;

        if (Input.GetKeyDown(left) && canMove) {
            movement = new Vector3(-0.9f, 0f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.left, out hit, 0.9f)) {
                if (hit.transform.gameObject.CompareTag("Stop")) {
                    movement = Vector3.zero;
                }
                else if ((pushLayer & (1 << hit.transform.gameObject.layer)) != 0) {
                    if (!hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        movement = Vector3.zero;
                    }
                }
            }
            else if (targetPosition.x + movement.x < minX) {
                movement = Vector3.zero;
            }
        }
        else if (Input.GetKeyDown(right) && canMove) {
            movement = new Vector3(0.9f, 0f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.right, out hit, 0.9f)) {
                if (hit.transform.gameObject.CompareTag("Stop")) {
                    movement = Vector3.zero;
                }
                else if ((pushLayer & (1 << hit.transform.gameObject.layer)) != 0) {
                    if (!hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        movement = Vector3.zero;
                    }
                }
            }
            else if (targetPosition.x + movement.x > maxX){
                movement = Vector3.zero;
            }
        }
        else if (Input.GetKeyDown(up) && canMove) {
            movement = new Vector3(0f, 0.9f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.up, out hit, 0.9f)) {
                if (hit.transform.gameObject.CompareTag("Stop")) {
                    movement = Vector3.zero;
                }
                else if ((pushLayer & (1 << hit.transform.gameObject.layer)) != 0) {
                    if (!hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        movement = Vector3.zero;
                    }
                }
            }
            else if (targetPosition.y + movement.y > maxY) {
                movement = Vector3.zero;
            }
        }
        else if (Input.GetKeyDown(down) && canMove) {
            movement = new Vector3(0f, -0.9f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.down, out hit, 0.9f)) {
                if (hit.transform.gameObject.CompareTag("Stop")) {
                    movement = Vector3.zero;
                }
                else if ((pushLayer & (1 << hit.transform.gameObject.layer)) != 0) {
                    if (!hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        movement = Vector3.zero;
                    }
                }
            }
            else if (targetPosition.y + movement.y < minY) {
                movement = Vector3.zero;
            }
        }

        if (movement != Vector3.zero) {
            if (movement.x == -0.9f) {
                direction = 2;
            }
            else if (movement.x == 0.9f) {
                direction = 3;
            }
            else if (movement.y == 0.9f) {
                direction = 4;
            }
            else {
                direction = 1;
            }

            if (gameObject.name == "Baba") {
                gameObject.GetComponent<BabaAnimation>().Move();
            }

            GameObject dust = Instantiate(manager.GetComponent<GameManager>().VFX[0], transform.position, Quaternion.identity);
            dust.GetComponent<VFXAnimation>().SetDirection(direction);
        }

        targetPosition += movement;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
    }

    // If the Object is "Win"
    private void OnTriggerEnter(Collider col) {
        if (gameObject.CompareTag("Win") && col.gameObject.CompareTag("You")) {
            col.GetComponent<BlockBehavior>().canMove = false;
            for (int i = 0; i < 8; i++) {
                Instantiate(manager.GetComponent<GameManager>().VFX[2], transform.position, Quaternion.identity);
            }
        }
    }

    private void IsWin() { 
        if (goalParticleTimer >= goalParticleThreshold) {
            goalParticleThreshold = Random.Range(goalParticleMin, goalParticleMax);
            Instantiate(manager.GetComponent<GameManager>().VFX[1], transform.position, Quaternion.identity);
            goalParticleTimer = 0f;
        }
    }

    // If the Object can be Pushed
    public bool CanPush(int direction) {
        RaycastHit hit;
        
        if (direction == 1) {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.9f)) {
                if (hit.transform.gameObject.CompareTag("Stop")) {
                    return false;
                }
                else if ((pushLayer & (1 << hit.transform.gameObject.layer)) != 0) {
                    if (hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        MoveBlock(direction);
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    MoveBlock(direction);
                    return true;
                }
            }
            else {
                MoveBlock(direction);
                return true;
            }
        }
        else if (direction == 2) {
            if (Physics.Raycast(transform.position, Vector3.left, out hit, 0.9f)) {
                if (hit.transform.gameObject.CompareTag("Stop")) {
                    return false;
                }
                else if ((pushLayer & (1 << hit.transform.gameObject.layer)) != 0) {
                    if (hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        MoveBlock(direction);
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    MoveBlock(direction);
                    return true;
                }
            }
            else {
                MoveBlock(direction);
                return true;
            }
        }
        else if (direction == 3) {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.9f)) {
                if (hit.transform.gameObject.CompareTag("Stop")) {
                    return false;
                }
                else if ((pushLayer & (1 << hit.transform.gameObject.layer)) != 0) {
                    if (hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        MoveBlock(direction);
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    MoveBlock(direction);
                    return true;
                }
            }
            else {
                MoveBlock(direction);
                return true;
            }
        }
        else {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.9f)) {
                if (hit.transform.gameObject.CompareTag("Stop")) {
                    return false;
                }
                else if ((pushLayer & (1 << hit.transform.gameObject.layer)) != 0) {
                    if (hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        MoveBlock(direction);
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    MoveBlock(direction);
                    return true;
                }
            }
            else {
                MoveBlock(direction);
                return true;
            }
        }
    }

    // Blocks currently move in the direction the player was previously facing as they move
    // because direction is updated based upon what the movement is, rather than being updated
    // before the movement
    private void MoveBlock(int direction) {
        if (direction == 1) {
            transform.position += Vector3.down * 0.9f;
        }
        else if (direction == 2) {
            transform.position += Vector3.left * 0.9f;
        }
        else if (direction == 3) {
            transform.position += Vector3.right * 0.9f;
        }
        else {
            transform.position += Vector3.up * 0.9f;
        }
    }
}
