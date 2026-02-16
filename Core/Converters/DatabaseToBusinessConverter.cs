using Core.Models.Business;
using DatabaseModels.Models.Database;

namespace Core.Converters
{
    public static class DatabaseToBusinessConverter
    {
        public static Group ToBusiness(this GroupDb dbModel)
        {
            return new Group 
            { 
                Id = dbModel.Id, 
                Name = dbModel.Name, 
                CreationDate = dbModel.CreationDate 
            };
        }

        public static Student ToBusiness(this StudentDb dbModel)
        {
            return new Student 
            { 
                Id = dbModel.Id, 
                GroupId = dbModel.GroupId, 
                Name = dbModel.Name, 
                Age = dbModel.Age 
            };
        }

        public static Curator ToBusiness(this CuratorDb dbModel)
        {
            return new Curator 
            { 
                Id = dbModel.Id, 
                GroupId = dbModel.GroupId, 
                Name = dbModel.Name, 
                Email = dbModel.Email 
            };
        }

        public static GroupDb ToDatabase(this Group businessModel)
        {
            return new GroupDb 
            { 
                Id = businessModel.Id, 
                Name = businessModel.Name, 
                CreationDate = businessModel.CreationDate 
            };
        }

        public static StudentDb ToDatabase(this Student businessModel)
        {
            return new StudentDb 
            { 
                Id = businessModel.Id, 
                GroupId = businessModel.GroupId, 
                Name = businessModel.Name, 
                Age = businessModel.Age 
            };
        }

        public static CuratorDb ToDatabase(this Curator businessModel)
        {
            return new CuratorDb 
            { 
                Id = businessModel.Id, 
                GroupId = businessModel.GroupId, 
                Name = businessModel.Name, 
                Email = businessModel.Email 
            };
        }
    }
}