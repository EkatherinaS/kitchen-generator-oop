namespace KitchenGenerator
{
    internal abstract class KitchenElement : GenerationElement
    {
        private int x = -1;
        private int y = -1;

        private int width;
        private int depth;
        private string text = "Элемент";
        private Kitchen kitchen;


        public KitchenElement(Kitchen kitchen, int width, int depth)
        {
            Kitchen = kitchen;
            Width = width;
            Depth = depth;
        }

        public abstract void GeneratePosition(bool moveLeft);

        public string HtmlCode
        {
            get
            {
                if (!IsVisible || x == -1 || y == -1) return "";
                return
                $@"context.lineWidth = 2;

                context.beginPath();
                context.fillStyle = 'white';
                context.strokeStyle = '{Color}';
                context.rect({x} + startPoint, {y} + startPoint, {width}, {depth});
                context.stroke();
                context.fillRect({x} + startPoint, {y} + startPoint, {width}, {depth});
                context.closePath();

                context.beginPath();
                context.fillStyle = '{Color}';
                context.fillText('{text}', {x} + startPoint + {width} / 2, {y} + startPoint + {depth} / 2 + 5);
                context.closePath();";
            }
        }

        public Kitchen Kitchen
        {
            get { return kitchen; }
            set
            {
                if (value != null) kitchen = value;
                else throw new ArgumentException("Кухня не может быть равна null");
            }
        }

        public string Text
        {
            get { return text; }
            set {
                if (!(value == null || value.Length == 0)) text = value;
                else throw new ArgumentException("Текст элемента не может быть пустым");
            }
        }

        public int Width
        {
            get { return width; }
            set {
                if (value < 0) throw new ArgumentException("Ширина элемента должна быть положительным числом");
                else if (value > kitchen.Width)
                {
                    GenerationError = errorOptions["smallRoom"];
                    IsVisible = false;
                }
                else width = value;
            }
        }

        public int Depth
        {
            get { return depth; }
            set
            {
                if (value < 0) throw new ArgumentException("Глубина элемента должна быть положительным числом");
                else if (value > kitchen.Height)
                {
                    GenerationError = errorOptions["smallRoom"];
                    IsVisible = false;
                }
                else depth = value;
            }
        }

        public int X
        {
            get { return x; }
            set
            {
                if (value >= 0 && value <= kitchen.Width - width) x = value;
                else
                {
                    GenerationError = errorOptions["cornerIntersect"];
                    if (value < 0) x = 0;
                    if (value > kitchen.Width - width) x = kitchen.Width - width;
                }
            }
        }

        public int Y
        {
            get { return y; }
            set
            {
                if (value >= 0 && value <= kitchen.Height - depth) y = value;
                else
                {
                    GenerationError = errorOptions["cornerIntersect"];
                    if (value < 0) y = 0;
                    if (value > kitchen.Height - depth) x = kitchen.Height - depth;
                }
            }
        }
    }
}
