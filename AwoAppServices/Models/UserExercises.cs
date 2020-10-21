using System;
using System.Collections.Generic;

namespace AwoAppServices.Models
{
    public partial class UserExercises
    {
        public int UserExerciseId { get; set; }
        public int GymUserId { get; set; }
        public int ExerciseId { get; set; }
        public DateTime ExerciseDate { get; set; }
        public int SetAmount { get; set; }
        public int RepAmount { get; set; }
        public int? WeightKg { get; set; }
        public int? TotalWeightKg { get; set; }

        public virtual Exercises Exercise { get; set; }
        public virtual GymUsers GymUser { get; set; }
    }
}
