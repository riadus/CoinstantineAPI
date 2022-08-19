using CoinstantineAPI.Data;

namespace CoinstantineAPI.Email
{
    public class EmailObject
    {
        public UserIdentity Recipient { get; set; }
        public string TemplateId { get; set; }
        public object Template { get; set; }
    }
}
