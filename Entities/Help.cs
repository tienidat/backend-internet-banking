using System.ComponentModel.DataAnnotations.Schema;

namespace BPIBankSystem.API.Entities
{
    public class Help
    {

        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
        public CategoryHelp Category { get; set; }
    }
}
