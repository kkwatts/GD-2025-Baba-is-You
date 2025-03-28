using UnityEngine;
using System.Collections.Generic;

public class BlockBehavior : MonoBehaviour {
    public Color activeColor;
    public Color inactiveColor;

    private KeyCode left;
    private KeyCode right;
    private KeyCode up;
    private KeyCode down;

    public List<string> tags;

    private BoxCollider col;
    private GameObject manager;
    private LayerMask ruleLayer, pushLayer, originalLayer;
    private GameObject soundFX;

    private float minX, maxX, minY, maxY;
    private bool canMove;
    private float moveBufferTimer;
    private int moveBufferDirection;

    private int orderYou, orderObjects, orderBlocks, orderBackground;

    private float goalParticleTimer;
    private float goalParticleThreshold;
    private float goalParticleMin, goalParticleMax;

    public int direction;
    public bool isConnected;
    public float moveBufferThreshold;
    public bool isYou;
    public Vector3 targetPosition;

    private Vector3 originalPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        left = KeyCode.LeftArrow;
        right = KeyCode.RightArrow;
        up = KeyCode.UpArrow;
        down = KeyCode.DownArrow;

        tags = new List<string>();
        if (isYou) {
            tags.Add("You");
        }

        col = GetComponent<BoxCollider>();
        manager = GameObject.FindGameObjectWithTag("GameController");
        ruleLayer = LayerMask.GetMask("Rules");
        pushLayer = LayerMask.GetMask("Push");
        originalLayer = gameObject.layer;
        soundFX = GameObject.FindGameObjectWithTag("Sound FX");

        minX = -10.8f;
        maxX = 10.8f;
        minY = -5.4f;
        maxY = 10.8f;

        moveBufferTimer = moveBufferThreshold + 1f;

        canMove = true;
        orderYou = 4;
        orderBlocks = 2;
        orderObjects = 1;
        orderBackground = 0;

        goalParticleTimer = 0f;
        goalParticleMin = 0.3f;
        goalParticleMax = 1f;
        goalParticleThreshold = 0f;

        targetPosition = transform.position;

        isConnected = false;
        originalPosition = transform.position;

