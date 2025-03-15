using UnityEngine;

public class TextAnimation : MonoBehaviour {
    private Animator anim;

    private int type;
    private bool active;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();

        if (gameObject.name == "Baba Text") {
            type = 1;
        }
        else if (gameObject.name == "Is Text") {
            type = 2;
        }
        else if (gameObject.name == "Flag Text") {
            type = 3;
        }
        else if (gameObject.name == "Wall Text") {
            type = 4;
        }
    }

    // Update is called once per frame
    void Update() {
        anim.SetInteger("Type", type);
        anim.SetBool("Active", active);
    }
}
