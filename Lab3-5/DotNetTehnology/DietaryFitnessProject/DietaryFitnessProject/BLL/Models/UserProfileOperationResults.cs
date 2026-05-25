using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.BLL.Models;

public enum UserProfileGetOutcome
{
    Success,
    Unauthorized,
    NotFound
}

public readonly record struct UserProfileGetResult(UserProfileGetOutcome Outcome, User? User);

public enum UpdateUserProfileOutcome
{
    Success,
    Unauthorized,
    NotFound
}
