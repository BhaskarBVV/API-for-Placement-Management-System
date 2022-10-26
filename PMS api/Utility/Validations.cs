using Microsoft.AspNetCore.Mvc;
using PMS_api.Data;
using PMS_api.Model;
namespace PMS_api.Utility
{
    public static class Validations
    {
         static ApiDbContext _dbC = new ApiDbContext();
        public static bool ValidateRoll(long roll)
        {
            var exists = _dbC.Student.FirstOrDefault(x => x.roll_number == roll);
            if (exists != null)
                return true;
            return false;
        }
        public static bool ValidateCompany(long id)
        {
            var exists = _dbC.Companies.FirstOrDefault(x => x.company_id == id);
            if (exists != null)
                return true;
            return false;
        }

        public static bool IsCompanyPresent(Companies cp)
        {
            var exists = _dbC.Companies.FirstOrDefault(x => x.company_name == cp.company_name && x.package == cp.package);
            if (exists == null)
                return false;
            return true;
        }

        public static bool ValidateCompanyName(string compName)
        {
            var compExists = _dbC.Companies.FirstOrDefault(x => x.company_name == compName);
            if (compExists == null)
                return false;
            return true;

        }
    }
}
