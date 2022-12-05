
public static class UserLogInData
{
    public enum UserType
    {
        Cleaner,
        Landlord
    }

    public static string userEmail;
    public static bool isUserLoggedIn;
    public static UserType userType;
}
