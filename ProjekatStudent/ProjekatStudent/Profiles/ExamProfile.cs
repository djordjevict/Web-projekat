using AutoMapper;

namespace ProjekatStudent.Profiles
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            CreateMap<Entities.Exam,Models.ExamDto>();
            CreateMap<Entities.Exam,Models.ExamWithoutStudents>();
            CreateMap<Models.ExamWithoutStudents,Entities.Exam>();
        }
    }
}
