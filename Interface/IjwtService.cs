namespace RestAPIPractica.Interface
{
    public interface IjwtService
    {
        public string generateToken(string user);
        dynamic GenerateToken(dynamic email);
    }
}
