using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DialogueManager : Singleton<DialogueManager>
{
    public static event Action<InteractionType> OnExtraInteractionEvent;
    [Header("Config")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image npcIcon;
    [SerializeField] private TextMeshProUGUI npcNameTMP;
    [SerializeField] private TextMeshProUGUI npcDialogueTMP;

    public NpcInteraction NpcSelected { get; set; }
    private bool dialogueStarted;
    private PlayerActions actions;
    private Queue<string> dialogueQueue = new Queue<string>();
    protected override void Awake()
    {
        base.Awake();
        actions = new PlayerActions();

    }
    private void Start()
    {
        actions.Dialogue.Interact.performed += ctx => ShowDialogue();
        actions.Dialogue.Continue.performed += ctx => ContinueDialogue();

    }
    public void CloseDialoguePanel()
    {
        dialoguePanel.SetActive(false);
        dialogueStarted = false;
        dialogueQueue.Clear();
    }
    private void LoadDialogueFromNpc()
    {
        if (NpcSelected.DialogueToShow.Dialogue.Length <= 0) return;
        foreach (string sentence in NpcSelected.DialogueToShow.Dialogue)
        {
            dialogueQueue.Enqueue(sentence); 
        }

    }
    private void ShowDialogue()
    {
        if (NpcSelected == null) return;
        if (dialogueStarted) return;
        dialoguePanel.SetActive(true);
        LoadDialogueFromNpc();
        npcIcon.sprite = NpcSelected.DialogueToShow.Icon;
        npcNameTMP.text = NpcSelected.DialogueToShow.Name;
        npcDialogueTMP.text = NpcSelected.DialogueToShow.Greeting;
        dialogueStarted = true;
    }
    private void ContinueDialogue()
    {
        if (NpcSelected == null)
        {
            dialogueQueue.Clear();
            return;
        }

        if (dialogueQueue.Count <= 0)
        {
            CloseDialoguePanel();
            if (NpcSelected.DialogueToShow.HasInteraction)
            {
                OnExtraInteractionEvent?.Invoke(NpcSelected.DialogueToShow.InteractionType);
            }
            return;
        }
        npcDialogueTMP.text = dialogueQueue.Dequeue();
    }

    void OnEnable()
    {
            actions.Enable();
    }
    void OnDisable()
    {
        actions.Disable();
    }
        

}
