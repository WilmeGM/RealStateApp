﻿namespace RealStateApp.Core.Application.Dtos.User
{
    public class UserListAgenteResponse
    {
        public string Id { get; set; }       
        public string FirstName { get; set; }  
        public string LastName { get; set; }   
        public string Email { get; set; }      
        public int PropertyCount { get; set; } 
        public bool IsActive { get; set; }

    }
}
