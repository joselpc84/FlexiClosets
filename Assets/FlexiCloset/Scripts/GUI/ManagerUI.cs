using UnityEngine;
using System.Collections;

public class ManagerUI : PersistentSingleton<ManagerUI>
{
    public Animator Menus_Intro;

    public Animator Menus_InGame;

    public string InTrigger = "In";
    public string OutTrigger = "Out";

    void Start()
    {
        // Menus_Intro.SetTrigger(InTrigger);
        Menus_InGame.SetTrigger(InTrigger);

    }

    public void ChangueToInGame()
    {
        Menus_Intro.SetTrigger(OutTrigger);
        Menus_InGame.SetTrigger(InTrigger);
    }
        
}