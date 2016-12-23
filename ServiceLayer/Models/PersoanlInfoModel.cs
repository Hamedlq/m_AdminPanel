using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ServiceLayer.Models
{
    public class PersoanlInfoModel
    {
        public string Name { set; get; }
        public string Family { set; get; }
        public string Gender { set; get; }
        public string NationalCode { set; get; }
        public string Email { set; get; }
        public string Mobile { set; get; }
        public string Base64UserPic { set; get; }
        public Guid? UserImageId { set; get; }
    }
}
