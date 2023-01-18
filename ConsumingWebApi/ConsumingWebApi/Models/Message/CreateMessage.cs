namespace ConsumingWebApi.Models.Message
{
    public class CreateMessage
    {
        public int? RoomId { get; set; }
        public string Message { get; set; } = null!;
    }
}
