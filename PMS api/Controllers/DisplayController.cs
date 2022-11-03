using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS_api.Data;
using System.Linq;
using PMS_api.Model;
using PMS_api.Utility;
using Microsoft.AspNetCore.Authorization;

namespace PMS_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisplayController : Controller
    {
        ApiDbContext _dbC = new ApiDbContext();


        // Dispplay all students
        [HttpGet]
        [Authorize]
        [Route("/Students")]
        public IActionResult Get()
        {
            return Ok(_dbC.Student);
        }


        //Display the record of a particular student.
        [HttpGet]
        [Authorize]
        [Route("/Students/{roll}")]
        public IActionResult GetParticularStudentDetails(long roll)
        {
            if (!Validations.ValidateRoll(roll))
                return NotFound("Roll Number doesn't exist");
            var rec = GetDataFromDb.GetParticualrStudentRecord(roll);
            return Ok(rec);
        }


        // Display all the placed students
        [HttpGet]
        [Authorize]
        [Route("/Placed")]
        public IActionResult GetPlaced()
        {
            if(_dbC.Placed.Count()==0)
                return Ok("No Student is placed yet");
            return Ok(_dbC.Placed);
        }


        // display if the particular student is placed or not, if yes where?
        [HttpGet]
        [Authorize]
        [Route("/Placed/{roll}")]
        public IActionResult IsCurrentPlaced(long roll)
        {
            if (!Validations.ValidateRoll(roll))
                return NotFound("Not a valid roll number");

            var isPlaced = _dbC.Placed.FirstOrDefault(x => x.roll_number == roll);
            if(isPlaced==null)
                return Ok("Student is not placed yet");

            var compName = GetDataFromDb.GetPlacedInWhichCompany(roll);
            return Ok("Selected student is placed in " + compName[0].name);
        }


        //Display all the unplaced studnets.
        [HttpGet]
        [Authorize]
        [Route("/UnPlaced")]
        public IActionResult GetUnPlaced()
        {
            var unplaced = GetDataFromDb.GetAllUnplaced();
            if (unplaced == null)
                return Ok("No one is unplaced now");
            return Ok(unplaced);  
        }


        //update the details of student
        [HttpPut("/update/{roll}")]
        [Authorize]
        public IActionResult UpdateStudentData(int roll, [FromBody] Student st)
        {
            if (!Validations.ValidateRoll(roll))
                return NotFound("Not a valid roll number");

            var data = _dbC.Student.First(x => x.roll_number == roll);
            data.name = st.name;
            // no need to add new row, directly update the database
            _dbC.SaveChanges();
            return Ok("Successfully Updated");
        }
    }
}
