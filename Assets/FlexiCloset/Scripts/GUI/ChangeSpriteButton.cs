using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeSpriteButton : MonoBehaviour
{
    public Sprite On;
    public Sprite Off;
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
                img.sprite = On;
            }
            else
            {
                img.sprite = Off;
            }
        }
    }


}
