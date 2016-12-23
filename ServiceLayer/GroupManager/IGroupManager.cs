using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Models;

namespace ServiceLayer.GroupManager
{
    public interface IGroupManager
    {
        RouteResponseModel ConfirmGroup(string token, AppointmentModel appointmentModel);
        RouteResponseModel FinalconfirmGroup(string token, AppointmentModel appointmentModel);
    }
}
