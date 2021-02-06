using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Data.Models
{
    public class UserWonChallange
    {
        public string UserId { get; set; }

        public PhotoAppUser User { get; set; }

        public int ChallangeId { get; set; }

        public Challange Challange { get; set; }
    }
}
