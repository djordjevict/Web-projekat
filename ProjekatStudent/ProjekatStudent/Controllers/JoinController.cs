using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjekatStudent.Entities;
using ProjekatStudent.Repos;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;

namespace ProjekatStudent.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/join")]
    public class JoinController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IExamRepository _examRepository;
        private readonly IJoinRepository _joinRepository;
        private readonly IConfiguration _configuration;

        public JoinController(IStudentRepository studentRepository, IExamRepository examRepository, 
                                IJoinRepository joinRepository, IConfiguration configuration)
        {
            _studentRepository = studentRepository;
            _examRepository = examRepository;
            _joinRepository = joinRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("searchStudents/{name}/{period}")]
        public async Task<ActionResult<IEnumerable<Student>>> SearchByNameAndPeriod(string name, string period)
        {
            try
            {
                var students = await _studentRepository.GetStudentsByNameAndPeriodAsync(name, period);
                if (students.Count() == 0)
                    return NotFound("No student passed the required exam within the required exam period.");
                return Ok
                    (students.Select(student => new
                    {
                        Index = student.Index,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        PassedExams = student.PassedExams.Select(q =>
                            new
                            {
                                Grade = q.Grade,
                            })
                    }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("searchExam/{index}")]
        public async Task<ActionResult<IEnumerable<Join>>> SearchByIndex(int index)
        {
            try
            {
                var student = await _studentRepository.GetStudentByIndexWithPassedExamAsync(index);

                if (student != null)
                {
                    return Ok
                    (
                        new
                        {
                            Index = student.Index,
                            FirstName = student.FirstName,
                            LastName = student.LastName,
                            PassedExams = student.PassedExams.Select(q =>
                                new
                                {
                                    Grade = q.Grade,
                                    ExaminationPeriod = q.ExaminationPeriod,
                                    Name = q.Exam.Name,
                                    Semester = q.Exam.Semester,
                                })
                        });
                }

                return NotFound("There is no wanted student.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("add/{name}/{grade}/{examinationPeriod}/{index}")]
        [Authorize(Roles = "PROFESSOR")]
        public async Task<ActionResult> AddPassedExam(string name, int grade, string examinationPeriod, int index)
        {
            try
            {
                var exam = await _examRepository.GetExamByNameAsync(name);
                if (exam == null)
                    return NotFound("Non-existent exam.");

                var student = await _studentRepository.GetStudentByIndexAsync(index);
                if (student == null)
                    return NotFound("Non-existent student.");

                if (await _joinRepository.ExistJoinAsync(exam.Id, student.Id))
                    return BadRequest("The given student has already passed the given exam.");

                Join join = new Join
                {
                    Exam = exam,
                    Student = student,
                    Grade = grade,
                    ExaminationPeriod = examinationPeriod
                };
               
                HttpResponseMessage response;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.elasticemail.com");
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("apikey", _configuration["Keys:ElasticEmail"]),
                        new KeyValuePair<string, string>("from", _configuration["Emails:Teodora2"]),
                        new KeyValuePair<string, string>("to", _configuration["Emails:Teodora1"]),
                        new KeyValuePair<string, string>("subject", "Congratulation"),
                        new KeyValuePair<string, string>("body", "Congratulations on successfully passing the exam!")
                    });

                    response = await client.PostAsync("/v2/email/send", content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Email successfully sent!");
                    }
                    else
                    {
                        Console.WriteLine("Email sending failed.");
                    }
                }

                var result = await _joinRepository.AddJoinAsync(join);
                return Ok(
                    new
                    {
                        Index = result.Student.Index,
                        FirstName = result.Student.FirstName,
                        LastName = result.Student.LastName,
                        Name = result.Exam.Name,
                        Grade = result.Grade,
                        ExaminationPeriod = result.ExaminationPeriod
                    }
                    );
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("update/{index}/{name}/{grade}/{examinationPeriod}")]
        [Authorize(Roles = "PROFESSOR")]
        public async Task<ActionResult> UpdateStudentExam(int index, string name, int grade, string examinationPeriod)
        {
            try
            {
                var student = await _studentRepository.GetStudentByIndexAsync(index);
                if (student == null)
                    return NotFound("Non-existent student.");

                var exam = await _examRepository.GetExamByNameAsync(name);
                if (exam == null)
                    return NotFound("Non-existent exam.");

                var join = await _joinRepository.GetJoinAsync(student.Id, exam.Id);
                if (join == null)
                    return NotFound("The given student did not pass the given exam.");

                join.ExaminationPeriod = examinationPeriod;
                join.Grade = grade;
                await _joinRepository.SaveAsync();

                return Ok("Successfully updated passed exam.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{index}/{name}")]
        [Authorize(Roles = "PROFESSOR")]
        public async Task<ActionResult> DeleteStudentExam(int index, string name)
        {
            var student = await _studentRepository.GetStudentByIndexAsync(index);
            if (student == null)
                return NotFound("Non-existent student.");

            var exam = await _examRepository.GetExamByNameAsync(name);
            if (exam == null)
                return NotFound("Non-existent exam.");

            var join = await _joinRepository.GetJoinAsync(student.Id, exam.Id);
            if (join == null)
                return NotFound("The given student did not pass the given exam.");

            await _joinRepository.DeleteJoinAsync(join);
            return Ok("You have successfully canceled the exam.");
        }

        [HttpPost]
        [Route("add/{fileName}")]
        public async Task<ActionResult> AddPassedExamFromPDF(string fileName)
        {
            List<List<string>> tableData = new List<List<string>>();
            using (WordprocessingDocument doc = WordprocessingDocument.Open("C:\\Users\\Teodora\\source\\repos\\ProjekatStudent\\ProjekatStudent\\" + fileName, false))
            {
                DocumentFormat.OpenXml.Drawing.Table table = doc.MainDocumentPart.Document.Body.Elements<DocumentFormat.OpenXml.Drawing.Table>().First();
                foreach (TableRow row in table.Elements<TableRow>())
                {
                    List<string> rowData = new List<string>();
                    foreach (TableCell cell in row.Elements<TableCell>())
                    {
                        rowData.Add(cell.InnerText);
                    }
                    tableData.Add(rowData);
                }
            }
            return Ok(tableData);
        }
    }
}
