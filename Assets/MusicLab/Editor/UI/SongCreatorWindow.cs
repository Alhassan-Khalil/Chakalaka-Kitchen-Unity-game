using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using InspectorClip;
using SavWav;

namespace WindowClasses
{
    public class SongCreatorWindow : EditorWindow
    {
        public VisualTreeAsset welcomeClassUXML;
        public VisualTreeAsset songCrtClassUXML;

        private static WelcomeClass _welcomeClass;
        private static SongCreatorClass _creatorClass;
        
        [MenuItem("Tools/Music Lab/Song Creator")]
        public static void ShowExample()
        {
            SongCreatorWindow wnd = GetWindow<SongCreatorWindow>();
            wnd.titleContent = new GUIContent("Song Creator");
            wnd.maxSize = new Vector2(650, 500);
            wnd.minSize = wnd.maxSize;
        }

        public void CreateGUI()
        {
            _creatorClass = new SongCreatorClass(songCrtClassUXML);
            _welcomeClass = new WelcomeClass(_creatorClass, welcomeClassUXML);
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;


            _welcomeClass.AddToContainer(root);


        }

        private void OnDestroy()
        {
            InspectorClip.InspectorClip.StopAllClips();
        }
    }
}