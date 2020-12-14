using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Data.Models
{
    public class AccountUserPhoto
    {
        public int AccountUserPhotoId { get; set; }

        public string PhotoLink { get; set; }

        public string UserId { get; set; }

        public PhotoAppUser User { get; set; }
    }
}
