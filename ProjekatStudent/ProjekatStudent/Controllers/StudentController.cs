using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ProjekatStudent.Entities;
using ProjekatStudent.Models;
using ProjekatStudent.Repos;

namespace ProjekatStudent.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/student")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getStudents")]
        public async Task<ActionResult<IEnumerable<StudentWithoutExamDto>>> GetStudents()
        {
            try
            {
                var students = await _studentRepository.GetStudentsAsync();

                if (students.Count() != 0)
                {
                    return Ok(_mapper.Map<IEnumerable<StudentWithoutExamDto>>(students));
                }

                return NotFound("There are no students to show.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("getStudent/{index}")]
        public async Task<ActionResult<IEnumerable<StudentWithoutExamDto>>> GetStudent(int index)
        {
            try
            {
                var student = await _studentRepository.GetStudentByIndexAsync(index);

                if (student != null)
                {
                    return Ok(_mapper.Map<StudentWithoutExamDto>(student));
                }

                return NotFound("The required student does not exist.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("addStudent")]
        public async Task<ActionResult<StudentWithoutExamDto>> AddStudent([FromBody] StudentWithoutExamDto student)
        {
            try
            {
                if (await _studentRepository.ExistStudentByIndexAsync(student.Index))
                {
                    return Ok("A student with that index number already exists");
                }

                var result = await _studentRepository.AddStudentAsync(_mapper.Map<Student>(student));
                return Ok(_mapper.Map<StudentWithoutExamDto>(result));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("updateStudent/{index}")]
        public async Task<ActionResult> PartiallyUpdateStudent(int index, 
            JsonPatchDocument<StudentWithoutExamDto> patchDocument)
        {
            try
            {
                var student = await _studentRepository.GetStudentByIndexAsync(index);

                if (student == null)
                {
                    return NotFound("The required student does not exist.");
                }

                var studentForUpdate = _mapper.Map<StudentWithoutExamDto>(student);

                patchDocument.ApplyTo(studentForUpdate, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!TryValidateModel(studentForUpdate))
                {
                    return BadRequest(ModelState);
                }

                _mapper.Map(studentForUpdate, student);
                await _studentRepository.SaveAsync();

                return Ok("Successfully updated student.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("deleteStudent/{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _studentRepository.GetStudentByIdAsync(id);

                if (student == null)
                {
                    return NotFound("The required student does not exist.");
                }

                await _studentRepository.DeleteStudentAsync(student);
                return Ok("The student was successfully deleted.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
