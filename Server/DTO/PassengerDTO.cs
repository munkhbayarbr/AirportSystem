namespace Server.DTO
{
   public record PassengerCreateDTO(string Firstname, string Lastname, DateTime Birthday, string PhoneNumber, int PassportId);

    public record PassengerReadDTO(int Id, string Firstname, string Lastname, DateTime Birthday, string PhoneNumber, int PassportId);

    public record PassengerUpdateDTO(int Id, string Firstname, string Lastname, DateTime Birthday, string PhoneNumber, int PassportId);
}
