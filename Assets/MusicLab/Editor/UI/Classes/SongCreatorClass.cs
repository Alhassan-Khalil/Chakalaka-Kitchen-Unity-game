using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MusicForge;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using MLScriptableObjs;
using SavWav;
using WFC;
using TMPro;

namespace WindowClasses
{
    public class SongCreatorClass : WindowClass
    {
        private VisualTreeAsset _uxml;


        private DropdownField   _genrePopup;
        private DropdownField   _moodPopup;
        private Button          _createButton;
        private Button          _saveSong;
        private Button          _playSong;
        private VisualElement   _idImage;
        private IntegerField    _songDurIntF;
        private Label           _durationLbl;
        private PlayPauseBtn    _playPauseBtn;


        private static int              _songDuration;
        private static Music_Library    _currentLibrary;
        private static Genre_Data       _currentGenre;
        private static Mood_Data        _currentMood;

        private SaveClass _saveClass;

        public static Music_Library CurrentLibrary { get { return _currentLibrary; } }
        public static Genre_Data CurrentGenre { get { return _currentGenre; } }
        public static Mood_Data CurrenMood { get { return _currentMood; } }

        public static int SongDuration { get { return _songDuration; } }

        public SongCreatorClass(VisualTreeAsset UXML)
        {
            VisualElements = new List<VisualElement>();

            _uxml = UXML;
        }

        public override void AddToContainer(VisualElement container)
        {
            root = container;
            _uxml.CloneTree(root);

            _genrePopup     = root.Q<DropdownField>("GenreDrpD");
            _saveSong       = root.Q<Button>("SaveBtn");
            _playSong       = root.Q<Button>("PlayBtn");
            _createButton   = root.Q<Button>("CreateSongBtn");
            _moodPopup      = root.Q<DropdownField>("MoodDrpD");
            _idImage        = root.Q<VisualElement>("SongId");
            _songDurIntF    = root.Q<IntegerField>("SongDuration");
            _durationLbl    = root.Q<Label>("DurationLbl");
            _playPauseBtn   = new PlayPauseBtn(root);
            
            _createButton   .clicked += UpdateWindows;
            _saveSong       .clicked += SaveClass.OpenExamineWindow;
            _genrePopup     .RegisterCallback<ChangeEvent<string>>(OnGenreChange);
            _moodPopup      .RegisterCallback<ChangeEvent<string>>(OnMoodChange);
            _songDurIntF    .RegisterCallback<ChangeEvent<int>>(UpdateTimeDuration);
            
            LoadLibrary();
            UpdateTimeDuration();
        }

        private void LoadLibrary()
        {
            _currentLibrary = RulesLoader.LibraryFromEditorAssets("VanillaLibrary");

            _genrePopup.choices = _currentLibrary.GenreNames;
            _genrePopup.index = 0;
            _currentGenre = _currentLibrary.GetGenreFromName(_genrePopup.value);

            _moodPopup.choices = _currentGenre.GetMoodNames;
            _moodPopup.index = 0;
            _currentMood = _currentGenre.GetMoodsFromName(_moodPopup.value);

        }


        #region Events


        private void OnGenreChange(ChangeEvent<string> evt)
        {
            _currentGenre = _currentLibrary.GetGenreFromName(evt.newValue);
            _moodPopup.choices = _currentGenre.GetMoodNames;
            _moodPopup.index = 0;
            UpdateTimeDuration();

        }

        private void OnMoodChange(ChangeEvent<string> evt)
        {
            _currentMood = _currentGenre.GetMoodsFromName(evt.newValue);
        }

        private void UpdateWindows()
        {
            _songDuration       = _songDurIntF.value;
            _saveSong.visible   = true;
            _playSong.visible   = true;
            
            Music_Forge.CreateSong();

            UpdateIDImage();
        }

        private void UpdateIDImage()
        {
            _idImage.style.backgroundImage = Music_Forge.ResImage;
            _idImage.style.width = new StyleLength(_songDuration * 20);
        }

        private void UpdateTimeDuration(ChangeEvent<int> evt)
        {
            if (evt.newValue > 0)
            {
                float seconds = evt.newValue * (CurrenMood.fragmentList[0].clip_fragment.length - (CurrentGenre.crossfadeAmount / 1000f));
                seconds = Mathf.Round(seconds * 100) / 100;
                _durationLbl.text = "Song duration: " + seconds + " s.";
            }
            else
                ((IntegerField)evt.currentTarget).value = 1;
        }

        private void UpdateTimeDuration()
        {
            float seconds = CurrenMood.fragmentList[0].clip_fragment.length - (CurrentGenre.crossfadeAmount / 1000f);
            seconds = Mathf.Round(seconds * 100) / 100;
            _durationLbl.text = "Song duration: " + seconds + " s.";
        }
        #endregion
    }
}