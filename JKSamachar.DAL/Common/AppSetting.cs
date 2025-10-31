using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKSamachar.DAL.Common
{
    public class AppSetting
    {
        public string Secret { get; set; }
        public string DefaultPassword { get; set; }
        public string ImagesPath { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
