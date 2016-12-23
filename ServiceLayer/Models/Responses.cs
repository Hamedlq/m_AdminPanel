using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace ServiceLayer.Models
{
    public abstract class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
        public string Type { get; set; }
        public List<MessageResponse> Errors { get; set; }
        public List<MessageResponse> Warnings { get; set; }
        public List<MessageResponse> Infos { get; set; }
        public List<string> Messages { get; set; }
    }

    public class TokenResponse : Response
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string userName { get; set; }
        public string name { get; set; }
        public string family { get; set; }
        public string error { get; set; }
        public bool isMobileConfirmed { get; set; }
    }

    public class RequestReponse : Response
    {

    }

    public class RegisterResponse : Response
    {

    }

    public class RouteResponseModel : Response
    {

    }
    public class PersonalInfoResponseModel : Response
    {

    }
    public class LicenseInfoResponseModel : Response
    {

    }
    public class CarInfoResponseModel : Response
    {

    }
}
