namespace OrdersService.Application.Dtos
{
    public class OrderCreateDto
    {
        public int Status { get; set; }

        public string Destination { get; set; }

        public string Address { get; set; }
    }
}