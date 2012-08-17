using UnityEditor;
using UnityEngine;
using System;

sealed class DialogueCreateWizard : ScriptableObjectWizard
{
    [MenuItem("Dialogue/Create TagFilter")]
    static void CreateFilterWizard()
    {
        CreateWizard<DialogueCreateWizard, 
            TagFilter>("Resources/Data/Dialogue/").ShowUtility();
    }

    [MenuItem("Dialogue/Create Tag")]
    static void CreateTagWizard()
    {
        CreateWizard<DialogueCreateWizard,
            Tag>("Resources/Data/Dialogue/").ShowUtility();
    }

    [MenuItem("Dialogue/Create Dialogue")]
    static void CreateDialogueWizard()
    {
        CreateWizard<DialogueCreateWizard,
            Dialogue>("Resources/Data/Dialogue/").ShowUtility();
    }


    [MenuItem("Dialogue/Events/Create Audio Event")]
    static void CreateAudioEvent()
    {
        CreateWizard<DialogueCreateWizard,
            DialogueAudioEvent>("Resources/Data/Dialogue/Events/").ShowUtility();
    }

    [MenuItem("Dialogue/Events/Creat Scene Event")]
    static void CreateSceneEvent()
    {
        CreateWizard<DialogueCreateWizard, 
            DialogueSceneEvent>("Resources/Data/Dialogue/Events/").ShowUtility();
    }

    [MenuItem("Dialogue/Events/Create Tag Change Event")]
    static void CreateTagChangeEvent()
    {
        CreateWizard<DialogueCreateWizard,
            DialogueTagChangeEvent>("Resources/Data/Dialogue/Events/").ShowUtility();
    }

    [MenuItem("Dialogue/Events/Create Animation Event")]
    static void CreateAnimationEvent()
    {
        CreateWizard<DialogueCreateWizard,
            DialogueAnimationEvent>("Resources/Data/Dialogue/Events/").ShowUtility();
    }


    [MenuItem("Dialogue/Events/Create Point Change Event")]
    static void CreatePointChangeEvent()
    {
        CreateWizard<DialogueCreateWizard,
            DialoguePointChangeEvent>("Resources/Data/Dialogue/Events/").ShowUtility();
    }

    [MenuItem("Dialogue/Events/Create Remote Change Event")]
    static void CreateRemoteEventEvent()
    {
        CreateWizard<DialogueCreateWizard,
            DialogueRemoteEvent>("Resources/Data/Dialogue/Events/").ShowUtility();
    }
}

