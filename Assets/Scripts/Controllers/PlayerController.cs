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
        //moveInput = Input.GetAxisRaw(leftHorizontalCtrl);
        leftXInput = Input.GetAxisRaw(leftHorizontalCtrl);
        leftYInput = Input.GetAxisRaw(leftVerticalCtrl);
        aimButtonInput = Input.GetAxisRaw(aimButton);
        aimDirectionInput = Input.GetAxisRaw(rightHorizontalCtrl);
        basicAttackInput = Input.GetAxisRaw(basicAttackButton);

        direction = new Vector3(leftXInput, leftYInput);


        if (direction.x != 0 && Input.GetButton(sprintButton) && cCollisions.isGrounded)
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
                if (cCollisions.isGrounded || jumpCounter < numberOfJumps || isWallSliding || distanceToGround <= preJumpHeight)
                {
                    jumpRequest = true;
                    jumpCounter++;
                    cCollisions.isGrounded = false;
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
        }
        else
        {
            if (crouchRequest && !cCollisions.isContactAbove)
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
