using PMS_api.Data;
using PMS_api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS_api.Utility;


namespace PMS_api.Utility
{
    public static class UpdateDatabase
    {
        static ApiDbContext _dbC = new ApiDbContext();

        public static void UpdateAllowedStudents(dynamic studentAllowedInNewCompany, Companies cp)
        {
            foreach (var x in studentAllowedInNewCompany)
            {
                AllowedStudents rec = new AllowedStudents();
                rec.student_roll_no = x.student_roll_no;
                rec.company_id = cp.company_id;
                //Console.WriteLine(rec.student_roll_no);
                using (ApiDbContext dc = new ApiDbContext())
                {
                    dc.AllowedStudents.Add(rec);
                    dc.SaveChanges();
                }
            }
        }
    }
}
