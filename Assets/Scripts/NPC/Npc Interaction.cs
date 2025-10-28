using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private NpcDialogue dialogueToShow;
    [SerializeField] private GameObject interactionBox;
    public NpcDialogue DialogueToShow => dialogueToShow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.NpcSelected = this;
            interactionBox.SetActive(true);

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
         if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.NpcSelected = null;
            DialogueManager.Instance.CloseDialoguePanel();
            interactionBox.SetActive(false);

        }
    }

}
