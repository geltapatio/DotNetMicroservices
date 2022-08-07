namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public string UserName { get; set; }
        //Extended mean that will retrieve some additional information from the Catalog microservices
        public List<BasketItemExtendedModel> Items { get; set; } = new List<BasketItemExtendedModel>();
        public decimal TotalPrice { get; set; }
    }
}
