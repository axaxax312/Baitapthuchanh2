using System.ComponentModel.DataAnnotations;

namespace HoTrungNguyenBTTH2.Models{
  
    public class Person{
        [Key]
        public string PersonAge{get;set;}
        public string PersonName {get;set;}
    }
}