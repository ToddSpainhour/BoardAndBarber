using System;

namespace BoardAndBarber.Models
{
    public class Customer
    {
        //model classes should not have behavior, shouldn't do any work, they are just places to store information that's going our or coming in

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string FavoriteBarber { get; set; }
        public string Notes { get; set; }
    }
}
