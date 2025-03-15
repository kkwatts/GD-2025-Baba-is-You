using UnityEngine;

public class GrassAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer render;

    private int type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        string name = render.sprite.name;

        if (name == "Grass_0" || name == "Grass_1" || name == "Grass_2") {
            // Single
            type = 1;
        }
        else if (name == "Grass_3" || name == "Grass_4" || name == "Grass_5") {
            // Open on right
            type = 2;
        }
        else if (name == "Grass_6" || name == "Grass_7" || name == "Grass_8") {
            // Open on top
            type = 3;
        }
        else if (name == "Grass_9" || name == "Grass_10" || name == "Grass_11") {
            // Open on top and right
            type = 4;
        }
        else if (name == "Grass_12" || name == "Grass_13" || name == "Grass_14") {
            // Open on left
            type = 5;
        }
        else if (name == "Grass_15" || name == "Grass_16" || name == "Grass_17") {
            // Open on left and right
            type = 6;
        }
        else if (name == "Grass_18" || name == "Grass_19" || name == "Grass_20") {
            // Open on top and left
            type = 7;
        }
        else if (name == "Grass_21" || name == "Grass_22" || name == "Grass_23") {
            // Open on top, left, and right
            type = 8;
        }
        else if (name == "Grass_24" || name == "Grass_25" || name == "Grass_26") {
            // Open on bottom
            type = 9;
        }
        else if (name == "Grass_27" || name == "Grass_28" || name == "Grass_29") {
            // Open on bottom and right
            type = 10;
        }
        else if (name == "Grass_30" || name == "Grass_31" || name == "Grass_32") {
            // Open on top and bottom
            type = 11;
        }
        else if (name == "Grass_33" || name == "Grass_34" || name == "Grass_35") {
            // Open on top, bottom, and right
            type = 12;
        }
        else if (name == "Grass_36" || name == "Grass_37" || name == "Grass_38") {
            // Open on bottom and left
            type = 13;
        }
        else if (name == "Grass_39" || name == "Grass_40" || name == "Grass_41") {
            // Open on bottom, left, and right
            type = 14;
        }
        else if (name == "Grass_42" || name == "Grass_43" || name == "Grass_44") {
            // Open on top, bottom, and left
            type = 15;
        }
        else if (name == "Grass_45" || name == "Grass_46" || name == "Grass_47") {
            // Open on all sides
            type = 16;
        }
    }

    // Update is called once per frame
    void Update() {
        anim.SetInteger("Type", type);
    }
}
