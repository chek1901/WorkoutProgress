using System;
using System.Collections.Generic;

namespace AwoAppServices.Models
{
    public partial class GymUsers
    {
        public GymUsers()
        {
            Objectives = new HashSet<Objectives>();
            UserExercises = new HashSet<UserExercises>();
        }

        public int GymUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public virtual ICollection<Objectives> Objectives { get; set; }
        public virtual ICollection<UserExercises> UserExercises { get; set; }
    }
}
