using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UUWebstore.Models.IRepositories
{
   public interface IthemeServices
    {
        List<theme> getallTheme();
        int ApplyTheme(int themeId);
        int uploadLogo(string logoUrl);
        string SelectLogo();
        string SelectFavicon();
        int uploadFavicon(string faviconUrl);
      
    }

}
