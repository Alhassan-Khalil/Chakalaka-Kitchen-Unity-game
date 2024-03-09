using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using InspectorClip;
using MusicForge;

namespace WindowClasses
{
    public class PlayPauseBtn : VisualElement
    {
        private Button _playBtn;
        private bool _isPlaying;

        public bool GetIsPlaying
        {
            get { return _isPlaying; }
        }

        public PlayPauseBtn(VisualElement parent)
        {
            _isPlaying = false;
            _playBtn = parent.Q<Button>("PlayBtn");
            _playBtn.clicked += ChangeStateOfSong;
            _playBtn.style.backgroundImage = AppConstants.PLAY_IMG;

        }


        public void ChangeStateOfSong()
        {
            if (!_isPlaying)
            {
                InspectorClip.InspectorClip.PlayClip();
                ChangePlayIconTo(BUTTON_STATES.PAUSE);
                _isPlaying = true;
            }
            else
            {
                InspectorClip.InspectorClip.StopAllClips();
                ChangePlayIconTo(BUTTON_STATES.PLAY);
                _isPlaying = false;
            }
        }

        private void ChangePlayIconTo(BUTTON_STATES buttonState)
        {
            if (buttonState == BUTTON_STATES.PLAY)
            {
                _playBtn.style.backgroundImage = AppConstants.PLAY_IMG;
            }
            if (buttonState == BUTTON_STATES.PAUSE)
            {
                _playBtn.style.backgroundImage = AppConstants.PAUSE_IMG;
            }
        }

    }

    public enum BUTTON_STATES
    {
        PLAY, PAUSE
    }
}