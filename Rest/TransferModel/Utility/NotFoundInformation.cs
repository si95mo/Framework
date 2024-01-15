namespace Rest.TransferModel.Utility
{
    public class NotFoundInformation : Information
    {
        private const string HeaderConst = "Not found";
        
        public string Header => HeaderConst;
        public string Description { get; set; } = string.Empty;

        public NotFoundInformation()
        { }

        public NotFoundInformation(string description)
        {
            Description = description;
        }
    }
}
