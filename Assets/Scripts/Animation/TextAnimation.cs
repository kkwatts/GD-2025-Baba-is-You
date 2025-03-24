using UnityEngine;

public class TextAnimation : MonoBehaviour {
    private Animator anim;

    private int type;
    private bool active;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();
        active = false;

        if (gameObject.name == "Baba") {
            type = 1;
        }
        else if (gameObject.name == "Is") {
            type = 2;
        }
        else if (gameObject.name == "Flag") {
            type = 3;
        }
        else if (gameObject.name == "Wall") {
            type = 4;
        }
        else if (gameObject.name == "Rock") {
            type = 5;
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
