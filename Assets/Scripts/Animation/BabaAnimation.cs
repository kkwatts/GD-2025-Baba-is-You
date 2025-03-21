using UnityEngine;

public class BabaAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer render;

    public int direction; // 1 = Down, 2 = Left, 3 = Right, 4 = Up
    public float minIdleThreshold, maxIdleThreshold;

    private float idleTimer;
    private float idleThreshold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        idleTimer = 0f;
        idleThreshold = Random.Range(minIdleThreshold, maxIdleThreshold);
    }

    // Update is called once per frame
    void Update() {
        direction = GetComponent<BlockBehavior>().direction;
        anim.SetInteger("Direction", direction);

        if (!anim.GetBool("Moving")) {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleThreshold) {
                idleThreshold = Random.Range(minIdleThreshold, maxIdleThreshold);
                anim.SetBool("Blinking", true);
            }
        }

        if (anim.GetInteger("Direction") == 2) {
            render.flipX = true;
        }
        else {
            render.flipX = false;
        }
    }

    public void Move() {
        Reset();
        anim.SetBool("Moving", true);
    }

    public void Reset() {
        anim.SetBool("Moving", false);
        anim.SetBool("Blinking", false);
        idleTimer = 0f;
    }
}
