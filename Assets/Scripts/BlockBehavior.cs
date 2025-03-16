using UnityEngine;

public class BlockBehavior : MonoBehaviour {
    private KeyCode left;
    private KeyCode right;
    private KeyCode up;
    private KeyCode down;

    private BoxCollider col;
    private GameObject manager;

    private float minX, maxX, minY, maxY;
    private float originalZ, youZ;
    private Vector3 targetPosition;

    public int direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        left = KeyCode.LeftArrow;
        right = KeyCode.RightArrow;
        up = KeyCode.UpArrow;
        down = KeyCode.DownArrow;

        col = GetComponent<BoxCollider>();
        manager = GameObject.FindGameObjectWithTag("GameController");

        minX = -10.8f;
        maxX = 10.8f;
        minY = -5.4f;
        maxY = 10.8f;

        originalZ = transform.position.z;
        youZ = -4.5f;

        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (gameObject.tag == "You") {
            IsYou();
            transform.position = new Vector3(transform.position.x, transform.position.y, youZ);
        }
        else {
            transform.position = new Vector3(transform.position.x, transform.position.y, originalZ);
        }

        col.size = new Vector3(col.size.x, col.size.y, 20f);
    }

    // If the Object is "You"
    private void IsYou() {
        Debug.DrawRay(targetPosition, Vector3.left * 0.9f, Color.red);
        RaycastHit hit;
        Vector3 movement = Vector3.zero;

        if (Input.GetKeyDown(left)) {
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
        else if (Input.GetKeyDown(right)) {
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
        else if (Input.GetKeyDown(up)) {
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
        else if (Input.GetKeyDown(down)) {
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

            Instantiate(manager.GetComponent<GameManager>().VFX[0], transform.position, Quaternion.identity);
        }

        targetPosition += movement;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
    }

    // If the Object is "Win"
    private void OnCollisionEnter2D(Collision2D col) { 
        if (gameObject.tag == "Win" && col.gameObject.tag == "You") {
            Debug.Log("Win");
        }
    }
}
