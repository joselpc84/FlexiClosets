/*****************************************************
 * 													 *
 * Asset:		 Smart FPS Meter					 *
 * Script:		 BaseData.cs                  		 *
 * 													 *
 * Copyright(c): Victor Klepikov					 *
 * Support: 	 http://bit.ly/vk-Support			 *
 * 													 *
 * mySite:       http://vkdemos.ucoz.org			 *
 * myAssets:     http://u3d.as/5Fb                   *
 * myTwitter:	 http://twitter.com/VictorKlepikov	 *
 * myFacebook:	 http://www.facebook.com/vikle4 	 *
 * 													 *
 ****************************************************/


using UnityEngine;
using SmartFPSMeter.labels;

namespace SmartFPSMeter.Data
{
    /// <summary>
    /// Сonditions for meters, yet using only fps meter
    /// </summary>
    public enum Сonditions : byte
    {
        Fine,
        Warning,
        Danger
    }

    /// <summary>
    /// Bace class for all meters.
    /// </summary>
    [System.Serializable]
    public abstract class BaseData
    {
        [SerializeField]
        protected bool enabled = true;

        [SerializeField]
        protected TextAnchor anchor = TextAnchor.UpperRight;

        [SerializeField]
        protected Color32 myColor = new Color32( 255, 255, 255, 255 );
        protected string currentColor = string.Empty;

        protected System.Text.StringBuilder dataText = new System.Text.StringBuilder();
        protected string dataInfo = string.Empty;
        protected bool outdated = false;

        protected static MainMeter main = null;
        protected static bool inited = false;

#if UNITY_EDITOR
        [HideInInspector]
        public bool foldout = true;
#endif

        internal static void Init( MainMeter reference )
        {
            main = reference;
            inited = true;
        }

        /// <summary>
        /// Enables or disables meter with immediate label refresh.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if( enabled == value ) return;
                enabled = value;

                if( enabled )
                {
                    DataAwake();
                }
                else
                {
                    if( inited ) main.ClearData();
                    ClearData();
                }
            }
        }

        /// <summary>
        /// Sets the memory data label position.
        /// </summary>
        public TextAnchor Anchor
        {
            get { return anchor; }
            set
            {
                if( anchor == value ) return;
                anchor = value;
                outdated = true;
                if( inited ) main.SetAllText( true );
            }
        }

        /// <summary>
        /// Text color for this meter.
        /// </summary>
        public Color32 MyColor
        {
            get { return myColor; }
            set
            {
                if( myColor.a == value.a && myColor.b == value.b && myColor.g == value.g && myColor.r == value.r ) 
                    return;

                myColor = value;
                currentColor = TextLabel.ColorToString( myColor );
                outdated = true;
                if( inited ) main.SetAllText( false );
            }
        }


        // DataAwake
        internal virtual void DataAwake()
        {
            currentColor = TextLabel.ColorToString( myColor );
            dataInfo = string.Empty;
            outdated = true;
            if( inited ) main.SetAllText( true );
        }

        // ClearData
        internal virtual void ClearData()
        {
            dataText.Length = 0;
        }
    }
}