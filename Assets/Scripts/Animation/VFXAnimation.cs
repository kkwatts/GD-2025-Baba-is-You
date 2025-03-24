using UnityEngine;

public class VFXAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer render;
    private GameObject player;

    private int direction;

    private Vector3 targetPosition;
    private float maxGoalTravelDistance;
    private float maxWinTravelDistance;
    private float maxCloudTravelDistance;

    public int type;
    public float dustSpeed;
    public float goalSpeed;
    public float winSpeed;
    public float cloudSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        maxGoalTravelDistance = 1f;
        maxWinTravelDistance = 5f;
        maxCloudTravelDistance = 1f;

        if (type == 2) {
            targetPosition = new Vector3(transform.position.x + Random.Range(-maxGoalTravelDistance, maxGoalTravelDistance), transform.position.y + Random.Range(-maxGoalTravelDistance, maxGoalTravelDistance), transform.position.z);
        }
        else if (type == 3) {
            targetPosition = new Vector3(transform.position.x + Random.Range(-maxWinTravelDistance, maxWinTravelDistance), transform.position.y + Random.Range(-maxWinTravelDistance, maxWinTravelDistance), transform.position.z);
        }
        else if (type == 4) {
            targetPosition = new Vector3(transform.position.x + Random.Range(-maxCloudTravelDistance, maxCloudTravelDistance), transform.position.y + Random.Range(-maxCloudTravelDistance, maxCloudTravelDistance), transform.position.z);
        }
    }

    // Update is called once per frame
    void Update() {
        anim.SetInteger("Type", type);

        if (type == 1) {
            if (direction == 1) {
                transform.position += Vector3.up * dustSpeed;
            }
            else if (direction == 2) {
                transform.position += Vector3.right * dustSpeed;
            }
            else if (direction == 3) {
                transform.position += Vector3.left * dustSpeed;
            }
            else {
                transform.position += Vector3.down * dustSpeed;
            }
        }
        else if (type == 2) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, goalSpeed);
        }
        else if (type == 3) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, winSpeed);
        }
        else if (type == 4) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, cloudSpeed);
        }
    }

    public void DestroyParticle() {
        Destroy(gameObject);
    }

    public void SetDirection(int dir) {
        direction = dir;
    }
}
