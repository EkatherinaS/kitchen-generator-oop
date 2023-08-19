namespace KitchenGenerator
{
    internal class HtmlGenerator
    {
        //Возможные сообщения о результатах генерации
        private readonly Dictionary<string, string> messageOptions = new Dictionary<string, string>() {
                                                            { "success", "Генерация прошла успешно" },
                                                            { "fail", "Генерация прошла неуспешно: " } };
        private int cornerIndent;
        private string messageText;
        private string messageColor;

        public Kitchen Kitchen { get; set; }
        public Stove Stove { get; set; }
        public Sink Sink { get; set; }
        public Pipe Pipe { get; set; }
        public int СornerIndent { 
            get { return cornerIndent; }
            set {
                if (value >= 0) cornerIndent = value;
                else throw new ArgumentException("Отступ не может быть меньше нуля");
            }
        }

        public HtmlGenerator(int width, int height, int pipeX, int pipeY, int sinkWidth, int sinkDepth, bool sinkCorner, 
            int stoveWidth, int stoveDepth, int cornerIndent) 
        {
            Kitchen = new Kitchen(width, height);
            Pipe = new Pipe(Kitchen, pipeX, pipeY);
            Sink = new Sink(Kitchen, Pipe, sinkWidth, sinkDepth, sinkCorner, cornerIndent);
            Stove = new Stove(Kitchen, Sink, stoveWidth, stoveDepth);
        }

        private void updateMessage(bool success, string error = "")
        {
            if (success)
            {
                messageText = messageOptions["success"];
                messageColor = "black";
            }
            else
            {
                messageText = messageOptions["fail"] + error;
                messageColor = "red";
            }
        }

        public string GenerateHtml(bool rightSideStove)
        {
            if (!Pipe.GenerationSuccessful)
            {
                updateMessage(false, Pipe.GenerationError);
                return HtmlCode; 
            }
            Sink.GeneratePosition(rightSideStove);
            if (!Sink.GenerationSuccessful)
            {
                Stove.IsVisible = false;
                updateMessage(false, Sink.GenerationError);
                return HtmlCode;
            }
            Stove.GeneratePosition(rightSideStove);
            if (!Stove.GenerationSuccessful)
            {
                updateMessage(false, Stove.GenerationError);
                return HtmlCode;
            }
            updateMessage(true);
            return HtmlCode;
        }

        public string HtmlCode
        {
            get
            {
                return $@"
                <canvas id=myCanvas width='{Kitchen.Width + 40}' height='{Kitchen.Height + 60}' style='border: 1px solid #d3d3d3;'></canvas>
                <script>
                    var c = document.getElementById('myCanvas');
                    var context = c.getContext('2d');
                    context.font = '10px Arial';
                    context.textAlign = 'center';

                    let startPoint = 20;

                    let kitchenWidth = {Kitchen.Width} + startPoint;
                    let kitchenDepth = {Kitchen.Height} + startPoint;

                    context.beginPath();
                    context.lineWidth = 1;
                    context.strokeStyle = 'grey';
                    for (let i = startPoint; i <= kitchenWidth; i += 10) {{
                        context.moveTo(i, startPoint);
                        context.lineTo(i, kitchenDepth);
                        context.stroke();
                    }}
                    for (let j = startPoint; j <= kitchenDepth; j += 10) {{
                        context.moveTo(startPoint, j);
                        context.lineTo(kitchenWidth, j);
                        context.stroke();
                    }}
                    context.closePath();

                    context.beginPath();
                    context.lineDepth = 2;
                    context.strokeStyle = 'blue';
                    for (let i = startPoint; i <= kitchenWidth; i += 100) {{
                        context.moveTo(i, startPoint);
                        context.lineTo(i, kitchenDepth);
                        context.stroke();
                    }}
                    for (let j = startPoint; j <= kitchenDepth; j += 100) {{
                        context.moveTo(startPoint, j);
                        context.lineTo(kitchenWidth, j);
                        context.stroke();
                    }}
                    context.closePath();

                    context.fillStyle = 'black';
                    context.beginPath();
                    for (let i = 0; i < kitchenWidth; i += 100) {{
                        context.fillText(i / 100 + 'м', i + startPoint, 15);
                    }}
                    for (let j = 100; j < kitchenDepth; j += 100) {{
                        context.fillText(j / 100 + 'м', 10, j + startPoint);
                    }}
                    context.closePath();

                    {Sink.HtmlCode}
                    {Stove.HtmlCode}

                    context.beginPath();
                    context.fillStyle = '{messageColor}';
                    context.textAlign = 'left';
                    context.fillText('{messageText}', startPoint, kitchenDepth + startPoint );
                    context.closePath();
                </script>";
            }
        }
    }
}
