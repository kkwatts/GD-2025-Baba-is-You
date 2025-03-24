using UnityEngine;

public class SingleTileAnimation : MonoBehaviour {
    private Animator anim;

    private int type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();

        if (gameObject.name == "Flag") {
            type = 1;
        }
        else if (gameObject.name == "Tile") {
            type = 2;
        }
        else if (gameObject.name == "Rock") {
            type = 3;
        }
    }

    // Update is called once per frame
    void Update() {
        anim.SetInteger("Type", type);
    }
}
