using UnityEngine;
using System;
using SmartFPSMeter;
using SmartFPSMeter.labels;

public class API_Demo : MonoBehaviour 
{
    private enum dataEnums : byte { MainData, FPSData, MemoryData, HardwareInfo, ScreenData, LevelInfo }
    private dataEnums dataEnum = dataEnums.MainData;

    private string[] stateNames = { "OFF", "ON" };


    // OnGUI
    void OnGUI() 
    {
        GUILayout.BeginArea( new Rect( 25, 125, Screen.width - 50, Screen.height - 250 ) );
        GUILayout.BeginVertical( "Box" );

        GUIStyle style = GUI.skin.GetStyle( "Label" );
        style.richText = true;
        style.alignment = TextAnchor.UpperCenter;        
        style.normal.textColor = Color.red;
        GUILayout.Label( "<b>API Demo</b>", style );
        style.richText = false;
        style.alignment = TextAnchor.UpperLeft;
        style.normal.textColor = Color.white;
        
        dataEnum = ( dataEnums ) GUILayout.Toolbar( ( int )dataEnum, Enum.GetNames( typeof( dataEnums ) ) );

        GUILayout.Space( 10 );

        GUILayout.BeginVertical( "Box" );
        GUILayout.Space( 15 );
        switch( dataEnum )
        {
            case dataEnums.MainData:     MainData();     break;
            case dataEnums.FPSData:      FPSData();      break;
            case dataEnums.MemoryData:   MemoryData();   break;
            case dataEnums.HardwareInfo: HardwareInfo(); break;
            case dataEnums.ScreenData:   ScreenData();   break;
            case dataEnums.LevelInfo:    LevelInfo();    break;
        }
        GUILayout.EndVertical();

        GUILayout.EndVertical();
        GUILayout.EndArea();        
	}
    

    // MainData
    private void MainData()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label( "Meter Modes", GUILayout.Width( 100 ) );
        MainMeter.Instance.MeterMode = ( MeterModes ) GUILayout.Toolbar( ( int )MainMeter.Instance.MeterMode, Enum.GetNames( typeof( MeterModes ) ) );
        GUILayout.EndHorizontal();

        GUILayout.Space( 10 );

        GUILayout.BeginHorizontal();
        GUILayout.Label( "Corner", GUILayout.Width( 100 ) );
        TextAnchor targetCorner = MainMeter.Instance.targetCorner;
        targetCorner = ( TextAnchor )GUILayout.Toolbar( ( int )targetCorner, Enum.GetNames( typeof( TextAnchor ) ) );
        MainMeter.Instance.targetCorner = targetCorner;
        GUILayout.EndHorizontal();
        
        GUILayout.Space( 10 );

        float lineSpacing = MainMeter.Instance.LabelLineSpacing;
        lineSpacing = customSlider( "Line Spacing", 100, lineSpacing, 1f, 4f );
        MainMeter.Instance.LabelLineSpacing = lineSpacing;

        GUILayout.Space( 10 );

        float minOffsetX = 0f;
        float minOffsetY = 0f;
        if( targetCorner == TextAnchor.LowerCenter || targetCorner == TextAnchor.UpperCenter ) minOffsetX = -35f;
        else if( targetCorner == TextAnchor.MiddleCenter )
        {
            minOffsetX = -35f;
            minOffsetY = -35f;
        }
        else if( targetCorner == TextAnchor.MiddleLeft || targetCorner == TextAnchor.MiddleRight ) minOffsetY = -35f;


        float offsetX = MainMeter.Instance.LabelOffsetX;
        offsetX = customSlider( "Offset X", 100, offsetX, minOffsetX, 35f );
        MainMeter.Instance.LabelOffsetX = offsetX;

        float offsetY = MainMeter.Instance.LabelOffsetY;
        offsetY = customSlider( "Offset Y", 100, offsetY, minOffsetY, 35f );
        MainMeter.Instance.LabelOffsetY = offsetY;

        GUILayout.Space( 10 );
   
