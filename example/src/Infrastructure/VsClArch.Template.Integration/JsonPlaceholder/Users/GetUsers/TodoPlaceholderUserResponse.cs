using System;

namespace VsClArch.Template.Integration.JsonPlaceholder.Users.GetUsers;

/// <summary>
/// Represents a user from the JSON Placeholder API.
/// </summary>
public class TodoPlaceholderUserResponse
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the user's address.
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the user's website.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Gets or sets the user's company information.
    /// </summary>
    public Company? Company { get; set; }
}

/// <summary>
/// Represents the address of a user.
/// </summary>
public class Address
{
    /// <summary>
    /// Gets or sets the street.
    /// </summary>
    public string? Street { get; set; }

    /// <summary>
    /// Gets or sets the suite or apartment number.
    /// </summary>
    public string? Suite { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Gets or sets the ZIP code.
    /// </summary>
    public string? Zipcode { get; set; }

    /// <summary>
    /// Gets or sets the geographical coordinates.
    /// </summary>
    public Geo? Geo { get; set; }
}

/// <summary>
/// Represents geographical coordinates.
/// </summary>
public class Geo
{
    /// <summary>
    /// Gets or sets the latitude.
    /// </summary>
    public string? Lat { get; set; }

    /// <summary>
    /// Gets or sets the longitude.
    /// </summary>
    public string? Lng { get; set; }
}

/// <summary>
/// Represents company information.
/// </summary>
public class Company
{
    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the company's catchphrase.
    /// </summary>
    public string? CatchPhrase { get; set; }

    /// <summary>
    /// Gets or sets the company's business slogan.
    /// </summary>
    public string? Bs { get; set; }
}
