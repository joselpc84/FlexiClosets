/*****************************************************
 * 													 *
 * Asset:		 Smart FPS Meter					 *
 * Script:		 ScreenData.cs               		 *
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
    /// Shows the screen data and quality level information.
    /// </summary>
    [System.Serializable]
    public class ScreenData : BaseData
    {
        internal ScreenData()
        {
            Anchor = TextAnchor.LowerLeft;
            MyColor = new Color32( 238, 100, 0, 255 );
        }
             
        [SerializeField]
        private bool showQL = true;

        [SerializeField]
        private bool showSR = true;

        internal int screenWidth = 0;
        internal int screenHeight = 0;

        /// <summary>
        /// Quality level info string data.
        /// </summary>
        public string DataQL { get; private set; }

        /// <summary>
        /// Screen resolution info string data.
        /// </summary>
        public string DataSR { get; private set; }
        private string scrInfo = string.Empty;
        private string dpiInfo = string.Empty;

        private int currentQualityLevel = 0;
         
        /// <summary>
        /// Shows current quality level.
        /// </summary>
        public bool ShowQL
        {
            get { return showQL; }
            set 
            {
                if( showQL == value ) return;
                showQL = value; 
                outdated = true;
                if( inited ) main.SetAllText();
            }
        }

        /// <summary>
        /// Shows screen info ( resolution, window size & dpi ).
        /// </summary>
        public bool ShowSR
        {
            get { return showSR; }
            set 
            {
                if( showSR == value ) return;
                showSR = value; 
                outdated = true;
                if( inited ) main.SetAllText();
            }
        }

             
        // CalculateSCR
        internal void CalculateSCR()
        {
            if( showQL )
            {
                if( QualitySettings.GetQualityLevel() != currentQualityLevel )
                {
                    currentQualityLevel = QualitySettings.GetQualityLevel();
                    outdated = true;
                    if( inited ) main.SetAllText();
                }
            }

            if( showSR )
            {
                if( Screen.width == screenWidth && Screen.height == screenHeight ) return;

                screenWidth = Screen.width;
                screenHeight = Screen.height;
                outdated = true;
                if( main )
                {
                    main.CalculateFontSize();
                    main.SetAllText();
                }
            }
        }

        //Get Screen Info
        internal string GetScreenInfo()
        {
            if( !outdated ) return dataInfo;
            
            bool needNewLine = false;

            dataText.Length = 0;
            dataText.Append( currentColor );

            // showQL
            if( showQL )
            {
                DataQL = "Quality: " + QualitySettings.names[ currentQualityLevel ];
                dataText.Append( DataQL );
                needNewLine = true;
            }
            else DataQL = null;

            // showSR
            if( showSR )
            {
                if( needNewLine ) dataText.Append( TextLabel.ADD_NEWLINE );

                scrInfo = "Screen: " + Screen.currentResolution.width.ToString() + "x" + Screen.currentResolution.height.ToString() +
                          "@" + Screen.currentResolution.refreshRate.ToString() + "hz" +
                          " [window: " + screenWidth.ToString() + "x" + screenHeight.ToString() ;


                if( Screen.dpi > 0 )
                {
                    dpiInfo = ", dpi: " + Screen.dpi.ToString();
                    DataSR = scrInfo + dpiInfo + "]";
                }
                else DataSR = scrInfo + "]";

                dataText.Append( DataSR );
            }
            else DataSR = null;

            dataText.Append( TextLabel.ADD_ENDLINE );
            dataInfo = dataText.ToString();
            outdated = false;

            return dataInfo;
        }

        // ClearData
        internal override void ClearData()
        {
            base.ClearData();
            DataQL = string.Empty;
            DataSR = string.Empty;
        }
    }
}