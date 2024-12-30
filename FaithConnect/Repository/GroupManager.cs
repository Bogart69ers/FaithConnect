using System;
using System.Collections.Generic;
using System.Linq;
using FaithConnect.Utils;

namespace FaithConnect.Repository
{
    public class GroupManager
    {
        private readonly BaseRepository<Groups> _groupRepo;
        private readonly BaseRepository<GroupMembership> _membershipRepo;

        public GroupManager()
        {
            _groupRepo = new BaseRepository<Groups>();
            _membershipRepo = new BaseRepository<GroupMembership>();
        }
        public ErrorCode UpdateGroup(Groups group, ref string errMsg)
        {
            try
            {
                return _groupRepo.Update(group.id ,group, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return ErrorCode.Error;
            }
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

        private List<int> GetGroupIdsByStatus(int userId, MembershipStatus status)
        {
            return _membershipRepo.GetAll()
                                  .Where(m => m.userId == userId && m.status == (int)status && m.groupId.HasValue) // Filter out nulls
                                  .Select(m => m.groupId.Value) // Convert int? to int
                                  .ToList();
        }


        public List<Groups> GetJoinedGroups(int userId)
        {
            var joinedGroupIds = GetGroupIdsByStatus(userId, MembershipStatus.Joined);
            return _groupRepo.GetAll().Where(g => joinedGroupIds.Contains(g.id)).ToList();
        }

        public List<Groups> GetPendingGroups(int userId)
        {
            var pendingGroupIds = GetGroupIdsByStatus(userId, MembershipStatus.Pending);
            return _groupRepo.GetAll().Where(g => pendingGroupIds.Contains(g.id)).ToList();
        }


        public List<GroupMembership> GetMembershipsByGroupId(int groupId)
        {
            return _membershipRepo.GetAll()
                                  .Where(m => m.groupId == groupId && m.status == 1)
                                  .ToList(); // Get all group memberships with status = 1
        }

        public List<GroupMembership> GetMembershipsByGroup(int groupId)
        {
            return _membershipRepo.GetAll()
                                  .Where(m => m.groupId == groupId && m.status == 0)
                                  .ToList(); // Get all group memberships with status = 1
        }

        public Groups GetGroupById(int id)
        {
            return _groupRepo.Get(id);
        }

        public Groups GetGroupByIds(int? id)
        {
            return _groupRepo.Get(id);
        }

        public List<Groups> GetGroupByGroupId(int groupId)
        {
            return _groupRepo._table.Where(m => m.id == groupId).ToList();
        }


        public ErrorCode CreateGroup(Groups group, ref string errMsg)
        {
            try
            {
                return _groupRepo.Create(group, out errMsg);
            }
            catch (Exception ex)
            {
                // Log error
                // logger.LogError(ex, "Error creating group");
                errMsg = ex.Message;
                return ErrorCode.Error;
            }
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

        public GroupMembership GetGroupMembershipByGroupIdAndUserId(int groupId, int userId)
        {
            return _membershipRepo._table.FirstOrDefault(m => m.groupId == groupId && m.userId == userId);
        }

        public ErrorCode UpdateGroupMembership(GroupMembership membership, ref string errMsg)
        {
            return _membershipRepo.Update(membership.id, membership, out errMsg);
        }
        public ErrorCode UpdateMembershipStatus(int id, int groupId, int status, ref string errMsg)
        {
            try
            {
                // Retrieve the membership record by its ID
                var membership = _membershipRepo._table.FirstOrDefault(m => m.id == id);
                if (membership != null)
                {
                    // Update the membership status
                    membership.status = status;
                    return _membershipRepo.Update(id, membership, out errMsg);
                }
                else
                {
                    errMsg = "Membership not found.";
                    return ErrorCode.Error;
                }
            }
            catch (Exception ex)
            {
                // Log exception if necessary
                errMsg = ex.Message;
                return ErrorCode.Error;
            }
        }
        public ErrorCode UpdateGroupStatus (int id, ref string errMsg)
        {
            try
            {
                // Retrieve the membership record by its ID
                var group = _groupRepo._table.FirstOrDefault(m => m.id == id);

                if (group != null)
                {
                    // Update the membership status
                    group.status = 1;
                    return _groupRepo.Update(id, group, out errMsg);
                }
                else
                {
                    errMsg = "Membership not found.";
                    return ErrorCode.Error;
                }
            }
            catch (Exception ex)
            {
                // Log exception if necessary
                errMsg = ex.Message;
                return ErrorCode.Error;
            }
        }

    }
}
