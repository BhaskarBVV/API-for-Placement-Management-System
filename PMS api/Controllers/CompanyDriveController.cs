using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS_api.Data;
using PMS_api.Model;
using PMS_api.Utility;
using System.ComponentModel.DataAnnotations;

namespace PMS_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDriveController : Controller
    {
        ApiDbContext _dbC = new ApiDbContext();


        // Add new company
        [Authorize]
        [HttpPost("/AddCompany")]
        public IActionResult AddCompany([FromBody] Companies cp)
        {
            cp.company_name = cp.company_name.ToLower();
            if(Validations.IsCompanyPresent(cp))
                return Ok("Company already exists");

            // company doesn't exist, so add the company to DB.
            _dbC.Companies.Add(cp);
            _dbC.SaveChanges();

            // fetch the list of students allowed in this company
            var studentAllowedInNewCompany = GetDataFromDb.GetAllEligibleStudents(cp);

            // add the allowed students in the table of all allowedStudents
            UpdateDatabase.UpdateAllowedStudents(studentAllowedInNewCompany, cp);

            //return list of allowed students
            return Ok(studentAllowedInNewCompany);
        }


        //Display all the companies
        [HttpGet]
        [Authorize]
        [Route("/Companies")]
        public IActionResult GetAllComapnies()
        {
            if (_dbC.Companies.Count() == 0)
                return Ok("No companies visited yet");
            return Ok(_dbC.Companies);
        }


        // Add a selected student
        [Authorize]
        [HttpPost("/AddPlaced")]
        public IActionResult AddPlaced([FromBody] Placed st)
        {
            if (!Validations.ValidateRoll(st.roll_number))
                return NotFound("Not a valid roll number");
            if (!Validations.ValidateCompany(st.company_id))
                return NotFound("Not a valid company Id");

            //check if the student is allowed in this company or not.
            if(!GetDataFromDb.IsAllowedToSit(st))
                return BadRequest("Student was not allowed to sit in this company");

            //current package of the student
            var cur_pkg = GetDataFromDb.CurPkg(st.roll_number);

            // new package and new company
            var new_pkg = GetDataFromDb.NewPkg(st.company_id);

            //checking if the older was at a difference of 4 or not.
            if (cur_pkg.Count != 0 && (cur_pkg[0].package + 4.00 >= new_pkg[0].package))
                return Ok("Already at good package");

            if (_dbC.Placed.FirstOrDefault(x => x.roll_number == st.roll_number) == null)
                _dbC.Placed.Add(st);
            else
                _dbC.Placed.First(x => x.roll_number == st.roll_number).company_id = st.company_id;
            
            _dbC.SaveChanges();
            return Ok(" Successfully placed student ");
        }


        //Display all companies in which a student is allowed
        [Authorize]
        [HttpGet("/Student/CompaniesAllowed/{roll}")]
        public IActionResult GetAllAllowedCompanies(long roll)
        {
            if (!Validations.ValidateRoll(roll))
                return NotFound("Roll Number doesn't exist");
            var companies = GetDataFromDb.AllowedCompanies(roll);
            if (companies==null)
                return Ok("Not allowed in any company yet");
            return Ok(companies);
        }


        //Display all students which are allowed in a company
        [Authorize]
        [HttpGet("/Companies/StudentAllowed/{CompName}")]
        public IActionResult FetchAllAllowedInComp(string compName)
        {
            compName = compName.ToLower();

            if(!Validations.ValidateCompanyName(compName))
                return NotFound("No such company came for placement");

            long compId = GetDataFromDb.GetCompanyId(compName);

            var allStudents = GetDataFromDb.AllowedStudents(compId);
            return Ok(allStudents);
        }


        //Delete a Placed student
        [Authorize]
        [HttpDelete("/Placed/Delete/{roll}")]
        public IActionResult DeletePlaced(long roll)
        {
            if (!Validations.ValidateRoll(roll))
                return NotFound("Roll Number doesn't exist");
            var isPlaced = _dbC.Placed.FirstOrDefault(x => x.roll_number == roll);
            if (isPlaced == null)
                return NotFound("Particular is not yet placed");

            var toDel = _dbC.Placed.FirstOrDefault(x => x.roll_number == roll);
            _dbC.Placed.Remove(toDel);
            _dbC.SaveChanges();
            return Ok("Successfully removed the student from the placed record");
        }
    }
}
