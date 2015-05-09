using UnityEngine;
using System.Collections;

public class ManagerUI : PersistentSingleton<ManagerUI>
{

    protected Animator _anim;

    public Animator animator
    {
    
        get
        {
            if (_anim == null)
                _anim = GetComponent<Animator>();
            return _anim;
        }
    }

    public void ChangueToInGame()
    {
        animator.SetTrigger("InGame");
        animator.SetTrigger("Out");
    }
}