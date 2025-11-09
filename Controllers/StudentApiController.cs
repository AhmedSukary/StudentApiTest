using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApi.DataSimulation;
using StudentApi.Model;

namespace StudentApi.Controllers
{
    [ApiController] // Marks the class as a Web API controller with enhanced features.
                    // [Route("[controller]")] // Sets the route for this controller to "students", based on the controller name.
    [Route("api/Students")]

    public class StudentsController : ControllerBase // Declare the controller class inheriting from ControllerBase.
    {

        [HttpGet("All", Name = "GetAllStudents")] // Marks this method to respond to HTTP GET requests.
        public ActionResult<IEnumerable<Student>> GetAllStudents() // Define a method to get all students.
        {
            return Ok(StudentDataSimulation.StudentsList); // Returns the list of students.
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
        public ActionResult<IEnumerable<Student>> GetPassedStudents() // Define a method to get all students.
        {
            var passedStudents = StudentDataSimulation.StudentsList.Where(student => student.Grade >= 50).ToList();
            return Ok(passedStudents); // Returns the list of students who passed.
        }

        [HttpGet("AverageGrade", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<double> GetAverageGrade()
        {
            //StudentDataSimulation.StudentsList.Clear();
            if (StudentDataSimulation.StudentsList.Count == 0)
            {
                return NotFound("No Students Found.");
            }
            var averageGrade = StudentDataSimulation.StudentsList.Average(student => student.Grade);
            return Ok(averageGrade); // Returns the average grade list of students.
        }

        [HttpGet("GetStudentById/{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> GetStudentById(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }
            var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            return Ok(student);
        }

        //for add new we use Http Post
        [HttpPost("AddStudent", Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> AddStudent(Student newStudent)
        {
            //we validate the data here
            if (newStudent == null || string.IsNullOrEmpty(newStudent.Name) || newStudent.Age < 0 || newStudent.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }
            newStudent.Id = StudentDataSimulation.StudentsList.Count > 0 ? StudentDataSimulation.StudentsList.Max(s => s.Id) + 1 : 1;
            StudentDataSimulation.StudentsList.Add(newStudent);

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtAction("GetStudentById", new { id = newStudent.Id }, newStudent);

            /* return CreatedAtRoute("GetStudent", new { id = newStudent.Id }, newStudent);
            is used to return a response indicating that a new resource has been created.This is a common pattern
            in RESTful APIs to inform the client about the location of the newly created resource.    

            Here’s a breakdown of what this line does:         
            ------------CreatedAtRoute Method--------------

            The CreatedAtRoute method is a helper method provided by ASP.NET Core. It does three main things:

            1 - Sets the HTTP status code to 201(Created): This indicates that the request has been fulfilled
                and a new resource has been created.

            2 - Provides the location of the created resource: This is done using a route name.
                The client can use this location to retrieve the newly created resource. 

            3 - Returns the created resource: This includes the data of the created resource in the response body.

            ---------- - Parameters of CreatedAtRoute-----------

            The CreatedAtRoute method takes three parameters:

            1 - Route Name("GetStudentById"): This is the name of the route that can be used to retrieve the            
                created resource. In this case, it corresponds to the route defined by the GetStudentById method.

            2 - Route Values(new { id = newStudent.Id }): This is an anonymous object that contains the route            
                parameters required to generate the URL for the GetStudentById route.Here, it specifies the id
                of the newly created student.

            3 - Resource(newStudent): This is the created resource that will be included in the response body.
            */
        }

        //here we use HttpDelete method
        [HttpDelete("DeleteStudent/{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudent(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }
            var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            StudentDataSimulation.StudentsList.Remove(student);
            return Ok($"Student with ID:{id} has been deleted.");
        }

        //here we use http put method for update
        [HttpPut("UpdateStudent/{id}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> UpdateStudent(int id, Student updatedStudent)
        {
            if (id < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.Age < 0 || updatedStudent.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }
            var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Grade = updatedStudent.Grade;
            return Ok(student);
        }

    }
}