        float fontSize = MainMeter.Instance.FontSize;
        fontSize = customSlider( "Font Size", 100, fontSize, 1f, 35f ); 
        MainMeter.Instance.FontSize = ( int )fontSize;
    }

    // FPSData
    private void FPSData()
    {
        TextAnchor anchor = MainMeter.Instance.fpsData.Anchor;
        bool onOff = MainMeter.Instance.fpsData.Enabled;
        customToolbars( "FPS Data", ref onOff, ref anchor );
        MainMeter.Instance.fpsData.Enabled = onOff;
        MainMeter.Instance.fpsData.Anchor = anchor;

        MainMeter.Instance.fpsData.UpdateInterval = customSlider( "Update Interval", 100, MainMeter.Instance.fpsData.UpdateInterval, 0.1f, 5f );

        MainMeter.Instance.fpsData.ShowMS = GUILayout.Toggle( MainMeter.Instance.fpsData.ShowMS, "  Show MS" );
        MainMeter.Instance.fpsData.ShowFPS = GUILayout.Toggle( MainMeter.Instance.fpsData.ShowFPS, "  Show FPS" );
        MainMeter.Instance.fpsData.ShowMinMax = GUILayout.Toggle( MainMeter.Instance.fpsData.ShowMinMax, "  Show [Min..Max]" );

        int warningValue = MainMeter.Instance.fpsData.WarningValue;
        int dangerValue = MainMeter.Instance.fpsData.DangerValue;

        warningValue = ( int )customSlider( "Warning Value", 100, warningValue, dangerValue, 60 );
        dangerValue = ( int )customSlider( "Danger Value", 100, dangerValue, 1, warningValue );

        MainMeter.Instance.fpsData.WarningValue = warningValue;
        MainMeter.Instance.fpsData.DangerValue = dangerValue;

        GUILayout.Space( 15 );

        GUIStyle style = GUI.skin.GetStyle( "Label" );
        style.alignment = TextAnchor.UpperRight;
        if( MainMeter.Instance.MeterMode != MeterModes.Disable && MainMeter.Instance.fpsData.Enabled )
            GUILayout.Label( "FPS Value Сondition: " + MainMeter.Instance.fpsData.FPSСondition.ToString(), style );
        else
            GUILayout.Label( "", style );
        GUILayout.Label( MainMeter.Instance.fpsData.DataMS, style );
        GUILayout.Label( MainMeter.Instance.fpsData.DataFPS, style );
    }

    // MemoryData
    private void MemoryData()
    {
        TextAnchor anchor = MainMeter.Instance.memData.Anchor;
        bool onOff = MainMeter.Instance.memData.Enabled;
        customToolbars( "Memory Data", ref onOff, ref anchor );
        MainMeter.Instance.memData.Enabled = onOff;
        MainMeter.Instance.memData.Anchor = anchor;
        
        MainMeter.Instance.memData.UpdateInterval = customSlider( "Update Interval", 100, MainMeter.Instance.memData.UpdateInterval, 0.1f, 5f );

        MainMeter.Instance.memData.DecimalMEM = GUILayout.Toggle( MainMeter.Instance.memData.DecimalMEM, "  Decimal" );
        MainMeter.Instance.memData.ShowMEMmono = GUILayout.Toggle( MainMeter.Instance.memData.ShowMEMmono, "  Show [mono]" );
        MainMeter.Instance.memData.ShowMEMalloc = GUILayout.Toggle( MainMeter.Instance.memData.ShowMEMalloc, "  Show [alloc]" );
        MainMeter.Instance.memData.ShowMEMreserv = GUILayout.Toggle( MainMeter.Instance.memData.ShowMEMreserv, "  Show [reserv]" );

        GUILayout.Space( 15 );

        GUIStyle style = GUI.skin.GetStyle( "Label" );
        style.alignment = TextAnchor.UpperRight;
        GUILayout.Label( MainMeter.Instance.memData.DataMEMmono, style );
        GUILayout.Label( MainMeter.Instance.memData.DataMEMalloc, style );
        GUILayout.Label( MainMeter.Instance.memData.DataMEMreserv, style );
    }

    // HardwareInfo
    private void HardwareInfo()
    {
        TextAnchor anchor = MainMeter.Instance.hrwData.Anchor;
        bool onOff = MainMeter.Instance.hrwData.Enabled;
        customToolbars( "Hardware Info", ref onOff, ref anchor );
        MainMeter.Instance.hrwData.Enabled = onOff;
        MainMeter.Instance.hrwData.Anchor = anchor;

        MainMeter.Instance.hrwData.ShowCPU = GUILayout.Toggle( MainMeter.Instance.hrwData.ShowCPU, "  Show CPU" );
        MainMeter.Instance.hrwData.ShowRAM = GUILayout.Toggle( MainMeter.Instance.hrwData.ShowRAM, "  Show RAM" );
        MainMeter.Instance.hrwData.ShowGPU = GUILayout.Toggle( MainMeter.Instance.hrwData.ShowGPU, "  Show GPU" );
        MainMeter.Instance.hrwData.ShowGDV = GUILayout.Toggle( MainMeter.Instance.hrwData.ShowGDV, "  Show GDV" );

        GUILayout.Space( 15 );

        GUIStyle style = GUI.skin.GetStyle( "Label" );
        style.alignment = TextAnchor.UpperRight;
        GUILayout.Label( MainMeter.Instance.hrwData.DataCPU, style );
        GUILayout.Label( MainMeter.Instance.hrwData.DataRAM, style );
        GUILayout.Label( MainMeter.Instance.hrwData.DataGPU, style );
        GUILayout.Label( MainMeter.Instance.hrwData.DataGDV, style );
    }

    // ScreenData
    private void ScreenData()
    {
        TextAnchor anchor = MainMeter.Instance.scrData.Anchor;
        bool onOff = MainMeter.Instance.scrData.Enabled;
        customToolbars( "Screen Data", ref onOff, ref anchor );
        MainMeter.Instance.scrData.Enabled = onOff;
        MainMeter.Instance.scrData.Anchor = anchor;

        MainMeter.Instance.scrData.ShowQL = GUILayout.Toggle( MainMeter.Instance.scrData.ShowQL, "  Quality Level" );
        MainMeter.Instance.scrData.ShowSR = GUILayout.Toggle( MainMeter.Instance.scrData.ShowSR, "  Screen Resolution" );

        GUILayout.Space( 15 );

        GUIStyle style = GUI.skin.GetStyle( "Label" );
        style.alignment = TextAnchor.UpperRight;
        GUILayout.Label( MainMeter.Instance.scrData.DataQL, style );
        GUILayout.Label( MainMeter.Instance.scrData.DataSR, style );
    }

    // LevelInfo
    private void LevelInfo()
    {
        TextAnchor anchor = MainMeter.Instance.levData.Anchor;
        bool onOff = MainMeter.Instance.levData.Enabled;
        customToolbars( "Screen Data", ref onOff, ref anchor );
        MainMeter.Instance.levData.Enabled = onOff;
        MainMeter.Instance.levData.Anchor = anchor;

        MainMeter.Instance.levData.ShowLN = GUILayout.Toggle( MainMeter.Instance.levData.ShowLN, "  Level Name" );
        MainMeter.Instance.levData.ShowPT = GUILayout.Toggle( MainMeter.Instance.levData.ShowPT, "  Play Time" );

        GUILayout.Space( 15 );

        GUIStyle style = GUI.skin.GetStyle( "Label" );
        style.alignment = TextAnchor.UpperRight;
        GUILayout.Label( MainMeter.Instance.levData.DataLN, style );
        GUILayout.Label( MainMeter.Instance.levData.DataPT, style );
    }
    

    // customToolbars
    private void customToolbars( string label, ref bool onOff, ref TextAnchor anchor )
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label( label, GUILayout.Width( 100 ) );
        onOff = Convert.ToBoolean( GUILayout.Toolbar( Convert.ToInt32( onOff ), stateNames ) );
        GUILayout.EndHorizontal();

        GUILayout.Space( 5 );

        GUILayout.BeginHorizontal();
        GUILayout.Label( "Anchor", GUILayout.Width( 100 ) );
        anchor = ( TextAnchor )GUILayout.Toolbar( ( int )anchor, Enum.GetNames( typeof( TextAnchor ) ) );
        GUILayout.EndHorizontal();

        GUILayout.Space( 5 );
    }

    // customSlider
    private float customSlider( string label, int width, float currentValue, float minValue, float maxValue )
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label( label, GUILayout.Width( width ) );
        currentValue = GUILayout.HorizontalSlider( currentValue, minValue, maxValue );
        GUILayout.Space( 10 );
        GUILayout.Label( string.Format( "{0:F2}", currentValue ), GUILayout.MaxWidth( 50 ) );
        GUILayout.EndHorizontal();
        return currentValue;
    }
}
