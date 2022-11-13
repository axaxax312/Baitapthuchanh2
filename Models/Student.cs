using System.ComponentModel.DataAnnotations;

namespace HoTrungNguyenBTTH2.Models{
    
    public class Student{
        [Key]
        public string StudentID{get;set;}
        public string StudentName {get;set;}
    }
}