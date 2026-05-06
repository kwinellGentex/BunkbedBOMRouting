namespace BunkbedBOMRouting.Classes
{
    /// <summary>
    /// Struct to represent a part in the Bill of Materials (BOM) with its description and quantity.
    /// </summary>
    public struct Part
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
