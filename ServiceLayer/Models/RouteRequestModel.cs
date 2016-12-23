using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Models
{
    public class RouteRequestModel
    {
        public int? RouteId { set; get; }
        public string SrcGAddress { set; get; }
        public string SrcDetailAddress { set; get; }
        public string SrcLatitude { set; get; }
        public string SrcLongitude { set; get; }
        public string DstGAddress { set; get; }
        public string DstDetailAddress { set; get; }
        public string DstLatitude { set; get; }
        public string DstLongitude { set; get; }
        public TimingOptions? TimingOption { set; get; }
        public DateTime? TheTime { set; get; }
        public DateTime? TheDate { set; get; }
        public DateTime? SatDatetime { set; get; }
        public DateTime? SunDatetime { set; get; }
        public DateTime? MonDatetime { set; get; }
        public DateTime? TueDatetime { set; get; }
        public DateTime? WedDatetime { set; get; }
        public DateTime? ThuDatetime { set; get; }
        public DateTime? FriDatetime { set; get; }
        public int? AccompanyCount { set; get; }
        public bool? IsDrive { set; get; }
        public PayingOptions? PayOption { set; get; }
        public decimal? CostMinMax { set; get; }
    }

    public class ConfirmationModel
    {
        public string Ids { set; get; }
        public string ConfirmedText { set; get; }
    }

}
