using AutoMapper;

namespace ProjekatStudent.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Entities.Student, Models.StudentWithoutExamDto>();
            CreateMap<Entities.Student, Models.StudentDto>();
            CreateMap<Models.StudentWithoutExamDto, Entities.Student>();
        }
    }
}
