using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    private const string IS_WALKING = "IsWalking";
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
      
    }

    // Update is called once per frame
    void Update()
    {
          animator.SetBool(IS_WALKING,playerMovement.IsWalking());
    }
}
