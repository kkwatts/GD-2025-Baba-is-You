using UnityEngine;

public class BlockAnimation : MonoBehaviour {
    private Animator anim;

    private int type;
    private bool active;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();
        active = false;

        if (gameObject.name == "Stop") {
            type = 1;
        }
        else if (gameObject.name == "Win") {
            type = 2;
        }
        else if (gameObject.name == "You") {
            type = 3;
        }
        else if (gameObject.name == "Push") {
            type = 4;
        }
    }

    // Update is called once per frame
    void Update() {
        anim.SetInteger("Type", type);
        anim.SetBool("Active", active);
    }

    public void Activate() {
        active = true;
    }

    public void Deactivate() {
        active = false;
    }
}
