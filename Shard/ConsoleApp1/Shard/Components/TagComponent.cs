using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Components
{

    class TagComponent : BaseComponent
    {
        private List<string> tags;

        public TagComponent()
        {
            tags = new List<string>();
        }

        public override void initialize() { }

        public override void update()
        {
            base.update();
            // You could add any tag-related behavior here if needed.
        }

        public bool checkTag(string tag)
        {
            return tags.Contains(tag);
        }

        // Add a tag to the GameObject
        public void addTag(string tag)
        {
            if (!tags.Contains(tag))
            {
                tags.Add(tag);
            }
        }

        // Check if a GameObject has a certain tag
        public bool hasTag(string tag)
        {
            return tags.Contains(tag);
        }

        // Remove a tag from the GameObject
        public void removeTag(string tag)
        {
            tags.Remove(tag);
        }

        // Retrieve all tags attached to the GameObject
        public List<string> getTags() => tags;

        //protected override void UpdateComponent()
        //{
        //    throw new NotImplementedException();
        //}
    }
    
}
