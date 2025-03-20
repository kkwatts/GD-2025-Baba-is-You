using UnityEngine;

public class BlockBehavior : MonoBehaviour {
    private KeyCode left;
    private KeyCode right;
    private KeyCode up;
    private KeyCode down;

    private BoxCollider col;
    private GameObject manager;
    private LayerMask pushLayer;

    private float minX, maxX, minY, maxY;
    private float originalZ, youZ;
    private Vector3 targetPosition;
    private bool canMove;

    private float goalParticleTimer;
    private float goalParticleThreshold;
    private float goalParticleMin, goalParticleMax;

    public int direction;

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
    }

    // Update is called once per frame
    void Update() {
        // If the Object is "You"
        if (gameObject.tag == "You") {
            IsYou();
            col.isTrigger = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, youZ);
        }
        else {
            col.isTrigger = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, originalZ);
        }

        // If the Object is "Win"
        if (gameObject.tag == "Win") {
            goalParticleTimer += Time.deltaTime;
            IsWin();
        }
        else {
            goalParticleTimer = 0f;
            goalParticleThreshold = 0f;
        }

        // If the Object can be Pushed
        if (gameObject.layer == pushLayer) {
            IsPushable();
        }

        col.size = new Vector3(col.size.x, col.size.y, 20f);
    }

    // If the Object is "You"
    private void IsYou() {
        Debug.DrawRay(targetPosition, Vector3.left * 0.9f, Color.red);
        RaycastHit hit;
        Vector3 movement = Vector3.zero;

        if (Input.GetKeyDown(left) && canMove) {
            movement = new Vector3(-0.9f, 0f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.left, out hit, 0.9f)) {
                if (hit.transform.gameObject.tag == "Stop") {
                    movement = Vector3.zero;
                }
            }
            else if (targetPosition.x + movement.x < minX) {
                movement = Vector3.zero;
            }
        }
        else if (Input.GetKeyDown(right) && canMove) {
            movement = new Vector3(0.9f, 0f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.right, out hit, 0.9f)) {
                if (hit.transform.gameObject.tag == "Stop") {
                    movement = Vector3.zero;
                }
            }
            else if (targetPosition.x + movement.x > maxX){
                movement = Vector3.zero;
            }
        }
        else if (Input.GetKeyDown(up) && canMove) {
            movement = new Vector3(0f, 0.9f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.up, out hit, 0.9f)) {
                if (hit.transform.gameObject.tag == "Stop") {
                    movement = Vector3.zero;
                }
            }
            else if (targetPosition.y + movement.y > maxY) {
                movement = Vector3.zero;
            }
        }
        else if (Input.GetKeyDown(down) && canMove) {
            movement = new Vector3(0f, -0.9f, 0f);
            if (Physics.Raycast(targetPosition, Vector3.down, out hit, 0.9f)) {
                if (hit.transform.gameObject.tag == "Stop") {
                    movement = Vector3.zero;
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
        if (gameObject.tag == "Win" && col.gameObject.tag == "You") {
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
    private void IsPushable() {
        GameObject you = GameObject.FindGameObjectWithTag("You");
        Debug.Log(gameObject.name);
    }
}
