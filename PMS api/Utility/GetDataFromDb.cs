using Microsoft.AspNetCore.Mvc;
using PMS_api.Data;
using PMS_api.Model;
namespace PMS_api.Utility
{
    public static class GetDataFromDb
    {

        static ApiDbContext _dbC = new ApiDbContext();
        public static dynamic CurPkg(long rollNumber)
        {
            var res = (from p in _dbC.Placed
             join comp in _dbC.Companies on p.company_id equals comp.company_id into t
             from r in t.DefaultIfEmpty()
             where p.roll_number == rollNumber
             select new
             {
                 package = r.package
             }).ToList();
            return res;
        }

        public static dynamic NewPkg(long id)
        {
            var ans =(from c in _dbC.Companies
             where c.company_id ==id
             select new
             {
                 package = c.package,
                 compName = c.company_name
             }).ToList();
            return ans;
        }

        public static dynamic GetParticualrStudentRecord(long roll)
        {
            var rec = from s in _dbC.Student
                    join p in _dbC.Placed on s.roll_number equals p.roll_number into g
                    from r in g.DefaultIfEmpty()
                    join c in _dbC.Companies on r.company_id equals c.company_id into g2
                    from r2 in g2.DefaultIfEmpty()
                    where s.roll_number == roll
                    select new
                    {
                        name = s.name,
                        rollNumber = s.roll_number,
                        company = (long?)r2.company_id ?? null,
                        package = (float?)r2.package ?? null
                    };
            return rec;
        }


        public static dynamic GetPlacedInWhichCompany(long roll)
        {
            var res = (from p in _dbC.Placed
                       join c in _dbC.Companies on p.company_id equals c.company_id into g
                       from r in g.DefaultIfEmpty()
                       where p.roll_number == roll
                       select new
                       {
                           name = r.company_name
                       }).ToList();
            return res;
        }

        public static dynamic GetAllUnplaced()
        {
            var res = from stu in _dbC.Student
                      join placd in _dbC.Placed on stu.roll_number equals placd.roll_number into g
                      from r in g.DefaultIfEmpty()
                      where ((long?)r.company_id ?? 0) == 0
                      select new
                      {
                          stu.roll_number,
                          stu.name,
                      };
            return res;
        }

        public static bool IsAllowedToSit(Placed st)
        {
            var isAllowedInNewComp = _dbC.AllowedStudents
                                         .FirstOrDefault(x => x.company_id == st.company_id && x.student_roll_no == st.roll_number);
            if (isAllowedInNewComp == null)
                return false;
            return true;
        }

        public static dynamic GetAllEligibleStudents(Companies cp)
        {
            var res = from stu in _dbC.Student
                      join placd in _dbC.Placed on stu.roll_number equals placd.roll_number into t
                      from rt in t.DefaultIfEmpty()
                      join comp in _dbC.Companies on rt.company_id equals comp.company_id into t2
                      from r3 in t2.DefaultIfEmpty()
                      orderby stu.roll_number
                      where ((float?)r3.package ?? 0) + 4.00 < cp.package || ((float?)r3.package ?? 0) == 0
                      select new
                      {
                          company_id = (long?)r3.company_id ?? null,
                          student_roll_no = stu.roll_number,
                      };
            return res;
        }
        public static dynamic AllowedCompanies(long roll)
        {
            var res = from a in _dbC.AllowedStudents
                      join c in _dbC.Companies on a.company_id equals c.company_id into g
                      from t in g.DefaultIfEmpty()
                      where a.student_roll_no == roll
                      select new
                      {
                          t.company_id,
                          t.company_name
                      };
            return res;
        }

        public static long GetCompanyId(string compName)
        {
            long id = _dbC.Companies.FirstOrDefault(x => x.company_name == compName).company_id;
            return id;
        }

        public static dynamic AllowedStudents(long compId)
        {
            var res = from s in _dbC.Student
                      join p in _dbC.AllowedStudents on s.roll_number equals p.student_roll_no into g
                      from r in g.DefaultIfEmpty()
                      where r.company_id == compId
                      select new
                      {
                          name = s.name,
                          roll = s.roll_number
                      };
            return res;
        }

    }
}
