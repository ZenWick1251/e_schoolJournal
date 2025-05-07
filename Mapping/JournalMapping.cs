using AutoMapper;
using e_journal.Models;
using e_journal.DTOs;

public class JournalMapping : Profile
{
    public JournalMapping()
    {
        
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>();

        
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.SchoolClass.Name));
        CreateMap<StudentDto, Student>();

        
        CreateMap<Teacher, TeacherDto>();
        CreateMap<TeacherDto, Teacher>();

        
        CreateMap<SchoolClass, SchoolClassDto>();
        CreateMap<SchoolClassDto, SchoolClass>();

        
        CreateMap<Subject, SubjectDto>();
        CreateMap<SubjectDto, Subject>();

        
        CreateMap<Grade, GradeDto>();
        CreateMap<GradeDto, Grade>();

        
        CreateMap<LessonSchedule, LessonScheduleDto>();
        CreateMap<LessonScheduleDto, LessonSchedule>();
    }
}