        manager.GetComponent<GameManager>().GameUpdate();
    }

    // Update is called once per frame
    void Update() {
        // If the Object is "You"
        if (tags.Contains("You")) {
            IsYou();
            col.isTrigger = true;
            GetComponent<SpriteRenderer>().sortingOrder = orderYou;
            moveBufferTimer += Time.deltaTime;
        }
        else {
            col.isTrigger = false;
            moveBufferTimer = moveBufferThreshold + 1f;
            if (gameObject.CompareTag("Connector Block") || gameObject.CompareTag("Object Block") || gameObject.CompareTag("Attribute Block")) {
                GetComponent<SpriteRenderer>().sortingOrder = orderBlocks;
            }
            else if (gameObject.name == "Grass" || gameObject.name == "Tile") {
                GetComponent<SpriteRenderer>().sortingOrder = orderBackground;
            }
            else {
                GetComponent<SpriteRenderer>().sortingOrder = orderObjects;
            }
        }

        // If the Object is "Win"
        if (tags.Contains("Win")) {
            goalParticleTimer += Time.deltaTime;
            IsWin();
        }
        else {
            goalParticleTimer = 0f;
            goalParticleThreshold = 0f;
        }

        // If the Object can be Pushed
        if (tags.Contains("Push")) {
            gameObject.layer = LayerMask.NameToLayer("Push");
        }
        else {
            gameObject.layer = originalLayer;
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

        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
    }

    // If the Object is "You"
    private void IsYou() {
        Debug.DrawRay(targetPosition, Vector3.left * 0.9f, Color.red);
        RaycastHit hit;
        Vector3 movement = Vector3.zero;

        if ((Input.GetKeyDown(left) || (moveBufferTimer <= moveBufferThreshold && moveBufferDirection == 2)) && canMove && transform.position == targetPosition) {
            moveBufferTimer = moveBufferThreshold + 1f;
            direction = 2;
            movement = new Vector3(-0.9f, 0f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.left, out hit, 0.9f)) {
                if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("Stop")) {
                    movement = Vector3.zero;
                }
                
                else if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("You") || Physics.Raycast(targetPosition, Vector3.left, out hit, 0.9f, pushLayer) || Physics.Raycast(targetPosition, Vector3.left, out hit, 0.9f, ruleLayer)) {
                    if (!hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        movement = Vector3.zero;
                    }
                }
            }
            else if (targetPosition.x + movement.x < minX) {
                movement = Vector3.zero;
            }
        }
        else if (Input.GetKeyDown(left) && canMove && !(transform.position == targetPosition)) {
            moveBufferTimer = 0f;
            moveBufferDirection = 2;
        }
        else if ((Input.GetKeyDown(right) || (moveBufferTimer <= moveBufferThreshold && moveBufferDirection == 3)) && canMove && transform.position == targetPosition) {
            moveBufferTimer = moveBufferThreshold + 1f;
            direction = 3;
            movement = new Vector3(0.9f, 0f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.right, out hit, 0.9f)) {
                if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("Stop")) {
                    movement = Vector3.zero;
                }
                else if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("You") || Physics.Raycast(targetPosition, Vector3.right, out hit, 0.9f, pushLayer) || Physics.Raycast(targetPosition, Vector3.right, out hit, 0.9f, ruleLayer)) {
                    if (!hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        movement = Vector3.zero;
                    }
                }
            }
            else if (targetPosition.x + movement.x > maxX) {
                movement = Vector3.zero;
            }
        }
        else if (Input.GetKeyDown(right) && canMove && !(transform.position == targetPosition)) {
            moveBufferTimer = 0f;
            moveBufferDirection = 3;
        }
        else if ((Input.GetKeyDown(up) || (moveBufferTimer <= moveBufferThreshold && moveBufferDirection == 4)) && canMove && transform.position == targetPosition) {
            moveBufferTimer = moveBufferThreshold + 1f;
            direction = 4;
            movement = new Vector3(0f, 0.9f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.up, out hit, 0.9f)) {
                if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("Stop")) {
                    movement = Vector3.zero;
                }
                else if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("You") || Physics.Raycast(targetPosition, Vector3.up, out hit, 0.9f, pushLayer) || Physics.Raycast(targetPosition, Vector3.up, out hit, 0.9f, ruleLayer)) {
                    if (!hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        movement = Vector3.zero;
                    }
                }
            }
            else if (targetPosition.y + movement.y > maxY) {
                movement = Vector3.zero;
            }
        }
        else if (Input.GetKeyDown(up) && canMove && !(transform.position == targetPosition)) {
            moveBufferTimer = 0f;
            moveBufferDirection = 4;
        }
        else if ((Input.GetKeyDown(down) || (moveBufferTimer <= moveBufferThreshold && moveBufferDirection == 1)) && canMove && transform.position == targetPosition) {
            moveBufferTimer = moveBufferThreshold + 1f;
            direction = 1;
            movement = new Vector3(0f, -0.9f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.down, out hit, 0.9f)) {
                if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("Stop")) {
                    movement = Vector3.zero;
                }
                else if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("You") || Physics.Raycast(targetPosition, Vector3.down, out hit, 0.9f, pushLayer) || Physics.Raycast(targetPosition, Vector3.down, out hit, 0.9f, ruleLayer)) {
                    if (!hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        movement = Vector3.zero;
                    }
                }
            }
            else if (targetPosition.y + movement.y < minY) {
                movement = Vector3.zero;
            }
        }
        else if (Input.GetKeyDown(down) && canMove && !(transform.position == targetPosition)) {
            moveBufferTimer = 0f;
            moveBufferDirection = 1;
        }

        if (movement != Vector3.zero) {
            soundFX.GetComponent<AudioScript>().PlaySound("Step");
            if (gameObject.name == "Baba") {
                gameObject.GetComponent<BabaAnimation>().Move();
            }

            GameObject dust = Instantiate(manager.GetComponent<GameManager>().VFX[0], transform.position, Quaternion.identity);
            dust.GetComponent<VFXAnimation>().SetDirection(direction);
            dust.GetComponent<SpriteRenderer>().color = activeColor;
        }

        targetPosition += movement;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
    }

    // If the Object is "Win"
    private void OnTriggerEnter(Collider col) {
        if (tags.Contains("Win") && col.gameObject.GetComponent<BlockBehavior>().tags.Contains("You")) {
            col.GetComponent<BlockBehavior>().canMove = false;
            soundFX.GetComponent<AudioScript>().PlaySound("Win");
            for (int i = 0; i < 8; i++) {
                Instantiate(manager.GetComponent<GameManager>().VFX[2], transform.position, Quaternion.identity);
            }
            manager.GetComponent<GameManager>().GoToLevelSelect();
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
                if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("Stop")) {
                    return false;
                }
                else if (Physics.Raycast(targetPosition, Vector3.down, out hit, 0.9f, pushLayer) || Physics.Raycast(targetPosition, Vector3.down, out hit, 0.9f, ruleLayer)) {
                    if (hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        if (!tags.Contains("You")) {
                            MoveBlock(direction);
                        }
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    if (!tags.Contains("You")) {
                        MoveBlock(direction);
                    }
                    return true;
                }
            }
            else if (targetPosition.y - 0.9f < minY) {
                return false;
            }
            else {
                if (!tags.Contains("You")) {
                    MoveBlock(direction);
                }
                return true;
            }
        }
        else if (direction == 2) {
            if (Physics.Raycast(transform.position, Vector3.left, out hit, 0.9f)) {
                if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("Stop")) {
                    return false;
                }
                else if (Physics.Raycast(targetPosition, Vector3.left, out hit, 0.9f, pushLayer) || Physics.Raycast(targetPosition, Vector3.left, out hit, 0.9f, ruleLayer)) {
                    if (hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        if (!tags.Contains("You")) {
                            MoveBlock(direction);
                        }
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    if (!tags.Contains("You")) {
                        MoveBlock(direction);
                    }
                    return true;
                }
            }
            else if (targetPosition.x - 0.9f < minX) {
                return false;
            }
            else {
                if (!tags.Contains("You")) {
                    MoveBlock(direction);
                }
                return true;
            }
        }
        else if (direction == 3) {
            if (Physics.Raycast(transform.position, Vector3.right, out hit, 0.9f)) {
                if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("Stop")) {
                    return false;
                }
                else if (Physics.Raycast(targetPosition, Vector3.right, out hit, 0.9f, pushLayer) || Physics.Raycast(targetPosition, Vector3.right, out hit, 0.9f, ruleLayer)) {
                    if (hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        if (!tags.Contains("You")) {
                            MoveBlock(direction);
                        }
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    if (!tags.Contains("You")) {
                        MoveBlock(direction);
                    }
                    return true;
                }
            }
            else if (targetPosition.x + 0.9f > maxX) {
                return false;
            }
            else {
                if (!tags.Contains("You")) {
                    MoveBlock(direction);
                }
                return true;
            }
        }
        else {
            if (Physics.Raycast(transform.position, Vector3.up, out hit, 0.9f)) {
                if (hit.transform.gameObject.GetComponent<BlockBehavior>().tags.Contains("Stop")) {
                    return false;
                }
                else if (Physics.Raycast(targetPosition, Vector3.up, out hit, 0.9f, pushLayer) || Physics.Raycast(targetPosition, Vector3.up, out hit, 0.9f, ruleLayer)) {
                    if (hit.transform.gameObject.GetComponent<BlockBehavior>().CanPush(direction)) {
                        if (!tags.Contains("You")) {
                            MoveBlock(direction);
                        }
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    if (!tags.Contains("You")) {
                        MoveBlock(direction);
                    }
                    return true;
                }
            }
            else if (targetPosition.y + 0.9f > maxY) {
                return false;
            }
            else {
                if (!tags.Contains("You")) {
                    MoveBlock(direction);
                }
                return true;
            }
        }
    }

    private void MoveBlock(int direction) {
        if (direction == 1) {
            targetPosition += Vector3.down * 0.9f;
        }
        else if (direction == 2) {
            targetPosition += Vector3.left * 0.9f;
        }
        else if (direction == 3) {
            targetPosition += Vector3.right * 0.9f;
        }
        else {
            targetPosition += Vector3.up * 0.9f;
        }
        GameObject dust = Instantiate(manager.GetComponent<GameManager>().VFX[0], transform.position, Quaternion.identity);
        dust.GetComponent<VFXAnimation>().SetDirection(direction);
        if (isConnected) {
            dust.GetComponent<SpriteRenderer>().color = activeColor;
        }
        else {
            dust.GetComponent<SpriteRenderer>().color = inactiveColor;
        }
    }

    public void ConnectionFormed() {
        for (int i = 0; i < 6; i++) {
            GameObject cloud = Instantiate(manager.GetComponent<GameManager>().VFX[3], transform.position, Quaternion.identity);
            cloud.GetComponent<SpriteRenderer>().color = activeColor;
        }
        if (gameObject.tag == "Connector Block") {
            soundFX.GetComponent<AudioScript>().PlaySound("Goal");
        }
    }

    public void ResetPos() {
        direction = 3;
        canMove = true;
        targetPosition = originalPosition;
        transform.position = targetPosition;
        tags.Clear();
    }
}