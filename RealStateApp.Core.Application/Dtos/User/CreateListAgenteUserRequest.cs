namespace RealStateApp.Core.Application.Dtos.User
{
    public class CreateListAgenteUserRequest
    {
        public string FirstName { get; set; }  
        public string LastName { get; set; }   
        public string Email { get; set; }      
        public bool IsActive { get; set; }     
        public int PropertyCount { get; set; }
    }
}
