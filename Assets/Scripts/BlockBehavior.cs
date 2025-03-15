using UnityEngine;

public class BlockBehavior : MonoBehaviour {
    private KeyCode left;
    private KeyCode right;
    private KeyCode up;
    private KeyCode down;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        left = KeyCode.LeftArrow;
        right = KeyCode.RightArrow;
        up = KeyCode.UpArrow;
        down = KeyCode.DownArrow;
    }

    // Update is called once per frame
    void Update() {
        if (gameObject.tag == "You") {
            IsYou();
        }
        else if (gameObject.tag == "Stop") {
            
        }
    }

    // If the Object is "You"
    private void IsYou() {
        Debug.DrawRay(transform.position, Vector3.left * 0.9f, Color.red);
        RaycastHit hit;
        Vector3 movement = Vector3.zero;

        if (Input.GetKeyDown(left)) {
            movement = new Vector3(-0.9f, 0f, 0f);
            if (Physics.Raycast(transform.position, Vector3.left, out hit, 0.9f)) { 
                if (hit.transform.gameObject.tag == "Stop") {
                    movement = Vector3.zero;
                }
            }
        }
        else if (Input.GetKeyDown(right)) {
            movement = new Vector3(0.9f, 0f, 0f);
            if (Physics.Raycast(transform.position, Vector3.right, out hit, 0.9f)) {
                if (hit.transform.gameObject.tag == "Stop") {
                    movement = Vector3.zero;
                }
            }
        }
        else if (Input.GetKeyDown(up)) {
            movement = new Vector3(0f, 0.9f, 0f);
            if (Physics.Raycast(transform.position, Vector3.up, out hit, 0.9f)) {
                if (hit.transform.gameObject.tag == "Stop") {
                    movement = Vector3.zero;
                }
            }
        }
        else if (Input.GetKeyDown(down)) {
            movement = new Vector3(0f, -0.9f, 0f);
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.9f)) {
                if (hit.transform.gameObject.tag == "Stop") {
                    movement = Vector3.zero;
                }
            }
        }

        transform.position += movement;
    }

    // If the Object is "Win"
    private void OnCollisionEnter2D(Collision2D col) { 
        if (gameObject.tag == "Win" && col.gameObject.tag == "You") {
            Debug.Log("Win");
        }
    }
}
