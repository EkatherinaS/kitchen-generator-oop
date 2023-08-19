namespace KitchenGenerator
{
    internal class Kitchen
    {
        public int MainWall { get; set; }
        private int width;
        private int height;

        public Kitchen(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width 
        { 
            get { return width; }
            set
            {
                if (value > 0) width = value;
                else throw new ArgumentException("Ширина кухни должна быть положительным числом");
            }
        }
        public int Height
        { 
            get { return height; }
            set
            {
                if (value > 0) height = value;
                else throw new ArgumentException("Длина кухни должна быть положительным числом");
            }
        }
    }
}
