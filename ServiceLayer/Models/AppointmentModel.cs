using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Models
{
    public class AppointmentModel
    {
        public int GroupId { set; get; }
        public int RouteId { set; get; }
        public string GAppointLatitude { set; get; }
        public string GAppointLongitude { set; get; }
        public string GAppointAddress { set; get; }
        public DateTime AppointTime { set; get; }
        public decimal? ConfirmedPrice { set; get; }
    }
}
