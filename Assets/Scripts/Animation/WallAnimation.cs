using UnityEngine;

public class WallAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer render;

    private int type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        string name = render.sprite.name;

        if (name == "Walls_0" || name == "Walls_1" || name == "Walls_2") {
            // Single
            type = 1;
        }
        else if (name == "Walls_3" || name == "Walls_4" || name == "Walls_5") {
            // Open on right
            type = 2;
        }
        else if (name == "Walls_6" || name == "Walls_7" || name == "Walls_8") {
            // Open on top
            type = 3;
        }
        else if (name == "Walls_9" || name == "Walls_10" || name == "Walls_11") {
            // Open on top and right
            type = 4;
        }
        else if (name == "Walls_12" || name == "Walls_13" || name == "Walls_14") {
            // Open on left
            type = 5;
        }
        else if (name == "Walls_15" || name == "Walls_16" || name == "Walls_17") {
            // Open on left and right
            type = 6;
        }
        else if (name == "Walls_18" || name == "Walls_19" || name == "Walls_20") {
            // Open on top and left
            type = 7;
        }
        else if (name == "Walls_21" || name == "Walls_22" || name == "Walls_23") {
            // Open on top, left, and right
            type = 8;
        }
        else if (name == "Walls_24" || name == "Walls_25" || name == "Walls_26") {
            // Open on bottom
            type = 9;
        }
        else if (name == "Walls_27" || name == "Walls_28" || name == "Walls_29") {
            // Open on bottom and right
            type = 10;
        }
        else if (name == "Walls_31" || name == "Walls_32" || name == "Walls_33") {
            // Open on top, bottom, and right
            type = 11;
        }
        else if (name == "Walls_34" || name == "Walls_35" || name == "Walls_36") {
            // Open on bottom and left
            type = 12;
        }
        else if (name == "Walls_37" || name == "Walls_38" || name == "Walls_39") {
            // Open on bottom, left, and right
            type = 13;
        }
        else if (name == "Walls_40" || name == "Walls_41" || name == "Walls_42") {
            // Open on top, bottom, and left
            type = 14;
        }
        else if (name == "Walls_43" || name == "Walls_44" || name == "Walls_45") {
            // Open on all sides
            type = 15;
        }
    }

    // Update is called once per frame
    void Update() {
        anim.SetInteger("Type", type);
    }
}
