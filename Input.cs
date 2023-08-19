namespace KitchenGenerator
{
    public class Input
    {
        //Для ввода значений boolean при консольном вводе
        public static bool enterBoolean()
        {
            switch (Console.ReadLine())
            {
                case "y": return true;
                case "n": return false;
                default:
                    Console.Write("Некорректный ввод, введите y или n: ");
                    return enterBoolean();
            }
        }

        //Для ввода значений int при консольном вводе
        public static int enterInteger()
        {
            try
            {
                return Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.Write("Некорректный формат ввода, введите целое число: ");
            }
            return enterInteger();
        }
    }
}
