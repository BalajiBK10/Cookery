using System;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5f;

    bool isWalking;

    [SerializeField] private GameInput gameInput;
    void Update()
    {
        Vector2 inputVector = gameInput.GetVectorInputNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        
        float moveDistance =  moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight , playerRadius,moveDir,moveDistance);
       
      
        if (!canMove)
        {
            //cannot move towards moveDir
            //Attempt only on x movement
             Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                // move only on x direction
                moveDir = moveDirX;
            }

            else
            {
                //cannot move towards moveDir
                //Attempt to move on z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                 if (canMove)
            {
                //move only on z direction
                moveDir = moveDirZ;
            }
            }
            
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
       

        isWalking = moveDir != Vector3.zero;

        transform.rotation = Quaternion.LookRotation(moveDir);
        float rotationSpeed = 10f;
        Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);

    }
     public bool IsWalking()
    {
        return isWalking;
    }
}
