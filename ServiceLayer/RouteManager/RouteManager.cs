using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Extensions.MonoHttp;
using ServiceLayer.Models;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Routing;
using System.Web.Script.Serialization;
using RestSharp.Deserializers;
using System.Configuration;

namespace ServiceLayer
{
    public class RouteManager : IRouteManager
    {
        private string ApiUrl = "http://localhost:50226/";

        public RouteManager()
        {
            ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        }

        public RouteResponseModel SubmitNewRoute(RouteRequestModel model, string clientToken)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("InsertUserRoute", Method.POST);
            request.AddHeader("Authorization", "Bearer " + clientToken);
            request.AddParameter("SrcGAddress", model.SrcGAddress);
            request.AddParameter("SrcDetailAddress", model.SrcDetailAddress);
            request.AddParameter("SrcLatitude", model.SrcLatitude);
            request.AddParameter("SrcLongitude", model.SrcLongitude);
            request.AddParameter("DstGAddress", model.DstGAddress);
            request.AddParameter("DstDetailAddress", model.DstDetailAddress);
            request.AddParameter("DstLatitude", model.DstLatitude);
            request.AddParameter("DstLongitude", model.DstLongitude);
            request.AddParameter("AccompanyCount", model.AccompanyCount);
            request.AddParameter("TimingOption", model.TimingOption);
            request.AddParameter("PriceOption", model.PayOption);
            request.AddParameter("TheDate", model.TheDate);
            request.AddParameter("TheTime", model.TheTime);
            request.AddParameter("SatDatetime", model.SatDatetime);
            request.AddParameter("SunDatetime", model.SunDatetime);
            request.AddParameter("MonDatetime", model.MonDatetime);
            request.AddParameter("TueDatetime", model.TueDatetime);
            request.AddParameter("WedDatetime", model.WedDatetime);
            request.AddParameter("ThuDatetime", model.ThuDatetime);
            request.AddParameter("FriDatetime", model.FriDatetime);
            request.AddParameter("CostMinMax", model.CostMinMax);
            request.AddParameter("IsDrive", model.IsDrive);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RouteResponseModel routeResponseModel = new RouteResponseModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                routeResponseModel = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return routeResponseModel;
        }

        public RouteResponseModel GetUserRoutes(string token)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetUserRoutes", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel GetAllRoutes(string token)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetAllRoutes", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }


        public RouteResponseModel ConfirmRoute(ConfirmationModel confirmModel, string token)
        {
            var list = new List<RouteRequestModel>();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("ConfirmRoute", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("RouteIdsCommaSeprated", confirmModel.Ids);
            request.AddParameter("ConfirmedText", confirmModel.ConfirmedText);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RouteResponseModel routeResponseModel = new RouteResponseModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                routeResponseModel = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return routeResponseModel;

        }

        public RouteResponseModel GetRouteSuggest(string token)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetRouteSuggests", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel GetSuggestRouteByRouteId(string token, int routeId)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetUserSuggestRoute", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("RouteRequestId", routeId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public PersoanlInfoModel GetRouteUserByRouteId(string token, int routeId)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetRouteUser", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("RouteRequestId", routeId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            PersoanlInfoModel piModel = new PersoanlInfoModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                piModel = js.Deserialize<PersoanlInfoModel>(response.Content);
            }
            return piModel;
        }
        

        public RouteResponseModel AcceptSuggestRoute(string token, int routeId, int selRouteId)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("AcceptSuggestedRoute", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("RouteId", routeId);
            request.AddParameter("SelfRouteId", selRouteId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel JoinGroup(string token, int routeId, int groupId)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("JoinGroup", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("RouteId", routeId);
            request.AddParameter("GroupId", groupId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel LeaveGroup(string token, int routeId, int groupId)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("LeaveGroup", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("RouteId", routeId);
            request.AddParameter("GroupId", groupId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel DeleteRoute(string token, int routeId)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("DeleteRoute", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("RouteRequestId", routeId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel DeleteGroupSuggest(string token, int routeId, int groupId)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("DeleteGroupSuggest", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("RouteId", routeId);
            request.AddParameter("GroupId", groupId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel DeleteRouteSuggest(string token, int selfRouteId, int routeId)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("DeleteRouteSuggest", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("RouteId", routeId);
            request.AddParameter("SelfRouteId", selfRouteId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel GetComments(string token, int groupId)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetGroupComments", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("GroupId", groupId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel SubmitComment(string token, int groupId, string comment)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("SubmitComment", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("GroupId", groupId);
            request.AddParameter("Comment", comment);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;

        }

        public RouteResponseModel SubmitChat(string token, string mobile, string chat)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("SubmitChatByMobile", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("Mobile", mobile);
            request.AddParameter("Comment", chat);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel GetPassengerConfirmInfo(string token, int groupId, int routeId)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetPassengerConfirm", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("RouteId", routeId);
            request.AddParameter("GroupId", groupId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;
        }

        public RouteResponseModel ValidateTiming(RouteRequestModel model, string token)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("ValidateTiming", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("TimingOption", model.TimingOption);
            request.AddParameter("TheDate", model.TheDate);
            request.AddParameter("TheTime", model.TheTime);
            request.AddParameter("SatDatetime", model.SatDatetime);
            request.AddParameter("SunDatetime", model.SunDatetime);
            request.AddParameter("MonDatetime", model.MonDatetime);
            request.AddParameter("TueDatetime", model.TueDatetime);
            request.AddParameter("WedDatetime", model.WedDatetime);
            request.AddParameter("ThuDatetime", model.ThuDatetime);
            request.AddParameter("FriDatetime", model.FriDatetime);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RouteResponseModel routeResponseModel = new RouteResponseModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                routeResponseModel = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return routeResponseModel;

        }

        public RouteResponseModel GetUserRoutesByMobile(string token, string mobile)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetUserRoutesBymobile", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("Mobile", mobile);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                list = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return list;

        }

        public RouteResponseModel DistributeRequest(List<DistributeModel> distributeModelList)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("Distribute", Method.POST);
            var distributeRequest= new DistributeRequestModel();
            var deliverPoints = new List<DeliverPoint>();
            foreach (var distributeModel in distributeModelList)
            {
                deliverPoints.Add(new DeliverPoint() { Id = distributeModel.Counter, Lat = distributeModel.Lat, Lng = distributeModel.Lng });
            }
            distributeRequest.DeliverPoints = deliverPoints;
            distributeRequest.IsReturn = 1;
            distributeRequest.Origin.Lat = "35.716900820";
            distributeRequest.Origin.Lng = "51.426422596";
            var json =
                request.JsonSerializer.Serialize(distributeRequest);
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RouteResponseModel routeResponseModel = new RouteResponseModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                routeResponseModel = js.Deserialize<RouteResponseModel>(response.Content);
            }
            return routeResponseModel;
        }

        public RouteResponseModel GetChats(string token, string mobile)
        {
            var list = new RouteResponseModel();
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetChatsByMobile", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("Mobile", mobile);
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
