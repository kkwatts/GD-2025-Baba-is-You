using UnityEngine;

public class MenuScript : MonoBehaviour {
    public GameObject[] mainMenuButtons, settingsMenuButtons, creditsMenuButtons;
    public GameObject mainMenu, settingsMenu, creditsMenu;

    public float offset;

    private GameObject manager;

    private int buttonNum;
    private int menu;
    private bool canSelect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        manager = GameObject.FindGameObjectWithTag("GameController");

        if (gameObject.name == "Selector") {
            menu = 1;
            buttonNum = 1;
            canSelect = true;
            transform.position = new Vector3(transform.position.x, mainMenuButtons[0].transform.position.y + offset, transform.position.z);
        }
    }

    public void LoadMenu() {
        if (gameObject.name == "Selector") {
            menu = 1;
            buttonNum = 1;
            canSelect = true;
            transform.position = new Vector3(transform.position.x, mainMenuButtons[0].transform.position.y + offset, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update() {
        if (gameObject.name == "Selector") {
            if (buttonNum == 1) {
                transform.position = new Vector3(-5.24f, mainMenuButtons[0].transform.position.y + offset, transform.position.z);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    canSelect = false;
                    manager.GetComponent<GameManager>().GoToLevelSelect();
                }
            }
            else if (buttonNum == 2) {
                transform.position = new Vector3(-5.24f, mainMenuButtons[1].transform.position.y + offset, transform.position.z);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    buttonNum = 5;
                    menu = 2;
                }
            }
            else if (buttonNum == 3) {
                transform.position = new Vector3(-5.24f, mainMenuButtons[2].transform.position.y + offset, transform.position.z);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    buttonNum = 6;
                    menu = 3;
                }
            }
            else if (buttonNum == 4) {
                transform.position = new Vector3(-5.24f, mainMenuButtons[3].transform.position.y + offset, transform.position.z);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    Application.Quit();
                }
            }
            else if (buttonNum == 5) {
                transform.position = new Vector3(-5.24f, settingsMenuButtons[0].transform.position.y + offset, transform.position.z);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    buttonNum = 2;
                    menu = 1;
                }
            }
            else if (buttonNum == 6) {
                transform.position = new Vector3(-5.24f, creditsMenuButtons[0].transform.position.y + offset, transform.position.z);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    buttonNum = 3;
                    menu = 1;
                }
            }

            if (buttonNum > 1 && canSelect && menu == 1 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))) {
                buttonNum--;
            }
            else if (buttonNum < 4 && canSelect && menu == 1 && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))) {
                buttonNum++;
            }

            if (menu == 1) {
                mainMenu.SetActive(true);
                settingsMenu.SetActive(false);
                creditsMenu.SetActive(false);
            }
            else if (menu == 2) {
                mainMenu.SetActive(false);
                settingsMenu.SetActive(true);
                creditsMenu.SetActive(false);
            }
            else if (menu == 3) {
                mainMenu.SetActive(false);
                settingsMenu.SetActive(false);
                creditsMenu.SetActive(true);
            }
        }
    }
}
