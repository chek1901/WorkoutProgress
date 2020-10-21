using System;
using System.Collections.Generic;

namespace AwoAppServices.Models
{
    public partial class Objectives
    {
        public int GymObjectiveId { get; set; }
        public int GymUserId { get; set; }
        public int ExerciseId { get; set; }
        public string OjectiveInfo { get; set; }
        public int? ObjectiveSet { get; set; }
        public int? ObjectiveRep { get; set; }
        public int? ObjectiveKg { get; set; }
        public int? MaxKg { get; set; }
        public int? MaxOneRep { get; set; }

        public virtual Exercises Exercise { get; set; }
        public virtual GymUsers GymUser { get; set; }
    }
}
