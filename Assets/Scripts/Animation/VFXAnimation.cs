using UnityEngine;

public class VFXAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer render;
    private GameObject player;

    private int type;
    private int direction;

    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("You");

        direction = player.GetComponent<BlockBehavior>().direction;

        if (render.sprite.name == "DustParticle_0") {
            type = 1;
        }
    }

    // Update is called once per frame
    void Update() {
        anim.SetInteger("Type", type);

        if (direction == 1) {
            transform.position += Vector3.up * speed;
        }
        else if (direction == 2) {
            transform.position += Vector3.right * speed;
        }
        else if (direction == 3) {
            transform.position += Vector3.left * speed;
        }
        else {
            transform.position += Vector3.down * speed;
        }
    }

    public void DestroyParticle() {
        Destroy(gameObject);
    }
}
