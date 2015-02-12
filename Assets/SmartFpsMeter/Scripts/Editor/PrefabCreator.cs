/*****************************************************
 * 													 *
 * Asset:		 Smart FPS Meter					 *
 * Script:		 PrefabCreator.cs                    *
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
using UnityEditor;

namespace SmartFPSMeter.Data
{
    public class PrefabCreator : Editor
    {
        private const string mainGOName = "_SmartFpsMeter";
        private const string menuAbbrev = "GameObject/Create Other";


        // CreateJoysManager 
        [MenuItem( menuAbbrev + "/Smart FPS Meter" )]
        private static void CreateMeter()
        {
            GameObject meterGO = new GameObject( mainGOName, typeof( MainMeter ) );
            meterGO.transform.position = Vector3.zero;
            meterGO.GetComponent<MainMeter>().MeterSetup();
        }

        [MenuItem( menuAbbrev + "/Smart FPS Meter", true )]
        private static bool ValidateCreateMeter()
        {
            return !FindObjectOfType<MainMeter>();
        }
    }
}