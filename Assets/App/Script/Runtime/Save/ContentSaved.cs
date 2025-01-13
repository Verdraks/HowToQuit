using System.Collections.Generic;

namespace BT.Save
{
    [System.Serializable]
    public class ContentSaved
    {
        public List<string> achievementsIdCompleted = new();
        public string lastAchievementCompletedId;
    }
}

