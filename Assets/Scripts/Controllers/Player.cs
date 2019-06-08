﻿using UnityEngine;

public class Player : Controller
{
    [Header("Input Settings")]
    public string jumpButton = "Jump_P1";
    public string horizontalCtrl = "Horizontal_P1";
        
    protected override void GetInput()
    {
        moveInput = Input.GetAxisRaw(horizontalCtrl);

        if (moveInput != 0)
        {
            facingDirection = moveInput;
        }

        if (facingDirection >= 0.0f)
        {
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

        if (Input.GetButton(jumpButton))
        {
            if (jumpRequestTime == 0f)
            {
                if (isGrounded || jumpRequestTime < 1f && jumpCounter < 2)
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
    }
}