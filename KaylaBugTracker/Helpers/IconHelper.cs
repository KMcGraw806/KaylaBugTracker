using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace KaylaBugTracker.Helpers
{
    public class IconHelper
    {
        public static string GetIcon(string fileExtension)
        {
            var fileExtensions = WebConfigurationManager.AppSettings["AllowableExtensions"].Split(',');
            var imgExtensions = WebConfigurationManager.AppSettings["AllowableImageExtensions"].Split(',');
            foreach(var extension in fileExtensions.Concat(imgExtensions))
            {
                if(extension == fileExtension)
                    return $"/Images/{extension.TrimStart('.')}.png";
            }
            return "/Images/default.png";
        }
    }
}