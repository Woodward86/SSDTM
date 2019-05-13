using UnityEngine;

public class MenuButton : MonoBehaviour
{

    public enum ButtonType {SinglePlayer, MultiPlayer, Settings, Back, Exit}

    [SerializeField] int thisIndex = 0;
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    public MenuButtonFunctions menuButtonFunctions;
    public ButtonType buttonType;

    private void Start()
    {
        menuButtonController = GetComponentInParent<MenuButtonController>();
        animator = GetComponent<Animator>();
        animatorFunctions = GetComponent<AnimatorFunctions>();
        menuButtonFunctions = GetComponent<MenuButtonFunctions>();
    }


    void Update()
    {
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1)
            {
                animator.SetBool("pressed", true);
                if (buttonType == ButtonType.SinglePlayer)
                {
                    menuButtonFunctions.SinglePlayerGame();
                }
                else if (buttonType == ButtonType.MultiPlayer)
                {
                    menuButtonFunctions.MultiPlayerGame();
                }
                else if (buttonType == ButtonType.Settings)
                {
                    menuButtonFunctions.SettingsMenu();
                }
                else if (buttonType == ButtonType.Back)
                {
                    menuButtonFunctions.GoBack();
                }
                else if (buttonType == ButtonType.Exit)
                {
                    menuButtonFunctions.ExitGame();
                }
                
            }
            else if (animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
                animatorFunctions.disableOnce = true;
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }
    }

}
