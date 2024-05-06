namespace DocnetCorePractice.Model
{

    public class UpdateOrderRequestModel
    {
        public string orderId { get; set; }
        public List<AddItems> addItems { get; set; }
        public List<UpdateItems> updateItems { get; set; }
        public List<RemoveItems> removeItems { get; set; }

    }

    public class RemoveItems
    {
        public string orderItemId { get; set; }
    }

    public class AddItems
    {
        public string caffeId { get; set; } 
        public int volumn { get; set; }
    }
    public class UpdateItems
    {
        public string orderItemId { get; set; }
        public int volumn { get; set; }
    }
}
