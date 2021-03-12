using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.Challange
{
    public class AdminAllChallangesServiceModel
    {
        public IEnumerable<AdminChallangeServiceModel> Challanges { get; set; }
    }
}
