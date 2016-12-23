using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using RestSharp;
using ServiceLayer.Models;

namespace ServiceLayer.GroupManager
{
    public class GroupManager:IGroupManager
    {
        public string ApiUrl = "http://localhost:50226/";
        public RouteResponseModel ConfirmGroup(string token, AppointmentModel appointmentModel)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("ConfirmAppointment", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("AppointTime", appointmentModel.AppointTime);
            request.AddParameter("ConfirmedPrice", appointmentModel.ConfirmedPrice);
            request.AddParameter("GAppointAddress", appointmentModel.GAppointAddress);
            request.AddParameter("GAppointLatitude", appointmentModel.GAppointLatitude);
            request.AddParameter("GAppointLongitude", appointmentModel.GAppointLongitude);
            request.AddParameter("GroupId", appointmentModel.GroupId);
            request.AddParameter("RouteId", appointmentModel.RouteId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel FinalconfirmGroup(string token, AppointmentModel appointmentModel)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("AppointFinalConfirm", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("GroupId", appointmentModel.GroupId);
            request.AddParameter("RouteId", appointmentModel.RouteId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }
    }
}
