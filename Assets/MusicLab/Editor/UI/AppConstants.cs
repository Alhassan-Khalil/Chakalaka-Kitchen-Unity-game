using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MusicForge
{
    public class AppConstants
    {
        public static readonly string ASSETPATH = Application.dataPath + "/MusicLab";
        public static readonly string RELATIVEASSETPATH = "Assets/MusicLab";

        public static readonly Texture2D PLAY_IMG   = AssetDatabase.LoadAssetAtPath<Texture2D>(RELATIVEASSETPATH + "/Editor/Images/play.png");
        public static readonly Texture2D PAUSE_IMG  = AssetDatabase.LoadAssetAtPath<Texture2D>(RELATIVEASSETPATH + "/Editor/Images/pause.png");
    }

}