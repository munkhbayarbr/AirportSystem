namespace Server.DTO
{
    /// <summary>
    /// DTO for creating a passenger.
    /// </summary>
    /// <param name="Firstname"></param>
    /// <param name="Lastname"></param>
    /// <param name="Birthday"></param>
    /// <param name="PhoneNumber"></param>
    /// <param name="PassportId"></param>
    public record PassengerCreateDTO(string Firstname, string Lastname, DateTime Birthday, string PhoneNumber, int PassportId);

    /// <summary>
    /// DTO for reading a passenger.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Firstname"></param>
    /// <param name="Lastname"></param>
    /// <param name="Birthday"></param>
    /// <param name="PhoneNumber"></param>
    /// <param name="PassportId"></param>
    public record PassengerReadDTO(int Id, string Firstname, string Lastname, DateTime Birthday, string PhoneNumber, int PassportId);

    /// <summary>
    /// DTO for updating a passenger.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Firstname"></param>
    /// <param name="Lastname"></param>
    /// <param name="Birthday"></param>
    /// <param name="PhoneNumber"></param>
    /// <param name="PassportId"></param>
    public record PassengerUpdateDTO(int Id, string Firstname, string Lastname, DateTime Birthday, string PhoneNumber, int PassportId);
}
