using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BPIBankSystem.API.Entities
{

    public class CategoryHelp
    {

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
        [JsonIgnore]
        public ICollection<Help> Helps { get; set; }
    }
}
