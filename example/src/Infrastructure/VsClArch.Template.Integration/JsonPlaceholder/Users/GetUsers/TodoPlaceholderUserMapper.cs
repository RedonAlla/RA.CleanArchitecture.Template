using System;
using Riok.Mapperly.Abstractions;
using VsClArch.Template.Application.Models.TodoJsonPlaceholder;

namespace VsClArch.Template.Integration.JsonPlaceholder.Users.GetUsers;

/// <summary>
/// A mapper class for converting <see cref="TodoPlaceholderUserResponse"/> objects to <see cref="TodoPlaceholderUser"/> objects.
/// This class uses the Mapperly source generator to generate the mapping implementation.
/// </summary>
[Mapper]
public partial class TodoPlaceholderUserMapper
{
    /// <summary>
    /// Maps a <see cref="TodoPlaceholderUserResponse"/> object to a <see cref="TodoPlaceholderUser"/> object.
    /// </summary>
    /// <param name="source">The source <see cref="TodoPlaceholderUserResponse"/> object.</param>
    /// <returns>The mapped <see cref="TodoPlaceholderUser"/> object.</returns>
    [MapperIgnoreSource(nameof(TodoPlaceholderUserResponse.Id))] // Id is not needed in the target model
    [MapperIgnoreSource(nameof(TodoPlaceholderUserResponse.Address))] // Address is not needed in the target model
    [MapperIgnoreSource(nameof(TodoPlaceholderUserResponse.Phone))] // Phone is not needed in the target model
    [MapperIgnoreSource(nameof(TodoPlaceholderUserResponse.Website))] // Website is not needed in the target model
    [MapperIgnoreSource(nameof(TodoPlaceholderUserResponse.Company))] // Company is not needed in the target model
    public partial TodoPlaceholderUser Map(TodoPlaceholderUserResponse source);
}
