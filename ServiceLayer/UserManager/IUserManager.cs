using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CoreManager.Models;
using ServiceLayer.Models;

namespace ServiceLayer
{
    public interface IUserManager
    {
        RequestReponse SubmitContactUs(ContactUsModel model);
        TokenResponse CallLogin(LoginModel loginmodel);
        RegisterResponse RegisterNewUser(RegisterModel model);
        RequestReponse SubmitPersonalPic(HttpPostedFileBase userPicfile, string token);
        RequestReponse SubmitPersonalInfo(PersoanlInfoModel model,string token);
        PersoanlInfoModel GetPersonalInfo(string token);
        RequestReponse SubmitLicenseInfo(LicenseInfoModel model, string token);
        LicenseInfoModel GetLicenseInfo(string token);
        RequestReponse SubmitCarInfo(CarInfoModel model, string token);
        RequestReponse SubmitCarPic(HttpPostedFileBase carCardPic, string token);
        CarInfoModel GetCarInfo(string token);
        RequestReponse SubmitLicensePic(HttpPostedFileBase licensePic, string token);
        PersoanlInfoModel GetRouteUserImage(string token, int routeId);
        PersoanlInfoModel GetCommentUserImage(string token, int commentId);
        RequestReponse SubmitBankPic(HttpPostedFileBase bankCardPic, string token);
        RequestReponse SubmitBankInfo(BankInfoModel model, string token);
        BankInfoModel GetBankInfo(string token);
        bool ConfirmMobile(string token, string mobileNo);
        PersonalInfoResponseModel GetAllUsers(string token);
        PersoanlInfoModel GetUserPersonalInfo(string token, string mobile);
        LicenseInfoModel GetUserLicenseInfo(string token, string mobile);
        CarInfoModel GetUserCarInfo(string token, string mobile);
        PersonalInfoResponseModel GetUserByInfo(string token, UserModel model);
        ImageResponse GetImageById(string token, Guid? model);
    }
}
