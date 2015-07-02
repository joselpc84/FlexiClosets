using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeColorButton : MonoBehaviour
{

    public Color On;
    public Color Off;

    public Image img;
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
                img.color = On;
            }
            else
            {
                img.color = Off;
            }
        }
    }

    public void SetIsShow(bool value)
    {
        _isShow = value;
        if (_isShow)
        {
            img.color = On;
        }
        else
            img.color = Off;

    }
}
    

