using UnityEngine;


public class PlayerController : Controller
{
    
    [Header("Input Settings")]
    public string leftHorizontalCtrl = "L_Horizontal_P1";
    public string leftVerticalCtrl = "L_Vertical_P1";
    public string rightHorizontalCtrl = "R_Horizontal_P1";
    public string rightVerticalCtrl = "R_Vertical_P1";
    public string sprintButton = "Sprint_P1";
    public string jumpButton = "Jump_P1";
    public string crouchButton = "Crouch_P1";

    public string aimButton = "Aim_P1";
    protected float aimButtonInput;

    public string basicAttackButton = "BasicAttack_P1";
    protected float basicAttackInput;

    public string blockButton = "Block_P1";

            
    protected override void GetInput()
    {
        moveInput = Input.GetAxisRaw(leftHorizontalCtrl);
        aimButtonInput = Input.GetAxisRaw(aimButton);
        aimDirectionInput = Input.GetAxisRaw(rightHorizontalCtrl);
        basicAttackInput = Input.GetAxisRaw(basicAttackButton);


        if (moveInput != 0 && Input.GetButton(sprintButton) && isGrounded)
        {
            sprintRequest = true;
        }
        else
        {
            ResetSprint();
        }


        if (Input.GetButton(jumpButton))
        {
            if (jumpRequestTime == 0f)
            {
                if (isGrounded || jumpCounter < numberOfJumps)
                {
                    jumpRequest = true;
                    jumpCounter++;
                    isGrounded = false;
                    jumpTimeCounter = jumpTime;
                }
            }

            if (jumpRequestTime > 0f && jumpRequest)
            {
                if (jumpTimeCounter > 0)
                {
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    jumpRequest = false;
                }
            }

            jumpRequestTime += Time.deltaTime;
        }
        else
        {
            ResetJump();
        }


        if (Input.GetButton(crouchButton))
        {
            crouchRequest = true;
            isCrouching = true;
        }
        else
        {
            if (isCrouching && !isContactAbove)
            {
                ResetCrouch();
            }            
        }


        if (Input.GetButton(aimButton) || aimButtonInput != 0f)
        {
            aimRequest = true;
            isAiming = true;
            aimVector.x = Input.GetAxisRaw(rightHorizontalCtrl);
            aimVector.y = Input.GetAxisRaw(rightVerticalCtrl);
            if (aimRequest && aimVector == Vector3.zero)
            {
                aimVector = transform.right;
            }
        }
        else
        {
            ResetAim();
        }


        if (Input.GetButton(basicAttackButton) || basicAttackInput != 0f)
        {
            basicAttackRequest = true;
        }
        else
        {
            ResetBasicAttack();
        }


        if (Input.GetButton(blockButton))
        {
            blockRequest = true;
        }
        else
        {
            ResetBlock();
        }
               
    }
}
