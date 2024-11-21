namespace ComputerApi.Models
{
    public record CreateOsystemDto(string Name);
    public record UpdateOsystemDto(string Name);

    public record CreateComputerDto(string? Brand, string? Type, double? Display, int? Memory, Guid OsId);
}
