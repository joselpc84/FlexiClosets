using UnityEngine;
using System.Collections;

public class InGameUI : PersistentSingleton<InGameUI>
{

    public Animator LeftPanel;

	
    // Update is called once per frame
    public void ClickPanel()
    {
        LeftPanel.SetBool("Open", !LeftPanel.GetBool("Open"));
    }
    // Update is called once per frame
    public void OffPanel()
    {
        LeftPanel.SetBool("Open", false);
    }
}
