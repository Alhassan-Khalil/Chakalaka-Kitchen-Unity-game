using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLScriptableObjs
{
    [CreateAssetMenu(fileName = "Genre_Data", menuName = "ScriptableObjects/Genre data", order = 1)]
    [System.Serializable]
    public class Genre_Data : ScriptableObject
    {
        public int crossfadeAmount = 427;
        public List<Mood_Data> moodList;

        private List<string> _moodNames;

        public List<string> GetMoodNames
        {
            get
            {
                _moodNames = new List<string>();
                for (int i = 0; i < moodList.Count; i++)
                {
                    _moodNames.Add(moodList[i].name);
                }
                return _moodNames;
            }
        }

        public Mood_Data GetMoodsFromName(string name)
        {
            Mood_Data res = null;
            for (int i = 0; i < moodList.Count; i++)
            {
                if (string.Compare(name, moodList[i].name) == 0)
                {
                    res = moodList[i];
                    break;
                }
            }

            return res;
        }
    }
}