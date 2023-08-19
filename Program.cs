namespace KitchenGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Вводим входные данные через консоль
            Console.WriteLine("Введите данные для генерации кухни (в см)");
            Console.Write("Длина кухни: ");
            int width = Input.enterInteger();
            Console.Write("Ширина кухни: ");
            int height = Input.enterInteger();
            Console.Write("Отступ от угла для неугловой мойки: ");
            int cornerIndent = Input.enterInteger();
            Console.Write("Расположение трубы по x: ");
            int pipeX = Input.enterInteger();
            Console.Write("Расположение трубы по y: ");
            int pipeY = Input.enterInteger();
            Console.Write("Ширина мойки: ");
            int sinkWidth = Input.enterInteger();
            Console.Write("Глубина мойки: ");
            int sinkDepth = Input.enterInteger();
            Console.Write("Может ли мойка быть угловой (y/n): ");
            bool sinkCorner = Input.enterBoolean();
            Console.Write("Ширина плиты: ");
            int stoveWidth = Input.enterInteger();
            Console.Write("Глубина плиты: ");
            int stoveDepth = Input.enterInteger();

            //Генерируем два файла htmlRight и htmlLeft в папке C:\temp
            HtmlGenerator htmlGenerator = new HtmlGenerator(width, height, pipeX, pipeY, sinkWidth, sinkDepth, sinkCorner, stoveWidth, stoveDepth, cornerIndent);
            
            //HtmlGenerator htmlGenerator = new HtmlGenerator(600, 400, 590, 400, 300, 100, false, 140, 125, 50);
            string html1 = htmlGenerator.GenerateHtml(true);
            string html2 = htmlGenerator.GenerateHtml(false);

            Directory.CreateDirectory("C:\\temp");
            File.WriteAllText("C:\\temp\\htmlRight.html", html1);
            File.WriteAllText("C:\\temp\\htmlLeft.html", html2);
            Console.WriteLine("Генерация завершена, файлы htmlRight и htmlLeft расположены в папке C:\\temp");
        }

        /*//Функция генерации html для определенного положения плиты относительно мойки
        static string GenerateHtml(
              int width,
              int height,
              int pipeX,
              int pipeY,
              int sinkWidth,
              int sinkDepth,
              bool sinkCorner,
              int stoveWidth,
              int stoveDepth,
              int cornerIndent,
              bool rightSideStove)
        {
            //Возможные сообщения о результатах генерации
            Dictionary<string, string> messageOptions = new Dictionary<string, string>() {
                                                            { "success", "Генерация прошла успешно" },
                                                            { "fail", "Генерация прошла неуспешно: " },
                                                            { "intersect", "пересекается с предыдущим модулем" },
                                                            { "pipePlace", "труба расположена не у стены" },
                                                            { "smallRoom", "модуль не помещается в комнату" },
                                                            { "cornerIntersect", "модуль не влезает в угол" } };

            string sinkColor = "black";
            string sinkText = "Мойка";
            int sinkX = -1;
            int sinkY = -1;
            bool sinkVisible = true;

            string stoveColor = "black";
            string StoveText = "Плита";
            int stoveX = -1;
            int stoveY = -1;
            bool stoveVisible = true;

            string messageColor = "black";
            string messageText = messageOptions["success"];
            string sinkCode = "";
            string stoveCode = "";
            int mainWall = -1;

            //Первично определяется главная стена (на которой будут располагаться элементы)
            //Стены определяются следующим образом:
            //0 - левая, 1 - верхняя, 2 - правая, 3 - нижняя
            if (pipeX == 0) mainWall = 0;
            else if (pipeY == 0) mainWall = 1;
            else if (pipeX == width) mainWall = 2;
            else if (pipeY == height) mainWall = 3;
            else messageText = messageOptions["fail"] + messageOptions["pipePlace"];

            //Если труба находится не у стены, генерация прерывается
            if (mainWall == -1)
            {
                messageColor = "red";
                sinkVisible = false;
                stoveVisible = false;
            }
            else
            {
                //Угловую мойку можно вращать
                if (sinkCorner)
                {
                    cornerIndent = 0;
                    if (sinkWidth > sinkDepth)
                    {
                        int temp = sinkWidth;
                        sinkWidth = sinkDepth;
                        sinkDepth = temp;
                    }
                }

                //Определяем промежуток, в который должна попадать мойка относительно трубы
                int sinkMin;
                int sinkMax;
                if (mainWall % 2 == 0)
                {
                    sinkMin = Math.Max(cornerIndent, pipeY - 50 - sinkWidth);
                    sinkMax = Math.Min(height - cornerIndent, pipeY + 50 + sinkWidth);
                }
                else
                {
                    sinkMin = Math.Max(cornerIndent, pipeX - 50 - sinkWidth);
                    sinkMax = Math.Min(width - cornerIndent, pipeX + 50 + sinkWidth);
                }

                //Корректируем главную стену в зависимости от угла, в который попадает труба, и стороны плиты
                if (sinkCorner)
                {
                    sinkText += " угловая";
                    if (sinkMin == 0 && (mainWall == 0 || mainWall == 1))
                    {
                        if (rightSideStove)
                        {
                            mainWall = 1;
                            sinkMin = 0;
                            sinkMax = sinkWidth;
                        }
                        else
                        {
                            mainWall = 0;
                            sinkMin = 0;
                            sinkMax = sinkWidth;
                        }
                    }
                    else if (sinkMax == width && mainWall == 1 || sinkMin == 0 && mainWall == 2)
                    {
                        if (rightSideStove)
                        {
                            mainWall = 2;
                            sinkMin = 0;
                            sinkMax = sinkWidth;
                        }
                        else
                        {
                            mainWall = 1;
                            sinkMin = width - sinkWidth;
                            sinkMax = width;
                        }
                    }
                    else if (sinkMax == height && mainWall == 2 || sinkMax == width && mainWall == 3)
                    {
                        if (rightSideStove)
                        {
                            mainWall = 3;
                            sinkMin = width - sinkWidth;
                            sinkMax = width;
                        }
                        else
                        {
                            mainWall = 2;
                            sinkMin = height - sinkWidth;
                            sinkMax = height;
                        }
                    }
                    else if (sinkMin == 0 && mainWall == 3 || sinkMax == height && mainWall == 0)
                    {
                        if (rightSideStove)
                        {
                            mainWall = 0;
                            sinkMin = height - sinkWidth;
                            sinkMax = height;
                        }
                        else
                        {
                            mainWall = 3;
                            sinkMin = 0;
                            sinkMax = sinkWidth;
                        }
                    }
                }

                //Меняем ширину и глубину местами для корректного отображения на боковых стенах
                if (mainWall % 2 == 0)
                {
                    int temp = sinkWidth;
                    sinkWidth = sinkDepth;
                    sinkDepth = temp;

                    temp = stoveWidth;
                    stoveWidth = stoveDepth;
                    stoveDepth = temp;
                }

                //Определяем координаты элементов
                switch (mainWall)
                {
                    case 0:
                        sinkX = 0;
                        stoveX = 0;
                        if (rightSideStove)
                        {
                            sinkY = sinkMax - sinkDepth;
                            stoveY = sinkY - stoveDepth;
                            break;
                        }
                        sinkY = sinkMin;
                        stoveY = sinkY + sinkDepth;
                        break;
                    case 1:
                        if (rightSideStove)
                        {
                            sinkX = sinkMin;
                            stoveX = sinkX + sinkWidth;
                        }
                        else
                        {
                            sinkX = sinkMax - sinkWidth;
                            stoveX = sinkX - stoveWidth;
                        }
                        sinkY = 0;
                        stoveY = 0;
                        break;
                    case 2:
                        sinkX = width - sinkWidth;
                        stoveX = width - stoveWidth;
                        if (rightSideStove)
                        {
                            sinkY = sinkMin;
                            stoveY = sinkY + sinkDepth;
                            break;
                        }
                        sinkY = sinkMax - sinkDepth;
                        stoveY = sinkY - stoveDepth;
                        break;
                    case 3:
                        if (rightSideStove)
                        {
                            sinkX = sinkMax - sinkWidth;
                            stoveX = sinkX - stoveWidth;
                        }
                        else
                        {
                            sinkX = sinkMin;
                            stoveX = sinkX + sinkWidth;
                        }
                        sinkY = height - sinkDepth;
                        stoveY = height - stoveDepth;
                        break;
                }

                //Корректируем координаты при неуспешной генерации
                //Изменяем сообщение об успешной генерации на сообщение об ошибке
                if (sinkX < 0 || sinkX > width - sinkWidth || sinkY < 0 || sinkY > height - sinkDepth)
                {
                    sinkColor = "red";
                    messageColor = "red";
                    stoveVisible = false;
                    if (sinkWidth > width || sinkDepth > height)
                    {
                        messageText = messageOptions["fail"] + messageOptions["smallRoom"];
                        sinkVisible = false;
                    }
                    else messageText = messageOptions["fail"] + messageOptions["intersect"];
                    if (sinkX < 0) sinkX = 0;
                    if (sinkX > width - sinkWidth) sinkX = width - sinkWidth;
                    if (sinkY < 0) sinkY = 0;
                    if (sinkY > height - sinkDepth) sinkY = height - sinkDepth;
                }
                else if (stoveX < 0 || stoveX > width - stoveWidth || stoveY < 0 || stoveY > height - stoveDepth)
                {
                    stoveColor = "red";
                    messageColor = "red";
                    if (stoveWidth > width || stoveDepth > height)
                    {
                        messageText = messageOptions["fail"] + messageOptions["smallRoom"];
                        stoveVisible = false;
                    }
                    else
                        messageText = messageOptions["fail"] + messageOptions["intersect"];
                    if (stoveX < 0) stoveX = 0;
                    if (stoveX > width - stoveWidth) stoveX = width - stoveWidth;
                    if (stoveY < 0) stoveY = 0;
                    if (stoveY > height - stoveDepth) stoveY = height - stoveDepth;
                }
            }

            //Если видимость мойки установлена на true, показываем ее с вычисленными значениями координат
            if (sinkVisible)
            {
                sinkCode = $@"context.lineWidth = 2;

            context.beginPath();
            context.fillStyle = 'white';
            context.strokeStyle = '{sinkColor}';
            context.rect({sinkX} + startPoint, {sinkY} + startPoint, {sinkWidth}, {sinkDepth});
            context.stroke();
            context.fillRect({sinkX} + startPoint, {sinkY} + startPoint, {sinkWidth}, {sinkDepth});
            context.closePath();

            context.beginPath();
            context.fillStyle = '{sinkColor}';
            context.fillText('{sinkText}', {sinkX} + startPoint + {sinkWidth} / 2, {sinkY} + startPoint + {sinkDepth} / 2 + 5);
            context.closePath();";
            }

            //Если видимость плиты установлена на true, показываем ее с вычисленными значениями координат
            if (stoveVisible)
            {
                stoveCode = $@"context.lineWidth = 2;

            context.beginPath();
            context.fillStyle = 'white';
            context.strokeStyle = '{stoveColor}';
            context.rect({stoveX} + startPoint, {stoveY} + startPoint, {stoveWidth}, {stoveDepth});
            context.stroke();
            context.fillRect({stoveX} + startPoint, {stoveY} + startPoint, {stoveWidth}, {stoveDepth});
            context.closePath();

            context.beginPath();
            context.fillStyle = '{stoveColor}';
            context.fillText('{StoveText}', {stoveX} + startPoint + {stoveWidth} / 2, {stoveY} + startPoint + {stoveDepth} / 2 + 5);
            context.closePath();";
            }

            //Код для элементов и вычисленные значения заполняются в финальный html
            string code = $@"
        <canvas id=myCanvas width='{width + 40}' height='{height + 60}' style='border: 1px solid #d3d3d3;'></canvas>
        <script>
            var c = document.getElementById('myCanvas');
            var context = c.getContext('2d');
            context.font = '10px Arial';
            context.textAlign = 'center';

            let startPoint = 20;

            let kitchenWidth = {width} + startPoint;
            let kitchenDepth = {height} + startPoint;

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

            {sinkCode}
            {stoveCode}

            context.beginPath();
            context.fillStyle = '{messageColor}';
            context.textAlign = 'left';
            context.fillText('{messageText}', startPoint, kitchenDepth + startPoint );
            context.closePath();
        </script>";
            return code;
        }*/
    }
}
