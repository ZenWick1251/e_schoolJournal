using AutoMapper;
using e_journal.Models;
using e_journal.DTOs;

public class JournalMapping : Profile
{
    public JournalMapping()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<CreateUserDto, User>();

        CreateMap<Student, StudentDto>().ReverseMap();
        CreateMap<Teacher, TeacherDto>().ReverseMap();
        CreateMap<Subject, SubjectDto>().ReverseMap();
        CreateMap<SchoolClass, SchoolClassDto>().ReverseMap();
        CreateMap<Grade, GradeDto>().ReverseMap();
        CreateMap<LessonSchedule, LessonScheduleDto>().ReverseMap();
    }
}