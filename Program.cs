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
            
            string html1 = htmlGenerator.GenerateHtml(true);
            string html2 = htmlGenerator.GenerateHtml(false);

            Directory.CreateDirectory("C:\\temp");
            File.WriteAllText("C:\\temp\\htmlRight.html", html1);
            File.WriteAllText("C:\\temp\\htmlLeft.html", html2);
            Console.WriteLine("Генерация завершена, файлы htmlRight и htmlLeft расположены в папке C:\\temp");
        }
    }
}
