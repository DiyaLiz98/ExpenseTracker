﻿namespace ExpenseTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  // 🔹 Plain text for now
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; }
    }

}
