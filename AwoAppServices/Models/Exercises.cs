using System;
using System.Collections.Generic;

namespace AwoAppServices.Models
{
    public partial class Exercises
    {
        public Exercises()
        {
            Objectives = new HashSet<Objectives>();
            UserExercises = new HashSet<UserExercises>();
        }

        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        public virtual Categories Category { get; set; }
        public virtual ICollection<Objectives> Objectives { get; set; }
        public virtual ICollection<UserExercises> UserExercises { get; set; }
    }
}
