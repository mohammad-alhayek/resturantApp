using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RestaurantStore.Core.Models
{
    

    
        public class ApplicationUser : IdentityUser
        {
            public string FullName { get; set; }          // الاسم الكامل
            public string City { get; set; }              // المدينة
            public string State { get; set; }             // المحافظة/الولاية
            public string Street { get; set; }            // اسم الشارع
        }
    
}
