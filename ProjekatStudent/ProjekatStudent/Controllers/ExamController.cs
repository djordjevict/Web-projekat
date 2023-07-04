using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ProjekatStudent.Entities;
using ProjekatStudent.Models;
using ProjekatStudent.Repos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProjekatStudent.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/exam")]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _examRepository;
        private readonly IMapper _mapper;

        public ExamController(IExamRepository examRepository, IMapper mapper)
        {
            _examRepository = examRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getExams")]
        public async Task<ActionResult<IEnumerable<ExamWithoutStudents>>> GetExams()
        {
            try
            {
                var exams = await _examRepository.GetExamsAsync();

                if (exams != null)
                {
                    return Ok(_mapper.Map<IEnumerable<ExamWithoutStudents>>(exams));
                }

                return NotFound("There are no exams to show.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("getExam/{id}")]
        public async Task<ActionResult<ExamWithoutStudents>> GetExam(int id)
        {
            try
            {
                var exam = await _examRepository.GetExamByIdAsync(id);

                if (exam != null)
                {
                    return Ok(_mapper.Map<ExamWithoutStudents>(exam));
                }

                return NotFound("The required exam does not exist.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("addExam")]
        [Authorize(Roles = "PROFESSOR")]
        public async Task<ActionResult<ExamWithoutStudents>> AddExam([FromBody] ExamWithoutStudents exam)
        {
            try
            {
                if (await _examRepository.ExistExamByNameAsync(exam.Name))
                {
                    return Ok("An exam with that name already exists.");
                }

                var result = await _examRepository.AddExamAsync(_mapper.Map<Exam>(exam));
                return Ok(_mapper.Map<ExamWithoutStudents>(result));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        [Route("updateExam/{id}")]
        [Authorize(Roles = "PROFESSOR")]
        public async Task<ActionResult> PartiallyUpdateExam(int id, 
                    JsonPatchDocument<ExamWithoutStudents> patchDocument)
        {
            try
            {
                var exam = await _examRepository.GetExamByIdAsync(id);

                if (exam == null)
                {
                    return NotFound("The exam you want to update does not exist.");
                }

                var examForUpdate = _mapper.Map<ExamWithoutStudents>(exam);

                patchDocument.ApplyTo(examForUpdate, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!TryValidateModel(examForUpdate))
                {
                    return BadRequest(ModelState);
                }

                _mapper.Map(examForUpdate, exam);
                await _examRepository.SaveAsync();

                return Ok("Successfully updated exam.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("deleteExam/{id}")]
        [Authorize(Roles = "PROFESSOR")]
        public async Task<ActionResult> DeleteExam(int id)
        {
            try
            {
                var exam = await _examRepository.GetExamByIdAsync(id);

                if (exam != null)
                {
                    await _examRepository.DeleteExamAsync(exam);
                    return Ok("Successfully deleted exam.");
                }

                return NotFound("The exam you want to delete does not exist.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
