using Lab_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Lab_2.Data
{
    public class DbInitializer
    {
        public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            // create the database if it doesn't exist
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] rolesToBeAdded = { "Manager", "Player" };
            string managerEmail = Environment.GetEnvironmentVariable("manager-email-ssd");
            string playerEmail = Environment.GetEnvironmentVariable("player-email-ssd");
            ApplicationUser[] users = {
                new ApplicationUser
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    FirstName = "Xavi",
                    LastName = "Hernandez",
                    EmailConfirmed = true
                },
                new ApplicationUser
                {
                    UserName = playerEmail,
                    Email = playerEmail,
                    FirstName = "Ousmane",
                    LastName = "Dembele",
                    EmailConfirmed = true
                }
            };
            var result = new IdentityResult();

            foreach (var role in rolesToBeAdded)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    result = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        Console.WriteLine("Something went wrong when creating the role " + role);
                        return 1;
                    }
                }
                else
                {
                    Console.WriteLine(role + " already exists");
                    return 2;
                }
            }

            foreach (var user in users)
            {

                var exists = await userManager.FindByEmailAsync(user.Email);

                if (exists == null)
                {
                    result = await userManager.CreateAsync(user, Environment.GetEnvironmentVariable("user-password-ssd"));

                    if (!result.Succeeded)
                    {
                        Console.WriteLine("Something went wrong when creating the new user " + user.Email);

                        return 3;
                    }
                }
                else
                {
                    Console.WriteLine(user.Email + " already exists");

                    return 4;
                }
            }

            result = await userManager.AddToRoleAsync(users[0], "Manager");
            if (!result.Succeeded)
            {
                Console.WriteLine("Something went wrong when Adding the role Manager to the user " + users[0].Email);
                return 5;
            }

            result = await userManager.AddToRoleAsync(users[1], "Player");
            if (!result.Succeeded)
            {
                Console.WriteLine("Something went wrong when Adding the role Player to the user " + users[1].Email);
                return 6;
            }

            Team[] teams =
            {
                new Team{
                    TeamName = "Manchester City",
                    Email = Environment.GetEnvironmentVariable("team1-ssd")
                },
                new Team{
                    TeamName = "Manchester United",
                    Email = Environment.GetEnvironmentVariable("team2-ssd")
                },
                new Team{
                    TeamName = "Real Madrid",
                    Email = Environment.GetEnvironmentVariable("team3-ssd")
                }
            };

            foreach(var team in teams)
            {
                try
                {
                    await context.AddAsync(team);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something went wrong while adding team " + team.Email);
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return 0;
        }

    }
}
