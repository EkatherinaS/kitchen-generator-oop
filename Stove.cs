namespace KitchenGenerator
{
    internal class Stove : KitchenElement
    {
        Sink Sink { get; set; }
        public Stove(Kitchen kitchen, Sink sink, int width, int depth) : base(kitchen, width, depth)
        {
            Text = "Плита";
            Sink = sink;
        }

        public override void GeneratePosition(bool moveRight)
        {
            //Определяем координаты
            switch (Kitchen.MainWall)
            {
                case 0:
                    X = 0;
                    if (moveRight) Y = Sink.Y - Depth;
                    else Y = Sink.Y + Sink.Depth;
                    break;
                case 1:
                    if (moveRight) X = Sink.X + Sink.Width;
                    else X = Sink.X - Width;
                    Y = 0;
                    break;
                case 2:
                    X = Kitchen.Width - Width;
                    if (moveRight) Y = Sink.Y + Sink.Depth;
                    else Y = Sink.Y - Depth;
                    break;
                case 3:
                    if (moveRight) X = Sink.X - Width;
                    else X = Sink.X + Sink.Width;
                    Y = Kitchen.Height - Depth;
                    break;
            }
        }
    }
}
