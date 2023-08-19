namespace KitchenGenerator
{
    internal class Pipe : GenerationElement
    {
        private readonly Kitchen kitchen;
        private readonly int x;
        private readonly int y;

        public Pipe(Kitchen kitchen, int x, int y)
        {
            this.kitchen = kitchen;
            if ((x == 0 || x == kitchen.Width) && (y >= 0 && y <= kitchen.Height) ||
                (x >= 0 && x <= kitchen.Width) && (y == 0 || y == kitchen.Height))
            {
                this.x = x;
                this.y = y;
            }
            else GenerationError = errorOptions["pipePlace"];
        }

        public Kitchen Kitchen { get { return kitchen; } }

        public int X { get { return x; } }

        public int Y { get { return y; } }
    }
}
