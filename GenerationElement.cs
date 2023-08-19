namespace KitchenGenerator
{
    internal class GenerationElement
    {
        internal readonly Dictionary<string, string> errorOptions = new Dictionary<string, string>() {
                                                            { "smallRoom", "модуль не помещается в комнату" },
                                                            { "cornerIntersect", "модуль не влез в угол" },
                                                            { "pipePlace", "труба расположена не у стены" } };

        internal GenerationElement() {
            IsVisible = true;
            GenerationSuccessful = true;
        }

        private string generationError = "";
        public bool IsVisible { get; set; }
        public bool GenerationSuccessful { get; set; }
        public string Color 
        {
            get {
                if (GenerationSuccessful) return "black";
                else return "red";
            }
        }


        public string GenerationError
        {
            get { return generationError; }
            set
            {
                generationError = value;
                GenerationSuccessful = false;
            }
        }
    }
}
