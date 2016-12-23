using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Models
{
    public class RouteSuggestModel
    {
        public RouteRequestModel SuggestRouteRequest { set; get; }
        public RouteRequestModel SelfRouteRequest { set; get; }
        public CarInfoModel CarInfo { set; get; }
        public string SrcDistance { set; get; }
        public string DstDistance { set; get; }
        public string TimingString { set; get; }
        public string Gender { set; get; }
        public string NameFamily { set; get; }
        public bool IsSuggestSeen { set; get; }
        public bool IsSuggestAccepted { set; get; }
        public bool IsSuggestRejected { set; get; }

    }

}
