using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Models;

namespace ServiceLayer
{
    public interface IRouteManager
    {
        RouteResponseModel SubmitNewRoute(RouteRequestModel model, string clientToken);
        RouteResponseModel GetUserRoutes(string token);
        RouteResponseModel GetAllRoutes(string token);
        RouteResponseModel ConfirmRoute(ConfirmationModel confirmModel, string token);
        RouteResponseModel GetRouteSuggest(string token);
        RouteResponseModel GetSuggestRouteByRouteId(string token, int routeId);
        PersoanlInfoModel GetRouteUserByRouteId(string token, int routeId);
        RouteResponseModel AcceptSuggestRoute(string token, int routeId, int selRouteId);
        RouteResponseModel JoinGroup(string token, int routeId, int groupId);
        RouteResponseModel LeaveGroup(string token, int routeId, int groupId);
        RouteResponseModel DeleteRoute(string token, int routeId);
        RouteResponseModel DeleteGroupSuggest(string token, int routeId, int groupId);
        RouteResponseModel DeleteRouteSuggest(string token, int selfRouteId, int routeId);

        RouteResponseModel GetComments(string token, int groupId);
        RouteResponseModel SubmitComment(string token, int groupId, string comment);
        RouteResponseModel SubmitChat(string token, string mobile, string chat);
        RouteResponseModel GetPassengerConfirmInfo(string token, int groupId, int routeId);
        RouteResponseModel ValidateTiming(RouteRequestModel routeModel, string token);
        RouteResponseModel GetUserRoutesByMobile(string token, string mobile);
        RouteResponseModel DistributeRequest(List<DistributeModel> distributeModelList);
        RouteResponseModel GetChats(string token, string mobile);
        
    }
}
