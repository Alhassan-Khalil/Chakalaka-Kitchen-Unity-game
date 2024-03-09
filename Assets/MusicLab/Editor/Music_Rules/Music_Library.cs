using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLScriptableObjs;

namespace MLScriptableObjs
{
    [CreateAssetMenu(fileName = "Library", menuName = "ScriptableObjects/LibraryData", order = 3)]
    [System.Serializable]
    public class Music_Library : ScriptableObject
    {
        public List<Genre_Data> GenreList;
        private List<string> _genreNames;

        public List<string> GenreNames {
            get {
                _genreNames= new List<string>();
                for (int i = 0; i < GenreList.Count; i++)
                {
                    _genreNames.Add(GenreList[i].name);
                }
                return _genreNames; }
        }

        public Genre_Data GetGenreFromName(string name)
        {
            Genre_Data res = null;
            for (int i = 0; i < GenreList.Count; i++)
            {
                if (string.Compare(name, GenreList[i].name) == 0)
                {
                    res = GenreList[i];
                    break;
                }
            }

            return res;
        }
    }
}