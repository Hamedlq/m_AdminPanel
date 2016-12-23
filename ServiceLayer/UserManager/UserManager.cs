using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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
using System.Web.Script.Serialization;
using CoreManager.Models;
using RestSharp.Deserializers;

namespace ServiceLayer
{
    public class UserManager : IUserManager
    {
        private string ApiUrl = "http://localhost:50226/";

        public UserManager()
        {
            ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        }

        public RequestReponse SubmitContactUs(ContactUsModel model)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("ContactUs", Method.POST);
            request.AddParameter("Name", model.ContactName);
            request.AddParameter("Email", model.ContactEmail);
            request.AddParameter("Text", model.ContactText);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RequestReponse requestReponse = new RequestReponse();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                requestReponse = js.Deserialize<RequestReponse>(response.Content);
            }
            return requestReponse;
        }

        public TokenResponse CallLogin(LoginModel loginmodel)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("TokenAuthentication", Method.POST);
            request.AddParameter("username", loginmodel.Loginmobile);
            request.AddParameter("password", loginmodel.Pass);
            request.AddParameter("grant_type", "password");
            request.AddParameter("response_type", "token");
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            var tokenResponse = js.Deserialize<TokenResponse>(response.Content);
            tokenResponse.StatusCode = response.StatusCode;
            return tokenResponse;
        }

        //public RegisterResponse RegisterAgencyDriver(AgencyDriver model, string clientToken)
        //{
        //    var client = new RestClient(ApiUrl);
        //    var request = new RestRequest("RegisterTaxiAgencyDriver", Method.POST);
        //    request.AddHeader("Authorization", "Bearer " + clientToken);
        //    request.AddParameter("mobile", model.Mobile);
        //    request.AddParameter("password", model.Password);
        //    request.AddParameter("name", model.Name);
        //    request.AddParameter("gender", model.Gender);
        //    request.AddParameter("family", model.Family);
        //    request.AddParameter("nationalCode", model.NationalCode);
        //    request.AddParameter("licenseno", model.LicenseNo);
        //    request.AddParameter("cartype", model.CarType);
        //    request.AddParameter("carplate", model.CarPlate);
        //    request.AddParameter("carcolor", model.CarColor);
        //    request.AddParameter("bankname", model.BankName);
        //    request.AddParameter("bankaccount", model.BankAccount);
        //    request.AddParameter("bankcardno", model.BankCardNo);

        //    IRestResponse response = client.Execute(request);
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    RegisterResponse registerResponse=new RegisterResponse();
        //    if (!string.IsNullOrWhiteSpace(response.Content))
        //    {
        //        registerResponse = js.Deserialize<RegisterResponse>(response.Content);
        //    }
        //    registerResponse.StatusCode = response.StatusCode;
        //    return registerResponse;
        //}

        public RegisterResponse RegisterNewUser(RegisterModel model)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("RegisterWebUser", Method.POST);
            request.AddParameter("Mobile", model.Mobile);
            request.AddParameter("Password", model.Password);
            //request.AddParameter("Name", model.Name);
            //request.AddParameter("Gender", model.Gender);
            //request.AddParameter("Family", model.Family);
            //request.AddParameter("Email", model.Email);

            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RegisterResponse registerResponse = new RegisterResponse();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                registerResponse = js.Deserialize<RegisterResponse>(response.Content);
            }
            return registerResponse;

        }


        public RequestReponse SubmitPersonalPic(HttpPostedFileBase userPicfile, string clientToken)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("InsertPersonalPic", Method.POST);
            request.AddHeader("Authorization", "Bearer " + clientToken);
            request.AddHeader("Content-Type", "multipart/form-data");
            byte[] userPic = null;
            using (var binaryReader = new BinaryReader(userPicfile.InputStream))
            {
                userPic = binaryReader.ReadBytes(userPicfile.ContentLength);
            }
            request.AddFile("UserPic", userPic, userPicfile.FileName, userPicfile.ContentType);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RequestReponse routeResponseModel = new RequestReponse();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                routeResponseModel = js.Deserialize<RequestReponse>(response.Content);
            }
            return routeResponseModel;
        }

        public RequestReponse SubmitPersonalInfo(PersoanlInfoModel model, string clientToken)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("InsertPersoanlInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + clientToken);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddParameter("Name", model.Name);
            request.AddParameter("Family", model.Family);
            request.AddParameter("NationalCode", model.NationalCode);
            request.AddParameter("Gender", model.Gender);
            request.AddParameter("Email", model.Email);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RequestReponse PersonalInfoResponseModel = new RequestReponse();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                PersonalInfoResponseModel = js.Deserialize<RequestReponse>(response.Content);
            }
            return PersonalInfoResponseModel;
        }
        public PersoanlInfoModel GetPersonalInfo(string token)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetPersonalInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            PersoanlInfoModel piModel = new PersoanlInfoModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                piModel = js.Deserialize<PersoanlInfoModel>(response.Content);
            }
            return piModel;
        }

        public RequestReponse SubmitLicenseInfo(LicenseInfoModel model, string token)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("InsertLicenseInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddParameter("LicenseNo", model.LicenseNo);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RequestReponse licenseInfoResponseModel = new RequestReponse();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                licenseInfoResponseModel = js.Deserialize<RequestReponse>(response.Content);
            }
            return licenseInfoResponseModel;
        }
        public RequestReponse SubmitLicensePic(HttpPostedFileBase model, string clientToken)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("InsertLicensePic", Method.POST);
            request.AddHeader("Authorization", "Bearer " + clientToken);
            request.AddHeader("Content-Type", "multipart/form-data");
            byte[] licensePic = null;
            using (var binaryReader = new BinaryReader(model.InputStream))
            {
                licensePic = binaryReader.ReadBytes(model.ContentLength);
            }
            request.AddFile("LicensePic", licensePic, model.FileName, model.ContentType);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RequestReponse responseModel = new RequestReponse();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                responseModel = js.Deserialize<RequestReponse>(response.Content);
            }
            return responseModel;
        }

        public PersoanlInfoModel GetRouteUserImage(string token, int routeId)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetRouteUserImage", Method.POST);
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

        public PersoanlInfoModel GetCommentUserImage(string token, int commentId)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetCommentUserImage", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("CommentId", commentId);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            PersoanlInfoModel piModel = new PersoanlInfoModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                piModel = js.Deserialize<PersoanlInfoModel>(response.Content);
            }
            return piModel;
        }

        public RequestReponse SubmitBankPic(HttpPostedFileBase model, string token)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("InsertBankCardPic", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "multipart/form-data");
            byte[] bankCardPic = null;
            using (var binaryReader = new BinaryReader(model.InputStream))
            {
                bankCardPic = binaryReader.ReadBytes(model.ContentLength);
            }
            request.AddFile("BankCardPic", bankCardPic, model.FileName, model.ContentType);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RequestReponse responseModel = new RequestReponse();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                responseModel = js.Deserialize<RequestReponse>(response.Content);
            }
            return responseModel;
        }

        public RequestReponse SubmitBankInfo(BankInfoModel model, string token)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("InsertBankInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddParameter("BankName", model.BankName);
            request.AddParameter("BankAccountNo", model.BankAccountNo);
            request.AddParameter("BankCardNo", model.BankCardNo);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RequestReponse bankInfoResponseModel = new RequestReponse();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                bankInfoResponseModel = js.Deserialize<RequestReponse>(response.Content);
            }
            return bankInfoResponseModel;
        }

        public BankInfoModel GetBankInfo(string token)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetBankInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            BankInfoModel bankModel = new BankInfoModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                bankModel = js.Deserialize<BankInfoModel>(response.Content);
            }
            return bankModel;
        }

        public bool ConfirmMobile(string token, string mobileNo)
        {
            var res = false;
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("ConfirmMobileNo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("Mobile", mobileNo);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                res = js.Deserialize<bool>(response.Content);
            }
            return res;
        }

        public PersonalInfoResponseModel GetAllUsers(string token)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetAllUsers", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            PersonalInfoResponseModel piModel = new PersonalInfoResponseModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                piModel = js.Deserialize<PersonalInfoResponseModel>(response.Content);
            }
            return piModel;
        }

        public PersoanlInfoModel GetUserPersonalInfo(string token, string mobile)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetUserPersonalInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("Mobile", mobile);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            PersoanlInfoModel piModel = new PersoanlInfoModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                piModel = js.Deserialize<PersoanlInfoModel>(response.Content);
            }
            return piModel;
        }

        public LicenseInfoModel GetUserLicenseInfo(string token, string mobile)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetUserLicenseInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("Mobile", mobile);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            LicenseInfoModel liModel = new LicenseInfoModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                liModel = js.Deserialize<LicenseInfoModel>(response.Content);
            }
            return liModel;
        }

        public CarInfoModel GetUserCarInfo(string token, string mobile)
        {

            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetUserCarInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("Mobile", mobile);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            CarInfoModel ciModel = new CarInfoModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                ciModel = js.Deserialize<CarInfoModel>(response.Content);
            }
            return ciModel;
        }

        public PersonalInfoResponseModel GetUserByInfo(string token, UserModel model)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetUserByInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("Mobile", model.Mobile);
            request.AddParameter("Name", model.Name);
            request.AddParameter("Family", model.Family);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            PersonalInfoResponseModel piModel = new PersonalInfoResponseModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                piModel = js.Deserialize<PersonalInfoResponseModel>(response.Content);
            }
            return piModel;
        }

        public ImageResponse GetImageById(string token, Guid? model)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetImageById", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("ImageId", model);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            ImageResponse resModel = new ImageResponse();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                resModel = js.Deserialize<ImageResponse>(response.Content);
            }
            return resModel;
        }

        public LicenseInfoModel GetLicenseInfo(string token)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetLicenseInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            LicenseInfoModel liModel = new LicenseInfoModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                liModel = js.Deserialize<LicenseInfoModel>(response.Content);
            }
            return liModel;
        }

        public RequestReponse SubmitCarInfo(CarInfoModel model, string token)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("InsertCarInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddParameter("CarType", model.CarType);
            request.AddParameter("CarColor", model.CarColor);
            request.AddParameter("CarPlateNo", model.CarPlateNo);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RequestReponse carInfoResponseModel = new RequestReponse();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                carInfoResponseModel = js.Deserialize<RequestReponse>(response.Content);
            }
            return carInfoResponseModel;
        }

        public RequestReponse SubmitCarPic(HttpPostedFileBase model, string clientToken)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("InsertCarPics", Method.POST);
            request.AddHeader("Authorization", "Bearer " + clientToken);
            request.AddHeader("Content-Type", "multipart/form-data");
            byte[] carCardPic = null;
            using (var binaryReader = new BinaryReader(model.InputStream))
            {
                carCardPic = binaryReader.ReadBytes(model.ContentLength);
            }
            request.AddFile("CarCardPic", carCardPic, model.FileName, model.ContentType);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            RequestReponse responseModel = new RequestReponse();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                responseModel = js.Deserialize<RequestReponse>(response.Content);
            }
            return responseModel;
        }

        public CarInfoModel GetCarInfo(string token)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("GetCarInfo", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            JavaScriptSerializer js = new JavaScriptSerializer();
            CarInfoModel ciModel = new CarInfoModel();
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                ciModel = js.Deserialize<CarInfoModel>(response.Content);
            }
            return ciModel;
        }

    }
}
