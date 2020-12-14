using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Data.Models
{
    public class PhotoChallange
    {
        public int PhotoId { get; set; }

        public Photo Photo { get; set; }


        public int ChallangeId { get; set; }

        public Challange Challange { get; set; }

        public int VotesCount { get; set; }
    }
}
