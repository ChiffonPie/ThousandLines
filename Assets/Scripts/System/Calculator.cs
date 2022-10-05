namespace ThousandLines
{
    public static class Calculator
    {
        public static int SUM(int numA, int numB)
        {
            return numA + numB;
        }

        public static double SUM(double numA, double numB)
        {
            return numA + numB;
        }

        public static int Multiplication(int numA, int numB)
        {
            return numA * numB;
        }

        public static double Multiplication(double numA, double numB)
        {
            return numA * numB;
        }

        public static double MultiplicationPercent(double baseNum, double percentNum)
        {
            return baseNum += (baseNum * percentNum * 0.01f);
        }
    }
}