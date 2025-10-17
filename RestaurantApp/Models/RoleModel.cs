using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantApp.Models
{
    public class RoleModel
    {
        public string Email { get; set; }
        public List<string> roles { get; set; }
        public string SelectedRole { get; set; }

        public RoleModel() {
            roles = new List<string>();
        }
    }
}