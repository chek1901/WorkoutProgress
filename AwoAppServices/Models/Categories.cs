using System;
using System.Collections.Generic;

namespace AwoAppServices.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Exercises = new HashSet<Exercises>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Exercises> Exercises { get; set; }
    }
}
