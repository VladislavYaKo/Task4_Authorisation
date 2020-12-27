using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task4_Authorisation.Data.Services
{
    public class TrackStatusesChangeService
    {
        private readonly List<int> unloginedIds;
        public List<int> UnloginedIds => unloginedIds;
        public TrackStatusesChangeService()
        {
            unloginedIds = new List<int>();
        }
        public void Unlogine(int userId)
        {
            if (!unloginedIds.Contains(userId))
                unloginedIds.Add(userId);
        }
        public void Unlogine(int[] usersIds)
        {
            foreach (int id in usersIds)
            {
                if (!unloginedIds.Contains(id))
                    unloginedIds.Add(id);
            }
        }
        public void ResetUnlogine(int userId)
        {
            if (unloginedIds.Contains(userId))
                unloginedIds.Remove(userId);
        }
        public void ResetUnlogine(int[] userIds)
        {
            foreach (int id in userIds)
            {
                if (unloginedIds.Contains(id))
                    unloginedIds.Remove(id);
            }
        }
        public void ResetAll()
        {
            unloginedIds.Clear();
        }
    }
}
