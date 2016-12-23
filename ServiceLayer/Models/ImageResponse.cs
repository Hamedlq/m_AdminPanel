using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Models;

namespace CoreManager.Models
{
    public class ImageResponse
    {
        public string ImageId { set; get; }
        public ImageType ImageType { set; get; }
        public string Base64ImageFile { set; get; }
    }
}
