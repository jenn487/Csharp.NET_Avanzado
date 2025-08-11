using System.Threading.Tasks;

namespace TaskManagement.Domain.DTO
{
    public class CreateTaskFromTemplateDto
    {
        public string TemplateName { get; set; } // son alta, media, normal" espanglish high, low
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string ExtraData { get; set; }
    }

}
