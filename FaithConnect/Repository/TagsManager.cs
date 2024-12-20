using System;
using System.Collections.Generic;
using System.Linq;
using FaithConnect.Utils;

namespace FaithConnect.Repository
{
    public class TagsManager
    {
        private readonly BaseRepository<Tags> _tagsRepository;
        private readonly BaseRepository<PostTags> _postTagsRepository;

        public TagsManager()
        {
            _tagsRepository = new BaseRepository<Tags>();
            _postTagsRepository = new BaseRepository<PostTags>();
        }

        // Add or Get a Tag by Name
        public Tags AddOrGetTag(string tagName)
        {
            // Check if the tag already exists
            var tag = _tagsRepository._table.FirstOrDefault(t => t.tagName == tagName);

            if (tag == null)
            {
                // Create the tag if it doesn't exist
                tag = new Tags
                {
                    tagName = tagName
                };
                string errorMsg;
                _tagsRepository.Create(tag, out errorMsg);
            }
            return tag;
        }

        // Associate Tags with a Post
        public void AssociateTagsWithPost(int postId, List<string> tagNames)
        {
            foreach (var tagName in tagNames)
            {
                if (string.IsNullOrWhiteSpace(tagName)) continue;

                // Add or get the tag
                var tag = AddOrGetTag(tagName);

                // Check if the PostTag association already exists
                var existingAssociation = _postTagsRepository._table
                    .FirstOrDefault(pt => pt.PostId == postId && pt.TagId == tag.Id);

                if (existingAssociation == null)
                {
                    // Create a new PostTags entry
                    var postTag = new PostTags
                    {
                        PostId = postId,
                        TagId = tag.Id
                    };
                    string errorMsg;
                    _postTagsRepository.Create(postTag, out errorMsg);
                }
            }
        }

        // Get Tags for a Specific Post
        public List<string> GetTagsForPost(int postId)
        {
            return _postTagsRepository._table
                .Where(pt => pt.PostId == postId)
                .Select(pt => pt.Tags.tagName) // Assuming navigation property Tags is configured
                .ToList();
        }
    }
}
