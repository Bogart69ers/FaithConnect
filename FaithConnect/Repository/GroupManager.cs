using System;
using System.Collections.Generic;
using System.Linq;
using FaithConnect.Utils;

namespace FaithConnect.Repository
{
    public class GroupManager
    {
        private BaseRepository<Groups> _groupRepo;
        private BaseRepository<GroupMembership> _membershipRepo;

        public GroupManager()
        {
            _groupRepo = new BaseRepository<Groups>();
            _membershipRepo = new BaseRepository<GroupMembership>();
        }

        public List<Groups> GetAllGroups()
        {
            return _groupRepo.GetAll();
        }

        public List<GroupMembership> ListGroupsJoinedByUserId(int id)
        {
            return _membershipRepo._table.Where(m => m.userId == id).ToList();
        }
        public List<GroupMembership> GetAllGroupMemberships()
        {
            return _membershipRepo.GetAll();
        }

        public List<Groups> GetJoinedGroups(int userId)
        {
            var joinedGroupIds = _membershipRepo.GetAll()
                                                 .Where(m => m.userId == userId && m.status == (Int32)MembershipStatus.Joined)
                                                 .Select(m => m.groupId)
                                                 .ToList();
            return _groupRepo.GetAll()
                              .Where(g => joinedGroupIds.Contains(g.id))
                              .ToList();
        }

        public List<Groups> GetPendingGroups(int userId)
        {
            var pendingGroupIds = _membershipRepo.GetAll()
                                                  .Where(m => m.userId == userId && m.status == (Int32)MembershipStatus.Pending)
                                                  .Select(m => m.groupId)
                                                  .ToList();
            return _groupRepo.GetAll()
                              .Where(g => pendingGroupIds.Contains(g.id))
                              .ToList();
        }

        public Groups GetGroupById(int id)
        {
            return _groupRepo.Get(id);
        }

        public ErrorCode CreateGroup(Groups group, ref string errMsg)
        {
            return _groupRepo.Create(group, out errMsg);
        }

        public ErrorCode DeleteGroup(int id, ref string errMsg)
        {
            return _groupRepo.Delete(id, out errMsg);
        }

        public ErrorCode AddGroupMembership(GroupMembership membership, ref string errMsg)
        {
            return _membershipRepo.Create(membership, out errMsg);
        }

        public ErrorCode RemoveGroupMembership(int membershipId, ref string errMsg)
        {
            return _membershipRepo.Delete(membershipId, out errMsg);
        }
    }
}
