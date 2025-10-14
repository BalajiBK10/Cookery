using UnityEngine;

// This script represents a kitchen counter that the player can interact with.
// Think of it like a table in your home where you place or pick up items.
public class ClearCounter : MonoBehaviour
{
    // Called when the player interacts with this counter.
    // Example: Like tapping the table to place or grab something.
    public void Interact()
    {
        // For now, just print a message to show interaction happened.
        // Imagine you're saying "I touched the counter!" in the game.
        Debug.Log("Interact!");
    }
}
