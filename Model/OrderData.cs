namespace MajorTest.Model
{
    public class OrderData
    {
        public int Id { get; set; }
        public string AdressSender { get; set; }
        public string AdressReceiver { get; set; }
        public double? Weight { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double Volume { get; set; }
        public string Status { get; set; }
        public bool Checker { get; set; }
        public string CancelationReason { get; set; }

    }
}
