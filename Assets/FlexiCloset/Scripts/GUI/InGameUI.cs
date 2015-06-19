using UnityEngine;
using System.Collections;

public class InGameUI : PersistentSingleton<InGameUI>
{

    public Animator LeftPanel;

	
    // Update is called once per frame
    public void ClickPanel()
    {
        LeftPanel.SetBool("Open", !LeftPanel.GetBool("Open"));
        if (!LeftPanel.GetBool("Open"))
        {
            ModuloUI.Instance.HidePopUp();
        }

    }
    // Update is called once per frame
    public void OffPanel()
    {
        if (LeftPanel.GetBool("Open"))
        {
            LeftPanel.SetBool("Open", false);
            ModuloUI.Instance.HidePopUp();
        }
    }
}
