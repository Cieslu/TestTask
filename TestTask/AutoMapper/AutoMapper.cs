using AutoMapper;
using TestTask.Models;
using TestTask.ModelsDTO;

namespace TestTask.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()//This tool is used to map one object to another, for example: from ToDo to ToDoDTO.
        {
            CreateMap<ToDo, ToDoDTO>();//From ToDo to ToDoDTO.
            CreateMap<ToDoDTO, ToDo>();//From ToDoDTO to ToDo.
            CreateMap<NewToDoDTO, ToDo>();//From NewToDoDTO to ToDo.
        }
    }
}
