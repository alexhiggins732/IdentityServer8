namespace IdentityServerHost.Quickstart.UI;

public class TestUsers
{

    static readonly object UserAddress = new
    {
        street_address = "One Hacker Way",
        locality = "Heidelberg",
        postal_code = 69118,
        country = "Germany"
    };

    public static List<TestUser> Users => new List<TestUser>
    {
        new()
        {
            SubjectId = "818727",
            Username = "alice",
            Password = "alice",
            Claims =
            {
                new (Name, "Alice Smith"),
                new (GivenName, "Alice"),
                new (FamilyName, "Smith"),
                new (Email, "AliceSmith@email.com"),
                new (EmailVerified, "true", ClaimValueTypes.Boolean),
                new (WebSite, "http://alice.com"),
                new (Address, JsonSerializer.Serialize(UserAddress), IdentityServerClaimValueTypes.Json)
            }
        },
        new()
        {
            SubjectId = "88421113",
            Username = "bob",
            Password = "bob",
            Claims =
            {
                new (Name, "Bob Smith"),
                new (GivenName, "Bob"),
                new (FamilyName, "Smith"),
                new (Email, "BobSmith@email.com"),
                new (EmailVerified, "true", ClaimValueTypes.Boolean),
                new (WebSite, "http://bob.com"),
                new (Address, JsonSerializer.Serialize(UserAddress), IdentityServerClaimValueTypes.Json)
            }
        }
    };
}
