using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using MusicForge;

namespace WindowClasses
{
    public class WelcomeClass : WindowClass
    {
        
        private VisualTreeAsset     _uxml;
        private SongCreatorClass    _creatorClass;
        private Button              _continueBtn;

        public WelcomeClass(SongCreatorClass creatorClass, VisualTreeAsset UXML)
        {
            _creatorClass = creatorClass;
            _uxml = UXML;
            VisualElements = new List<VisualElement>();
            Texture logo = AssetDatabase.LoadAssetAtPath<Texture>(AppConstants.RELATIVEASSETPATH + "/Editor/Images/01LogoBlanco.png");

        }


        public override void AddToContainer(VisualElement container)
        {
            root = container;
            _uxml.CloneTree(root);
            _continueBtn = root.Q<Button>("ContinueBtn");
            _continueBtn.clicked += moveToCreatorClass;
        }


        void moveToCreatorClass()
        {
            RemoveFromContainer();
            _creatorClass.AddToContainer(root);
        }

    }
}