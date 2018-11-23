using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace UUWebstore.Models
{
    public class viewModel
    {
    }
    public class LoginIdViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
    public class resetPassword
    {
        [Required]
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("newPassword")]
        public string confirmPassword { get; set; }

    }
    public class forgotPassword
    {
        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string confirmPassword { get; set; }

        public Int64 userID { get; set; }
    }

    [MetadataType(typeof(userMetaData))]
    public partial class user
    {
    }
    public class userMetaData
    {
        [Required]
        [Remote("IsUserExists", "users", ErrorMessage = "User name already exists in database.", AdditionalFields = "PreviousUsername")]
        [Display(Name = "User Name")]
        public string userName { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }


        [Required]
        [Display(Name = "Full Name")]
        public string fullName { get; set; }

        [Display(Name = "Phone")]
        public string phone { get; set; }

        [Required]
        [Display(Name = "Mobile")]
        [Remote("IsMobileExists", "users", ErrorMessage = "Mobile already exists in database.", AdditionalFields = "Previousmobile")]
        public string mobile { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [Remote("IsEmailExists", "users", ErrorMessage = "Email address already exists in database.", AdditionalFields = "PreviousemailAddress")]
        public string emailAddress { get; set; }

        [Display(Name = "Website")]
        public string website { get; set; }

        [Display(Name = "Address")]
        public string address { get; set; }

        [Display(Name = "Location")]
        public string location { get; set; }

        [Required]
        [Display(Name = "City")]
        public int cityId { get; set; }

        [Required]
        [Display(Name = "State")]
        public int stateId { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int countryId { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string zipcode { get; set; }


        [Required]
        [Display(Name = "Supplier PPAI/ASI")]
        public string supplier_PPAI_ASI { get; set; }

        public int roleID { get; set; }
        public long createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public long modifiedBy { get; set; }
        public System.DateTime modifiedDate { get; set; }

        [Display(Name = "Active")]
        public bool isActive { get; set; }

        [Display(Name = "Delete")]
        public bool isDelete { get; set; }
        public bool isBlocked { get; set; }
    }
        [MetadataType(typeof(ProductOrderMetaData))]
        public partial class ProductOrder
        {
        }
        public class ProductOrderMetaData
        {
            [Required]
            [Display(Name = "Full Name")]
            public string fullName { get; set; }

            [Display(Name = "Phone")]
            public string phone { get; set; }

            [Required]
            [Display(Name = "Mobile")]
            public string mobile { get; set; }

            [Required]
            [Display(Name = "Email Address")]
            public string emailAddress { get; set; }

            [Display(Name = "Website")]
            public string website { get; set; }

            [Display(Name = "Location")]
            public string location { get; set; }
            [Display(Name = "Sipping Address")]
            public string shippingAddress { get; set; }

            [Required]
            [Display(Name = "City")]
            public int cityId { get; set; }

            [Required]
            [Display(Name = "State")]
            public int stateId { get; set; }

            [Required]
            [Display(Name = "Country")]
            public int countryId { get; set; }

            [Required]
            [Display(Name = "Zip Code")]
            public string zipcode { get; set; }
            [Required]
            [Display(Name = "ImprintLine")]
            public string ImprintLine1 { get; set; }
            [Required]
            [Display(Name = "Quantity")]
            public int quantity { get; set; }
            [Required]
            [Display(Name = "Expected Delivery Date")]
            public Nullable<System.DateTime> OrderDeliveryExpectedDate { get; set; }
        }
        public class search
        {
            public int roleID { get; set; }
            public string userName { get; set; }
            public string fullName { get; set; }
            public Nullable<int> cityId { get; set; }
            public Nullable<int> stateId { get; set; }
            public Nullable<int> countryId { get; set; }
            public string zipcode { get; set; }
        }
        [MetadataType(typeof(bannerMetaData))]
        public partial class banner
        {
        }
        public class bannerMetaData
        {
            [Required]
            public string name { get; set; }
            public int srNumber { get; set; }
            public bool isActive { get; set; }
        }
        public class ContactUsContent
        {
            [AllowHtml]
            public string ContactUsContentHtml { get; set; }

        }
        public class AboutUsHtml
        {
            [AllowHtml]
            public string AboutUsHtmlContent { get; set; }

        }
        public class HomePageHtml
        {
            [AllowHtml]
            public string HomePageHtmlContent { get; set; }

        }
        public class getClientWebsiteReference_sp_Result
        {
            public Int64 productCategoryId { get; set; }
            public string name { get; set; }
            public string sku { get; set; }
            public string productTitle { get; set; }
            public string slugURL { get; set; }
            public string imgWebAddress { get; set; }
            public string Type { get; set; }
            public bool isFeatured { get; set; }
        public string LinkedIn { get; set; }
        public string pinterest { get; set; }
        public string Instagram { get; set; }
        public string YouTube { get; set; }
        public string Twiter { get; set; }
        public string GPlus { get; set; }
        public string Facebook { get; set; }
    }
    public class SocailMedia
    {
        public string LinkedIn { get; set; }
        public string pinterest { get; set; }
        public string Instagram { get; set; }
        public string YouTube { get; set; }
        public string Twiter { get; set; }
        public string GPlus { get; set; }
        public string Facebook { get; set; }
      
    }
    [MetadataType(typeof(ContactUMetaData))]
    public partial class ContactU
    {
    }
    public class ContactUMetaData
    {
        [Required]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string ContactEmail { get; set; }

        [Required]
        [Display(Name = "Instructions")]
        public string Message { get; set; }

        [Display(Name = "Create Date")]
        public Nullable<System.DateTime> createdate { get; set; }

       
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }


        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

       
        [Display(Name = "Company Website")]
        public string CompanyUrl { get; set; }

        [Display(Name = "Read")]
        public bool isRead { get; set; }

    }
}
