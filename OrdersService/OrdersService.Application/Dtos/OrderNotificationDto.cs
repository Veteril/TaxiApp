namespace OrdersService.Application.Dtos
{
    public class OrderNotificationDto
    {
        public string Id { get; set; }

        public int Status { get; set; }

        public string Destination { get; set; }

        public string Address { get; set; }

        public string OrderId { get; set; }
    }
}
