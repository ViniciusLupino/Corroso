namespace MimDaNota.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Endereco { get; set; }
        public string Cpf { get; set; }
    }
}
