using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ZipNTuck.Data.EF
{
    #region ArticlesOfClothing Metadata
    public class ArticlesOfClothingMetadata
    {
        //public int ArticleID { get; set; }

        [Required(ErrorMessage = " * Article Item Name is Required * ")]
        [StringLength(50, ErrorMessage = " * Item name cannot be longer than 50 characters * ")]
        [Display(Name = "Article Name")]
        public string ArticleName { get; set; }

        //public string UserID { get; set; } 

        [DisplayFormat(NullDisplayText = "Not Available")]
        public string ArticlePhoto { get; set; }

        [StringLength(300, ErrorMessage = " * Notes cannot be longer than 300 characters * ")]
        [UIHint("MultilineText")]
        [Display(Name = "Special Notes")]
        [DisplayFormat(NullDisplayText = "Not Available")]
        public string SpecialNotes { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = " * Date is Required * ")]
        [Display(Name = "Date Added")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime DateAdded { get; set; }
    }

    [MetadataType(typeof(ArticlesOfClothingMetadata))]
    public partial class ArticlesOfClothing
    {

    }
    #endregion

    #region Location Metadata
    public class LocationMetadata
    {
        //public int LocationID { get; set; }

        [Required(ErrorMessage = " * Location Name is Required * ")]
        [StringLength(50, ErrorMessage = " * Location cannot be longer than 50 characters * ")]
        [Display(Name = "Location Name")]
        public string LocationName { get; set; }

        [Required(ErrorMessage = " * Address is Required * ")]
        [StringLength(100, ErrorMessage = " * Adress cannot be longer than 100 characters * ")]
        public string Address { get; set; }

        [Required(ErrorMessage = " * City is Required * ")]
        [StringLength(100, ErrorMessage = " * City cannot be longer than 100 characters * ")]
        public string City { get; set; }

        [Required(ErrorMessage = " * State is Required * ")]
        [StringLength(2, ErrorMessage = " * State cannot be longer than 2 characters * ")]
        public string State { get; set; }

        [Required(ErrorMessage = " * Zip Code is Required * ")]
        [StringLength(5, ErrorMessage = " * Zip Code cannot be longer than 50 characters * ")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Range(1, 5, ErrorMessage = " * Reservations cannot exceed 5 per day * ")]
        [Required] //Error Message?
        [Display(Name = "Reservation Limit")]
        public byte ReservationLimit { get; set; } //Not sure if correct
    }
    
    [MetadataType(typeof(LocationMetadata))]
    public partial class Location
    {

    }
    #endregion

    #region Reservation Metadata
    public class ReservationMetadata
    {
        //public int ReservationID { get; set; }
        //public int ArticleID { get; set; }
        //public int LocationID { get; set; }

        [Required(ErrorMessage = " * Reservation Date is Required * ")]
        [Display(Name = "Reservation Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime ReservationDate { get; set; }
    }

    [MetadataType(typeof(ReservationMetadata))]
    public partial class Reservation
    {

    }
    #endregion

    #region UserDetail Metadata
    public class UserDetailMetadata
    {
        //public string UserID { get; set; }

        [Required(ErrorMessage = " * First Name is Required * ")]
        [StringLength(50, ErrorMessage = " * First Name cannot be longer than 50 characters * ")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = " * Last Name is Required * ")]
        [StringLength(50, ErrorMessage = " * Last Name cannot be longer than 50 characters * ")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DisplayFormat(NullDisplayText = " * Not available * ")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail
    {
        [Display(Name = "Customer Name")]
        public string FullName { get { return FirstName + " " + LastName; } }
    }
    #endregion

}//end Namespace
