using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Entities
{
    public class Storage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }



        public decimal FloorLength { get; set; }

        public decimal FloorWidth { get; set; }

        //or


        public decimal Heigth { get; set; }



        public decimal SquareMeter { get; set; }




        public decimal CubicMeter { get; set; }



        public string Location { get; set; }





        public string NameOfOwner { get; set; }




        public string Email { get; set; }



        public string PhoneNumber { get; set; }



        public string Description { get; set; }



        public decimal PricePerMonth { get; set; }








    }
}
