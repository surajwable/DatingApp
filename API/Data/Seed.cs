using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            //checking if our Users table has any data already if yes then return without doing nothing
            if(await context.Users.AnyAsync()) return;

            //reading data from UserSeedData.json file
            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            //in case if we made any mistake in casing in our seed data we will also create some options
            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive=true};

            //now deserialize in the form of list of objects/users 
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            //now generating password for each dummy user
            foreach (var user in users)
            {
                using var hmac =  new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;
                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
            
        }
    }
}