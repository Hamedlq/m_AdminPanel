using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ServiceLayer.Resources;

namespace ServiceLayer.Models
{
    public class RegisterModel
    {
        //public string Gender { set; get; }
        //public string Name { set; get; }
        //public string Family { set; get; }
        public string Mobile{ set; get; }
        //public string Email{ set; get; }
        public string Password { set; get; }
    }
    public class LoginModel
    {

        [Display(Name = "Mobile", ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        public string Loginmobile { set; get; }
        [Display(Name = "Password", ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        public string Pass{ set; get; }
        [Display(Name = "RemeberMe", ResourceType = typeof(Strings))]
        public bool RememberMe{ set; get; }
    }

    public class UserModel
    {
        public UserModel()
        {
            UserPic = new List<HttpPostedFileBase>();
        }
        [Display(Name = "Gender", ResourceType = typeof(Strings))]
        public string Gender { set; get; }
        [Display(Name = "Name", ResourceType = typeof(Strings))]
        public string Name { set; get; }
        [Display(Name = "Family", ResourceType = typeof(Strings))]
        public string Family { set; get; }
        [Display(Name = "NationalCode", ResourceType = typeof(Strings))]
        public string NationalCode { set; get; }
        [Display(Name = "Mobile", ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        public string Mobile { set; get; }
        [Display(Name = "Password", ResourceType = typeof(Strings))]
        [StringLength(10, MinimumLength = 4, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PassLength")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        public string Password { set; get; }
        public List<HttpPostedFileBase> UserPic { get; set; }
    }

    
    public class LicenseInfoModel
    {
        public string LicenseNo { set; get; }
        public HttpPostedFileBase LicensePic { set; get; }
        public Guid? LicenseImageId { set; get; }
        public string Base64LicensePic { set; get; }

    }
    public class CarInfoModel
    {
        public string CarType { set; get; }
        public string CarPlateNo { set; get; }
        public string CarColor { set; get; }
        public HttpPostedFileBase CarCardPic { set; get; }
        public string Base64CarCardPic { set; get; }
        public string Base64CarCardBckPic { set; get; }
        public Guid? CarBackImageId { set; get; }
        public Guid? CarFrontImageId { set; get; }
    }
    public class BankInfoModel
    {
        public string BankName { set; get; }
        public string BankAccountNo { set; get; }
        public string BankCardNo { set; get; }
        public HttpPostedFileBase BankCardPic { set; get; }
        public string Base64BankCardPic { set; get; }
        public Guid? BankCardImageId { set; get; }
    }
    public class AgencyDriver: UserModel
    {
        [Display(Name = "LicenseNo", ResourceType = typeof(Strings))]
        public string LicenseNo { set; get; }
        [Display(Name = "CarType", ResourceType = typeof(Strings))]
        public string CarType { set; get; }
        [Display(Name = "CarPlate", ResourceType = typeof(Strings))]
        public string CarPlate { set; get; }
        [Display(Name = "CarColor", ResourceType = typeof(Strings))]
        public string CarColor { set; get; }
        [Display(Name = "BankName", ResourceType = typeof(Strings))]
        public string BankName { set; get; }
        [Display(Name = "BankAccount", ResourceType = typeof(Strings))]
        public string BankAccount { set; get; }
        [Display(Name = "BankCardNo", ResourceType = typeof(Strings))]
        public string BankCardNo { set; get; }
    }

}