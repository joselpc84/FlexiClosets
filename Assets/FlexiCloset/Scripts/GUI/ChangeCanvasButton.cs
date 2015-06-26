using UnityEngine;
using System.Collections;

public class ChangeCanvasButton : MonoBehaviour
{

    public CanvasGroup canvas;
    bool _isShow = true;

    public bool isShow
    {

        get
        {

            return _isShow;
        }
        set
        {
            _isShow = !_isShow;
            if (_isShow)
            {
                canvas.alpha = 1;
                canvas.blocksRaycasts = true;

            }
            else
            {
                canvas.alpha = 0;
                canvas.blocksRaycasts = false;
            }
        }
    }
}
