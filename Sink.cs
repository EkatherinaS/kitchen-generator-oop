namespace KitchenGenerator
{
    internal class Sink : KitchenElement
    {
        private bool Corner { get; set; }
        private int CornerIndent { get; set; }
        private Pipe Pipe { get; set; }

        public Sink(Kitchen kitchen, Pipe pipe, int width, int depth, bool corner, int cornerIndent) : base(kitchen, width, depth)
        {
            if (corner) Text = "Мойка угловая";
            else Text = "Мойка";
            Corner = corner;
            CornerIndent = cornerIndent;
            Pipe = pipe;
        }

        public override void GeneratePosition(bool moveLeft)
        {
            //Первично определяется главная стена (на которой будут располагаться элементы)
            //Стены определяются следующим образом:
            //0 - левая, 1 - верхняя, 2 - правая, 3 - нижняя
            if (Pipe.X == 0) Kitchen.MainWall = 0;
            else if (Pipe.Y == 0) Kitchen.MainWall = 1;
            else if (Pipe.X == Kitchen.Width) Kitchen.MainWall = 2;
            else if (Pipe.Y == Kitchen.Height) Kitchen.MainWall = 3;
            else { Kitchen.MainWall = -1; return; }

            //Угловую мойку можно вращать
            if (Corner)
            {
                CornerIndent = 0;
                if (Width > Depth)
                {
                    int temp = Width;
                    Width = Depth;
                    Depth = temp;
                }
            }

            //Определяем промежуток, в который должна попадать мойка относительно трубы
            int sinkMin;
            int sinkMax;
            if (Kitchen.MainWall % 2 == 0)
            {
                sinkMin = Math.Max(CornerIndent, Pipe.Y - 50 - Width);
                sinkMax = Math.Min(Kitchen.Height - CornerIndent, Pipe.Y + 50 + Width);
            }
            else
            {
                sinkMin = Math.Max(CornerIndent, Pipe.X - 50 - Width);
                sinkMax = Math.Min(Kitchen.Width - CornerIndent, Pipe.X + 50 + Width);
            }

            if (sinkMin > sinkMax || sinkMax - sinkMin < Width)
            {
                GenerationError = errorOptions["cornerIntersect"];
                int temp = sinkMin;
                sinkMin = sinkMax;
                sinkMax = temp;
            }

            //Корректируем главную стену в зависимости от угла, в который попадает труба,
            // и предпочитаемого положения относительно других элементов
            if (Corner)
            {
                if (sinkMin == 0 && (Kitchen.MainWall == 0 || Kitchen.MainWall == 1))
                {
                    if (moveLeft)
                    {
                        Kitchen.MainWall = 1;
                        sinkMin = 0;
                        sinkMax = Width;
                    }
                    else
                    {
                        Kitchen.MainWall = 0;
                        sinkMin = 0;
                        sinkMax = Width;
                    }
                }
                else if (sinkMax == Kitchen.Width && Kitchen.MainWall == 1 || sinkMin == 0 && Kitchen.MainWall == 2)
                {
                    if (moveLeft)
                    {
                        Kitchen.MainWall = 2;
                        sinkMin = 0;
                        sinkMax = Width;
                    }
                    else
                    {
                        Kitchen.MainWall = 1;
                        sinkMin = Kitchen.Width - Width;
                        sinkMax = Kitchen.Width;
                    }
                }
                else if (sinkMax == Kitchen.Height && Kitchen.MainWall == 2 || sinkMax == Kitchen.Width && Kitchen.MainWall == 3)
                {
                    if (moveLeft)
                    {
                        Kitchen.MainWall = 3;
                        sinkMin = Kitchen.Width - Width;
                        sinkMax = Kitchen.Width;
                    }
                    else
                    {
                        Kitchen.MainWall = 2;
                        sinkMin = Kitchen.Height - Width;
                        sinkMax = Kitchen.Height;
                    }
                }
                else if (sinkMin == 0 && Kitchen.MainWall == 3 || sinkMax == Kitchen.Height && Kitchen.MainWall == 0)
                {
                    if (moveLeft)
                    {
                        Kitchen.MainWall = 0;
                        sinkMin = Kitchen.Height - Width;
                        sinkMax = Kitchen.Height;
                    }
                    else
                    {
                        Kitchen.MainWall = 3;
                        sinkMin = 0;
                        sinkMax = Width;
                    }
                }
            }

            //Меняем ширину и глубину местами для корректного отображения на боковых стенах
            if (Kitchen.MainWall % 2 == 0)
            {
                int temp = Width;
                Width = Depth;
                Depth = temp;
            }

            //Определяем координаты
            switch (Kitchen.MainWall)
            {
                case 0:
                    X = 0;
                    if (moveLeft) Y = sinkMax - Depth;
                    else Y = sinkMin;
                    break;
                case 1:
                    if (moveLeft) X = sinkMin;
                    else X = sinkMax - Width;
                    Y = 0;
                    break;
                case 2:
                    X = Kitchen.Width - Width;
                    if (moveLeft) Y = sinkMin;
                    else Y = sinkMax - Depth;
                    break;
                case 3:
                    if (moveLeft) X = sinkMax - Width;
                    else X = sinkMin;
                    Y = Kitchen.Height - Depth;
                    break;
            }
        }
    }
}
