using System;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour

{
    public static PlayerMovement Instance{ get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }
   
    private ClearCounter clearCounter;
    [SerializeField] LayerMask counterLayerMask;
    [SerializeField] float moveSpeed = 5f;
    Vector3 lastInteraction;

    bool isWalking;

    [SerializeField] private GameInput gameInput;

    private ClearCounter selectedCounter;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one player instance so the error occurs");
        }
        Instance = this;
    }

    void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }       
    void Update()
    {
        HandleInteraction();
        HandlePlayerMovement();
    }
    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteraction()
    {
        // HandleInteraction:
        // Casts a ray in the last movement direction to detect counters.
        // If a ClearCounter is hit, calls its Interact method with the player reference.

        Vector2 inputVector = gameInput.GetVectorInputNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float interactDistance = 2f;

        if (moveDir != Vector3.zero)
        {
            lastInteraction = moveDir;
        }
        if (Physics.Raycast(transform.position, lastInteraction, out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (clearCounter != selectedCounter)
                {
                    SetSelectedChanged(clearCounter);

               }

            }
            else
            {
                SetSelectedChanged(null);
            }

        }
        else
        {
            SetSelectedChanged(null);
        }


    }

    private void HandlePlayerMovement()
    {
        // HandlePlayerMovement:
        // Uses CapsuleCast to check if movement is possible.
        // If blocked diagonally, tries X-only and Z-only movement.
        // Updates position, walking state, and rotation.

        Vector2 inputVector = gameInput.GetVectorInputNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);


        if (!canMove)
        {
            //cannot move towards moveDir
            // if we are trying to move both forwards and right 
            //Attempt only on x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
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
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //move only on z direction
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move on any direction
                }
            }

        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance; // moveDistance is the variable for moveSpeed and time.delta time which is declared above the if statement
        }


        isWalking = moveDir != Vector3.zero;
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
        }
        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }
    
    private void SetSelectedChanged(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
         {           selectedCounter =selectedCounter     
              });
    }
   
    
}
