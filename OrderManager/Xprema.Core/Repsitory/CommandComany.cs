using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xprema.Core.Model;

namespace Xprema.Core.Repsitory
{
  public  class CommandComany
    {
      public List<Company> GetAllCompanies()
      {
            List<Company> ls = new List<Company>();
            ls.Add(new Company() {ComanyName = "Company 1",
                Description = "description 1",Id = 1,LogoUrl = "Logo"});
            ls.Add(new Company()
            {
                ComanyName = "Company 2",
                Description = "description 2",
                Id = 1,
                LogoUrl = "Logo"
            });
            ls.Add(new Company()
            {
                ComanyName = "Company 2",
                Description = "description 2",
                Id = 1,
                LogoUrl = "Logo"
            });
            ls.Add(new Company()
            {
                ComanyName = "Company 2",
                Description = "description 2",
                Id = 1,
                LogoUrl = "Logo"
            });
            ls.Add(new Company()
            {
                ComanyName = "Company 2",
                Description = "description 2",
                Id = 1,
                LogoUrl = "Logo"
            });
          return ls;
      }
    }
}
