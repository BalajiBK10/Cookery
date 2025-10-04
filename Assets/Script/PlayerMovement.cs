using System;
using JetBrains.Annotations;
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
        transform.position += moveDir * moveSpeed * Time.deltaTime;

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
