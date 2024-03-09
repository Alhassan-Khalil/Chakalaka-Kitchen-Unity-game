using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using MusicForge;
using SavWav;
using WindowClasses;
using System;

namespace SavWav
{
    public class SaveClass : WindowClass
    {
        private static string _path;
        public Image songResult;

        public SaveClass()
        {
            VisualElements = new List<VisualElement>();
            songResult.scaleMode = ScaleMode.ScaleToFit;
        }

        #region CallBacks
        static void SaveSong()
        {
            if (_path != null || _path.CompareTo("") != 0)
            {
                if (MusicForge.Music_Forge.Clip != null)
                {
                    SavWav.Save(_path, Music_Forge.Clip);
                    AssetDatabase.Refresh();
                }
                else Debug.Log("No song created");
            }
        }

        public static void OpenExamineWindow()
        {
            _path = EditorUtility.SaveFilePanel(
                "Save texture as PNG",
                "",
                Music_Forge.ClipName + TimeStamp(DateTime.Now),
                "wav");
            if (_path != null && _path != "")
            {
                SaveSong();
            }
            
        }
        #endregion
        public static string TimeStamp(DateTime value)
        {
            return value.ToString("yyyy_mm_dd_hh_mm_ss");
        }
    }
}